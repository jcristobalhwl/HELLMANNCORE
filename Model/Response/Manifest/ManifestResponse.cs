using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response.Manifest
{
    public class ManifestResponse
    {
        public IEnumerable<TBL_MAN_MANIFEST> Manifests { get; set; }
        public int INT_TOTALREGISTERS { get; set; }
        public int INT_CURRENTPAGE { get; set; }

    }
}
