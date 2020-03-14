using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response
{
    public class ManifestTest
    {
        public string VCH_DIRECTMASTERGUIDE { get; set; }
        public decimal DEC_MANIFESTSHIPDETDOCID { get; set; }

        public DateTime? DAT_DEPARTUREDATE { get; set; }

        public decimal NUM_MANIFESTID { get; set; }
        public int? VCH_DETAIL { get; set; }
        public string VCH_AIRGUIDE { get; set; }

    }
}
