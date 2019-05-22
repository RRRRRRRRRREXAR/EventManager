using EventManager.DAL.Context;
using EventManager.DAL.Entities;
using EventManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private EventContext db;
        private EfRepository<Event> EventsRepository;
        private EfRepository<User> UsersRepository;
        private EfRepository<Role> RolesRepository;
        private EfRepository<Subscription> SubscriptionsRepository;
        private EfRepository<EventType> EventTypesRepository;
        private EfRepository<Comment> CommentsRepository;
        private EfRepository<Image> ImagesRepository;
        private MongoEventRepository MongoEventsRepository;

       public IRepository<Event> Events
        {
            get
            {
                if (EventsRepository==null)
                {
                    EventsRepository = new EfRepository<Event>(db);
                }
                return EventsRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (UsersRepository == null)
                {
                    UsersRepository = new EfRepository<User>(db);
                }
                return UsersRepository;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (RolesRepository == null)
                {
                    RolesRepository = new EfRepository<Role>(db);
                }
                return RolesRepository;
            }
        }

        public IRepository<Subscription> Subscriptions
        {
            get
            {
                if (SubscriptionsRepository == null)
                {
                    SubscriptionsRepository = new EfRepository<Subscription>(db);
                }
                return SubscriptionsRepository;
            }
        }

        public IRepository<EventType> EventTypes
        {
            get
            {
                if (EventTypesRepository == null)
                {
                    EventTypesRepository = new EfRepository<EventType>(db);
                }
                return EventTypesRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (CommentsRepository == null)
                {
                    CommentsRepository = new EfRepository<Comment>(db);
                }
                return CommentsRepository;
            }
        }

        public IRepository<Image> Images
        {
            get
            {
                if (ImagesRepository == null)
                {
                    ImagesRepository = new EfRepository<Image>(db);
                }
                return ImagesRepository;
            }
        }

        public IRepository<MongoEvent> MongoEvents
        {
            get
            {
                if (MongoEventsRepository == null)
                {
                    MongoEventsRepository = new MongoEventRepository(db);
                }
                return MongoEventsRepository;
            }
        }

        public UnitOfWork(string ConnectionName)
        {
            db = new EventContext(ConnectionName);
        }

        public void Save()
        {
            db.SaveChanges();
        }



        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
