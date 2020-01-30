using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GoupServiceTests : EntityServiceBaseTest<Group>
    {
        public override (Group entity, Group secondEntity) GetEntities()
        {
            return (SampleData.CreateEnchantedForestGroup(), SampleData.CreateCastleGroup());
        }

        public override EntityService<Group> GetService(ApplicationDbContext dbContext)
        {
            return new GroupService(dbContext, Mapper);
        }

        public override void AssertEntitiesAreEqual(Group expected, Group actual)
        {
            Assert.AreEqual(actual.Title, expected.Title);

        }

        public override Group UpdateEntity(Group entity, string update)
        {
            entity.Title = update;
            return entity;
        }
    }
}
