using Punchout.DAL;
using Punchout.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punchout.BAL
{
    public class BALProduct
    {
        DALProduct objDALProductList = new DALProduct();

        public List<CI_Item> GetProductList(string searchText, string ItemText, string siteId, string sitedesc)
        {
            return objDALProductList.GetProductList(searchText, ItemText, siteId, sitedesc);
        }

        public List<CI_Item> GetProductListByItemCode(string itemText, string site_code, string site_desc)
        {
            return objDALProductList.GetProductListByItemCode(itemText, site_code, site_desc);
        }

        public CI_Item GetProductDetails(string id)
        {
            return objDALProductList.GetProductDetails(id);
        }
    }
}
