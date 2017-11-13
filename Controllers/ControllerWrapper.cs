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

    }
}
