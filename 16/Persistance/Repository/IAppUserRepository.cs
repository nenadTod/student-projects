using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IAppUserRepository : IRepository<AppUser, int>
    {
        IEnumerable<AppUser> GetAll(int pageIndex, int pageSize);
        IEnumerable<AppUser> GetUnbannedManagers();
        IEnumerable<AppUser> GetBannedManagers();
        IEnumerable<AppUser> GetAwaitingClients();
        bool NotifyViaEmail(string targetEmail, string subject, string body);
        JwtSecurityToken DecodeJwt(string protectedToken);
        bool UserExistsByEmail(string email);
    }
}
