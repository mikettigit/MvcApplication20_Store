﻿using MvcApplication20.Helpers;
using MvcApplication20.Helpers;
using MvcApplication20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication20.Controllers
{
    public class CartController : ControllerWrapper
    {
        //
        // GET: /Cart/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetTotalCount()
        {
            JsonMessage jm = new JsonMessage();
            jm.Object = Cart.items.Count;
            jm.Result = true;
            jm.Message = Cart.items.Count.ToString();
            return Json(jm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetTotalAmount()
        {
            JsonMessage jm = new JsonMessage();
            jm.Object = Cart.items.Sum(i => i.Subtotal);
            jm.Result = true;
            jm.Message = Cart.items.Count.ToString();
            return Json(jm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddItem(FormCollection collection)
        {
            JsonMessage jm = new JsonMessage();
            try
            {
                CartItem cartItem = new CartItem();
                string ItemURL = collection["ItemURL"];
                bool update = Convert.ToBoolean(collection["update"]);
                cartItem.Item = new Item(ItemURL.Replace(CatalogParams.BaseItemUrl + "/","") + ".item");
                int count = 1;
                if (!Int32.TryParse(collection["count"], out count))
                {
                    count = 1;
                }
                if (update)
                {
                    cartItem.Count = count;
                }
                else
                {
                    cartItem.Count += count;
                }
                if (cartItem.Count < 0) { cartItem.Count = 0; }

                IEnumerable<CartItem> existingItems = Cart.items.Where(i => i.Item.RelativeUrl == ItemURL);
                if (existingItems.Count() > 0)
                {
                    foreach (var existingItem in existingItems)
                    {
                        if (update)
                        {
                            existingItem.Count = count;
                        } 
                        else
                        {
                            existingItem.Count += count;
                        }
                        if (existingItem.Count < 0) { existingItem.Count = 0; }
                        cartItem = existingItem;
                    }
                }
                else
                {
                    Cart.items.Add(cartItem);
                }

                jm.Message = "Добавлено " + count + " шт. " + cartItem.Item.Name;
                jm.Object = cartItem.Subtotal;
                jm.Result = true;
            }
            catch (Exception e)
            {
                jm.Result = false;
                jm.Message = "Во время обработки произошла ошибка - " + e.ToString();
            }
            return Json(jm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DelItem(FormCollection collection)
        {
            JsonMessage jm = new JsonMessage();
            try
            {
                string ItemURL = collection["ItemURL"];

                List<CartItem> existingItems = Cart.items.Where(i => i.Item.RelativeUrl == ItemURL).ToList();
                if (existingItems.Count() > 0)
                {
                    Cart.items = Cart.items.Except(existingItems).ToList();
                    jm.Message = "Позиция удалена";
                }

                jm.Result = true;
            }
            catch (Exception e)
            {
                jm.Result = false;
                jm.Message = "Во время обработки произошла ошибка - " + e.ToString();
            }
            return Json(jm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendOrder(FormCollection collection)
        {
            JsonMessage jm = new JsonMessage();

            string order = "";
            foreach (var item in Cart.items)
            {
                order += item.Item.Description + " (" + item.Comment + "): " + item.Count.ToString() + "\r\n";
            }

            try
            {
                string name = collection["name"];
                string phone = collection["phone"];
                string email = collection["email"];
                string body = "Имя: " + name + "\n";
                body += "Телефон: " + phone + "\n";
                body += "Email: " + email + "\n" + "\n";
                body += "Заказ: " + order;
                string subject = "Заказ c сайта";

                MailSender.Send(subject, body);

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

        public ActionResult Index()
        {
            return View(Cart);
        }
    }
}


