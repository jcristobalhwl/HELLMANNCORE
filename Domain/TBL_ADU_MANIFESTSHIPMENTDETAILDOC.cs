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
    
    public partial class TBL_ADU_MANIFESTSHIPMENTDETAILDOC
    {
        public Nullable<decimal> NUM_MANIFESTID { get; set; }
        public string VCH_AIRGUIDE { get; set; }
        public string VCH_DIRECTMASTERGUIDE { get; set; }
        public Nullable<int> VCH_DETAIL { get; set; }
        public string VCH_TERMINALCODE { get; set; }
        public Nullable<decimal> DEC_WEIGHTORIGIN { get; set; }
        public Nullable<int> INT_PACKAGEORIGIN { get; set; }
        public Nullable<decimal> DEC_WEIGHTRECEIVED { get; set; }
        public Nullable<int> INT_PACKAGERECEIVED { get; set; }
        public string VCH_CONSIGNEE { get; set; }
        public Nullable<System.DateTime> DAT_DATETRANSMISSIONDOCUMENT { get; set; }
        public string VCH_DESCRIPTION { get; set; }
        public Nullable<decimal> DEC_MANIFESTEDWEIGHT { get; set; }
        public Nullable<int> INT_MANIFESTEDPACKAGE { get; set; }
        public string VCH_SHIPPER { get; set; }
        public decimal DEC_MANIFESTSHIPDETDOCID { get; set; }
        public Nullable<bool> BIT_COMPLETED { get; set; }
    }
}
