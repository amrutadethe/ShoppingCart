using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punchout.Web.Models
{
    public class hw_sites
    {
        public string site_code { get; set; }
        public string site_desc { get; set; }
        public List<hw_sites> siteinfo { get; set; }

       
    }
}