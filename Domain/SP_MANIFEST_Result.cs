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
    
    public partial class SP_MANIFEST_Result
    {
        public int INT_MANIFESTID { get; set; }
        public Nullable<int> INT_DAY { get; set; }
        public Nullable<int> INT_MONTH { get; set; }
        public Nullable<int> INT_YEAR { get; set; }
        public Nullable<int> INT_WEEK { get; set; }
        public string VCH_AIRLINE { get; set; }
        public string VCH_FLIGHTNUMBER { get; set; }
        public string VCH_AIRGUIDE { get; set; }
        public string VCH_DIRECTMASTERGUIDE { get; set; }
        public string VCH_DESCRIPTION { get; set; }
        public string VCH_TERMINALCODE { get; set; }
        public Nullable<decimal> DEC_WEIGHTORIGIN { get; set; }
        public Nullable<decimal> DEC_PACKAGEORIGIN { get; set; }
        public Nullable<decimal> DEC_MANIFESTEDWEIGHT { get; set; }
        public Nullable<decimal> DEC_MANIFESTEDPACKAGE { get; set; }
        public Nullable<decimal> DEC_WEIGHTRECEIVED { get; set; }
        public Nullable<decimal> DEC_PACKAGERECEIVED { get; set; }
        public string VCH_CONSIGNEE { get; set; }
        public Nullable<System.DateTime> DAT_DATETRANSMISSIONDOCUMENT { get; set; }
        public string VCH_ANOTHERAGENT { get; set; }
        public string VCH_DESTINATION { get; set; }
        public string VCH_SHIPPER { get; set; }
        public string VCH_ORIGIN { get; set; }
        public Nullable<System.DateTime> DAT_DEPARTUREDATE { get; set; }
        public string VCH_MANIFESTNUMBER { get; set; }
    }
}
