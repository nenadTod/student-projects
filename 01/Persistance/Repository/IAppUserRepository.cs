using RentApp.Models.Entities;
using System.Collections.Generic;

namespace RentApp.Persistance.Repository
{
    public interface IAppUserRepository : IRepository<AppUser, int>
    {
        AppUser GetActiveUser(string username);
    }
}
