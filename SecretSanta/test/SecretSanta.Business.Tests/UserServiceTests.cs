using System.Threading.Tasks;
using AutoMapper;
using BlogEngine.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserServiceTests : TestBase
    {
        [TestMethod]
        public async Task InsertAsync_InigoAndPrincess_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContext, Mapper);

            var inigo = SampleData.CreateUserInigo();
            var princess = SampleData.CreateUserButtercup();

            await service.InsertAsync(inigo);
            await service.InsertAsync(princess);

            Assert.IsNotNull(inigo.Id);
            Assert.IsNotNull(princess.Id);
        }

        [TestMethod]
        public async Task UpdateUser_ShouldSaveIntoDatabase()
        {

            (User inigo, User princess) = await InsertUsersIntoDatabase();
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);


            // Act
            using var dbContextFetch = new ApplicationDbContext(Options);
            User inigoFromDb = await dbContextFetch.Users.SingleAsync(item => item.Id == inigo.Id);

            const string montoyaThe3rd = "Montoya The 3rd";
            inigoFromDb.LastName = montoyaThe3rd;

            // Update Inigo Montoya using the princesses Id.
            await service.UpdateAsync(princess.Id, inigoFromDb);

            // Assert
            using var dbContextAssert = new ApplicationDbContext(Options);
            inigoFromDb = await dbContextAssert.Users.SingleAsync(item => item.Id == inigo.Id);
            var princessFromDb = await dbContextAssert.Users.SingleAsync(item => item.Id == 2); 

            Assert.AreEqual(
                (SampleData.Inigo, montoyaThe3rd), (princessFromDb.FirstName, princessFromDb.LastName));

            Assert.AreEqual(
                (SampleData.Inigo, SampleData.Montoya), (inigoFromDb.FirstName, inigoFromDb.LastName));
        }
    
        [TestMethod]
        public async Task Delete_OneUser_ShouldOnlyRemoveOneFromDatabase()
        {
            (User inigo, User princess) = await InsertUsersIntoDatabase();
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);           
            
            //act 
            bool deleted = await service.DeleteAsync(inigo.Id);
            using var dbContextFetch = new ApplicationDbContext(Options);
            User princessFromDb = await dbContextFetch.Users.SingleAsync(item => item.Id == princess.Id);
            
            //Assert 
            Assert.IsTrue(deleted);
            Assert.IsNotNull(princessFromDb);
            Assert.AreEqual(princess.Id,princessFromDb.Id);
            Assert.AreEqual(princess.FirstName,princessFromDb.FirstName);
            Assert.AreEqual(princess.LastName,princessFromDb.LastName);
        }

        [TestMethod]
        public async Task Fetch_UserFromDatabase()
        {
            (User inigo, User princess) = await InsertUsersIntoDatabase();

            using var dbContextFetch = new ApplicationDbContext(Options);
            
            User inigoFromDatabase = await dbContextFetch.Users.SingleAsync(item => item.Id == inigo.Id);
            User princessFromDb = await dbContextFetch.Users.SingleAsync(item => item.Id == princess.Id);

            Assert.AreEqual(
                (inigo.Id, SampleData.Inigo, SampleData.Montoya), (inigoFromDatabase.Id, inigoFromDatabase.FirstName, inigoFromDatabase.LastName));

            Assert.AreEqual(
                (princess.Id, SampleData.Princess, SampleData.Buttercup), (princessFromDb.Id, princessFromDb.FirstName, princessFromDb.LastName));


        }

        private async Task<(User inigo, User princess)> InsertUsersIntoDatabase()
        {
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);

            var inigo = SampleData.CreateUserInigo();
            var princess = SampleData.CreateUserButtercup();

            await service.InsertAsync(inigo);
            await service.InsertAsync(princess);

            return (inigo, princess);
        }

    }
}
