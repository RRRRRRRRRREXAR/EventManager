using AutoMapper;
using EventManager.Models;
using EventManager.Variables;
using EventMangerBLL.DTO;
using EventMangerBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManager.Controllers
{
    public class EventController : Controller
    {
        IEventService service;

        public EventController(IEventService eventService)
        {
            service = eventService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<EventDTO> eventDTOs = service.GetEvents();
            var mapper = new MapperConfiguration(cfg=>cfg.CreateMap<EventDTO,EventViewModel>()).CreateMapper();
            var events = mapper.Map<IEnumerable<EventDTO>, List<EventViewModel>>(eventDTOs);
            return View(events);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EventViewModel eventView,IEnumerable<HttpPostedFileBase> imgs)
        {
            List<ImageDTO> im = new List<ImageDTO>();
            foreach (var e in imgs)
            {
                im.Add(new ImageDTO { Content=e});
            }
            service.CreateEvent(new EventDTO {Description=eventView.Description, EventTypeId=1, Images=im, Location=new DAL.Entities.Vectord2D { X=eventView.Lat,Y=eventView.Lng}, Name=eventView.Name, ShortDescription=eventView.ShortDescription, UserId=StaticVariables.CurrentUser.Id});
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(service.GetItem(id));
        }

        public JsonResult GetData()
        {
            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}