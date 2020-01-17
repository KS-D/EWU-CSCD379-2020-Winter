using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    { 
        private const int Id = 1;
        private const string Title = "fancy gift";
        private const string Description = "fancy gift description";
        private const string Url = "www.fancyGift.com";
        private readonly User _User = new User(1, "firstName", "LastName", new List<Gift>());       
        
        [TestMethod]
        public void Create_Gift_Success()
        {

            Gift gift = new Gift(Id, Title, Description, Url, _User);

            Assert.AreEqual(Id, gift.Id);
            Assert.AreEqual(Title, gift.Title);
            Assert.AreEqual(Description, gift.Description);
            Assert.AreEqual(Url, gift.Url);
            Assert.AreEqual(_User.Id, gift.User.Id);
            Assert.AreEqual(_User.FirstName, gift.User.FirstName);
            Assert.AreEqual(_User.LastName, gift.User.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_TitleNull_ThrowArgumentNullException()
        {
           
            _ = new Gift(Id, null!, Description, Url, _User);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_DescriptionNull_ThrowArgumentNullException()
        {
            _ = new Gift(Id, Title, null!, Url, _User);
        }
  
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_URLNull_ThrowArgumentNullException()
        {
            _ =new Gift(Id, Title, Description, null!, _User);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_UserNull_ThrowArgumentNullException()
        { 
            _ = new Gift(Id, Title, Description, Url, null!);
        }

    }
}
