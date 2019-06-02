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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EventDTO, EventViewModel>()).CreateMapper();
            var events = mapper.Map<IEnumerable<EventDTO>, List<EventViewModel>>(eventDTOs);
            return View(events);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EventViewModel eventView, IEnumerable<HttpPostedFileBase> imgs)
        {
            List<ImageDTO> im = new List<ImageDTO>();
            foreach (var e in imgs)
            {
                im.Add(new ImageDTO { Content = e });
            }
            service.CreateEvent(new EventDTO { Description = eventView.Description, EventTypeId = 1, Images = im, Lat = eventView.Lat, Lng = eventView.Lng, Name = eventView.Name, ShortDescription = eventView.ShortDescription, UserId = StaticVariables.CurrentUser.Id });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var EEvent = service.GetItem(id);
            return View(new EventViewModel { Id = EEvent.Id, Description = EEvent.Description, EventTypeId = EEvent.EventTypeId, Images = EEvent.Images, MongoId = EEvent.MongoId, Name = EEvent.Name, ShortDescription = EEvent.ShortDescription, UserId = EEvent.UserId, Lat = EEvent.Lat, Lng = EEvent.Lng });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var EEvent = service.GetItem(id);
            return View(new EventViewModel { Id = EEvent.Id, Description = EEvent.Description, EventTypeId = EEvent.EventTypeId, Images = EEvent.Images, MongoId = EEvent.MongoId, Name = EEvent.Name, ShortDescription = EEvent.ShortDescription, UserId = EEvent.UserId, Lat = EEvent.Lat, Lng = EEvent.Lng });

        }
        [HttpPost]
        public ActionResult Delete(EventViewModel eve)
        {
            service.Delete(service.GetItem(eve.Id));
            return RedirectToAction("Index");
        }

        public JsonResult GetEvent(int id)
        {
            List<EventViewModel> events = new List<EventViewModel>();
            var s = service.GetItem(id);
            events.Add(new EventViewModel { Id = s.Id, Description = s.Description, EventTypeId = s.EventTypeId, Images = s.Images, Lat = s.Lat, Lng=s.Lng, MongoId = s.MongoId, Name = s.Name, ShortDescription = s.ShortDescription, UserId = s.UserId });
            return Json(events, JsonRequestBehavior.AllowGet);
        }

    }
}