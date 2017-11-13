using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MvcApplication20.Models
{
    public class CartItem
    {
        public Item Item;
        public Int32 Count;
        public string Comment;
        public decimal Subtotal
        {
            get
            {
                return Item.PriceDouble * Count;
            }
        }
        public CartItem()
        {

        }
    }

    public class Cart
    {
        public List<CartItem> items;
        public decimal Total
        {
            get
            {
                return items.Sum(i => i.Subtotal);
            }
        }
        public Cart()
        {
            items = new List<CartItem>();
        }
    }
}