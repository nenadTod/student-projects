using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace RentApp.Persistance.Repository
{
    public class UserRepository : Repository<AppUser, int>, IUserRepository
    {
        protected RADBContext Context { get { return context as RADBContext; } }
        public UserRepository(DbContext context) : base(context)
        {

        }

        public AppUser FirstOrDeafult(Expression<Func<AppUser, bool>> predicate)
        {
            return context.Set<AppUser>().FirstOrDefault(predicate);
        }

        public IEnumerable<AppUser> GetAll()
        {
            var users = Context.AppUsers.Where(x=>x.Activated==false);  // neodobreni
            return users.Where(x => x.Manage == false);
        }

        public IEnumerable<Rent> GetRents(int userId)
        {
            return Context.Rents.Where(x => x.User.Id == userId).ToList();
        }

        public void DeleteRents(int userId)
        {
            List<Rent> rents = Context.Rents.Where(x => x.User.Id == userId).ToList();
            foreach(var r in rents)
            {
                if(!r.Used)
                {
                    Context.Rents.Remove(r);
                }
            }
        }
        public AppUser GetUserInfo(string userName)
        {
            RAIdentityUser user = Context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return Context.AppUsers.Where(u => u.Email == user.Email).FirstOrDefault();
        }

        public IEnumerable<AppUser> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public AppUser Get(AppUser user)
        {
            return Context.AppUsers.Where(x => x.Id == user.Id).FirstOrDefault();
        }

        public IEnumerable<AppUser> GetManagers()
        {
            return Context.AppUsers.Where(x => x.Manage == true);
        }
    }
}