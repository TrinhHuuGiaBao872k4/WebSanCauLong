using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSanCauLong.Models;
using WebSanCauLong.Models.ViewModels;

namespace WebSanCauLong.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataModel _dataModel = new DataModel();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}