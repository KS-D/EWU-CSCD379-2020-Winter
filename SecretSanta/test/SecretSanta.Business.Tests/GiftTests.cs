using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
