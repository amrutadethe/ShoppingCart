using PagedList;
using Punchout.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punchout.Web.Models
{
    public class ProductListViewModel
    {
       public IPagedList<CI_Item> productList {get;set;}

        public int totalProductCount { get; set; }

        public ProductListViewModel()
        {
            totalProductCount = 0;
        }
    }
}