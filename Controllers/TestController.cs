using System;
using Microsoft.AspNetCore.Mvc;
using AKK.Classes.Models;
using System.Linq;
using AKK.Classes.Models.Repository;

namespace AKK.Controllers {
    [Route("test")]
    public class TestController : Controller {
        
        IRepository<Route> db;
        IRepository<Grade> _g;
        IRepository<Section> _s;

        public TestController (IRepository<Route> context, IRepository<Grade> grades, IRepository<Section> sections)
        {
            db = context;
            _g = grades;
            _s = sections;
        }
        
        [HttpGet]
        public string Index() {
            Route r =  new Route {Name = "99", ColorOfHolds = new Color(255, 0, 0), Member = new Member {DisplayName = "Anton"}, Grade =_g.GetAll().FirstOrDefault(), CreatedDate = new DateTime(2016, 03, 24) };
            _s.GetAll().FirstOrDefault().Routes.Add(r);
            _s.Save();
            return db.Find(r.Id)?.Name;
        }
    }
} 