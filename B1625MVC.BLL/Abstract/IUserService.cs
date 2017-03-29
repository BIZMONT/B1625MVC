using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using B1625MVC.BLL.DTO;

namespace B1625MVC.BLL.Abstract
{
    public interface IUserService : IDisposable
    {
        Task<ClaimsIdentity> AuthenticateAsync(string emailOrUserName, string password);
        Task<OperationDetails> CreateAsync(CreateUserData userData);
        Task<OperationDetails> DeleteAsync(string id);
        IEnumerable<UserInfo> Find(Func<UserInfo, bool> predicate);
        Task<UserInfo> GetAsync(string id);
        IEnumerable<UserInfo> GetAll();
        Task<OperationDetails> UpdateAsync(EditUserData userData);
        Task<bool> CheckPasswordAsync(string userId, string password);
        bool IsUserExist(string username);
    }
}