using B1625MVC.BLL.Abstract;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Filters
{
    public class AuthenticationUserChecker : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string username = filterContext.HttpContext.User.Identity.Name;
            if(username != null)
            {
                var accountManager = filterContext.HttpContext.GetOwinContext().Get<IUserService>();
                if (!accountManager.IsUserExist(username))
                {
                    filterContext.HttpContext.GetOwinContext().Authentication.SignOut();
                }
            }
        }
    }
}