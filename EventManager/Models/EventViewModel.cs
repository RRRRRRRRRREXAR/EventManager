using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.Models
{
    public class EventViewModel
    {
        public EventDTO Event = new EventDTO();
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}