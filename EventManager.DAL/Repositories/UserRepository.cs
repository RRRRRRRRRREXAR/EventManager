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
    class UserRepository : IRepository<User>
    {
        EventContext _context;
        IMongoCollection<User> _dbSet;

        public async Task Create(User item)
        {
            await _context.Users.InsertOneAsync(item);
        }

        public async Task Delete(int id)
        {
            await _context.Users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(id))));
        }

        public async Task<IEnumerable<User>> Find(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.Find<User>(predicate).ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _context.Users.Find(new BsonDocument("_id", new ObjectId(Convert.ToString(id)))).FirstOrDefaultAsync();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }

        public async Task Update(User item)
        {
            await _context.Users.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(item.Id))), item);
        }

    }
}
