using DocumentManagementProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return Redirect(@"/document/");
        }

    }
}