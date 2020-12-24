using Punchout.BAL;
using Punchout.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Punchout.Web.Filters
{
    public class LayoutFilter : ActionFilterAttribute
    {
        BALProduct objBALProductList = new BALProduct();
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //HttpContext.Current.Session["Sitedesc"] = "Site1";
            if(HttpContext.Current.Session["Sitedesc"] ==null)
            {
                HttpContext.Current.Session["Sitedesc"] = "Please select a site from the left.";
            }

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
                    objhwsite.site_code = Convert.ToString(HttpContext.Current.Session["Site_Code"]);
                    objhwsite.site_desc = Convert.ToString(HttpContext.Current.Session["Sitedesc"]);
                }
                con.Close();
                //if (HttpContext.Current.Session["PunchOut_CartID"] == null)
                //{
                //    HttpContext.Current.Session["PunchOut_CartID"] = "PunchOut_CartID";
                //}
                int numRows = 0;
               DataTable dt = new DataTable();
                dt = objBALProductList.GetQuantity(Convert.ToString(HttpContext.Current.Session["CartId"]));
                if (dt.Rows.Count > 0)
                {
                    
                    numRows = Convert.ToInt32(dt.Rows[0][0]);
                }
                HttpContext.Current.Session["numrows"] = numRows.ToString();
                filterContext.Controller.ViewData["layoutPageData"] = objhwsite;
            }
        }
    }
}