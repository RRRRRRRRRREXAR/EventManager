using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManager.DAL.Context;
using EventManager.DAL.Repositories;
using EventManager.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace EventManagerDAL.Tests.RepositoriesTests
{
    [TestFixture]
    class EfRepositoryTests
    {
        EventContext db = new EventContext();
        [Test]
        public void CreateEfEvent()
        {
            bool func(Event d) => d.ShortDescription == "ShortDescription" && d.Description=="TestDescription";
            
            EfRepository<Event> efRepository = new EfRepository<Event>(db);
         //   efRepository.Create(new Event { Description="TestDescription",ShortDescription="ShortDescription",Name="TestBAne"});
            var k = efRepository.Find(func).First();
            Assert.That(k!=null);
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteEfEvent()
        {
            bool func(Event d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription2";
            
            EfRepository<Event> efRepository = new EfRepository<Event>(db);
            efRepository.Create(new Event { Description="TestDescription2",ShortDescription="ShortDescription",Name="TestBAne"});
            efRepository.Delete(efRepository.Find(func).First().Id);
            var ex = Assert.Throws<InvalidOperationException>(() => efRepository.Find(func).First());
            Assert.That(ex.Message, Is.EqualTo("Последовательность не содержит элементов"));
        }
        [Test]
        public void FindEvents()
        {
            bool func(Event d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription";
            EfRepository<Event> efRepository = new EfRepository<Event>(db);
            Assert.That(efRepository.Find(func)!=null);
        }
        [Test]
        public void GetElement()
        {
            bool func(Event d) => d.ShortDescription == "ShortDescription" && d.Description == "TestDescription";
            EfRepository<Event> efRepository = new EfRepository<Event>(db);
            var k = efRepository.Find(func).First();
            Assert.That(efRepository.Get(k.Id)==k);
        }
    }
}
