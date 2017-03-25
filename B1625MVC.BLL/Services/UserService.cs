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


        public async Task<OperationDetails> CreateAsync(UserDataDto userDto, string password, string[] roles)
        {
            var accountByEmail = await b1625Repo.AccountManager.FindByEmailAsync(userDto.Email);
            var accountByName = await b1625Repo.AccountManager.FindByNameAsync(userDto.UserName);

            if (accountByEmail == null && accountByName == null)
            {
                var account = new UserAccount() { Email = userDto.Email, UserName = userDto.UserName, Profile = new UserProfile()
                {
                    Avatar = userDto.Avatar,
                    Gender = userDto.Gender,
                }};
                var result = await b1625Repo.AccountManager.CreateAsync(account, password);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t create new account");
                }

                await b1625Repo.AccountManager.AddToRolesAsync(account.Id, roles);
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

        public async Task<OperationDetails> UpdateAsync(UserDataDto userDto)
        {
            var account = await b1625Repo.AccountManager.FindByIdAsync(userDto.Id);
            var profile = b1625Repo.Profiles.Get(userDto.Id);

            if (account != null && profile != null)
            {
                if (account.Email != userDto.Email)
                {
                    account.Email = userDto.Email;
                    account.EmailConfirmed = false;
                }

                b1625Repo.AccountManager.RemoveFromRoles(userDto.Id, b1625Repo.RoleManager.Roles.Select(r => r.Name).ToArray());
                var result = await b1625Repo.AccountManager.AddToRolesAsync(userDto.Id, userDto.Roles.ToArray());

                if(!result.Succeeded)
                {
                    return new OperationDetails(false, "Can`t add this user to roles");
                }

                profile.Gender = userDto.Gender;
                profile.Avatar = userDto.Avatar;

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

        public IEnumerable<UserDataDto> Find(Func<UserDataDto, bool> predicate)
        {
            var allUsers = b1625Repo.AccountManager.Users.Join(b1625Repo.Profiles.GetAll(), ua => ua.Id, up => up.AccountId, (ua, up) => new UserDataDto()
            {
                Id = ua.Id,
                Email = ua.Email,
                UserName = ua.UserName,
                Gender = up.Gender,
                Avatar = up.Avatar,
                Roles = ua.Roles.Join(b1625Repo.RoleManager.Roles, umr => umr.RoleId, rmr => rmr.Id, (umr, rmr) => rmr.Name),
                Rating = up.Rating
            });

            return allUsers.Where(predicate);
        }

        public async Task<UserDataDto> GetAsync(string id)
        {
            UserDataDto userData = null;
            var account = await b1625Repo.AccountManager.FindByIdAsync(id);
            if (account != null)
            {
                var profile = b1625Repo.Profiles.Get(account.Id);
                userData = new UserDataDto()
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

        public IEnumerable<UserDataDto> GetAll()
        {
            return b1625Repo.AccountManager.Users.ToList().Join(b1625Repo.Profiles.GetAll(), ua => ua.Id, up => up.AccountId, (ua, up) => new UserDataDto()
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

        public void Dispose()
        {
            b1625Repo.Dispose();
        }
    }
}
