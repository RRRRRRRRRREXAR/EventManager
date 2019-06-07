using EventManager.DAL.Entities;
using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [UIHint("Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public IEnumerable<ImageDTO> Images { get; set; }
        public string MongoId { get; set; }
        public UserViewModel Owner { get; set; }
        public int SubsCount { get; set; }
    }
}