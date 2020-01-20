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
        public async Task Create_GroupUser_WithMultipleUsersAndGroups()
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

           user.GroupUsers = groupUsers;
           gift.User = user;

           using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
           {
               dbContext.Gifts.Add(gift);
               await dbContext.SaveChangesAsync();
           }


           using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
           {
               var giftFromDb = await dbContext.Gifts.Where(g => g.Id == gift.Id).Include(g => g.User)
                   .ThenInclude(gu => gu.GroupUsers).ThenInclude(g => g.Group).SingleOrDefaultAsync();

               Assert.IsNotNull(giftFromDb);
               Assert.AreEqual(gift.Title, giftFromDb.Title);
               Assert.AreEqual(gift.Description, giftFromDb.Description);
               Assert.AreEqual(gift.Url, giftFromDb.Url);
               Assert.AreEqual(user.Id, giftFromDb.User.Id);
               CollectionAssert.AreEqual(user.GroupUsers, gift.User.GroupUsers);
           }
        }
    }
}
