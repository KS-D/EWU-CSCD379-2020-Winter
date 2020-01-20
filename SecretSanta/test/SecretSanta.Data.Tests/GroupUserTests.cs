using System;
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
        private readonly User _User = new User
            {
                FirstName = "kyle",
                LastName = "Smith",
                GroupUsers = new List<GroupUser>()
            };

        private readonly Gift _Gift = new Gift
            {
                Title = "Gift",
                Description = "a nice gift",
                Url = "www.gifts.com"
            };

        private readonly Group _Group1 = new Group { Name = "A group" };
        private readonly Group  _Group2 = new Group { Name = "Another group" };
 
        [TestMethod]
        public async Task Create_GroupUser_WithMultipleGroups()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
               hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "kyle"));

            var groupUser1 = new GroupUser { User = _User, Group = _Group1 };
            var groupUser2 = new GroupUser { User = _User, Group = _Group2 };

            var groupUsers = new List<GroupUser>{ groupUser1, groupUser2 };
            
            _Gift.User = _User;
            _User.Gifts = new List<Gift>{ _Gift };
            _User.GroupUsers = groupUsers;
           
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            { 
                dbContext.Users.Add(_User);
                await dbContext.SaveChangesAsync();
            }


            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            { 
                var userFromDb = await dbContext.Users.Where(u => u.Id == _User.Id).Include(u => u.Gifts)
                    .Include(u => u.GroupUsers).ThenInclude(g => g.Group).SingleOrDefaultAsync();

                var giftOnUser = userFromDb.Gifts.ElementAt(0);
                    
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(_User.FirstName, userFromDb.FirstName);
                Assert.AreEqual(_User.LastName, userFromDb.LastName);
                Assert.AreEqual(_User.Id, userFromDb.Id);
                Assert.IsNotNull(_User.GroupUsers[0]);
                Assert.IsNotNull(_User.GroupUsers[1]);
                Assert.AreEqual(groupUsers[0].GroupId, userFromDb.GroupUsers[0].GroupId);
                Assert.AreEqual(groupUsers[0].UserId, userFromDb.GroupUsers[0].UserId);
                Assert.AreEqual(groupUsers[1].GroupId, userFromDb.GroupUsers[1].GroupId);
                Assert.AreEqual(groupUsers[1].UserId, userFromDb.GroupUsers[1].UserId);
                Assert.AreEqual(1, userFromDb.Gifts.Count);
                Assert.AreEqual(_Gift.Url, giftOnUser.Url);
                Assert.AreEqual(_Gift.Title, giftOnUser.Title);
                Assert.AreEqual(_Gift.Description, giftOnUser.Description);
            }
        }

        [TestMethod]
        public async Task Create_GroupUser_WithMultipleUsers()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
               hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "kyle"));

            var user2 = new User
            {
                FirstName = "two",
                LastName = "Two",
                Gifts = new List<Gift>()
            };
            
            var groupUser1 = new GroupUser { User = _User, Group = _Group1 };
            var groupUser2 = new GroupUser {User = user2, Group = _Group1 };

            var groupUsers = new List<GroupUser>{ groupUser1 };
            var groupUsers2 = new List<GroupUser> { groupUser2 };
            
            _Gift.User = _User;
            _User.Gifts = new List<Gift>{ _Gift };
            _User.GroupUsers = groupUsers;
            user2.GroupUsers = groupUsers2;
           
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                dbContext.Users.Add(_User);
                dbContext.Users.Add(user2);
                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var groupsFromDb = await dbContext.Groups.Where(g => g.Id == _Group1.Id)
                                                  .Include(u => u.GroupUsers)
                                                  .ThenInclude(u => u.User).SingleOrDefaultAsync();

                Assert.IsNotNull(groupsFromDb);
                Assert.AreEqual(_Group1.Id, groupsFromDb.Id);
                Assert.IsNotNull(groupsFromDb.GroupUsers);
                Assert.AreEqual(2, groupsFromDb.GroupUsers.Count);
                Assert.AreEqual(_User.Id, groupsFromDb.GroupUsers[0].UserId);
                Assert.AreEqual(user2.Id, groupsFromDb.GroupUsers[1].UserId);
            }

        }
    }
}
