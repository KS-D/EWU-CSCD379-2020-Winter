using AutoMapper;
using BlogEngine.Business;
using SecretSanta.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class AutomapperProfileConfigurationTests
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
            (User source, User target) = 
                ( new MockUser(42, SampleData.Inigo, SampleData.Montoya), 
                    new MockUser(0, SampleData.Princess,SampleData.Buttercup) );
            IMapper mapper = AutomapperProfileConfiguration.CreateMapper();
           
            mapper.Map(source, target);
           
            Assert.AreNotEqual(source.Id, target.Id);
            Assert.AreEqual(42,source.Id);
            Assert.AreEqual(source.FirstName, target.FirstName);
            Assert.AreEqual(source.LastName, target.LastName);
        }

        [TestMethod]
        public void Map_Gift_SuccessWithNoIdMapped()
        {
            (Gift source, Gift target) =
                ( new MockGift(42, SampleData.ArduinoTitle, SampleData.ArduinoDescription, SampleData.ArduinoUrl, SampleData.CreateUserInigo()),
                    new MockGift(80, SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl,
                        SampleData.CreateUserButtercup()) );
            IMapper mapper = AutomapperProfileConfiguration.CreateMapper();
           
            mapper.Map(source, target);

            Assert.AreNotEqual(source.Id, target.Id);
            Assert.AreEqual(42,source.Id);
            Assert.AreEqual(source.Title, target.Title);
            Assert.AreEqual(source.Description, target.Description);
            Assert.AreEqual(source.Url, target.Url);
            Assert.AreEqual(source.User.FirstName, target.User.FirstName);
            Assert.AreEqual(source.User.LastName, target.User.LastName);
        }

        [TestMethod]
        public void Map_Group_SuccessWithNoIdMapped()
        {
            (Group source, Group target) =
                ( new MockGroup(42, "Here a title"), new MockGroup(100,"Here is anotherTitle") );
            IMapper mapper = AutomapperProfileConfiguration.CreateMapper();
           
            mapper.Map(source, target);

            Assert.AreNotEqual(source.Id, target.Id);
            Assert.AreEqual(42,source.Id);
            Assert.AreEqual(source.Title, target.Title);
       }

    }
}
