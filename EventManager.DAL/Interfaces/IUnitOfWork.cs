using EventManager.DAL.Entities;
using EventManager.DAL.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<Event> Events { get; }
        IRepository<User> Users { get;  }
        IRepository<Role> Roles { get;  }
        IRepository<Subscription> Subscriptions { get; }
        IRepository<EventType> EventTypes { get;  }
        IRepository<Comment> Comments { get;  }
        IRepository<Image> Images { get;  }
        IRepository<MongoEvent> MongoEvents { get; }
        void Save();
    }
}
