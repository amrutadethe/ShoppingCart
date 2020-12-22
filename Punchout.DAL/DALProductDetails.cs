using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Punchout.Entities;
using System.Data;
using System.Data.SqlClient;
namespace Punchout.DAL
{
    public class DALProductDetails
    {
        

        public CI_Item GetProductDetails(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MAS_CML;User ID=sa;Password=pass123!@#");
                SqlCommand cmd = new SqlCommand("SELECT i.ExtendedDescriptionText,c.ItemCode,c.ItemCodeDesc,c.Category1,c.Category2  FROM [MAS_CML].[dbo].[CI_ExtendedDescription] i, [MAS_CML].[dbo].[CI_Item] c WHERE i.ExtendedDescriptionKey = c.ExtendedDescriptionKey AND ItemCode Like @itemText", con);
                cmd.Parameters.AddWithValue("@itemText", string.Format("%{0}%", id));
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CI_Item ProductDetail = new CI_Item();
                ProductDetail.ExtendedDescriptionKey = Convert.ToString(dt.Rows[0]["ExtendedDescriptionText"]);
                ProductDetail.ItemCode= Convert.ToString(dt.Rows[0]["ItemCode"]);
                ProductDetail.ItemCodeDesc = Convert.ToString(dt.Rows[0]["ItemCodeDesc"]);
                ProductDetail.Category1 = Convert.ToString(dt.Rows[0]["Category1"]);
                ProductDetail.Category2 = Convert.ToString(dt.Rows[0]["Category2"]);
                return ProductDetail;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
