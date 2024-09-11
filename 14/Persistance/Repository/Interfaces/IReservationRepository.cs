using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation,int>
    {
        IEnumerable<Reservation> GetAll(int pageIndex, int pageSize);
    }
}
