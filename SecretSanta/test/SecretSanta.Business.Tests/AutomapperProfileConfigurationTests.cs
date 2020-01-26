using AutoMapper;
using SecretSanta.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    class AutomapperProfileConfigurationTests
    {
        class MockUser : User
        {
            public MockUser(int id, string firstName, string lastName) : base(firstName, lastName)
            {
                base.Id = id;
            }
        }

        class MockGift : Gift
        {
            public MockGift(int id, string title, string description, string url, User user) : base(title, description, url, user)
            {
                base.Id = id;
            }
        }

        class MockGroup : Group
        {
            public MockGroup(int id, string title) : base(title)
            {
                base.Id = id;
            }
        }

        [TestMethod]
        public void Map_User_SuccessWithNoIdMapped()
        {

        }


    }
}
