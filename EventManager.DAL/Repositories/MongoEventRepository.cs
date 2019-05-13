using EventManager.DAL.Entities;
using EventManager.DAL.Interfaces;
using EventManager.DAL.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Repositories
{
    public class MongoEventRepository : IRepository<MongoEvent>
    {
        EventContext _context;
        IMongoCollection<MongoEvent> _dbSet;

        public void Create(MongoEvent item)
        {
             _context.MongoEvents.InsertOne(item);
        }

        public void Delete(int id)
        {
             _context.MongoEvents.DeleteOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(id))));
        }
        //todo
        public IEnumerable<MongoEvent> Find(Func<MongoEvent, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public MongoEvent Get(int id)
        {
            return  _context.MongoEvents.Find(new BsonDocument("_id", new ObjectId(Convert.ToString(id)))).FirstOrDefault();
        }

        public IEnumerable<MongoEvent> GetAll()
        {
            return _context.MongoEvents.AsQueryable();
        }

        public void Update(MongoEvent item)
        {
             _context.MongoEvents.ReplaceOne(new BsonDocument("_id", new ObjectId(Convert.ToString(item.Id))), item);
        }

    }
}
