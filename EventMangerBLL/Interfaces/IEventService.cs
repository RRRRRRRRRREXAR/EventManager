using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL.Interfaces
{
    //to do more service interfaces(((
    interface IEventService
    {
        void CreateEvent(EventDTO item);
        void Update(EventDTO item);
        void Delete(EventDTO item);
        void Comment(CommentDTO item);
        void UploadImages(IEnumerable<ImageDTO> items);
        EventDTO GetItem(int? id);
        IEnumerable<EventDTO> GetEvents();
        IEnumerable<EventDTO> Find(Func<EventDTO, bool> predicate);
        void Dispose();
    }
}
