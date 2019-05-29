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
        IUnitOfWork Uow { get;set; }

        public EventService(IUnitOfWork work)
        {
            Uow = work;
        }

        public void CreateEvent(EventDTO item)
        {
            Uow.MongoEvents.Create(new MongoEvent { Description= item.Description, Location=item.Location,Name= item.Name,ShortDescription= item.ShortDescription,EfEventId=item.Id,UserId=item.UserId });
            Uow.Events.Create(new Event {Description= item.Description,ShortDescription= item.ShortDescription, EventTypeId= item.EventTypeId, Name= item.Name, UserId= item.UserId });
            Func<Event, bool> f = d=>d.Name==item.Name && d.ShortDescription==item.ShortDescription&& d.UserId== item.UserId && d.Description== item.Description;
            var currentEvent = Uow.Events.Find(f).First();
            for (int i =0;i<item.Images.Count(); i++)
            {
                item.Images.ElementAt(i).EventId = currentEvent.Id;
            }
            UploadImages(item.Images);
            
        }

        public void Update(EventDTO item)
        {
            Uow.Events.Update(new Event {Id=item.Id, Description = item.Description, EventTypeId= item.EventTypeId, Name= item.Name, ShortDescription=item.ShortDescription, UserId= item.UserId });
            Uow.MongoEvents.Update(new MongoEvent {Id= item.MongoId, UserId= item.UserId, Description = item.Description, EfEventId= item.Id, Location=item.Location, Name=item.Name, ShortDescription = item.ShortDescription });
        }

        public void Delete(EventDTO item)
        {
            Uow.Events.Delete(item.Id.ToString());
            Uow.MongoEvents.Delete(item.MongoId);
        }

        public void Comment(CommentDTO item)
        {
            Uow.Comments.Create(new EventManager.DAL.Entities.Comment { Id=item.Id, EventId = item.EventId, Text= item.Text, UserId= item.UserId });
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
            if (id!=null)
            {
                var item = Uow.Events.Get(id.Value);
                var mongoitem = Uow.MongoEvents.Get(id.Value);
                Func<Image, bool> func = d => d.EventId == item.Id;
                var imgs = Find(func);

                return new EventDTO {MongoId=mongoitem.Id ,Id = item.Id, Description = item.Description, EventTypeId = item.EventTypeId, Images = imgs, Location = mongoitem.Location, Name = item.Name, ShortDescription = item.ShortDescription, UserId = item.UserId };
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<EventDTO> GetEvents()
        {
            var items = Uow.Events.GetAll();

            List<EventDTO> events = new List<EventDTO>();
            foreach (var item in items)
            {
                Func<MongoEvent, bool> f = d => d.EfEventId == item.Id && d.UserId == item.UserId;
                Func<Image, bool> imagefinder = d => d.EventId == item.Id;
                var MongoItem = Uow.MongoEvents.Find(f).First();
                var imgs = Find(imagefinder);
                events.Add(new EventDTO { MongoId = MongoItem.Id, Id = item.Id, Description = item.Description, EventTypeId = item.EventTypeId, Location = MongoItem.Location, UserId = item.UserId, Name = item.Name, ShortDescription = item.ShortDescription, Images = imgs });
            }
            return events;
        }
        
        public IEnumerable<ImageDTO> Find(Func<Image, bool> predicate)
        {
            var items = Uow.Images.Find(predicate);
            List<ImageDTO> im = new List<ImageDTO>();
            foreach (var e in items)
            {
                im.Add(new ImageDTO {Id=e.Id, Link = e.Content, Name= e.Name, EventId= e.EventId });
            }
            return im;
        }

        public IEnumerable<EventDTO> Find(Func<Event, bool> predicate)
        {
            var items = Uow.Events.Find(predicate);
            
            List<EventDTO> events = new List<EventDTO>();
            foreach (var item in items)
            {
                Func<MongoEvent, bool> f = d => d.EfEventId == item.Id && d.UserId==item.UserId;
                Func<Image, bool> imagefinder = d => d.EventId == item.Id;
                var MongoItem = Uow.MongoEvents.Find(f).First();
                var imgs = Find(imagefinder);
                events.Add(new EventDTO { MongoId = MongoItem.Id, Id = item.Id, Description = item.Description, EventTypeId = item.EventTypeId, Location = MongoItem.Location, UserId=item.UserId, Name = item.Name, ShortDescription = item.ShortDescription, Images = imgs});
            }
            return events;
        }

        public void Dispose()
        {
            Uow.Dispose();
        }

        public void Subscribe(EventDTO item, int UserId)
        {
            Uow.Subscriptions.Create(new Subscription {EventId=item.Id,UserId=UserId });
        }

        public void Unsubscribe(EventDTO item,int UserId)
        {
            Func<Subscription, bool> f = d => d.UserId ==UserId && d.EventId==item.Id;
            var sub = Uow.Subscriptions.Find(f).First();
            Uow.Subscriptions.Delete(sub.Id.ToString());
        }
        
        public IEnumerable<EventDTO> GetSubcriptions(int UserId)
        {
            Func<Subscription, bool> f = d => d.UserId == UserId;
            var subs= Uow.Subscriptions.Find(f);
            List<EventDTO> evnts = new List<EventDTO>();
            foreach (var element in subs)
            {
                evnts.Add(GetItem(element.EventId));
            }
            return evnts;
        }
    }
}
