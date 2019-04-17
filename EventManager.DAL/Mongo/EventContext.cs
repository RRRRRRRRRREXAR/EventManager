using EventManager.DAL.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Mongo
{
    class EventContext
    {
        public IMongoDatabase database;
        public IGridFSBucket gridFS;

        public IMongoCollection<Event> Events
        {
            get { return database.GetCollection<Event>("Events"); }
        }
        public IMongoCollection<User> Users
        {
            get { return database.GetCollection<User>("Users"); }
        }
        public IMongoCollection<Image> Images
        {
            get { return database.GetCollection<Image>("Images"); }
        }

        public EventContext()
        {
            string ConnectionString = "mongodb://localhost:27017/EventManager";
            var connection = new MongoUrlBuilder(ConnectionString);
            MongoClient mongoClient = new MongoClient(ConnectionString);
            database = mongoClient.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
        }
    }
}
