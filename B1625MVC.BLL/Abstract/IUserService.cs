using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using B1625MVC.BLL.DTO;
using System.Linq.Expressions;

namespace B1625MVC.BLL.Abstract
{
    public interface IUserService : IDisposable
    {
        IIdentityMessageService EmailService { get; set; }

        Task<ClaimsIdentity> AuthenticateAsync(string emailOrUserName, string password);
        Task<OperationDetails> CreateAsync(CreateUserData userData);
        Task<OperationDetails> DeleteAsync(string id);
        IEnumerable<UserInfo> Find(Expression<Func<UserInfo, bool>> predicate, PageInfo pageInfo);
        Task<UserInfo> GetByIdAsync(string id);
        IEnumerable<UserInfo> GetUsers(PageInfo pageInfo);
        Task<OperationDetails> EditAsync(EditUserData userData);
        Task<bool> CheckPasswordAsync(string userId, string password);
        bool IsUserExist(string username);
        Task<UserInfo> GetByNameAsync(string username);
        IEnumerable<string> GetAllRoles();
        Task SendVerificationEmailAsync(string userId, string baseUrl);
        Task<bool> CheckEmailToken(string userId, string token);
    }
}