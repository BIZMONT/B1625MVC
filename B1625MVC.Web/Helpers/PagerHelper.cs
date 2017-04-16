﻿using B1625MVC.BLL.DTO;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace B1625MVC.Web.Helpers
{
    public static class PagerHelper
    {
        public static MvcHtmlString Pager(this HtmlHelper helper, PageInfo pageInfo)
        {
            string content = string.Empty;
            for (int i = 0; i < pageInfo.TotalPages; i++)
            {
                content += helper.RouteLink(i.ToString(), new
                {
                    controller = helper.ViewContext.RouteData.Values["controller"],
                    action = helper.ViewContext.RouteData.Values["action"],
                    page = i + 1
                }, (pageInfo.CurrentPage == i ? new { @class = "current-page" } : null));
            }
            return new MvcHtmlString(content);
        }
    }
}