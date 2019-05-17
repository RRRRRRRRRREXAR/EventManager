using EventManager.DAL.Entities;
using EventManager.DAL.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Context
{
    public class EventContext:DbContext
    {
        public IMongoDatabase database;
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }

        public IMongoCollection<MongoEvent> MongoEvents
        {
            get { return database.GetCollection<MongoEvent>("Events"); }
        }

        public EventContext()
        {
            string ConnectionString = "mongodb://localhost:27017/EventManager";
            var connection = new MongoUrlBuilder(ConnectionString);
            MongoClient mongoClient = new MongoClient(ConnectionString);
            database = mongoClient.GetDatabase(connection.DatabaseName);
        }

    }
}
