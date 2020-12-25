using Newtonsoft.Json;
using Punchout.BAL;
using Punchout.DAL;
using Punchout.Entities;
using Punchout.Web.Classes;
using Punchout.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Punchout.Web.Controllers
{
    public class HomeController : Controller
    {
        DALProduct objDALproductList = new DALProduct();
        BALProduct objBALProductList = new BALProduct();
        MAS_CMLEntities objMAS_CMLEntities = new MAS_CMLEntities();

       // public const string CartId = "PunchOut_CartID";
        public ActionResult Index()
        {
            hw_sites objhwsite = new hw_sites();
            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#"))
            using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#"))
            {
                using (SqlCommand cmd = new SqlCommand("select * from hw_sites", con))

                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<hw_sites> sitelist = new List<hw_sites>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        hw_sites hwsite_obj = new hw_sites();
                        hwsite_obj.site_code = ds.Tables[0].Rows[i]["site_code"].ToString();
                        hwsite_obj.site_desc = ds.Tables[0].Rows[i]["site_desc"].ToString();

                        sitelist.Add(hwsite_obj);
                    }
                    objhwsite.siteinfo = sitelist;
                }
                con.Close();
            }
            
            return View(objhwsite);
        }

        public ActionResult PartialLayout()
        {
            hw_sites objhwsite = new hw_sites();
            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#"))
            using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#"))
            {
                using (SqlCommand cmd = new SqlCommand("select * from hw_sites", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<hw_sites> sitelist = new List<hw_sites>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        hw_sites hwsite_obj = new hw_sites();
                        hwsite_obj.site_code = ds.Tables[0].Rows[i]["site_code"].ToString();
                        hwsite_obj.site_desc = ds.Tables[0].Rows[i]["site_desc"].ToString();

                        sitelist.Add(hwsite_obj);
                    }
                    objhwsite.siteinfo = sitelist;
                }
                con.Close();
            }
            // return View(objhwsite);
            return PartialView(objhwsite);
        }

        /// <summary>
        /// Show all products for selected site
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="itemText"></param>
        /// <param name="site_code"></param>
        /// <param name="site_desc"></param>
        /// <returns></returns>
        public ActionResult ProductList(string searchText, string itemText, string site_code, string site_desc)
        {
            Session["Sitedesc"] = site_desc;
            Session["Site_Code"] = site_code;
            //TempData["Sitedesc"] = sitedesc;
            //TempData.Keep();
            List<CI_Item> getList = new List<CI_Item>();
            getList = objBALProductList.GetProductList(searchText, itemText, site_code, site_desc);
            return View("ProductList", getList);
        }

        public ActionResult ProductListByItemCode(string itemText, string site_code, string site_desc)
        {
            var getlist = objBALProductList.GetProductListByItemCode(itemText, site_code, site_desc);
            return View("ProductList", getlist);
        }

        public ActionResult ProductDetails(string id)
        {
            var getProductDetails = objBALProductList.GetProductDetails(id);
            return View("ProductDetails", getProductDetails);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult HSEApproval()
        {
            return View();
        }
        public string GetShoppingCartId()
        { 

            if (Session["CartId"] == null)
            {
            
             Session["CartId"] = System.Web.HttpContext.Current.Request.IsAuthenticated ?
                   User.Identity.Name : Guid.NewGuid().ToString();    
            }
            return Session["CartId"].ToString();
        }
        public ActionResult AddToCart(string id)
        {


            string productId;
            if (!String.IsNullOrEmpty(id))
            {
                MyShoppingCart usersShoppingCart = new MyShoppingCart();
               string cartId = GetShoppingCartId();
                productId = id;
                usersShoppingCart.AddItem(cartId, productId, 1);
                
            }
            else
            {
                Debug.Fail("ERROR : We should never get to AddToCart.aspx without a ProductId.");
                throw new Exception("ERROR : It is illegal to load AddToCart.aspx without setting a ProductId.");
            }
            // Response.Redirect("MyShoppingCart.aspx");
            return RedirectToAction("MyShoppingCart");
        }
        public ActionResult MyShoppingCart()
        {
                MyShoppingCart usersShoppingCart = new MyShoppingCart();
                String cartId = GetShoppingCartId();
            Session["PunchOut_CartID"] = cartId;
                decimal cartTotal = 0;
                cartTotal = usersShoppingCart.GetTotal(cartId);
                if (cartTotal > 0)
                {
                // lblTotal.Text = String.Format("{0:c}", usersShoppingCart.GetTotal(cartId));
                ViewBag.lblTotal = String.Format("{0:c}", usersShoppingCart.GetTotal(cartId));
            }
                else
                {
                ViewBag.ShoppingCartTitle = "Shopping Cart is Empty";
                ViewBag.lblTotal = "";
                ViewBag.LabelTotalText = "";
                    //LabelTotalText.Text = "";
                    //lblTotal.Text = "";
                    //ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                    //UpdateBtn.Visible = false;
                    //CheckoutBtn.Visible = false;
            }

                if (Request.UrlReferrer != null && Request.QueryString["method"] != "noadd")
                {
                    if (Request.UrlReferrer.ToString().Contains("ProductList") || Request.UrlReferrer.ToString().Contains("ProductDetails"))
                    {
                    // statusLabel.Text = "Successfully added item to shopping cart.";
                    ViewBag.statusLabel = "Successfully added item to shopping cart.";
                    }
                }
                else
                {
                ViewBag.statusLabel = "";
                   // statusLabel.Text = "";
                }
            int numRows = 0;
            DataTable dt = new DataTable();
            dt = objBALProductList.GetQuantity(Convert.ToString(Session["CartId"]));
            if(dt.Rows.Count>0)
            {
                numRows = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            var getCartitem = objBALProductList.GetMyShoopingCartList(Convert.ToString(Session["CartId"]));
            ////SiteMaster master = (SiteMaster)Page.Master;
            //master.CartTotal = Convert.ToString(numRows);
            return View("MyShoppingCart",getCartitem);
        }
        //public ActionResult UpdateShoppingCart()
        //{
        //    MyShoppingCart usersShoppingCart = new MyShoppingCart();
        //    String cartId = GetShoppingCartId();
        //    ShoppingCartUpdates[] cartUpdates = new ShoppingCartUpdates[sess];
        //    for (int i = 0; i < MyList.Rows.Count; i++)
        //    {
        //        IOrderedDictionary rowValues = new OrderedDictionary();
        //        rowValues = GetValues(MyList.Rows[i]);
        //        cartUpdates[i].ItemCode = Convert.ToString(rowValues["ItemCode"]);
        //        cartUpdates[i].PurchaseQuantity = Convert.ToInt32(rowValues["Quantity"]);
        //        CheckBox cbRemove = new CheckBox();
        //        cbRemove = (CheckBox)MyList.Rows[i].FindControl("Remove");
        //        cartUpdates[i].RemoveItem = cbRemove.Checked;
        //    }
        //    return View();
        //}
     public ActionResult UpdateShoppingCart(string str2)
        {
            MyShoppingCart usersShoppingCart = new MyShoppingCart();
                String cartId = GetShoppingCartId();
            DataTable list1 = (DataTable)JsonConvert.DeserializeObject(str2, (typeof(DataTable)));
            for (int i = 0; i < list1.Rows.Count; i++)
            {
                if (Convert.ToString(list1.Rows[i]["ItemCode"]) != "")
                {
                    //hInputPara.Add("@ItemCode", Convert.ToString(list1.Rows[i]["ItemCode"]));
                    //hInputPara.Add("@Quantity", Convert.ToString(list1.Rows[i]["Quantity"]));
                    //hInputPara.Add("@Remove", Convert.ToString(list1.Rows[i]["Remove"]));
                    // var result = sqlDataAccess.ExecuteStoreProcedure("usp_InsertCompPropNew", hInputPara);
                    usersShoppingCart.UpdateShoppingCartDatabase(cartId, Convert.ToString(list1.Rows[i]["ItemCode"]), Convert.ToString(list1.Rows[i]["Quantity"]),Convert.ToBoolean(list1.Rows[i]["Remove"]));
                }
            }
            ViewBag.lblTotal = String.Format("{0:c}", usersShoppingCart.GetTotal(cartId));
            ViewBag.statusLabel = "Updated Shopping cart.";
            int numRows = 0;
            DataTable dt = new DataTable();
            dt = objBALProductList.GetQuantity(Convert.ToString(Session["CartId"]));
            if (dt.Rows.Count > 0)
            {
                numRows = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return Json(new { success = true, Message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}