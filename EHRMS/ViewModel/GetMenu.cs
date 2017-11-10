using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHRMS.ViewModel
{
    public class GetMenu
    {
        public int Id { get; set; }
        public String Menu { get; set; }
        public String Class { get; set; }
        public SubMenu Submenu { get; set; }
    }

    public class SubMenu
    {
        public int Menu_Id { get; set; }
        public String Menu { get; set; }
        public String Class { get; set; }
    }
}