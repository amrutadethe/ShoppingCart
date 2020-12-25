using Punchout.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punchout.DAL
{
    public class DALProduct
    {
        MAS_CMLEntities objMASCMLEntities = new MAS_CMLEntities();

        public List<CI_Item> GetProductList(string searchText, string ItemText, string siteId, string sitedesc)
        {
            //List<CI_Item> CI_Item = objMASCMLEntities.CI_Item.ToList();
            //return CI_Item;
            try
            {
                DataTable dt = new DataTable();
                //SqlConnection con = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlCommand cmd = new SqlCommand("select ItemCode,ItemCodeDesc,SalesUnitOfMeasure,Category1,Category2,StandardUnitPrice,UDF_HNW_CLASSIFICATION_CODE from CI_Item where StandardUnitPrice>0 and ItemCodeDesc LIKE @searchText and ItemCode Like @itemText and SUBSTRING([ItemCode], 1, 2) = 'HW' and SUBSTRING([ItemCode], 4, 4) LIKE  @Site ", con);
                //cmd.Parameters.AddWithValue("@searchText",searchText);
                //cmd.Parameters.AddWithValue("@itemText",ItemText);
                //cmd.Parameters.AddWithValue("@Site", site);
                cmd.Parameters.AddWithValue("@searchText", string.Format("%{0}%", searchText));
                cmd.Parameters.AddWithValue("@itemText", string.Format("%{0}%", ItemText));
                cmd.Parameters.AddWithValue("@Site", string.Format("%{0}%", siteId));
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var allProductList = new List<CI_Item>();
                if (dt.Rows.Count > 0)
                {
                    allProductList = dt.AsEnumerable().Select(rows => new CI_Item
                    {
                        ItemCode = Convert.ToString(rows["ItemCode"]),
                        ItemCodeDesc = Convert.ToString(rows["ItemCodeDesc"]),
                        SalesUnitOfMeasure = Convert.ToString(rows["SalesUnitOfMeasure"]),
                        Category1 = Convert.ToString(rows["Category1"]),
                        Category2 = Convert.ToString(rows["Category2"]),
                        StandardUnitPrice = Convert.ToDecimal(rows["StandardUnitPrice"]),
                        UDF_HNW_CLASSIFICATION_CODE = Convert.ToString(rows["UDF_HNW_CLASSIFICATION_CODE"]),
                    }).ToList();
                }
                return allProductList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ViewCart> GetMyShoopingCartList(string cartId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=cmis_portal_uat;User ID=sa;Password=pass123!@#");
                SqlCommand cmd = new SqlCommand("select * from ViewCart where  CartID = '" + cartId + "' ", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var allProductList = new List<ViewCart>();
                if (dt.Rows.Count > 0)
                {
                    
                    allProductList = dt.AsEnumerable().Select(rows => new ViewCart
                    {
                        ItemCode = Convert.ToString(rows["ItemCode"]),
                        UDF_CHEM_CATEGORY = Convert.ToString(rows["UDF_CHEM_CATEGORY"]),
                        ItemCodeDesc = Convert.ToString(rows["ItemCodeDesc"]),
                        UnitCost = Convert.ToDecimal(rows["UnitCost"]),
                        Quantity = Convert.ToInt32(rows["Quantity"]),
                        SalesUnitOfMeasure = Convert.ToString(rows["SalesUnitOfMeasure"]),

                     //   StandardLeadTime = Convert.ToDecimal(rows["StandardLeadTime"],
                      StandardLeadTime=  decimal.Parse((rows["StandardLeadTime"] == null || rows["StandardLeadTime"].ToString() == String.Empty) ? "0" : rows["StandardLeadTime"].ToString()),
                    CartID = Convert.ToString(rows["CartID"]),
                        Category1 = Convert.ToString(rows["Category1"]),
                        Category2 = Convert.ToString(rows["Category2"]),
                        UDF_ENVIRONMENTAL= Convert.ToDecimal(rows["UDF_ENVIRONMENTAL"]),
                        UDF_FREIGHT= Convert.ToDecimal(rows["UDF_FREIGHT"]),
                        UDF_FUEL= Convert.ToDecimal(rows["UDF_FUEL"]),
                        UDF_OTHER= Convert.ToDecimal(rows["UDF_OTHER"]),
                        UDF_PALLET= Convert.ToDecimal(rows["UDF_PALLET"]),
                        UDF_HNW_CLASSIFICATION_CODE = Convert.ToString(rows["UDF_HNW_CLASSIFICATION_CODE"]),
                    }).ToList();
                }
                return allProductList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataTable GetQuantity(string cartId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlCommand cmd = new SqlCommand("SELECT isnull(SUM(Quantity),0) FROM [cmis_portal_uat].[dbo].[ShoppingCart] WHERE CartID = '" + cartId + "'", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<CI_Item> GetProductListByItemCode(string itemText, string site_code, string site_desc)
        {
            try
            {
                DataTable dt = new DataTable();
                //SqlConnection con = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlCommand cmd = new SqlCommand("select ItemCode,ItemCodeDesc,SalesUnitOfMeasure,Category1,Category2,StandardUnitPrice,UDF_HNW_CLASSIFICATION_CODE from CI_Item where StandardUnitPrice>0 and ItemCode Like @itemText  and SUBSTRING([ItemCode], 1, 2) = 'HW' and SUBSTRING([ItemCode], 4, 4) LIKE  @Site ", con);
                //cmd.Parameters.AddWithValue("@searchText",searchText);
                //cmd.Parameters.AddWithValue("@itemText",ItemText);
                //cmd.Parameters.AddWithValue("@Site", site);
                //cmd.Parameters.AddWithValue("@searchText", string.Format("%{0}%", searchText));
                cmd.Parameters.AddWithValue("@itemText", string.Format("%{0}%", itemText));
                cmd.Parameters.AddWithValue("@Site", string.Format("%{0}%", site_code));
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var allProductList = new List<CI_Item>();
                if (dt.Rows.Count > 0)
                {
                    allProductList = dt.AsEnumerable().Select(rows => new CI_Item
                    {
                        ItemCode = Convert.ToString(rows["ItemCode"]),
                        ItemCodeDesc = Convert.ToString(rows["ItemCodeDesc"]),
                        SalesUnitOfMeasure = Convert.ToString(rows["SalesUnitOfMeasure"]),
                        Category1 = Convert.ToString(rows["Category1"]),
                        Category2 = Convert.ToString(rows["Category2"]),
                        StandardUnitPrice = Convert.ToDecimal(rows["StandardUnitPrice"]),
                        UDF_HNW_CLASSIFICATION_CODE = Convert.ToString(rows["UDF_HNW_CLASSIFICATION_CODE"]),
                    }).ToList();
                }
                return allProductList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public CI_Item GetProductDetails(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlCommand cmd = new SqlCommand("SELECT i.ExtendedDescriptionText,c.ItemCode,c.ItemCodeDesc,c.Category1,c.Category2,c.UDF_MANUFACT,c.StandardUnitPrice  FROM [MAS_CML].[dbo].[CI_ExtendedDescription] i, [MAS_CML].[dbo].[CI_Item] c WHERE i.ExtendedDescriptionKey = c.ExtendedDescriptionKey AND ItemCode Like @itemText", con);
                cmd.Parameters.AddWithValue("@itemText", string.Format("%{0}%", id));
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CI_Item ProductDetail = new CI_Item();
                if (dt.Rows.Count > 0)
                {
                    ProductDetail.ExtendedDescriptionKey = Convert.ToString(dt.Rows[0]["ExtendedDescriptionText"]);
                    ProductDetail.ItemCode = Convert.ToString(dt.Rows[0]["ItemCode"]);
                    ProductDetail.ItemCodeDesc = Convert.ToString(dt.Rows[0]["ItemCodeDesc"]);
                    ProductDetail.Category1 = Convert.ToString(dt.Rows[0]["Category1"]);
                    ProductDetail.Category2 = Convert.ToString(dt.Rows[0]["Category2"]);
                    ProductDetail.UDF_MANUFACT = Convert.ToString(dt.Rows[0]["UDF_MANUFACT"]);
                    ProductDetail.StandardUnitPrice = Convert.ToDecimal(dt.Rows[0]["StandardUnitPrice"]);
                }
                return ProductDetail;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
