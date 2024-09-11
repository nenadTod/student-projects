using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IUserRepository : IRepository<AppUser, int>
    {
        IEnumerable<AppUser> GetAll(int pageIndex, int pageSize);
        AppUser FirstOrDeafult(System.Linq.Expressions.Expression<Func<AppUser, bool>> predicate);
        IEnumerable<Rent> GetRents(int userId);
        void DeleteRents(int userId);
        AppUser GetUserInfo(string email);
        IEnumerable<AppUser> GetAll();
        IEnumerable<AppUser> GetManagers();
        AppUser Get(AppUser user);
    }
}