using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaberdasheryModel.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int FabricID { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }
        public Fabric Fabric { get; set; }
    }
}
