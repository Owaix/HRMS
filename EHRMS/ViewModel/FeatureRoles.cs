using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHRMS.ViewModel
{
    public class FeatureRoles
    {
        public IEnumerable<RolesVM> Role { get; set; }
        public IEnumerable<FeaturesVM> Feature { get; set; }
    }
}