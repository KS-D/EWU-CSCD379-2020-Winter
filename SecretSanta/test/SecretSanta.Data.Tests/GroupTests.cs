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
    public class GroupTests : TestBase
    {
        private readonly Group _Group = new Group
        {
            Name = "TheGroup",
            GroupUsers = new List<GroupUser>()
        };

        [TestMethod]
        public async Task Create_Group_DatabaseShouldStoreGroup()
        { 
            var groupId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {

                applicationDbContext.Groups.Add(_Group);
                await applicationDbContext.SaveChangesAsync();

                groupId = _Group.Id;
            }

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                Group group = await applicationDbContext.Groups.Where(u => u.Id == groupId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(group);
                Assert.AreEqual(_Group.Name, group.Name);
            }
        } 
        
        [TestMethod]
        public async Task Create_Group_ShouldSetFingerPrintOnInitialSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "kyle"));

            var groupId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Groups.Add(_Group);
                await applicationDbContext.SaveChangesAsync();

                groupId = _Group.Id;
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Group group = await applicationDbContext.Groups.Where(g => g.Id == groupId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(group);
                Assert.AreEqual("kyle", group.CreatedBy);
                Assert.AreEqual("kyle", group.ModifiedBy);
            }
        }

        [TestMethod] 
        public async Task Create_Group_ShouldSetFingerPrintOnUpdateSave()
        {
            const string user = "kyle";
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, user));

            var groupId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Groups.Add(_Group);
                await applicationDbContext.SaveChangesAsync();

                groupId = _Group.Id;
            }

            const string updatedUser = "TotallyNotKyle";
            const string updatedName = "a new group name";

            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, updatedUser));
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Group group = await applicationDbContext.Groups.Where(g => g.Id == groupId).SingleOrDefaultAsync();
                group.Name = updatedName;

                await applicationDbContext.SaveChangesAsync();
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Group group = await applicationDbContext.Groups.Where(g => g.Id == groupId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(group);
                Assert.AreEqual(updatedName, group.Name);
                Assert.AreEqual(user, group.CreatedBy);
                Assert.AreEqual(updatedUser, group.ModifiedBy);
                Assert.IsTrue(group.ModifiedOn > group.CreatedOn);

            }
        }

    }
}
