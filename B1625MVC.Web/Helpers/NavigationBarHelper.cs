using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace B1625MVC.Web.Helpers
{
    public static class NavigationBarHelper
    {
        public static MvcHtmlString NavigationBar(this HtmlHelper helper, IEnumerable<RouteLink> leftLinks, IEnumerable<RouteLink> rightLinks)
        {
            TagBuilder list = new TagBuilder("ul");
            TagBuilder li;
            foreach (RouteLink link in leftLinks)
            {
                li = new TagBuilder("li");
                li.InnerHtml = helper.RouteLink(link.Title, link.RouteData).ToString();
                li.MergeAttribute("style", "float:left");
                list.InnerHtml += li.ToString();
            }

            foreach (RouteLink link in rightLinks)
            {
                li = new TagBuilder("li");
                li.InnerHtml += helper.RouteLink(link.Title, link.RouteData).ToString();
                li.MergeAttribute("style", "float:right");

                if (link.InnerLinks != null && link.InnerLinks.Count > 0)
                {
                    li.MergeAttribute("class", "nav-dropdown");
                    TagBuilder innerlist = new TagBuilder("ul");
                    TagBuilder innerLi;
                    foreach (var innerlink in link.InnerLinks)
                    {
                        innerLi = new TagBuilder("li");
                        innerLi.InnerHtml = helper.RouteLink(innerlink.Title, innerlink.RouteData).ToString();
                        innerlist.InnerHtml += innerLi.ToString();
                    }
                    innerlist.MergeAttribute("class", "nav-dropdown-content");
                    li.InnerHtml += innerlist.ToString();
                }

                list.InnerHtml += li.ToString();
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
            InnerLinks = new List<RouteLink>();
        }
        public ICollection<RouteLink> InnerLinks { get; set; }
        public string Title { get; set; }
        public object RouteData { get; set; }
    }
}