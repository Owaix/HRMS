using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRMS.ViewModel
{
    public class RolesVM
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
