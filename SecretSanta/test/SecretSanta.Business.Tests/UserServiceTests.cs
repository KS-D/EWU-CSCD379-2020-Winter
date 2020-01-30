using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserServiceTests : EntityServiceBaseTest<User>
    {
        public override (User entity, User secondEntity) GetEntities()
        {
            return (SampleData.CreateUserInigo(), SampleData.CreateUserButtercup());
        }

        public override EntityService<User> GetService(ApplicationDbContext dbContext)
        {
            return new UserService(dbContext, Mapper);
        }

        public override void AssertEntitiesAreEqual(User expected, User actual)
        {
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
        }

        public override User UpdateEntity(User entity, string update)
        {
            entity.FirstName = update;
            return entity;
        }
    }
}
