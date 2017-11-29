using MvcApplication20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication20.Controllers
{
    public class HomeController : ControllerWrapper
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(Dm);
        }

        public ActionResult Contacts()
        {
            return View(Dm);
        }

    }
}
