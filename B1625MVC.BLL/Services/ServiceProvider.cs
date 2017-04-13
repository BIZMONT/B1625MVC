using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.Services;
using B1625MVC.Model.Repositories;

namespace B1625MVC.BLL
{
    /// <summary>
    /// Class that provides services from buisness logic layer
    /// </summary>
    public static class ServiceProvider
    {
        public static IUserService GetUserService(string connectionString)
        {
            return new UserService(new B1625DbRepository(connectionString));
        }

        public static IContentService GetContentService(string connectionString)
        {
            return new ContentService(new B1625DbRepository(connectionString));
        }
    }
}
