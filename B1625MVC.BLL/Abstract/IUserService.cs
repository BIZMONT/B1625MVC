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
        Task<OperationDetails> CreateAsync(UserDataDto userDto, string password, string[] role);
        Task<OperationDetails> DeleteAsync(string id);
        IEnumerable<UserDataDto> Find(Func<UserDataDto, bool> predicate);
        Task<UserDataDto> GetAsync(string id);
        IEnumerable<UserDataDto> GetAll();
        Task<OperationDetails> UpdateAsync(UserDataDto userDto);
    }
}