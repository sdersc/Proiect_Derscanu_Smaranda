using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberdasheryModel.Models
{
    public class Fabric
    {
            public int ID { get; set; }
            public string Type { get; set; }
            public string Color { get; set; }
            [Column(TypeName = "decimal(6, 2)")]
            public decimal Price { get; set; }
            public ICollection<Order> Orders { get; set; }
            public ICollection<FabricWeaved> FabricWeaved { get; set; }
    }
}
