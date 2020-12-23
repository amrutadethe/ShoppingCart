using Punchout.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punchout.Web.Classes
{
    public class MyShoppingCart
    {
        public const string CartId = "PunchOut_CartID";
    

        //public void AddItem(string cartID, string productID, int quantity)
        //{
        //    using (cmis_portal_uatEntities db = new cmis_portal_uatEntities())
        //    {
        //        try
        //        {
        //            var myItem = (from c in db.ShoppingCarts
        //                          where c.CartID == cartID &&
        //                              c.ProductID == productID
        //                          select c).FirstOrDefault();
        //            if (myItem == null)
        //            {
        //                ShoppingCart cartadd = new ShoppingCart();
        //                cartadd.CartID = cartID;
        //                cartadd.Quantity = quantity;
        //                cartadd.ProductID = productID;
        //                cartadd.DateCreated = DateTime.Now;
        //                db.ShoppingCarts.Add(cartadd);
        //            }
        //            else
        //            {
        //                myItem.Quantity += quantity;
        //            }
        //            db.SaveChanges();
        //        }
        //        catch (Exception exp)
        //        {
        //            throw new Exception("ERROR: Unable to Add Item to Cart - " +
        //            exp.Message.ToString(), exp);
        //        }
        //    }
        //}
    }
}