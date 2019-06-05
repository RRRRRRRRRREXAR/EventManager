using EventManager.DAL.Entities;
using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL.Interfaces
{
    //to do more service interfaces(((
    public interface IEventService
    {
        void CreateEvent(EventDTO item);
        void Update(EventDTO item);
        void Delete(EventDTO item);
        void Comment(CommentDTO item);
        void UploadImages(IEnumerable<ImageDTO> items);
        void Subscribe(EventDTO item,int UserId);
        void Unsubscribe(EventDTO item, int UserId);
        UserDTO GetOwner(int id);
        EventDTO GetItem(int? id);
        IEnumerable<EventDTO> GetEvents();
        IEnumerable<EventDTO> Find(Func<Event, bool> predicate);
        IEnumerable<CommentDTO> GetComments(int EventId);
        IEnumerable<EventDTO> GetSubcriptions(int UserId);
        void Dispose();
    }
}
