using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentApp.Models.Entities;
using RentApp.Persistance.Repository;
namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IAppUserRepository: IRepository<User,int>
    {
        IEnumerable<User> GetAll(int pageIndex, int pageSize);
    }
}
