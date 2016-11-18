using DataTables.Queryable.Samples.Database;
using DataTables.Queryable.Samples.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace DataTables.Queryable.Samples.Controllers
{
    public class WebUIController : Controller
    {
        public ActionResult Sample1()
        {
            return View();
        }

        public ActionResult Sample2()
        {
            return View();
        }

        public ActionResult Sample3()
        {
            return View();
        }
    }
}