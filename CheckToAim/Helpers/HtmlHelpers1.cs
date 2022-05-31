using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheckToAim.Models;
namespace CheckToAim.Helpers
{
    public static class HtmlHelpers1
    {

        public static MvcHtmlString LikeCheckList(this HtmlHelper helper, Guid id)
        {
            CheckListDBContext context = new CheckListDBContext();
            context.CheckLists.Where(c => c.ID == id).FirstOrDefault().Update_Likes1();
            context.SaveChanges();
            return new MvcHtmlString(context.CheckLists.Where(c => c.ID == id).FirstOrDefault().Likes.ToString());
        }
        public static MvcHtmlString GetLikes(this HtmlHelper helper, Guid id)
        {
            CheckListDBContext context = new CheckListDBContext();
            return new MvcHtmlString(context.CheckLists.Where(c => c.ID == id).FirstOrDefault().Likes.ToString());
        }
    }
}