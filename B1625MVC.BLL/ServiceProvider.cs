using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.Services;
using B1625MVC.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.BLL
{
    public static class ServiceProvider
    {
        public static UserService GetUserService(string connectionString)
        {
            return new UserService(new B1625DbRepository(connectionString));
        }
    }
}
