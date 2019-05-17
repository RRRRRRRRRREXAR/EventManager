using EventMangerBLL.DTO;
using EventMangerBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL
{
    public class EventService : IService<EventDTO>
    {
        DbContext db { get; set; }
        public EventService(DbContext context)
        {
            db = context;
        }
        public void Delete(EventDTO item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventDTO> Find(Func<EventDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public EventDTO GetItem(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventDTO> GetPhones()
        {
            throw new NotImplementedException();
        }

        public void Make(EventDTO item)
        {
            throw new NotImplementedException();
        }

        public void Update(EventDTO item)
        {
            throw new NotImplementedException();
        }
    }
}
