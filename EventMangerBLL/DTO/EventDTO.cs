using EventManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EventMangerBLL.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int EventTypeId { get; set; }
        public Vectord2D Location { get; set; }
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }
}
