using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Request.Manifest
{
    public class ManifestRequest
    {
        public DateTime? DAT_STARTDATE { get; set; }
        public DateTime? DAT_ENDDATE{ get; set; }
        public string VCH_DIRECTMASTERGUIDE { get; set; }
        public string VCH_CONSIGNEE { get; set; }
        public string VCH_SHIPPER { get; set; }
        public string VCH_DESCRIPTION { get; set; }
        public string VCH_AIRLINE { get; set; }
        public string VCH_DESTINATION { get; set; }
        public int? INT_STARTWEEK { get; set; }
        public int? INT_ENDWEEK { get; set; }
        public int? INT_YEAR { get; set; }
        public int INT_CURRENTPAGE { get; set; } = 1;
        public int INT_LIMITPAGES { get; set; } = 25;


    }
}
