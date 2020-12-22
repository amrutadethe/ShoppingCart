using Punchout.BAL;
using Punchout.DAL;
using Punchout.Entities;
using Punchout.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Punchout.Web.Controllers
{
    public class HomeController : Controller
    {
        DALProduct objDALproductList = new DALProduct();
        BALProduct objBALProductList = new BALProduct();
        MAS_CMLEntities objMAS_CMLEntities = new MAS_CMLEntities();
        DALProductDetails objDALProductDetails = new DALProductDetails();
        BALProductDetails objBALProductDetails = new BALProductDetails();

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
        /// <summary>
        /// Clear Shopping Cart
        /// </summary>
        /// <returns></returns>
        public ActionResult ClicktoConfirm()
        {
            return View();
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
        public ActionResult Click(string id)
        {
            var getProductDetails = objBALProductDetails.GetProductDetails(id);
            return View("ProductDetails", getProductDetails);
        }
    }
}