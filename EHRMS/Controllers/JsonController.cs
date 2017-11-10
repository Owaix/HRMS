using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EHRMS.Controllers
{
    public class JsonController : ApiController
    {
        Employees[] employees = new Employees[]{
         new Employees { ID = 1, Name = "Mark", JoiningDate =
            DateTime.Parse(DateTime.Today.ToString()), Age = 30 },
         new Employees { ID = 2, Name = "Allan", JoiningDate =
            DateTime.Parse(DateTime.Today.ToString()), Age = 35 },
         new Employees { ID = 3, Name = "Johny", JoiningDate =
            DateTime.Parse(DateTime.Today.ToString()), Age = 21 }
      };
        [HttpGet]
        public IEnumerable<Employees> GetAllEmployees()
        {
            return employees;
            //   return JsonConvert.SerializeObject(employees);
        }
        [HttpGet]
        public Employees GetAllEmployees(int id)
        {
            return employees.Where(x => x.ID == id).FirstOrDefault();
            //   return JsonConvert.SerializeObject(employees);
        }
        [HttpGet]

        public IEnumerable<Employees> GetAll()
        {
            return employees;
            //   return JsonConvert.SerializeObject(employees);
        }
    }
    public class Employees
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime JoiningDate { get; set; }
        public int Age { get; set; }
    }
}
