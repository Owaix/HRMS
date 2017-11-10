using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace DataAccess.Models
{
    public class AcademicProfile : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int Emp_Id { get; set; }
        public String Institute { get; set; }
        public String Degree { get; set; }
        public String Year { get; set; }
        public Nullable<int> ImageId { get; set; }
        //       public HttpPostedFileWrapper ImageFile { get; set; }
    }
}