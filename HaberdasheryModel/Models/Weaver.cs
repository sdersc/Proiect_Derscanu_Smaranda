using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HaberdasheryModel.Models
{
    public class Weaver
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Weaver Name")]
        [StringLength(50)]
        public string WeaverName { get; set; }

        [StringLength(70)]
        public string Specialty { get; set; }
        public ICollection<FabricWeaved> FabricWeaved { get; set; }
    }
}
