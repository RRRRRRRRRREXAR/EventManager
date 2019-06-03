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
            if (StaticVariables.CurrentUser!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Create(EventViewModel eventView, IEnumerable<HttpPostedFileBase> imgs)
        {

            if(eventView.Description==null)
            {
                ModelState.AddModelError("Description","Отсутсвует описание");
            }
            if (eventView.ShortDescription == null)
            {
                ModelState.AddModelError("ShortDescription", "Отсутсвует краткое описание");
            }
            if (eventView.Lat == null&& eventView.Lng == null)
            {
                ModelState.AddModelError("Lat", "Отсутсвуют координаты");
            }
            if (eventView.Lat == null && eventView.Lng == null)
            {
                ModelState.AddModelError("Lat", "Отсутсвуют координаты");
            }
            if (ModelState.IsValid)
            {
                List<ImageDTO> im = new List<ImageDTO>();
                foreach (var e in imgs)
                {
                    im.Add(new ImageDTO { Content = e });
                }
                service.CreateEvent(new EventDTO { Description = eventView.Description, EventTypeId = 1, Images = im, Lat = eventView.Lat, Lng = eventView.Lng, Name = eventView.Name, ShortDescription = eventView.ShortDescription, UserId = StaticVariables.CurrentUser.Id });
                return RedirectToAction("Index");
            }
            else
            {
                return View(eventView);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (StaticVariables.CurrentUser!=null)
            {
                var EEvent = service.GetItem(id);
                DetailedViewModel detailedView = new DetailedViewModel();
                CommentViewModel commentView = new CommentViewModel();
                detailedView.Event = new EventViewModel { Id = EEvent.Id, Description = EEvent.Description, EventTypeId = EEvent.EventTypeId, Images = EEvent.Images, MongoId = EEvent.MongoId, Name = EEvent.Name, ShortDescription = EEvent.ShortDescription, UserId = EEvent.UserId, Lat = EEvent.Lat, Lng = EEvent.Lng };
                detailedView.Comment = commentView;
                var commentDTOs = service.GetComments(id);
                List<CommentViewModel> commentViewModels = new List<CommentViewModel>();
                foreach (var item in commentDTOs)
                {
                    commentViewModels.Add(new CommentViewModel { Id = item.Id, EventId = item.EventId, Text = item.Text, UserId = item.UserId });
                }
                detailedView.Comments = commentViewModels;
                var subs = service.GetSubcriptions(StaticVariables.CurrentUser.Id);
                foreach (var s in subs)
                {
                    if (s.Id == id)
                    {
                        detailedView.IsSubscribed = true;
                    }
                }
                return View(detailedView);
            }
            else
            {
                return RedirectToAction("Index");
            }
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

        
        [HttpPost]
        public ActionResult CreateComment(int? Id,DetailedViewModel cvModel)
        {
            CommentViewModel commentView = new CommentViewModel
            {
                Text = cvModel.Comment.Text,
                EventId = Id.Value
            };
            service.Comment(new CommentDTO {EventId=commentView.EventId, Text=commentView.Text, UserId=StaticVariables.CurrentUser.Id });
            return RedirectToAction("Details/"+Id.Value);
        }
        [HttpPost]
        public ActionResult Subscribe(int? id)
        {
            service.Subscribe(service.GetItem(id),StaticVariables.CurrentUser.Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Unsubscribe(int? id)
        {
            service.Unsubscribe(service.GetItem(id), StaticVariables.CurrentUser.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Subscriptions()
        {
            if (StaticVariables.CurrentUser!=null)
            {
                var subs = service.GetSubcriptions(StaticVariables.CurrentUser.Id);
                List<EventViewModel> eventViews = new List<EventViewModel>();
                foreach (var s in subs)
                {
                    eventViews.Add(new EventViewModel{Id = s.Id, Description = s.Description, EventTypeId = s.EventTypeId, Images = s.Images, Lat = s.Lat, Lng = s.Lng, MongoId = s.MongoId, Name = s.Name, ShortDescription = s.ShortDescription, UserId = s.UserId });
                }
                return View(eventViews);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}