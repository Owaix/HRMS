using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Employee : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Department { get; set; }
        public String Designation { get; set; }
        public Nullable<float> Salary { get; set; }
        public Nullable<int> Age { get; set; }
        public bool IsActive { get; set; }
    }
}
