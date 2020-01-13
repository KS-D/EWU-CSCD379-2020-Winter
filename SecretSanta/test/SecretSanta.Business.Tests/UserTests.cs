using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var gifts = new List<Gift>();

            var user = new User(id, firstName, lastName, gifts);

            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
            Assert.AreEqual(0, user.Gifts.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_FirstNameNull_ThrowArguementNullException()
        {
            _ = new User(1, null, "lastName", new List<Gift>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_LastNameNull_ThrowArguementNullException()
        {
            _ = new User(1, "firstName", null, new List<Gift>());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_GiftsNull_ThrowArguementNullException()
        {
            _ = new User(1, "firstName", "lastName", null);
        }
    }
}
