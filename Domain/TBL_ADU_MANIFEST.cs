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
    
    public partial class TBL_ADU_MANIFEST
    {
        public decimal NUM_MANIFESTID { get; set; }
        public string VCH_MANIFESTNUMBER { get; set; }
        public string VCH_TERMINAL { get; set; }
        public string VCH_AIRLINE { get; set; }
        public string VCH_SHIPMENTPORT { get; set; }
        public string VCH_FLIGHTNUMBER { get; set; }
        public Nullable<System.DateTime> DAT_DEPARTUREDATE { get; set; }
        public string CHR_REGIME { get; set; }
        public string CHR_VIA { get; set; }
        public Nullable<bool> BIT_COMPLETED { get; set; }
        public Nullable<System.DateTime> DAT_DOWNLOADDATE { get; set; }
    }
}