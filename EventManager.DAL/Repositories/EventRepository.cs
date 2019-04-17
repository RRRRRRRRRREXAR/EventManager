using EventManager.DAL.Entities;
using EventManager.DAL.Interfaces;
using EventManager.DAL.Mongo;
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
    class EventRepository : IRepository<Event>
    {
        EventContext _context;
        IMongoCollection<Event> _dbSet;

        public async Task Create(Event item)
        {
            await _context.Events.InsertOneAsync(item);
        }

        public async Task Delete(int id)
        {
            await _context.Events.DeleteOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(id))));
        }

        public async Task<IEnumerable<Event>> Find(Expression<Func<Event, bool>> predicate)
        {
            return await _context.Events.Find<Event>(predicate).ToListAsync();
        }

        public async Task<Event> Get(int id)
        {
            return await _context.Events.Find(new BsonDocument("_id", new ObjectId(Convert.ToString(id)))).FirstOrDefaultAsync();
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events.AsQueryable();
        }

        public async Task Update(Event item)
        {
            await _context.Events.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(item.Id))), item);
        }

    }
}
