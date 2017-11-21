﻿using MvcApplication10.Helpers;
using MvcApplication20.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication20.Controllers
{
    public class AccountController : ControllerWrapper
    {
        //
        // GET: /Cabinet/

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegisterNew(FormCollection collection)
        {
            JsonMessage jm = new JsonMessage();
            try
            {
                string Login = collection["Email"];
                char[] charInvalidFileChars = Path.GetInvalidFileNameChars();
                foreach (char charInvalid in charInvalidFileChars)
                {
                    Login = Login.Replace(charInvalid, ' ');
                }

                string UserPath = Server.MapPath("/Users/" + Login);
                if (Directory.Exists(UserPath))
                {
                    jm.Message = "Логин (email) занят";
                    jm.Result = false;
                }
                else
                {
                    DirectoryInfo UserDirectoryInfo = Directory.CreateDirectory(UserPath);
                    System.IO.File.AppendAllText(UserPath + "/" + collection["UserName"] + ".txt", "");
                    System.IO.File.AppendAllText(UserPath + "/" + collection["Password"].GetHashCode() + ".hash", "");

                    jm.Message = "Заявка отправлена. Ожидайте ответа нашего сотрудника по указанным вами данным для обратной связи";
                    jm.Result = true;
                }
            }
            catch (Exception e)
            {
                jm.Result = false;
                jm.Message = "Во время обработки произошла ошибка - " + e.ToString();
            }
            return Json(jm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Logon(FormCollection collection)
        {
            JsonMessage jm = new JsonMessage();
            try
            {
                string Login = collection["Email"];
                char[] charInvalidFileChars = Path.GetInvalidFileNameChars();
                foreach (char charInvalid in charInvalidFileChars)
                {
                    Login = Login.Replace(charInvalid, ' ');
                }

                string UserPath = Server.MapPath("/Users/" + Login);
                if (!Directory.Exists(UserPath))
                {
                    jm.Message = "Пользователь с таким логином не найден";
                    jm.Result = false;
                }
                else
                {
                    if (!System.IO.File.Exists(UserPath + "/" + collection["Password"].GetHashCode() + ".hash"))
                    {
                        jm.Message = "Неверный пароль";
                        jm.Result = false;
                    }
                    else
                    {
                        if (!System.IO.File.Exists(UserPath + "/approved"))
                        {
                            jm.Message = "Пользователь еще не одобрен модератором";
                            jm.Result = false;
                        }
                        else
                        {
                            Account.UserLogin = Login;
                            Account.UserEmail = Login;
                            jm.Result = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                jm.Result = false;
                jm.Message = "Во время обработки произошла ошибка - " + e.ToString();
            }
            return Json(jm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Logout(FormCollection collection)
        {
            JsonMessage jm = new JsonMessage();
            try
            {
                Account.Clear();
                jm.Result = true;
            }
            catch (Exception e)
            {
                jm.Result = false;
                jm.Message = "Во время обработки произошла ошибка - " + e.ToString();
            }
            return Json(jm);
        }
    }
}
