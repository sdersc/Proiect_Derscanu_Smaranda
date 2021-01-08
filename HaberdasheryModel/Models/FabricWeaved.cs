using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaberdasheryModel.Models
{
    public class FabricWeaved
    {
        public int WeaverID { get; set; }
        public int FabricID { get; set; }
        public Weaver Weaver { get; set; }
        public Fabric Fabric { get; set; }
    }
}
