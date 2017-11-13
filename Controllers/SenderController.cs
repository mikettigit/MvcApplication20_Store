using MvcApplication10.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication20.Controllers
{
    public class SenderController : Controller
    {
        //
        // GET: /Sender/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Send(FormCollection collection)
        {

            JsonMessage jm = new JsonMessage();

            try
            {
                string name = collection["name"];
                string phone = collection["phone"];
                string email = collection["email"];
                string order = collection["order"];
                string body = "Имя: " + name + "\n";
                body += "Телефон: " + phone + "\n";
                body += "Email: " + email + "\n" + "\n";
                body += "Заказ: " + order;
                string subject = "Заказ c сайта";

                //1
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress(ConfigurationManager.AppSettings["messageFrom"]);
                mailObj.To.Add(ConfigurationManager.AppSettings["messageTo"]);
                mailObj.Subject = subject;
                mailObj.Body = body;

                SmtpClient SMTPServer = new SmtpClient("localhost");
                SMTPServer.Send(mailObj);

                jm.Result = true;
                jm.Message = "Мы получили Ваш запрос и скоро свяжемся с Вами...";
            }
            catch (Exception e)
            {
                jm.Result = true;
                jm.Message = "Во время отправки произошла ошибка - " + e.ToString();
            }

            return Json(jm);
        }
    }
}
