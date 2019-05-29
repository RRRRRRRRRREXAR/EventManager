using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EventMangerBLL.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HttpPostedFileBase Content { get; set; }
        public string Link { get; set; }
        public int EventId { get; set; }
    }
}
