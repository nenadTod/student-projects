using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace RentApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/Grade")]
    public class GradeController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public GradeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


       /* public IEnumerable<Grade> GetGrades()
        {
            return unitOfWork.Grades.GetAll();
        }*/


        [HttpGet]
        public int GetServiceGrade(int a, int s)
        {
            AppUser loggedUser = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name);
            if(loggedUser.Id != a)
            {
                return -1;
            }

            Grade grade = unitOfWork.Grades.FirstOrDefault(g => g.AppUserId == a && g.ServiceId == s);

            if (grade == null)
                return 0;
            return grade.GradeNum;
        }


        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(Grade))]
        public IHttpActionResult PostGrade(Grade grade)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            grade.AppUserId = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id;
            unitOfWork.Grades.Add(grade);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = grade.Id }, grade);
        }



        [HttpGet]
        public bool Permission(int p)
        {
            AppUser user=unitOfWork.AppUsers.GetActiveUser(User.Identity.Name);

            List<Branch> branches = new List<Branch>(unitOfWork.Branches.GetAll().Where(b => b.ServiceId ==p));

            //da li je user koji zeli nda postavi komentar rezervisao vec u nekom od branceva servisa (tj u servisu) koji zeli da kom.
            if (unitOfWork.Rents.GetAll().Where(c => c.UserId == user.Id && branches.Find(b => b.Id == c.ReturnBranchId) != null).Count() == 0)
            {
                return false;
            }        

            if (unitOfWork.Rents.GetAll().Where(c => c.UserId ==user.Id && c.ReturnBranch.ServiceId == p && DateTime.Compare((DateTime)c.End, (DateTime)DateTime.Now) < 0).Count() == 0)
            {
                return false;
            }

            return true;
        }


        private bool Exists(int id)
        {
            return unitOfWork.Grades.Get(id) != null;
        }
    }
}

