using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

using B1625MVC.Model.Entities;
using B1625MVC.Model.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO.Enums;
using System.Linq.Expressions;

namespace B1625MVC.BLL.Services
{
    public class UserService : IUserService
    {
        IRepository repo;

        public UserService(IRepository repository)
        {
            repo = repository;
        }

        public async Task<OperationDetails> CreateAsync(CreateUserData userData)
        {
            UserAccount accountByEmail = await repo.AccountManager.FindByEmailAsync(userData.Email);
            UserAccount accountByName = await repo.AccountManager.FindByNameAsync(userData.UserName);

            if (accountByEmail == null && accountByName == null)
            {
                var account = new UserAccount()
                {
                    Email = userData.Email,
                    UserName = userData.UserName,
                    Profile = new UserProfile()
                    {
                        Avatar = userData.Avatar,
                        Gender = (Model.Enums.Gender)userData.Gender,
                        RegistrationDate = DateTime.Now
                    }
                };
                var result = await repo.AccountManager.CreateAsync(account, userData.NewPassword);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t create new account:" + string.Concat(result.Errors));
                }

                if (userData.Roles != null)
                {
                    foreach (string role in userData.Roles)
                    {
                        if (await repo.RoleManager.RoleExistsAsync(role))
                        {
                            result = await repo.AccountManager.AddToRoleAsync(account.Id, role);
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
                return new OperationDetails(true, "User account added suceessfully");
            }
            else
            {
                return new OperationDetails(false, "User with this email or username is already exists");
            }
        }

        public async Task<OperationDetails> DeleteAsync(string id)
        {
            var account = await repo.AccountManager.FindByIdAsync(id);
            if (account == null)
            {
                return new OperationDetails(false, $"Can`t find account with id \"{id}\"");
            }
            IdentityResult result = await repo.AccountManager.DeleteAsync(account);
            if (!result.Succeeded)
            {
                return new OperationDetails(false, $"Can`t delete account with id \"{id}\"");
            }
            return new OperationDetails(true, $"Account with id \"{id}\" successfully deleted");
        }

        public async Task<OperationDetails> EditAsync(EditUserData userData)
        {
            IdentityResult result = null;

            var accountTask = repo.AccountManager.FindByIdAsync(userData.Id);
            var profile = repo.Profiles.Get(userData.Id);

            Task.WaitAll(accountTask);

            UserAccount account = accountTask.Result;
            if (account != null && profile != null)
            {
                if (account.Email != userData.Email && !string.IsNullOrEmpty(userData.Email))
                {
                    account.Email = userData.Email;
                    account.EmailConfirmed = false;
                }

                if (!string.IsNullOrEmpty(userData.UserName))
                {
                    account.UserName = userData.UserName;
                }

                if (!string.IsNullOrEmpty(userData.NewPassword))
                {
                    result = await repo.AccountManager.ChangePasswordAsync(account, userData.NewPassword);
                    if (!result.Succeeded)
                    {
                        return new OperationDetails(false, $"Can`t change password");
                    }
                }

                if (userData.Roles != null)
                {
                    repo.AccountManager.RemoveFromRoles(userData.Id, repo.RoleManager.Roles.Select(r => r.Name).ToArray());
                    foreach (string role in userData.Roles)
                    {
                        if (await repo.RoleManager.RoleExistsAsync(role))
                        {
                            result = await repo.AccountManager.AddToRoleAsync(account.Id, role);
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

                profile.Gender = (Model.Enums.Gender)Enum.Parse(typeof(Model.Enums.Gender), Enum.GetName(typeof(Gender), userData.Gender));
                profile.Avatar = userData.Avatar;

                repo.Profiles.Update(profile);

                result = await repo.AccountManager.UpdateAsync(account);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t save changes to database");
                }

                await repo.SaveChangesAsync();
                return new OperationDetails(true, "User information was updated");
            }
            return new OperationDetails(false, "Can`t find user with this id!");
        }

        public async Task<UserInfo> GetByIdAsync(string id)
        {
            UserInfo userData = null;
            var account = await repo.AccountManager.FindByIdAsync(id);
            if (account != null)
            {
                var profile = repo.Profiles.Get(account.Id);
                userData = new UserInfo()
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
            return userData;
        }

        public async Task<UserInfo> GetByNameAsync(string username)
        {
            UserInfo userData = null;
            var account = await repo.AccountManager.FindByNameAsync(username);
            if (account != null)
            {
                var profile = repo.Profiles.Get(account.Id);
                userData = new UserInfo()
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
            return userData;
        }

        public IEnumerable<UserInfo> GetUsers(PageInfo pageInfo)
        {
            var allRoles = repo.RoleManager.Roles.ToList();

            var usersFromDb = repo.AccountManager.Users.Include(u => u.Profile).ToList();

            var result = usersFromDb.Select(ua => new UserInfo()
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
            }).OrderBy(u => u.RegistrationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToList();
            return result;
        }

        public IEnumerable<UserInfo> Find(Expression<Func<UserInfo, bool>> predicate, PageInfo pageInfo)
        {
            var allRoles = repo.RoleManager.Roles.AsEnumerable();
            var allUsers = repo.AccountManager.Users.Include(u => u.Profile).Select(ua => new UserInfo()
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
            var filterResult = allUsers.Where(predicate).OrderBy(u => u.RegistrationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage);
            return filterResult.ToList();
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(string emailOrUserName, string password)
        {
            ClaimsIdentity claim = null;

            var account = await repo.AccountManager.FindByEmailAsync(emailOrUserName);

            if (account == null)
            {
                account = await repo.AccountManager.FindAsync(emailOrUserName, password);
            }
            else
            {
                account = await repo.AccountManager.FindAsync(account.UserName, password);
            }

            if (account != null)
            {
                claim = await repo.AccountManager.CreateIdentityAsync(account, DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var account = await repo.AccountManager.FindByIdAsync(userId);
            return await repo.AccountManager.CheckPasswordAsync(account, password);
        }

        public bool IsUserExist(string username)
        {
            return repo.AccountManager.FindByName(username) != null;
        }

        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
