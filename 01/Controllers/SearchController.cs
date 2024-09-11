using RentApp.Models;
using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RentApp.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public SearchController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IEnumerable<Vehicle> Filter(Search search)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            if (search == null)
                vehicles = unitOfWork.Vehicles.GetAll().ToList();
            else
                vehicles = unitOfWork.Vehicles.Filter(search).ToList();

            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Type = unitOfWork.TypeOfVehicles.Get(vehicles[i].TypeId);
                vehicles[i].Branch = unitOfWork.Branches.Get((int)vehicles[i].BranchId);
            }

            return vehicles;
        }

        [HttpGet]
        public IEnumerable<Vehicle> Search(string search)
        {
            List<Vehicle> vehicles = new List<Vehicle>(unitOfWork.Vehicles.Search(search));
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Type = unitOfWork.TypeOfVehicles.Get(vehicles[i].TypeId);
                vehicles[i].Branch = unitOfWork.Branches.Get((int)vehicles[i].BranchId);
            }
            return vehicles;

        }

    }
}