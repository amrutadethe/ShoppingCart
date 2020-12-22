using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Punchout.DAL;
using Punchout.Entities;
namespace Punchout.BAL
{
    public class BALProductDetails
    {
        DALProductDetails objDALProductDetails = new DALProductDetails();

        public CI_Item GetProductDetails(string id)
        {
            return objDALProductDetails.GetProductDetails(id);
        }
    }
}
