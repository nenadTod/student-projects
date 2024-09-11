using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository.Interface
{
    public interface IAppUserRepository : IRepository<AppUser,int>
    {
        IEnumerable<AppUser> GetAll(int pageIndex, int pageSize);

        AppUser GetFromUsername(string username);
    }
}
