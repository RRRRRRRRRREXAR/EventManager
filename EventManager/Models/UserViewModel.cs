using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}