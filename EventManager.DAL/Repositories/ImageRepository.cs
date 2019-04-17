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
    class ImageRepository:IRepository<Image>
    {
        EventContext _context;
        IMongoCollection<Image> _dbSet;

        public async Task Create(Image item)
        {
            await _context.Images.InsertOneAsync(item); 
        }

        public async Task Delete(int id)
        {
            await _context.Images.DeleteOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(id))));
        }

        public async Task<IEnumerable<Image>> Find(Expression<Func<Image, bool>> predicate)
        {
            return await _context.Images.Find<Image>(predicate).ToListAsync();
        }

        public async Task<Image> Get(int id)
        {
            return await _context.Images.Find(new BsonDocument("_id", new ObjectId(Convert.ToString(id)))).FirstOrDefaultAsync();
        }

        public IEnumerable<Image> GetAll()
        {
            return _context.Images.AsQueryable();
        }

        public async Task Update(Image item)
        {
          await  _context.Images.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(Convert.ToString(item.Id))), item);
        }

    }
}
