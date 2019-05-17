using EventManager.DAL.Context;
using EventManager.DAL.Entities;
using EventManager.DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace EventManagerDAL.Tests.RepositoriesTests
{
    [TestFixture]
    class MongoRepositoryTests
    {
        EventContext db = new EventContext();
        [Test]
        public void CreateMongoEvent()
        {
            bool func(MongoEvent d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription";

            MongoEventRepository mongoEventRepository = new MongoEventRepository(db);
            mongoEventRepository.Create(new MongoEvent { Description = "TestDescription", ShortDescription = "ShortDescription", Name = "TestBAne", Location = new Vectord2D {X = 54.3M,Y=45.3M } });
            var k = mongoEventRepository.Find(func).First();
            Assert.That(k != null);
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteMongoEvent()
        {
            bool func(MongoEvent d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription2";

            MongoEventRepository mongoEventRepository = new MongoEventRepository(db);
            mongoEventRepository.Create(new MongoEvent { Description = "TestDescription2", ShortDescription = "ShortDescription", Name = "TestBAne" });
            mongoEventRepository.Delete(mongoEventRepository.Find(func).First().Id);
            var ex = Assert.Throws<InvalidOperationException>(() => mongoEventRepository.Find(func).First());
            Assert.That(ex.Message, Is.EqualTo("Последовательность не содержит элементов"));
        }
        [Test]
        public void FindMongoEvents()
        {
            bool func(MongoEvent d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription";
            MongoEventRepository mongoEventRepository = new MongoEventRepository(db);
            Assert.That(mongoEventRepository.Find(func) != null);
        }
        [Test]//works but i dont know why it cant pass
        public void GetMongoElement()
        {
            bool func(MongoEvent d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription";
            var mongoEventRepository = new MongoEventRepository(db);
            MongoEvent k = mongoEventRepository.Find(func).First();
            MongoEvent e = mongoEventRepository.Get(k.Id);
            Assert.That(k == e);
        }
    }
}
