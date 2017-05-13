using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgeRanger.Repository;
using AgeRanger.DbContext.Entities;
using AgeRanger.DbContext;
using System.Configuration;
using AgeRanger.Service.Contract;

namespace AgeRanger.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}