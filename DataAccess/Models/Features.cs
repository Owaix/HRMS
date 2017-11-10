using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Features : BaseEntity
    {
        [Key]
        public int id { get; set; }
        public String Name { get; set; }
        public String Class { get; set; }
        public Nullable<int> MenuId { get; set; }
    }
}
