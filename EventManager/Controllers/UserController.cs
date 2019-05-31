using EventManager.DAL.Entities;
using EventManager.Models;
using EventMangerBLL.Infrastructure;
using EventMangerBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Variables;

namespace EventManager.Controllers
{
    public class UserController : Controller
    {
        IUserService service;
        public UserController(IUserService userService)
        {
            service = userService;
        }
        
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserViewModel user)
        {
            service.SingUp(new EventMangerBLL.DTO.UserDTO {Email=user.Email, FirstName=user.FirstName,LastName=user.LastName, Password=user.Password });
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {
            Func<User, bool> f = d => d.Email == user.Email && d.Password==user.Password;
            try
            {
               var s= service.SignIn(f);
                if (s.IsEmailConfirmed!=true)
                {
                    throw new ValidationException("Подтвердите почту","");
                }
                else
                {
                    StaticVariables.CurrentUser = new UserViewModel {Id=s.Id, Password=s.Password,Email=s.Email, FirstName=s.FirstName, IsEmailConfirmed= s.IsEmailConfirmed, LastName=s.LastName, RoleId=s.RoleId   };
                    return RedirectToAction("Index","Event");
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex);
            }
            return View();
        }
        [HttpGet]
        public ActionResult ConfirmEmail(int id)
        {

            var s = service.Get(id);
            if (s.IsEmailConfirmed==false)
            {
                service.ConfirmEmail(s);
                StaticVariables.CurrentUser = new UserViewModel { Email = s.Email, FirstName = s.FirstName, Id = s.Id, IsEmailConfirmed = s.IsEmailConfirmed, LastName = s.LastName, Password = s.Password, RoleId = s.RoleId };
            }
            return RedirectToAction("Index","Event");
        }
        [HttpGet]
        public ActionResult Logout()
        {

            if (StaticVariables.CurrentUser != null)
            {
                StaticVariables.CurrentUser = null;
            }
            return RedirectToAction("Index", "Event");
        }
    }
}