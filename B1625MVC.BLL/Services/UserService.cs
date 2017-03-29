using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

using B1625MVC.BLL.DTO;
using B1625MVC.Model.Entities;
using B1625MVC.Model.Abstract;
using B1625MVC.BLL.Abstract;

namespace B1625MVC.BLL.Services
{
    public class UserService : IUserService
    {
        IRepository b1625Repo;

        public UserService(IRepository repository)
        {
            b1625Repo = repository;
        }


        public async Task<OperationDetails> CreateAsync(CreateUserData userData)
        {
            var accountByEmail = await b1625Repo.AccountManager.FindByEmailAsync(userData.Email);
            var accountByName = await b1625Repo.AccountManager.FindByNameAsync(userData.UserName);

            if (accountByEmail == null && accountByName == null)
            {
                var account = new UserAccount()
                {
                    Email = userData.Email,
                    UserName = userData.UserName,
                    Profile = new UserProfile()
                    {
                        Avatar = userData.Avatar,
                        Gender = userData.Gender,
                        RegistrationDate = DateTime.Now
                    }
                };
                var result = await b1625Repo.AccountManager.CreateAsync(account, userData.NewPassword);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t create new account:" + string.Concat(result.Errors));
                }

                if (userData.Roles != null)
                {
                    foreach (string role in userData.Roles)
                    {
                        if (await b1625Repo.RoleManager.RoleExistsAsync(role))
                        {
                            result = await b1625Repo.AccountManager.AddToRoleAsync(account.Id, role);
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
            var account = await b1625Repo.AccountManager.FindByIdAsync(id);
            if (account == null)
            {
                return new OperationDetails(false, $"Can`t find account with id \"{id}\"");
            }
            IdentityResult result = await b1625Repo.AccountManager.DeleteAsync(account);
            if (!result.Succeeded)
            {
                return new OperationDetails(false, $"Can`t delete account with id \"{id}\"");
            }
            return new OperationDetails(true, $"Account with id \"{id}\" successfully deleted");
        }

        public async Task<OperationDetails> UpdateAsync(EditUserData userData)
        {
            var account = await b1625Repo.AccountManager.FindByIdAsync(userData.Id);
            var profile = b1625Repo.Profiles.Get(userData.Id);
            IdentityResult result = null;
            if (account != null && profile != null)
            {
                if (account.Email != userData.Email && !string.IsNullOrEmpty(userData.Email))
                {
                    account.Email = userData.Email;
                    account.EmailConfirmed = false;
                }

                if(!string.IsNullOrEmpty(userData.UserName))
                {
                    account.UserName = userData.UserName;
                }

                if (!string.IsNullOrEmpty(userData.NewPassword))
                {
                    result = await b1625Repo.AccountManager.ChangePasswordAsync(account, userData.NewPassword);
                    if (!result.Succeeded)
                    {
                        return new OperationDetails(false, $"Can`t change password");
                    }
                }

                if (userData.Roles != null)
                {
                    b1625Repo.AccountManager.RemoveFromRoles(userData.Id, b1625Repo.RoleManager.Roles.Select(r => r.Name).ToArray());
                    foreach (string role in userData.Roles)
                    {
                        if (await b1625Repo.RoleManager.RoleExistsAsync(role))
                        {
                            result = await b1625Repo.AccountManager.AddToRoleAsync(account.Id, role);
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

                profile.Gender = userData.Gender;
                profile.Avatar = userData.Avatar;

                b1625Repo.Profiles.Update(profile);

                result = await b1625Repo.AccountManager.UpdateAsync(account);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t save changes to database");
                }

                await b1625Repo.SaveChangesAsync();
                return new OperationDetails(true, "User information was updated");
            }
            return new OperationDetails(false, "Can`t find user with this id!");
        }

        public IEnumerable<UserInfo> Find(Func<UserInfo, bool> predicate)
        {
            var allUsers = b1625Repo.AccountManager.Users.Join(b1625Repo.Profiles.GetAll(), ua => ua.Id, up => up.AccountId, (ua, up) => new UserInfo()
            {
                Id = ua.Id,
                Email = ua.Email,
                UserName = ua.UserName,
                Gender = up.Gender,
                Avatar = up.Avatar,
                Roles = ua.Roles.Join(b1625Repo.RoleManager.Roles, umr => umr.RoleId, rmr => rmr.Id, (umr, rmr) => rmr.Name),
                Rating = up.Rating
            });

            return allUsers.Where(predicate).ToList();
        }

        public async Task<UserInfo> GetAsync(string id)
        {
            UserInfo userData = null;
            var account = await b1625Repo.AccountManager.FindByIdAsync(id);
            if (account != null)
            {
                var profile = b1625Repo.Profiles.Get(account.Id);
                userData = new UserInfo()
                {
                    Id = account.Id,
                    Email = account.Email,
                    UserName = account.UserName,
                    Roles = b1625Repo.AccountManager.GetRoles(account.Id),
                    Avatar = profile.Avatar,
                    Gender = profile.Gender,
                    Rating = profile.Rating
                };
            }
            return userData;
        }

        public IEnumerable<UserInfo> GetAll()
        {
            return b1625Repo.AccountManager.Users.ToList().Join(b1625Repo.Profiles.GetAll(), ua => ua.Id, up => up.AccountId, (ua, up) => new UserInfo()
            {
                Id = ua.Id,
                Email = ua.Email,
                UserName = ua.UserName,
                Gender = up.Gender,
                Avatar = up.Avatar,
                Roles = ua.Roles.Join(b1625Repo.RoleManager.Roles, umr => umr.RoleId, rmr => rmr.Id, (umr, rmr) => rmr.Name),
                Rating = up.Rating //TODO: Problems with rating
            });
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(string emailOrUserName, string password)
        {
            ClaimsIdentity claim = null;

            var account = await b1625Repo.AccountManager.FindByEmailAsync(emailOrUserName);

            if (account == null)
            {
                account = await b1625Repo.AccountManager.FindAsync(emailOrUserName, password);
            }
            else
            {
                account = await b1625Repo.AccountManager.FindAsync(account.UserName, password);
            }

            if (account != null)
            {
                claim = await b1625Repo.AccountManager.CreateIdentityAsync(account, DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var account = await b1625Repo.AccountManager.FindByIdAsync(userId);
            return await b1625Repo.AccountManager.CheckPasswordAsync(account, password);
        }

        public void Dispose()
        {
            b1625Repo.Dispose();
        }
    }
}
