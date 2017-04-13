using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq.Expressions;

using B1625MVC.Model.Entities;
using B1625MVC.Model.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO.Enums;

namespace B1625MVC.BLL.Services
{
    public class UserService : IUserService
    {
        IRepository repo;

        public UserService(IRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Method for transform account and profile data to user info object
        /// </summary>
        /// <param name="account"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        private UserInfo GetUserInfo(UserAccount account, UserProfile profile)
        {
            return new UserInfo()
            {
                Id = account.Id,
                Email = account.Email,
                UserName = account.UserName,
                EmailConfirmed = account.EmailConfirmed,
                Roles = repo.AccountManager.GetRoles(account.Id),
                Avatar = profile.Avatar,
                RegistrationDate = profile.RegistrationDate,
                Gender = (Gender)profile.Gender,
                Rating = profile.Rating
            };
        }

        /// <summary>
        /// Method for creating query for transforming all user profiles and acount to user info collection
        /// </summary>
        /// <returns></returns>
        private IQueryable<UserInfo> GetAllUsers()
        {
            var allRoles = repo.RoleManager.Roles.AsEnumerable();
            return repo.AccountManager.Users.Include(u => u.Profile).Select(ua => new UserInfo()
            {
                Id = ua.Id,
                Email = ua.Email,
                EmailConfirmed = ua.EmailConfirmed,
                UserName = ua.UserName,
                Gender = (Gender)ua.Profile.Gender,
                Avatar = ua.Profile.Avatar,
                Rating = ua.Profile.Rating,
                RegistrationDate = ua.Profile.RegistrationDate,
                Roles = ua.Roles.Join(allRoles, umr => umr.RoleId, rmr => rmr.Id, (umr, rmr) => rmr.Name)
            });
        }


        /// <summary>
        /// Method for creating new user and store it to database
        /// </summary>
        /// <param name="userData">Data that was stored to database as new user</param>
        /// <returns></returns>
        public async Task<OperationDetails> CreateAsync(CreateUserData userData)
        {
            UserAccount accountByEmail = await repo.AccountManager.FindByEmailAsync(userData.Email); //find user with new user email
            UserAccount accountByName = await repo.AccountManager.FindByNameAsync(userData.UserName); //find user with new user name

            if (accountByEmail == null && accountByName == null) //if users with new user email or name is not exist
            {
                var account = new UserAccount() //create new user account model object
                {
                    Email = userData.Email,
                    UserName = userData.UserName,
                    Profile = new UserProfile() //create new user profile model object
                    {
                        Avatar = userData.Avatar,
                        Gender = (Model.Enums.Gender)userData.Gender,
                        RegistrationDate = DateTime.Now
                    }
                };
                var result = await repo.AccountManager.CreateAsync(account, userData.NewPassword); //add new user account to database
                if (!result.Succeeded) //if user was not added successfully
                {
                    return new OperationDetails(false, "Can`t create new account:" + string.Concat(result.Errors)); //return fail result with message
                }

                if (userData.Roles != null) //if new user data have information about user roles
                {
                    foreach (string role in userData.Roles) //for each role name in user data roles
                    {
                        if (await repo.RoleManager.RoleExistsAsync(role)) //if role with this name is exists
                        {
                            result = await repo.AccountManager.AddToRoleAsync(account.Id, role); //add user to this role
                            if (!result.Succeeded) //if can`t add user to role
                            {
                                return new OperationDetails(false, $"Can`t add role \"{role}\" to {userData.UserName}"); //return fail result with message
                            }
                        }
                        else
                        {
                            return new OperationDetails(false, $"Can`t add role \"{role}\" to {userData.UserName}, because this role doesn't exist in database");
                        }
                    }
                }
                return new OperationDetails(true, "User account added suceessfully");
            }
            else
            {
                return new OperationDetails(false, "User with this email or username is already exists");
            }
        }

        /// <summary>
        /// Method for deleting user by user id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperationDetails> DeleteAsync(string id)
        {
            var account = await repo.AccountManager.FindByIdAsync(id); //get user account with this id
            if (account == null) //if user with id is not exists
            {
                return new OperationDetails(false, $"Can`t find account with id \"{id}\""); //return fail result with message
            }
            IdentityResult result = await repo.AccountManager.DeleteAsync(account); //trying to delete user
            if (!result.Succeeded) //if previous operation was not succeded
            {
                return new OperationDetails(false, $"Can`t delete account with id \"{id}\""); //return fail result with message
            }
            return new OperationDetails(true, $"Account with id \"{id}\" successfully deleted"); //return success result with message
        }

        /// <summary>
        /// Method for edit user data in database
        /// </summary>
        /// <param name="userData">Data that must be updated in database</param>
        /// <returns></returns>
        public async Task<OperationDetails> EditAsync(EditUserData userData)
        {
            //TODO: Add transaction for this action
            IdentityResult result = null;

            UserAccount account = await repo.AccountManager.FindByIdAsync(userData.Id); //get from database user account by id
            var profile = repo.Profiles.Get(userData.Id); //get from database user profile by id

            if (account != null && profile != null) //if user exists
            {
                if (account.Email != userData.Email && !string.IsNullOrEmpty(userData.Email)) //if email was changed
                {
                    account.Email = userData.Email; //set new email for account
                    account.EmailConfirmed = false;
                }

                if (!string.IsNullOrEmpty(userData.UserName)) //if username was changed
                {
                    account.UserName = userData.UserName; //set new name for user
                }

                profile.Gender = (Model.Enums.Gender)userData.Gender; //set new gender for user
                profile.Avatar = userData.Avatar; //set new avatar
                repo.Profiles.Update(profile); //update profile information for current user
                await repo.SaveChangesAsync(); //apply changes to profile

                result = await repo.AccountManager.UpdateAsync(account); //apply changes to account information for current user
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t save changes to database");
                }

                if (!string.IsNullOrEmpty(userData.NewPassword)) //if password was changed
                {
                    result = await repo.AccountManager.ChangePasswordAsync(account, userData.NewPassword); //try to change password for account
                    if (!result.Succeeded)
                    {
                        return new OperationDetails(false, $"Can`t change password");
                    }
                }

                if (userData.Roles != null) //if roles was changed
                {
                    foreach (string role in repo.RoleManager.Roles.Select(r => r.Name).ToArray()) //remove all roles for user
                    {
                        repo.AccountManager.RemoveFromRole(userData.Id, role);
                    }

                    foreach (string role in userData.Roles) //for each role added roles
                    {
                        if (await repo.RoleManager.RoleExistsAsync(role)) //if role is exists
                        {
                            result = await repo.AccountManager.AddToRoleAsync(account.Id, role); //add current user to role
                            if (!result.Succeeded)
                            {
                                return new OperationDetails(false, $"Can`t add role \"{role}\" to {userData.UserName}");
                            }
                        }
                        else
                        {
                            return new OperationDetails(false, $"Can`t add role \"{role}\" to {userData.UserName}, because this role doesn't exist in database");
                        }
                    }
                }
                return new OperationDetails(true, "User information was updated");
            }
            return new OperationDetails(false, "Can`t find user with this id!");
        }

        /// <summary>
        /// Method for getting user info from database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User info</returns>
        public async Task<UserInfo> GetByIdAsync(string id)
        {
            UserInfo userData = null;
            var account = await repo.AccountManager.FindByIdAsync(id); //get from database user by id
            if (account != null) //if user is exists
            {
                var profile = repo.Profiles.Get(account.Id); //get user profile with current user id
                userData = GetUserInfo(account, profile); //get user info throuht profile and account
            }
            return userData;
        }

        /// <summary>
        /// Method for getting user info from database by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns>User info</returns>
        public async Task<UserInfo> GetByNameAsync(string username)
        {
            UserInfo userData = null;
            var account = await repo.AccountManager.FindByNameAsync(username); //get from database user by name
            if (account != null)
            {
                var profile = repo.Profiles.Get(account.Id); //get user profile with current user id
                userData = GetUserInfo(account, profile); //get user info throuht profile and account
            }
            return userData;
        }

        /// <summary>
        /// Method for getting from database all user with paging support
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public IEnumerable<UserInfo> GetUsers(PageInfo pageInfo)
        {
            var result = GetAllUsers()
                .OrderBy(u => u.RegistrationDate)
                .Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage)
                .Take(pageInfo.ElementsPerPage)
                .ToList(); //get one concrete page of users from all of them
            return result;
        }

        /// <summary>
        /// Method for finding users with filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public IEnumerable<UserInfo> Find(Expression<Func<UserInfo, bool>> predicate, PageInfo pageInfo)
        {
            var allUsers = GetAllUsers(); //create query to get all users from database
            var filterResult = allUsers
                .Where(predicate)
                .OrderBy(u => u.RegistrationDate)
                .Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage)
                .Take(pageInfo.ElementsPerPage);
            return filterResult.ToList();
        }

        /// <summary>
        /// Method for user authentication in system
        /// </summary>
        /// <param name="emailOrUserName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> AuthenticateAsync(string emailOrUserName, string password)
        {
            ClaimsIdentity claim = null;

            var account = await repo.AccountManager.FindByEmailAsync(emailOrUserName); //try to find user account by email

            if (account == null) //if account with this email is exists
            {
                account = await repo.AccountManager.FindAsync(emailOrUserName, password); //try to find account by username 
            }
            else
            {
                account = await repo.AccountManager.FindAsync(account.UserName, password); //try to check if user pass correct password by finding user account with this data
            }

            if (account != null) //if account with this username and password is exists
            {
                claim = await repo.AccountManager.CreateIdentityAsync(account, DefaultAuthenticationTypes.ApplicationCookie); //create claim for this user
            }

            return claim;
        }

        /// <summary>
        /// Method for checking if user password the same with passed password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var account = await repo.AccountManager.FindByIdAsync(userId); //find user with passed id
            if(userId != null) //if user is exists
            {
                return await repo.AccountManager.CheckPasswordAsync(account, password); //check if passwords are the same
            }
            return false;
        }

        /// <summary>
        /// Method for checking if user with passed username is exists in database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserExist(string username)
        {
            return repo.AccountManager.FindByName(username) != null;
        }

        /// <summary>
        /// Method for getting all user roles from databese
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllRoles()
        {
            return repo.RoleManager.Roles.Select(r => r.Name).ToList();
        }

        /// <summary>
        /// Implementation of IDisposable interface
        /// </summary>
        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
