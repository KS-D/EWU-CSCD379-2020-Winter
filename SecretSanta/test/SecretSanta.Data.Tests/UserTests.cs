using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                var user = await applicationDbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(user);
                Assert.AreEqual("Kyle", user.FirstName);
            }
        }

    }
}
