//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_MAN_MANIFEST
    {
        public int INT_MANIFESTID { get; set; }
        public Nullable<int> INT_DAY { get; set; }
        public Nullable<int> INT_MONTH { get; set; }
        public Nullable<int> INT_YEAR { get; set; }
        public Nullable<int> INT_WEEK { get; set; }
        public string VCH_AIRLINE { get; set; }
        public string VCH_FLIGHT { get; set; }
        public string VCH_AIR_GUIDE { get; set; }
        public string VCH_MASTERGUIDE { get; set; }
        public string VCH_DESCRIPTION { get; set; }
        public Nullable<int> INT_TERMINALCODE { get; set; }
        public Nullable<decimal> DEC_WEIGHTORIGIN { get; set; }
        public Nullable<decimal> DEC_ORIGINPACKAGES { get; set; }
        public Nullable<decimal> DEC_MANIFESTWEIGHT { get; set; }
        public Nullable<decimal> DEC_MANIFESTPACKAGES { get; set; }
        public Nullable<decimal> DEC_WEIGHTRECEIVED { get; set; }
        public Nullable<decimal> DEC_PACKAGESRECEIVED { get; set; }
        public string VCH_CONSIGNEE { get; set; }
        public Nullable<System.DateTime> DTM_TRANSMISSION_DATE { get; set; }
        public string VCH_ANOTHERAGENT { get; set; }
        public string VCH_DESTINATION { get; set; }
        public string VCH_SHIPPER { get; set; }
    }
}
