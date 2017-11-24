using AutoMapper;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using EHRMS.Models;
using EHRMS.ViewModel;
using DataAccess.Repository;
using DataAccess.UOW;

namespace EHRMS.Controllers
{
    public class HomeController : Controller
    {
        UserManager<ApplicationUser> _user;
        private GenericUnitOfWork unitOfWork;
        private GenericRepository<FeatureAccessConfig> FeatureConfig;
        private GenericRepository<Roles> RolRep;
        private GenericRepository<Features> FeaRep;
        List<ExcelClient> ClientsList = new List<ExcelClient>
            {
               //  new ExcelClient ( "Adam",  "Bielecki",  DateTime.ParseExact("22/05/1986"),       "adamb@example.com" ),
                 new ExcelClient (  "George", "Smith",  DateTime.Parse("10/10/1990"),  "george@example.com" )
            };

        public HomeController(UserManager<ApplicationUser> user)
        {
            _user = user;
            unitOfWork = new GenericUnitOfWork(new HrContext());
            RolRep = unitOfWork.Repository<Roles>();
            FeaRep = unitOfWork.Repository<Features>();
            FeatureConfig = unitOfWork.Repository<FeatureAccessConfig>();
        }
        //  [Authorize]
        public ActionResult Index()
        {
            //UserManager.AddToRole(user.Id, model.Role);
            //var b = UserManager.FindById(User.Identity.GetUserId()).Role;
            //var role = UserManager.GetRoles(user.Id);

            return View();
        }
        [HttpPost]
        public ActionResult Index(String Name, String Mail, String Address)
        {
            return View();
        }

        //public async Task<Iaction> ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}
        public ActionResult AccessConfig()
        {
            HrContext db = new HrContext();
            FeatureRoles Fr = new FeatureRoles();
            var RolesModel = db.role.ToList();// RolRep.GetAll();
            Fr.Role = Mapper.Map<IEnumerable<RolesVM>>(RolesModel);
            var FeatModel = db.feature.ToList();//FeaRep.GetAll();
            Fr.Feature = Mapper.Map<IEnumerable<FeaturesVM>>(FeatModel);
            return View();
        }
        public ActionResult AccessConfigJson()
        {
            HrContext db = new HrContext();
            FeatureRoles Fr = new FeatureRoles();
            var RolesModel = db.role.ToList();// RolRep.GetAll();
            Fr.Role = Mapper.Map<IEnumerable<RolesVM>>(RolesModel);
            var FeatModel = db.feature.ToList();//FeaRep.GetAll();
            Fr.Feature = Mapper.Map<IEnumerable<FeaturesVM>>(FeatModel);
            return Json(Fr, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFeature()
        {
            var getFeatures = FeatureConfig.GetAll();
            return Json(getFeatures, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddFeature(AllAccessConfig FeatureRole)
        {
            for (int i = 1; i < FeatureRole.Feature.Length; i++)
            {
                var FRcon = FeatureConfig.FindBy(x => x.Feature_Id == FeatureRole.Feature[i] && x.Role_Id == FeatureRole.Role[i]).FirstOrDefault();
                if (FRcon != null)
                {
                    FRcon.Feature_Id = FeatureRole.Feature[i];
                    FRcon.Role_Id = FeatureRole.Role[i];
                    FRcon.IsCheck = FeatureRole.IsActive[i] == 1 ? true : false;
                }
                else
                {
                    FRcon = new FeatureAccessConfig();
                    FRcon.Feature_Id = FeatureRole.Feature[i];
                    FRcon.Role_Id = FeatureRole.Role[i];
                    FRcon.IsCheck = FeatureRole.IsActive[i] == 1 ? true : false;
                    FeatureConfig.Add(FRcon);
                }
            }
            unitOfWork.Save();
            return Json("Inserted");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View(ClientsList);
        }
        public void ExportClientsListToCSV()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"First Name\",\"Last Name\",\"DOB\",\"Email\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_Users.csv");
            Response.ContentType = "text/csv";

            foreach (var line in ClientsList)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                           line.FirstName,
                                           line.LastName,
                                           line.Dob,
                                           line.Email));
            }
            Response.Write(sw.ToString());
            Response.End();
        }
    }
    public class AllAccessConfig
    {
        public int[] Feature { get; set; }
        public int[] Role { get; set; }
        public int[] IsActive { get; set; }
    }
}