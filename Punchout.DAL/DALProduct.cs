using Punchout.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punchout.DAL
{
    public class DALProduct
    {
         string cmis_portal = ConfigurationSettings.AppSettings["cmis_portal"].ToString();
         string MAS_CML = ConfigurationSettings.AppSettings["MAS_CML"].ToString();

        MAS_CMLEntities objMASCMLEntities = new MAS_CMLEntities();
        /// <summary>
        /// Get All Product List for selected site or Entered Itemcode or Item desc
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="ItemText"></param>
        /// <param name="siteId"></param>
        /// <param name="sitedesc"></param>
        /// <returns></returns>
        public List<CI_Item> GetProductList(string searchText, string ItemText, string siteId, string sitedesc)
        {
            
            try
            {
                DataTable dt = new DataTable();
                
                SqlConnection con = new SqlConnection(MAS_CML);
                SqlCommand cmd = new SqlCommand("select ItemCode,ItemCodeDesc,SalesUnitOfMeasure,Category1,Category2,StandardUnitPrice,UDF_HNW_CLASSIFICATION_CODE from CI_Item where StandardUnitPrice>0 and ItemCodeDesc LIKE @searchText and ItemCode Like @itemText and SUBSTRING([ItemCode], 1, 2) = 'HW' and SUBSTRING([ItemCode], 4, 4) LIKE  @Site order by ItemCodeDesc ", con);
      
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
        /// <summary>
        /// Get My Shopping cart List.show Cart added Item details 
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public List<ViewCart> GetMyShoopingCartList(string cartId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(cmis_portal);
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
                        UDF_ENVIRONMENTAL=decimal.Parse((rows["UDF_ENVIRONMENTAL"]==null || rows["UDF_ENVIRONMENTAL"].ToString()==string.Empty)?"0" : rows["UDF_ENVIRONMENTAL"].ToString()),
                        UDF_FREIGHT = decimal.Parse((rows["UDF_FREIGHT"] == null || rows["UDF_FREIGHT"].ToString() == string.Empty) ? "0" : rows["UDF_FREIGHT"].ToString()),
                        UDF_FUEL = decimal.Parse((rows["UDF_FUEL"] == null || rows["UDF_FUEL"].ToString() == string.Empty) ? "0" : rows["UDF_FUEL"].ToString()),
                        UDF_OTHER = decimal.Parse((rows["UDF_OTHER"] == null || rows["UDF_OTHER"].ToString() == string.Empty) ? "0" : rows["UDF_OTHER"].ToString()),
                        UDF_PALLET = decimal.Parse((rows["UDF_PALLET"] == null || rows["UDF_PALLET"].ToString() == string.Empty) ? "0" : rows["UDF_PALLET"].ToString()),
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
        /// <summary>
        /// Get Total Cart Quantity
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public DataTable GetQuantity(string cartId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(MAS_CML);
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
        /// <summary>
        /// Get Product List For Selected site
        /// </summary>
        /// <param name="itemText"></param>
        /// <param name="site_code"></param>
        /// <param name="site_desc"></param>
        /// <returns></returns>
        public List<CI_Item> GetProductListByItemCode(string itemText, string site_code, string site_desc)
        {
            try
            {
                DataTable dt = new DataTable();
                
                SqlConnection con = new SqlConnection(MAS_CML);
                SqlCommand cmd = new SqlCommand("select ItemCode,ItemCodeDesc,SalesUnitOfMeasure,Category1,Category2,StandardUnitPrice,UDF_HNW_CLASSIFICATION_CODE from CI_Item where StandardUnitPrice>0  and SUBSTRING([ItemCode], 1, 2) = 'HW' and SUBSTRING([ItemCode], 4, 4) LIKE  @Site order by ItemCodeDesc", con);

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
        /// <summary>
        /// Get Product Details for  selected Item Code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CI_Item GetProductDetails(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(MAS_CML);
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
