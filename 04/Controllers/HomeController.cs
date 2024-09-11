using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentApp.Controllers
{
    public class HomeController : Controller
    {
        IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            //AppUser user = new AppUser();
            //user.Birthday = DateTime.Now;
            //user.FullName = "pero peric";
            //user.LastName = "peric";

            //unitOfWork.AppUsers.Add(user);
            //unitOfWork.Complete();
            return View();
        }
    }
}
