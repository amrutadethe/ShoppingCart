//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Punchout.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewCart
    {
        public string ItemCode { get; set; }
        public string UDF_CHEM_CATEGORY { get; set; }
        public string ItemCodeDesc { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public int Quantity { get; set; }
        public string SalesUnitOfMeasure { get; set; }
        public Nullable<decimal> StandardLeadTime { get; set; }
        public string CartID { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public Nullable<decimal> UDF_ENVIRONMENTAL { get; set; }
        public Nullable<decimal> UDF_FREIGHT { get; set; }
        public Nullable<decimal> UDF_FUEL { get; set; }
        public Nullable<decimal> UDF_OTHER { get; set; }
        public Nullable<decimal> UDF_PALLET { get; set; }
        public string UDF_HNW_CLASSIFICATION_CODE { get; set; }
    }
}
