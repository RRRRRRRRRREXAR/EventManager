using EventManager.DAL.Entities;
using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int EventTypeId { get; set; }
        public Vectord2D Location = new Vectord2D();
        public string Lat { get; set; }
        public string Lng { get; set; }
        public IEnumerable<ImageDTO> Images { get; set; }
        public string MongoId { get; set; }
        public UserViewModel Owner { get; set; }
    }
}