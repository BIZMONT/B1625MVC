using System.Web.Mvc;
using B1625MVC.Web.Filters;

namespace B1625MVC.Web
{
    public static class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthenticationUserChecker());
        }
    }
}