using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaberdasheryModel.Models;

namespace Proiect_Derscanu_Smaranda.Models.HaberdasheryViewModels
{
    public class WeaverIndexData
    {
        public IEnumerable<Weaver> Weavers { get; set; }
        public IEnumerable<Fabric> Fabrics { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
