using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UserTests : TestBase
    {
        private readonly User _User = new User
        {
          FirstName = "Kyle",
          LastName = "Smith",
          Gifts = new List<Gift>(),
          GroupUsers = new List<GroupUser>()
        };

        [TestMethod]
        public async Task Create_User_DatabaseShouldSaveUser()
        {
            var userId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {

                applicationDbContext.Users.Add(_User);
                await applicationDbContext.SaveChangesAsync();

                userId = _User.Id;
            }

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                User user = await applicationDbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(user);
                Assert.AreEqual("Kyle", user.FirstName);
            }
        }

        [TestMethod]
        public async Task Create_User_ShouldSetFingerPrintOnInitialSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "kyle"));

            var userId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Users.Add(_User);
                await applicationDbContext.SaveChangesAsync();

                userId = _User.Id;
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user = await applicationDbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(user);
                Assert.AreEqual("kyle", user.CreatedBy);
                Assert.AreEqual("kyle", user.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task Create_User_ShouldSetFingerPrintOnUpdateSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "kyle"));

            var userId = -1;
            const string updatedFirstName = "TotallyNotKyle";
            const string updatedLastName = "TotallyNotSmith";
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Users.Add(_User);
                await applicationDbContext.SaveChangesAsync();

                userId = _User.Id;
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = await applicationDbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(user);
                Assert.AreEqual("kyle", user.CreatedBy);
                Assert.AreEqual("kyle", user.ModifiedBy);
            }

            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, updatedFirstName));
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user = await applicationDbContext.Users.Where(a => a.Id == userId).SingleOrDefaultAsync();
                user.FirstName = updatedFirstName;
                user.LastName = updatedLastName;

                await applicationDbContext.SaveChangesAsync();
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user = await applicationDbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(user);
                Assert.AreEqual(updatedFirstName, user.FirstName);
                Assert.AreEqual(updatedLastName, user.LastName);
                Assert.AreEqual("kyle", user.CreatedBy);
                Assert.AreEqual(updatedFirstName, user.ModifiedBy);
                Assert.IsTrue(user.ModifiedOn > user.CreatedOn);

            }

        }
    }
}
