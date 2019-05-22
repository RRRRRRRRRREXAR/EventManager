using EventManager.DAL.Context;
using EventManager.DAL.Entities;
using EventManager.DAL.Interfaces;
using EventMangerBLL.DTO;
using EventMangerBLL.Interfaces;
using EventMangerBLL.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL
{
    public class EventService : IEventService
    {
        EventContext db { get; set; }
        IUnitOfWork Uow { get;set; }

        public EventService(EventContext context)
        {
            db = context;
        }

        public void CreateEvent(EventDTO item)
        {
            Uow.MongoEvents.Create(new MongoEvent { });
            throw new NotImplementedException();
        }

        public void Update(EventDTO item)
        {
            throw new NotImplementedException();
        }

        public void Delete(EventDTO item)
        {
            throw new NotImplementedException();
        }

        public void Comment(CommentDTO item)
        {
            throw new NotImplementedException();
        }

        public void UploadImages(IEnumerable<ImageDTO> items)
        {

            foreach (var element in items)
            {
                Uow.Images.Create(new Image {Name=element.Name,Content=SaveImage.Save(element.Content),Id=element.Id,EventId=element.EventId });
            }
            Uow.Save();
        }

        public EventDTO GetItem(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventDTO> GetEvents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventDTO> Find(Func<EventDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
