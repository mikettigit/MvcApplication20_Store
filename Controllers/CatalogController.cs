using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication20.Models;

namespace MvcApplication20.Controllers
{
    public class CatalogController : ControllerWrapper
    {
        //
        // GET: /Index/


        public ActionResult Index()
        {
            return RedirectToAction("Category");
        }

        public ActionResult Category(string query = "")
        {
            Category category = new Category(query.TrimEnd(new [] {'/'}));
            return View(category);
        }

        public ActionResult Item(string query = "")
        {
            Item item = new Item(query.TrimEnd(new[] { '/' }) + ".item");
            return View(item);
        }

        public ActionResult Brands()
        {
            string xml = Dm.GetListXml("supplier", null);
            List<Post> posts = Dm.ParseList(xml);
            return View(posts);
        }

        public ActionResult Brand(string id)
        {
            Post post = Dm.GetItem(id);
            return View(post);
        }
    }
}
