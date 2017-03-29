using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace B1625MVC.Web.Helpers
{
    public static class NavigationBarHelper
    {
        public static MvcHtmlString NavigationBar(this HtmlHelper helper, IEnumerable<RouteLink> links, 
            RouteLink loginLink, RouteLink logoutLink, RouteLink userPageLink, RouteLink registrationLink)
        {
            TagBuilder list = new TagBuilder("ul");
            TagBuilder li;
            foreach (RouteLink link in links)
            {
                li = new TagBuilder("li");
                li.InnerHtml = helper.RouteLink(link.Title, link.RouteData).ToString();
                list.InnerHtml += li.ToString();
            }

            if(helper.ViewContext.HttpContext.User.Identity.IsAuthenticated)
            {
                TagBuilder logoutField = new TagBuilder("li");
                logoutField.InnerHtml = helper.RouteLink(logoutLink.Title, logoutLink.RouteData).ToString();
                logoutField.MergeAttribute("style", "float:right");
                list.InnerHtml += logoutField.ToString();

                TagBuilder userPageField = new TagBuilder("li");
                userPageField.InnerHtml = helper.RouteLink(userPageLink.Title, userPageLink.RouteData).ToString();
                userPageField.MergeAttribute("style", "float:right");
                list.InnerHtml += userPageField.ToString();
            }
            else
            {
                TagBuilder loginField = new TagBuilder("li");
                loginField.InnerHtml = helper.RouteLink(loginLink.Title, loginLink.RouteData).ToString();
                loginField.MergeAttribute("style", "float:right");
                list.InnerHtml += loginField.ToString();

                TagBuilder registerField = new TagBuilder("li");
                registerField.InnerHtml = helper.RouteLink(registrationLink.Title, registrationLink.RouteData).ToString();
                registerField.MergeAttribute("style", "float:right");
                list.InnerHtml += registerField.ToString();
            }

            return new MvcHtmlString(list.ToString());
        }
    }

    public class RouteLink
    {
        public RouteLink(string title, object routeData)
        {
            Title = title;
            RouteData = routeData;
        }

        public string Title { get; set; }
        public object RouteData { get; set; }
    }
}