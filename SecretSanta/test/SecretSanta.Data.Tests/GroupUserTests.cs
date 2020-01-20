using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GroupUserTests : TestBase
    {
        [TestMethod]
        public async Task Create_GroupUser_WithMultipleGroups()
        {
           IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
               hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "kyle"));
           
           var user = new User
           {
               FirstName = "kyle",
               LastName = "Smith",
           };

           var gift = new Gift
           {
               Title = "Gift",
               Description = "a nice gift",
               Url = "www.gifts.com"
           };

           var group1 = new Group { Name = "A group" };
           var group2 = new Group { Name = "Another group" };

           var groupUser1 = new GroupUser { User = user, Group = group1 };
           var groupUser2 = new GroupUser { User = user, Group = group2 };

           var groupUsers = new List<GroupUser>{ groupUser1, groupUser2 };
           
           gift.User = user;
           user.Gifts = new List<Gift>{ gift };
           user.GroupUsers = groupUsers;
           
           using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
           {
               dbContext.Users.Add(user);
               await dbContext.SaveChangesAsync();
           }


           using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
           {
               var UserFromDb = await dbContext.Users.Where(u => u.Id == user.Id).Include(u => u.Gifts)
                   .Include(u => u.GroupUsers).ThenInclude(g => g.Group).SingleOrDefaultAsync();

               var giftOnUser = UserFromDb.Gifts.ElementAt(0);

               Assert.IsNotNull(UserFromDb);
               Assert.AreEqual(user.FirstName, UserFromDb.FirstName);
               Assert.AreEqual(user.LastName, UserFromDb.LastName);
               Assert.AreEqual(1, UserFromDb.Gifts.Count);
               Assert.AreEqual(user.Id, UserFromDb.Id);
               Assert.AreEqual(gift.Url, giftOnUser.Url);
               Assert.AreEqual(gift.Title, giftOnUser.Title);
               Assert.AreEqual(gift.Description, giftOnUser.Description);
               Assert.IsNotNull(user.GroupUsers[0]);
               Assert.IsNotNull(user.GroupUsers[1]);
               CollectionAssert.AreEqual(user.GroupUsers, gift.User.GroupUsers);
           }
        }
    }
}
