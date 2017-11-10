using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHRMS.ViewModel
{
    public class AcademicProfileVM : BaseEntity
    {
        public int Id { get; set; }
        public int Emp_Id { get; set; }
        public String Institute { get; set; }
        public String Degree { get; set; }
        public String Year { get; set; }
        public Nullable<int> ImageId { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}