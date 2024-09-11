using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;
using RentApp.Services;

namespace RentApp.Persistance.Repository.Implementations
{
    public class AppUserRepository : Repository<AppUser, int>, IAppUserRepositroy
    {
        
        public AppUserRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<AppUser> GetRange(int pageIndex, int size)
        {
            return Context.AppUsers.Skip((pageIndex - 1) * size).Take(size);
        }

        public IEnumerable<AppUser> GetAllUnapprovedUsers()
        {
            return Context.AppUsers.Where(u => !string.IsNullOrEmpty(u.ImagePath) && !u.Approved);
        }

        public void ApproveUser(AppUser user)
        {
            var u = Context.AppUsers.FirstOrDefault(s => s.Id == user.Id);
            if (u != null && !u.Approved)
            {
                u.Approved = true;
            }
            Context.Entry(u).State = EntityState.Modified;
            SendMailToUserId(user.Id);
            Context.SaveChanges();
        }

        private void SendMailToUserId(int userId)
        {
            var u = Context.Users.FirstOrDefault(user => user.AppUser.Id == userId);
            if (u == null) return;
            const string subject = "Odobren nalog";
            const string content = "Postovani, \nVas nalog odobren je od strane administratora i sada ste u mogucnosti da koristite funkcionalnosti aplikacije.\nSrdacno,\nRent-a-car tim";
            smtp.SendMail(subject, content, u.Email);
        }

        public IEnumerable<RAIdentityUser> GetAllManagers()
        {
            var managers = Context.Users.Where(u => u.Roles.Any(r => r.RoleId.Equals("6b7ff08d-7607-498a-8bb3-d71a60c9588d"))).Include(i=>i.AppUser).ToList();
            if (managers.Count == 0) return managers;
            //izbaci mi one menadzere koji su vec blokirani
            foreach (var manager in managers.ToList()) //mora i ovde toList, tako ne izbacuje exception
            {
                if (manager.AppUser.Blocked)
                {
                    managers.Remove(manager);
                }
            }
            return managers;
        }

        public IdentityUser GetUserDetails(string username)
        {
            var user = Context.Users.Where(v => v.Id.Equals(username)).Include(i=>i.AppUser).FirstOrDefault();
            return user;

        }

        public void BlockManager(AppUser manager)
        {
            var blocked = Context.AppUsers.FirstOrDefault(s => s.Id == manager.Id);
            if (blocked != null && blocked.Blocked) return;
            if (blocked != null) blocked.Blocked = true;
            Context.Entry(blocked).State = EntityState.Modified;
        }

        public void UploadImage(string img, int id)
        {
            var user = Context.AppUsers.FirstOrDefault(u => u.Id == id);
            user.ImagePath = img;
            Context.Entry(user).State = EntityState.Modified;
            Context.SaveChanges();
        }

        protected RADBContext Context => context as RADBContext;
        private SmtpService smtp = new SmtpService();
    }
}