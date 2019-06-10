using AutoMapper;
using EventManager.DAL.Entities;
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
        public ActionResult Index(string sortOrder,string SearchString)
        {
            if (SearchString==null)
            {
                var events = service.GetEvents();
                List<EventViewModel> eventViews = new List<EventViewModel>();
                foreach (var s in events)
                {
                    var owner = service.GetOwner(s.UserId);
                    eventViews.Add(new EventViewModel { SubsCount = s.SubsCount, Time = s.Time, Owner = new UserViewModel { FirstName = owner.FirstName, LastName = owner.LastName }, Id = s.Id, Description = s.Description, EventTypeId = s.EventTypeId, Images = s.Images, Lat = s.Lat, Lng = s.Lng, MongoId = s.MongoId, Name = s.Name, ShortDescription = s.ShortDescription, UserId = s.UserId });
                }
                switch (sortOrder)
                {

                    case "Date":
                        eventViews = eventViews.OrderBy(s => s.Time).ToList();
                        break;
                    case "SubsCount":
                        eventViews = eventViews.OrderByDescending(s => s.SubsCount).ToList();
                        break;
                }
                return View(eventViews);
            }
            else
            {
                Func<Event, bool> searchFunc = d=>d.Name.Contains(SearchString) ||d.ShortDescription.Contains(SearchString) || d.Description.Contains(SearchString);
                var events = service.Find(searchFunc);
                List<EventViewModel> eventViews = new List<EventViewModel>();
                foreach (var s in events)
                {
                    var owner = service.GetOwner(s.UserId);
                    eventViews.Add(new EventViewModel { SubsCount = s.SubsCount, Time = s.Time, Owner = new UserViewModel { FirstName = owner.FirstName, LastName = owner.LastName }, Id = s.Id, Description = s.Description, EventTypeId = s.EventTypeId, Images = s.Images, Lat = s.Lat, Lng = s.Lng, MongoId = s.MongoId, Name = s.Name, ShortDescription = s.ShortDescription, UserId = s.UserId });
                }
                return View(eventViews);
            }
            
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (StaticVariables.CurrentUser!=null)
            {
                return View(new EventViewModel { Time=new DateTime()});
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
            if (eventView.Time<=DateTime.Now)
            {
                ModelState.AddModelError("Time","Неправильно выбрана дата");
            }
            if (ModelState.IsValid)
            {
                List<ImageDTO> im = new List<ImageDTO>();
                foreach (var e in imgs)
                {
                    im.Add(new ImageDTO { Content = e });
                }
                service.CreateEvent(new EventDTO {  Time=eventView.Time,Description = eventView.Description, EventTypeId = 1, Images = im, Lat = eventView.Lat, Lng = eventView.Lng, Name = eventView.Name, ShortDescription = eventView.ShortDescription, UserId = StaticVariables.CurrentUser.Id });
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
                detailedView.Event = new EventViewModel {  Time=EEvent.Time,Id = EEvent.Id, Description = EEvent.Description, EventTypeId = EEvent.EventTypeId, Images = EEvent.Images, MongoId = EEvent.MongoId, Name = EEvent.Name, ShortDescription = EEvent.ShortDescription, UserId = EEvent.UserId, Lat = EEvent.Lat, Lng = EEvent.Lng };
                detailedView.Comment = commentView;
                var commentDTOs = service.GetComments(id);
                List<CommentViewModel> commentViewModels = new List<CommentViewModel>();
                foreach (var item in commentDTOs)
                {
                    var owner = service.GetOwner(item.UserId);
                    commentViewModels.Add(new CommentViewModel {  Owner=new UserViewModel {Id=owner.Id, Email=owner.Email, FirstName=owner.FirstName, IsEmailConfirmed=owner.IsEmailConfirmed, LastName=owner.LastName, Password=owner.Password, RoleId=owner.RoleId },Id = item.Id, EventId = item.EventId, Text = item.Text, UserId = item.UserId });
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
            if (StaticVariables.CurrentUser!=null)
            {
                var EEvent = service.GetItem(id);
                return View(new EventViewModel { Time = EEvent.Time, Id = EEvent.Id, Description = EEvent.Description, EventTypeId = EEvent.EventTypeId, Images = EEvent.Images, MongoId = EEvent.MongoId, Name = EEvent.Name, ShortDescription = EEvent.ShortDescription, UserId = EEvent.UserId, Lat = EEvent.Lat, Lng = EEvent.Lng });
            }
            return RedirectToAction("Index");
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
        public ActionResult Edit(int id)
        {
            if (StaticVariables.CurrentUser!=null)
            {
                var even = service.GetItem(id);
                return View(new EventViewModel { Id = even.Id, Description = even.Description, EventTypeId = even.EventTypeId, Images = even.Images, Lat = even.Lat, Lng = even.Lng, MongoId = even.MongoId, Name = even.Name, ShortDescription = even.ShortDescription, UserId = StaticVariables.CurrentUser.Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(EventViewModel eventView,IEnumerable<HttpPostedFileBase> imgs)
        {
            if (eventView.Description == null)
            {
                ModelState.AddModelError("Description", "Отсутсвует описание");
            }
            if (eventView.ShortDescription == null)
            {
                ModelState.AddModelError("ShortDescription", "Отсутсвует краткое описание");
            }
            if (eventView.Lat == null && eventView.Lng == null)
            {
                ModelState.AddModelError("Lat", "Отсутсвуют координаты");
            }
            if (eventView.Lat == null && eventView.Lng == null)
            {
                ModelState.AddModelError("Lat", "Отсутсвуют координаты");
            }
            if (ModelState.IsValid)
            {
                bool imgChanged = false;
                List<ImageDTO> im = new List<ImageDTO>();
                if (imgs!=null)
                {
                    imgChanged = true;
                    foreach (var e in imgs)
                    {
                        im.Add(new ImageDTO { Content = e });
                    }
                }
                else
                {
                   im =service.GetItem(eventView.Id).Images.ToList();
                }
                service.Update(new EventDTO { Description = eventView.Description, EventTypeId = 1, Images = im, Lat = eventView.Lat, Lng = eventView.Lng, Name = eventView.Name, ShortDescription = eventView.ShortDescription, UserId = StaticVariables.CurrentUser.Id },imgChanged);
                return RedirectToAction("Index");
            }
            else
            {
                return View(eventView);
            }
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