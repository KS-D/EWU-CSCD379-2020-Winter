using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserTests
    {
        private const int Id = 131;
        private const string FirstName = "Firstname";
        private const string LastName = "Lastname";
        private readonly List<Gift> _Gifts = new List<Gift>();

        [TestMethod]
        public void Create_User_Success()
        {
            var user = new User(Id, FirstName, LastName, _Gifts);

            Assert.AreEqual(Id, user.Id);
            Assert.AreEqual(FirstName, user.FirstName);
            Assert.AreEqual(LastName, user.LastName);
            Assert.AreEqual(0, user.Gifts.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_FirstNameNull_ThrowArgumentNullException()
        { 
            _ = new User(Id, null!, LastName, _Gifts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_LastNameNull_ThrowArgumentGiftTestsNullException()
        {
            _ = new User(Id, FirstName, null!,_Gifts);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_GiftsNull_ThrowArgumentNullException()
        {
            _ = new User(Id, FirstName, LastName, null!);
        }
    }
}
