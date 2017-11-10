using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Roles : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
