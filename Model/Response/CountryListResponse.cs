using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response
{
    public class CountryListResponse
    {
        public int INT_COUNTRYID { get; set; }
        public string VCH_FREIGHTCODE { get; set; }
        public string VCH_COUNTRYNAME { get; set; }
        public string VCH_CURRENCYNAME { get; set; }
    }
}
