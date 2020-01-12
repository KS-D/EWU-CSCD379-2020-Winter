using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Create_User_Success()
        {
            int id = 131;
            string firstName = "FirstName";
            string lastName = "LastName";
            var gifts = new List<Gift>
            {
                new Gift(1, "title", "description", "www.gift.com", null),
            };

            var user = new User(id, firstName, lastName, gifts);

            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);

        }
    }
}
