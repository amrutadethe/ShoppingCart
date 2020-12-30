﻿using Newtonsoft.Json;
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
using PagedList;
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
        /// Partial Layout old view
        /// </summary>
        /// <returns></returns>
        //public ActionResult PartialLayout_old()
        //{
        //    hw_sites objhwsite = new hw_sites();
        //    DataSet ds = new DataSet();

        //    //using (SqlConnection con = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#"))
        //    using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#"))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("select * from hw_sites", con))
        //        {
        //            con.Open();
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            List<hw_sites> sitelist = new List<hw_sites>();
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                hw_sites hwsite_obj = new hw_sites();
        //                hwsite_obj.site_code = ds.Tables[0].Rows[i]["site_code"].ToString();
        //                hwsite_obj.site_desc = ds.Tables[0].Rows[i]["site_desc"].ToString();

        //                sitelist.Add(hwsite_obj);
        //            }
        //            objhwsite.siteinfo = sitelist;
        //        }
        //        con.Close();
        //    }
        //    // return View(objhwsite);
        //    return PartialView(objhwsite);
        //}

        /// <summary>
        /// Show all products for selected site
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="itemText"></param>
        /// <param name="site_code"></param>
        /// <param name="site_desc"></param>
        /// <returns></returns>
        public ActionResult ProductList(string searchText, string itemText, string site_code, string site_desc, int? page)
        {
            int PageIndex = 1;
            int PageSize = 20;
            PageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            Session["Sitedesc"] = site_desc;
            if (Session["Site_Code"] == null)
                Session["Site_Code"] = site_code;
            //TempData["Sitedesc"] = sitedesc;
            //TempData.Keep();
            ProductListViewModel model = new ProductListViewModel();
            List<CI_Item> getList = new List<CI_Item>();
            IPagedList<CI_Item> PageList = null;
            getList = objBALProductList.GetProductList(searchText, itemText, Convert.ToString(Session["Site_Code"]), site_desc);
            foreach (var item in getList)
            {
                // For each row which is bound, we need to also look up its lead time
                string selStr = "SELECT StandardLeadTime FROM [MAS_CML].[dbo].[IM_ItemVendor] WHERE ItemCode = '" + item.ItemCode + "';";
                //using (SqlConnection conLead = new SqlConnection("Data Source=massql\\massql;Initial Catalog=MAS_SYSTEM;Persist Security Info=True; User ID=CMIS;Password=Chemico;MultipleActiveResultSets=True"))
                using (SqlConnection conLead = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=MAS_CML;Persist Security Info=True; User ID=sa;Password=pass123!@#;MultipleActiveResultSets=True"))
                {
                    SqlCommand com = new SqlCommand(selStr, conLead);
                    conLead.Open();

                    SqlDataReader reader = com.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[0] != System.DBNull.Value)
                        {
                            int newDays = AddExtraDays(Convert.ToInt16(reader[0]));
                            //e.Row.Cells[4].Text = Convert.ToString(reader[0]);
                            // e.Row.Cells[4].Text = Convert.ToString(newDays);
                            item.UDF_HNW_CLASSIFICATION_CODE = Convert.ToString(newDays);
                        }
                        else
                        {
                            //e.Row.Cells[4].Text = "0";
                            item.UDF_HNW_CLASSIFICATION_CODE = "0";    
                        }
                    }
                    reader.Close();
                    conLead.Close();
                }
              
            }

            PageList = getList.ToPagedList(PageIndex, PageSize);
            model.totalProductCount = getList.Count();
            model.productList = PageList;
            return View(model);
        }
        protected static int AddExtraDays(int days)
        {
            int newDays = days;
            DateTime cur = System.DateTime.Now;

            switch (cur.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (days > 4 && days <= 9)
                    {
                        newDays += 2;
                    }
                    else if (days > 9 && days <= 14)
                    {
                        newDays += 4;
                    }

                    else if (days > 14 && days <= 19)
                    {
                        newDays += 6;
                    }
                    else if (days > 19 && days <= 24)
                    {
                        newDays += 8;
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (days > 3 && days <= 8)
                    {
                        newDays += 2;
                    }
                    else if (days > 8 && days <= 13)
                    {
                        newDays += 4;
                    }

                    else if (days > 13 && days <= 18)
                    {
                        newDays += 6;
                    }
                    else if (days > 18 && days <= 23)
                    {
                        newDays += 8;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (days > 2 && days <= 7)
                    {
                        newDays += 2;
                    }
                    else if (days > 7 && days <= 12)
                    {
                        newDays += 4;
                    }

                    else if (days > 12 && days <= 17)
                    {
                        newDays += 6;
                    }
                    else if (days > 17 && days <= 22)
                    {
                        newDays += 8;
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (days > 1 && days <= 6)
                    {
                        newDays += 2;
                    }
                    else if (days > 6 && days <= 11)
                    {
                        newDays += 4;
                    }

                    else if (days > 11 && days <= 16)
                    {
                        newDays += 6;
                    }
                    else if (days > 16 && days <= 21)
                    {
                        newDays += 8;
                    }
                    break;
                case DayOfWeek.Friday:
                    if (days > 6 && days <= 11)
                    {
                        newDays += 2;
                    }
                    else if (days > 11 && days <= 16)
                    {
                        newDays += 4;
                    }

                    else if (days > 16 && days <= 21)
                    {
                        newDays += 6;
                    }
                    else if (days > 21 && days <= 26)
                    {
                        newDays += 8;
                    }
                    newDays += 2;
                    break;
                default:
                    if (days > 6 && days <= 11)
                    {
                        newDays += 2;
                    }
                    else if (days > 11 && days <= 16)
                    {
                        newDays += 4;
                    }

                    else if (days > 16 && days <= 21)
                    {
                        newDays += 6;
                    }
                    else if (days > 21 && days <= 26)
                    {
                        newDays += 8;
                    }
                    newDays += 2;
                    break;
            }

            return newDays;
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
                ViewBag.lblTotal = String.Format("{0:c}", usersShoppingCart.GetTotal(cartId));
                ViewBag.ShoppingCartTitle = "Shopping Cart";
            }
            else
            {
                ViewBag.ShoppingCartTitle = "Shopping Cart is Empty";
                ViewBag.lblTotal = "";
                ViewBag.LabelTotalText = "";
            }
            if (Request.UrlReferrer != null && Request.QueryString["method"] != "noadd")
            {
                if (Request.UrlReferrer.ToString().Contains("ProductList") || Request.UrlReferrer.ToString().Contains("ProductDetails"))
                {
                    ViewBag.statusLabel = "Successfully added item to shopping cart.";
                }
            }
            else
            {
                ViewBag.statusLabel = "";
            }
            int numRows = 0;
            DataTable dt = new DataTable();
            dt = objBALProductList.GetQuantity(Convert.ToString(Session["CartId"]));
            if (dt.Rows.Count > 0)
            {
                numRows = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            var getCartitem = objBALProductList.GetMyShoopingCartList(Convert.ToString(Session["CartId"]));
            return View("MyShoppingCart", getCartitem);
        }
        public ActionResult UpdateShoppingCart(string str2)
        {
            MyShoppingCart usersShoppingCart = new MyShoppingCart();
            String cartId = GetShoppingCartId();
            DataTable list1 = (DataTable)JsonConvert.DeserializeObject(str2, (typeof(DataTable)));
            for (int i = 0; i < list1.Rows.Count; i++)
            {
                if (Convert.ToString(list1.Rows[i]["ItemCode"]) != "")
                {
                    usersShoppingCart.UpdateShoppingCartDatabase(cartId, Convert.ToString(list1.Rows[i]["ItemCode"]), Convert.ToString(list1.Rows[i]["Quantity"]), Convert.ToBoolean(list1.Rows[i]["Remove"]));
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
        public ActionResult CancelSite()
        {
            // Need to clear out the session var
            Session["Sitedesc"] = null;
            Session["Site_Code"] = null;
            // Remove all entries from the user's shopping cart
            // Now that we are all done, we need to clear out this user's shopping cart
            string deleteString = "DELETE FROM [cmis_portal_uat].[dbo].[ShoppingCart] WHERE CartID = '" + Session["CartId"] + "';";
            int numRows1 = 0;
            //using (SqlConnection condel = new SqlConnection("Data Source=massql\\massql;Initial Catalog=cmis_portal_uat;Persist Security Info=True;User ID=CMIS;Password=Chemico;MultipleActiveResultSets=True"))
            using (SqlConnection condel = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;Persist Security Info=True;User ID=sa;Password=pass123!@#;MultipleActiveResultSets=True"))
            {
                SqlCommand com3 = new SqlCommand(deleteString, condel);
                try
                {
                    condel.Open();
                    numRows1 = com3.ExecuteNonQuery();
                    condel.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("SQL ERROR: " + ex.Message);
                }
            }

            //  Response.Redirect(VirtualPathUtility.ToAbsolute("~/Default.aspx"));
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public ActionResult PagenationProductList(int? page)
        //{
        //    var dummyItems = Enumerable.Range(1, 150).Select(x => "Item " + x);
        //    var pager = new Pager(dummyItems.Count(), page);

        //    var viewModel = new PagerModel
        //    {
        //        Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
        //        Pager = pager
        //    };

        //    return View(viewModel);
        //}
    }
}