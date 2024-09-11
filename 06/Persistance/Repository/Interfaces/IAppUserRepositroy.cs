using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;

namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IAppUserRepositroy : IRepository<AppUser, int>
    {
        IEnumerable<AppUser> GetRange(int pageIndex, int size);
        IEnumerable<AppUser> GetAllUnapprovedUsers();
        void ApproveUser(AppUser user);
        IEnumerable<RAIdentityUser> GetAllManagers();
        IdentityUser GetUserDetails(string username);
        void BlockManager(AppUser manager);

        void UploadImage(string s, int id);
    }
}