using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRMS.ViewModel
{
    public class FeaturesVM
    {
        [Key]
        public int id { get; set; }
        public String Name { get; set; }
        public Nullable<int> SubMenuId { get; set; }
    }
}
