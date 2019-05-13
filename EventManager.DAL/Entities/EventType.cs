using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Entities
{
    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventId { get; set; }
    }
}
