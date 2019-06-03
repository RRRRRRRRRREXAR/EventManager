using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.Models
{
    public class DetailedViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public EventViewModel Event { get; set; }
    }
}