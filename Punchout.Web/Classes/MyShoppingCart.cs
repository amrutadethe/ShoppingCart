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
    }
}