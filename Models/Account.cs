using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication20.Models
{
    public class Account
    {
        public string UserLogin;
        public string UserEmail;

        public bool IsAuthorized
        {
            get { return !String.IsNullOrWhiteSpace(UserLogin); }
        }

        public void Clear()
        {
            UserLogin = "";
            UserEmail = "";
        }

        public Account()
        {
            Clear();
        }
    }
}