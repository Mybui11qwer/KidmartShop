﻿using System.Web.Mvc;

namespace KidMartStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index" ,Controller = "Home", id = UrlParameter.Optional },
                new[] { "KidMartStore.Areas.Admin.Controllers" }
            );
        }
    }
}