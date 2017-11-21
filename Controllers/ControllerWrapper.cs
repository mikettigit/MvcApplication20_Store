using MvcApplication10.Helpers;
using MvcApplication20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication20.Controllers
{
    public class ControllerWrapper : Controller
    {
        protected Account Account
        {
            get
            {
                Account result = null;

                SessionManager sm = new SessionManager();

                object account = sm.Get("Account");
                if (account != null)
                {
                    result = account as Account;
                }
                else
                {
                    result = new Account();
                    sm.Set("Account", result);
                }

                return result;
            }
        }

        protected Cart Cart
        {
            get
            {
                Cart result = null;

                SessionManager sm = new SessionManager();

                object cart = sm.Get("Cart");
                if (cart != null)
                {
                    result = cart as Cart;
                }
                else
                {
                    result = new Cart();
                    sm.Set("Cart", result);
                }

                return result;
            }
        }

        protected DataModel Dm
        {
            get
            {
                DataModel result = null;

                SessionManager sm = new SessionManager();

                object dm = sm.Get("Dm");
                if (dm != null)
                {
                    result = dm as DataModel;
                }
                else
                {
                    result = new DataModel();
                    sm.Set("Dm", result);
                }

                return result;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Account = Account;

            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (!Account.IsAuthorized)
            {
                if (controllerName == "Catalog")
                {
                    filterContext.Result = RedirectToAction("Login", "Account");
                }
            }
        }

    }
}
