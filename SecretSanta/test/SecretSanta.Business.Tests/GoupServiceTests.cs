using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GoupServiceTests : TestBase
    {
        [TestMethod]
        public async Task InsertAsync_EnchantedForestAndCastle_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);

            Group forest = SampleData.CreateEnchantedForestGroup();
            Group castle = SampleData.CreateCastleGroup();
        
            await service.InsertAsync(forest);
            await service.InsertAsync(castle);

            Assert.IsNotNull(forest.Id);
            Assert.IsNotNull(castle.Id);
        }
        
        [TestMethod]
        public async Task UpdateGroup_ShouldSaveIntoDatabase()
        {
            (Group forest, Group castle) = await InsertIntoDatabase();
            
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);


            // Act
            using var dbContextFetch = new ApplicationDbContext(Options);
            Group forestFromDb = await dbContextFetch.Groups.SingleAsync(item => item.Id == forest.Id);

            const string updatedTitle = "Enchanted ongoing land development project";
            forestFromDb.Title = updatedTitle;

            await service.UpdateAsync(castle.Id!.Value, forestFromDb);

            // Assert
            using var dbContextAssert = new ApplicationDbContext(Options);
            forestFromDb = await dbContextAssert.Groups.SingleAsync(item => item.Id == forest.Id);
            Group castleFromDb = await dbContextAssert.Groups.SingleAsync(item => item.Id == 2); 

            Assert.AreEqual(SampleData.ForestGroupTitle, forestFromDb.Title);
            Assert.AreEqual(updatedTitle, castleFromDb.Title);
        }
        
        private async Task<(Group forest, Group castle)> InsertIntoDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);

            Group forest = SampleData.CreateEnchantedForestGroup();
            Group castle = SampleData.CreateCastleGroup();
        
            await service.InsertAsync(forest);
            await service.InsertAsync(castle);

            return (forest, castle);
        }

        [TestMethod]
        public async Task Delete_OneGroup_ShouldOnlyRemoveOneFromDatabase()
        {
            (Group forest, Group castle) = await InsertIntoDatabase();
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);

            bool deleted = await service.DeleteAsync(forest.Id!.Value!);
            
            using var dbContextFetch = new ApplicationDbContext(Options);
            Group castleFromDb = await dbContextFetch.Groups.SingleAsync(item => item.Id == castle.Id);
            
            Assert.IsTrue(deleted);
            Assert.IsNotNull(castleFromDb);
            Assert.AreEqual(castle.Id,castleFromDb.Id);
            Assert.AreEqual(castle.Title,castleFromDb.Title);
        }
        
        [TestMethod]
        public async Task Fetch_OneGroupFromDatabase_Success()
        {
            (Group forest, Group _) = await InsertIntoDatabase();
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);

            Group forestFromdb = await service.FetchByIdAsync(forest!.Id!.Value);

            Assert.AreEqual(forest.Title, forestFromdb.Title);
        }

        [TestMethod]
        public async Task Fetch_AllGroupsFromDatabase_Success()
        {
            (Group forest, Group castle) = await InsertIntoDatabase();
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);

            List<Group> Groups = await service.FetchAllAsync();

            Assert.AreEqual(2, Groups.Count);
            Assert.AreEqual(forest.Title, Groups[0].Title);
            Assert.AreEqual(castle.Title, Groups[1].Title);
        }



    }
}
