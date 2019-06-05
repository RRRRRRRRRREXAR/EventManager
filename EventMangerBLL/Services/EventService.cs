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
            Uow.Events.Create(new Event {Description= item.Description,ShortDescription= item.ShortDescription, EventTypeId= item.EventTypeId, Name= item.Name, UserId= item.UserId });
            Func<Event, bool> f = d=>d.Name==item.Name && d.ShortDescription==item.ShortDescription&& d.UserId== item.UserId && d.Description== item.Description;
            var currentEvent = Uow.Events.Find(f).First();
            Uow.MongoEvents.Create(new MongoEvent {Description = item.Description, Name = item.Name, ShortDescription = item.ShortDescription, EfEventId = currentEvent.Id, UserId = item.UserId, Lat=item.Lat, Lng=item.Lng });
            for (int i =0;i<item.Images.Count(); i++)
            {
                item.Images.ElementAt(i).EventId = currentEvent.Id;
            }
            UploadImages(item.Images);
            
        }

        public void Update(EventDTO item)
        {
            Uow.Events.Update(new Event {Id=item.Id, Description = item.Description, EventTypeId= item.EventTypeId, Name= item.Name, ShortDescription=item.ShortDescription, UserId= item.UserId });
            Uow.MongoEvents.Update(new MongoEvent {Id= item.MongoId, UserId= item.UserId, Description = item.Description, EfEventId= item.Id, Name=item.Name, ShortDescription = item.ShortDescription, Lat=item.Lat, Lng=item.Lng });
        }

        public void Delete(EventDTO item)
        {
            Uow.Events.Delete(item.Id.ToString());
            Uow.MongoEvents.Delete(item.MongoId);
            Func<Comment, bool> commById = d => d.EventId == item.Id;
            var comments = Uow.Comments.Find(commById);
            foreach (var com in comments)
            {
                Uow.Comments.Delete(com.Id.ToString());
            }
            Func<Subscription, bool> subById = d => d.EventId == item.Id;
            var subscriptions = Uow.Subscriptions.Find(subById);
            foreach (var sub in subscriptions)
            {
                Uow.Subscriptions.Delete(sub.Id.ToString());
            }
            Func<Image, bool> ImageById = d => d.EventId == item.Id;
            var images = Uow.Images.Find(ImageById);
            foreach (var img in images)
            {
                Uow.Images.Delete(img.Id.ToString());
            }
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
            if (id != null)
            {
                var item = Uow.Events.Get(id.Value);
                Func<MongoEvent, bool> findMongoId = d => d.EfEventId == id.Value;
                var mongoitem = Uow.MongoEvents.Find(findMongoId).First();
                Func<Image, bool> func = d => d.EventId == item.Id;
                var imgs = Find(func);

                return new EventDTO { Lat=mongoitem.Lat, Lng=mongoitem.Lng,MongoId=mongoitem.Id ,Id = item.Id, Description = item.Description, EventTypeId = item.EventTypeId, Images = imgs, Name = item.Name, ShortDescription = item.ShortDescription, UserId = item.UserId };
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
                events.Add(new EventDTO { MongoId = MongoItem.Id, Id = item.Id, Description = item.Description, EventTypeId = item.EventTypeId,UserId = item.UserId, Name = item.Name, ShortDescription = item.ShortDescription, Images = imgs, Lat = MongoItem.Lat, Lng = MongoItem.Lng });
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
                events.Add(new EventDTO { MongoId = MongoItem.Id, Id = item.Id, Description = item.Description, EventTypeId = item.EventTypeId, UserId=item.UserId, Name = item.Name, ShortDescription = item.ShortDescription, Images = imgs, Lat = MongoItem.Lat, Lng = MongoItem.Lng });
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

        public IEnumerable<CommentDTO> GetComments(int EventId)
        {
            Func<Comment, bool> f = d => d.EventId == EventId;
            var items= Uow.Comments.Find(f);
            List<CommentDTO> comments = new List<CommentDTO>();
            foreach (var item in items)
            {
                comments.Add(new CommentDTO {Id=item.Id, EventId=item.EventId, Text=item.Text, UserId=item.UserId });
            }
            return comments;
        }

        public UserDTO GetOwner(int id)
        {
            var owner = Uow.Users.Get(id);
            return new UserDTO { Id = owner.Id, Email = owner.Email, FirstName = owner.FirstName, IsEmailConfirmed = owner.IsEmailConfirmed, LastName = owner.LastName, Password = owner.Password, RoleId = owner.RoleId };
        }
    }
}
