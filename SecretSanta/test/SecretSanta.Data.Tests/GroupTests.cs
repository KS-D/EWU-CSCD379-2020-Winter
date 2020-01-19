using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                var group = await applicationDbContext.Groups.Where(u => u.Id == groupId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(group);
                Assert.AreEqual("TheGroup", group.Name);
            }

        }
    }
}
