using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

namespace DataAccess
{
    public class HrContext : DbContext
    {
        //public HrContext() : base(ConfigurationManager.ConnectionStrings["HRMSCOnStr"].ConnectionString)
        //{
        //    Database.SetInitializer<HrContext>(null);
        //}
        public DbSet<Employee> employee { get; set; }
        public DbSet<Features> feature { get; set; }
        public DbSet<Roles> role { get; set; }
        public DbSet<FeatureAccessConfig> FRConfig { get; set; }
        //public DbSet<AcademicProfile> Academic { get; set; }
    }
}
