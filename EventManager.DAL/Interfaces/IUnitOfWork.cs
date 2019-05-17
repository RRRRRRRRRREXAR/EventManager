using EventManager.DAL.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Interfaces
{
    interface IUnitOfWork:IDisposable//Useless
    {
         DbSet<Event> Events { get; set; }
         DbSet<User> Users { get; set; }
         DbSet<Role> Roles { get; set; }
         DbSet<Subscription> Subscriptions { get; set; }
         DbSet<EventType> EventTypes { get; set; }
         DbSet<Comment> Comments { get; set; }
         DbSet<Image> Images { get; set; }
         IMongoCollection<MongoEvent> MongoEvents { get; set; }
    }
}
