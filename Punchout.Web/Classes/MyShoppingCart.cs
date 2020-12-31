using Punchout.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punchout.Web.Classes
{
    public class MyShoppingCart
    {
        public const string CartId = "PunchOut_CartID";

        /// <summary>
        /// Add Item to MyShopping Cart 
        /// </summary>
        /// <param name="cartID"></param>
        /// <param name="productID"></param>
        /// <param name="quantity"></param>
        public void AddItem(string cartID, string productID, int quantity)
        {
            using (cmis_portal_uatEntities1 db = new cmis_portal_uatEntities1())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCarts
                                  where c.CartID == cartID &&
                                      c.ProductID == productID
                                  select c).FirstOrDefault();
                    // var myItem = db.ShoppingCarts.SingleOrDefault(
                    //c => c.CartID == cartID
                    //&& c.ProductID == productID);
                    if (myItem == null)
                    {
                        ShoppingCart cartadd = new ShoppingCart();
                        cartadd.CartID = cartID;
                        cartadd.Quantity = quantity;
                        cartadd.ProductID = productID;
                        cartadd.DateCreated = DateTime.Now;
                        db.ShoppingCarts.Add(cartadd);
                    }
                    else
                    {
                        myItem.Quantity += quantity;
                    }
                    db.SaveChanges();
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Add Item to Cart - " +
                    exp.Message.ToString(), exp);
                }
            }
        }
        /// <summary>
        /// Get Cart Added Total Item quantity
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        internal decimal GetTotal(string cartId)
        {
            using (cmis_portal_uatEntities1 db = new cmis_portal_uatEntities1())
            {
                decimal cartTotal = 0;
                try
                {
                    var myCart = (from c in db.ViewCarts where c.CartID == cartId select c);
                    if (myCart.Count() > 0)
                    {
                        cartTotal = myCart.Sum(od => (decimal)od.Quantity * ((decimal)od.UnitCost + (decimal)od.UDF_ENVIRONMENTAL + (decimal)od.UDF_FREIGHT + (decimal)od.UDF_FUEL + (decimal)od.UDF_OTHER + (decimal)od.UDF_PALLET));
                        //cartTotal = myCart.Count();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Calculate Order Total - " +
                    exp.Message.ToString(), exp);
                }
                return (cartTotal);
            }
        }
        /// <summary>
        /// Update My Shopping Cart quantity and Remove Item
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ItemCode"></param>
        /// <param name="Quantity"></param>
        /// <param name="Remove"></param>
        public void UpdateShoppingCartDatabase(string cartId, string ItemCode, string Quantity, bool Remove)
        {
            if (Convert.ToInt32(Quantity) < 1 || Remove == true)
            {
                RemoveItem(cartId, ItemCode);

            }
            else
            {
                UpdateItem(cartId, ItemCode, Convert.ToInt32(Quantity));
            }
        }
        /// <summary>
        /// Remove Item in shopping Cart
        /// </summary>
        /// <param name="cartID"></param>
        /// <param name="productID"></param>
        public void RemoveItem(string cartID, string productID)
        {
            using (cmis_portal_uatEntities1 db = new cmis_portal_uatEntities1())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCarts
                                  where c.CartID == cartID &&
                                      c.ProductID == productID
                                  select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        db.ShoppingCarts.Remove(myItem);
                        db.SaveChanges();
                    }
                    HttpContext.Current.Session["CartTotal"] = Convert.ToInt16(HttpContext.Current.Session["CartTotal"]) - 1;
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Remove Cart Item - " +
                    exp.Message.ToString(), exp);
                }
            }
        }
        /// <summary>
        /// Update Item qunatity in shopping Cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ItemCode"></param>
        /// <param name="Quantity"></param>
        public void UpdateItem(string cartId, string ItemCode, int Quantity)
        {
            using (cmis_portal_uatEntities1 db = new cmis_portal_uatEntities1())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCarts
                                  where c.CartID == cartId &&
                                    c.ProductID == ItemCode
                                  select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        myItem.Quantity = Quantity;
                        db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Item - " +
                    exp.Message.ToString(), exp);
                }
            }
        }
    }
}