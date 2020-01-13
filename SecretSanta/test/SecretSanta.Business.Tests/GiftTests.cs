using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    {
        [TestMethod]
        public void Create_Gift_Success()
        {
            int id = 1;
            string title = "fancy gift";
            string desctription = "fancy gift description";
            string url = "www.fancyGift.com";
            User user = new User(1, "firstName", "LastName", new List<Gift>());

            Gift gift = new Gift(id, title, desctription, url, user);

            Assert.AreEqual(id, gift.Id);
            Assert.AreEqual(title, gift.Title);
            Assert.AreEqual(desctription, gift.Description);
            Assert.AreEqual(url, gift.Url);
            Assert.AreEqual(user, gift.User);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_TitleNull_ThrowArguementNullException()
        {
            var user = new User(1, "firstName", "LastName", new List<Gift>());
           
            _ = new Gift(1, null!, "description", "url", user);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_DescriptionNull_ThrowArguementNullException()
        {
            var user = new User(1, "firstName", "lastName", new List<Gift>());
            
            _ = new Gift(1, "title", null!, "url", user);
        }
  
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_URLNull_ThrowArguementNullException()
        {
            var user = new User(1, "firstName", "lastName", new List<Gift>());
            
            _ = new Gift(1, "title", "descripton", null!, user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_UserNull_ThrowArguementNullException()
        {
            _ = new Gift(1, "title", "descripton", "url", null!);
        }

    }
}
