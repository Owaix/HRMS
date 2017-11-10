using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class FeatureAccessConfig : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int Role_Id { get; set; }
        public int Feature_Id { get; set; }
        public bool IsCheck { get; set; }
    }
}
