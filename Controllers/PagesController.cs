using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication20.Controllers
{
    public class PagesController : ControllerWrapper
    {
        //
        // GET: /Page/

        public ActionResult Index(string param1)
        {
            string ViewFileName = "~/Views/Pages/" + param1;

            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewFileName);
            }
            else
            {
                return View(ViewFileName);
            }
        }

    }
}
