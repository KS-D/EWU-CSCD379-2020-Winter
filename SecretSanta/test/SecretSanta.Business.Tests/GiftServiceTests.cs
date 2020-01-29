using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;


namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftServiceTests : TestBase
    {
        [TestMethod]
        public async Task InsertAsync_TwoGroups_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);

            Gift ring = SampleData.CreateRingGift();
            Gift arduino = SampleData.CreateGiftArduino();

            await service.InsertAsync(ring);
            await service.InsertAsync(arduino);

            Assert.IsNotNull(ring.Id);
            Assert.IsNotNull(arduino.Id);
        }

        [TestMethod]
        public async Task UpdateGift_ShouldSaveIntoDatabase()
        {
            (Gift ring, Gift arduino) = await InsertGroupIntoDatabase();
            
            using var dbContextUpdate = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextUpdate, Mapper);

            using var dbContextFetch = new ApplicationDbContext(Options);
            Gift ringFromDatabase =  await dbContextFetch.Gifts.SingleAsync(item => item.Id == ring.Id);

            const string updatedDescription = "You can look at people on your door step";
            ringFromDatabase.Description = updatedDescription;

            await service.UpdateAsync(arduino.Id!.Value, ringFromDatabase); 

            using var dbContextAssert = new ApplicationDbContext(Options);
            ringFromDatabase = await dbContextAssert.Gifts.SingleAsync(item => item.Id == ring.Id);
            Gift arduinoFromDatabase = await dbContextAssert.Gifts.SingleAsync(item => item.Id == arduino.Id);
            
            Assert.AreEqual((SampleData.RingTitle, updatedDescription, SampleData.RingUrl),
                (arduinoFromDatabase.Title, arduinoFromDatabase.Description, arduinoFromDatabase.Url));
            Assert.AreEqual((SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl),
                (ringFromDatabase.Title, ringFromDatabase.Description, ringFromDatabase.Url));

        }

        private async Task<(Gift ring, Gift arduino)> InsertGroupIntoDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);

            Gift ring = SampleData.CreateRingGift();
            Gift arduino = SampleData.CreateGiftArduino();

            await service.InsertAsync(ring);
            await service.InsertAsync(arduino);

            return (ring, arduino);
        }

        [TestMethod]
        public async Task Delete_OneGift_ShouldOnlyRemoveOneFromDatabase()
        {
            (Gift ring, Gift arduino) = await InsertGroupIntoDatabase();
            
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);
            
            //act 
            bool deleted = await service.DeleteAsync(ring.Id!.Value);
            using var dbContextFetch = new ApplicationDbContext(Options);
            Gift arduinoFromdb = await dbContextFetch.Gifts.SingleAsync(item => item.Id == arduino.Id);
            
            //Assert 
            Assert.IsTrue(deleted);
            Assert.IsNotNull(arduinoFromdb);
            Assert.AreEqual(arduino.Id,arduinoFromdb.Id);
            Assert.AreEqual(arduino.Title,arduinoFromdb.Title);
            Assert.AreEqual(arduino.Description,arduinoFromdb.Description);
            Assert.AreEqual(arduino.Url,arduinoFromdb.Url);
        }
        
        [TestMethod]
        public async Task Fetch_AllGiftsFromDatabaseWithUsers_Success()
        { 
            (Gift ring, Gift arduino) = await InsertGroupIntoDatabase();
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);

            List<Gift> GiftList = await service.FetchAllAsync();

            User inigo = GiftList[0].User;
            User princess = GiftList[1].User;

            Assert.AreEqual(2, GiftList.Count);
            Assert.AreEqual((ring.Id, SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl), 
            
                (GiftList[0].Id, GiftList[0].Title, GiftList[0].Description, GiftList[0].Url));
            Assert.AreEqual((arduino.Id, SampleData.ArduinoTitle, SampleData.ArduinoDescription, SampleData.ArduinoUrl),
                (GiftList[1].Id, GiftList[1].Title, GiftList[1].Description, GiftList[1].Url));
            Assert.IsNotNull(inigo);
            Assert.IsNotNull(princess);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), (inigo.FirstName, inigo.LastName));
            Assert.AreEqual((SampleData.Princess, SampleData.Buttercup), (princess.FirstName, princess.LastName));

        }

        [TestMethod]
        public async Task Fetch_OneGiftFromDatabase_Success()
        {
            (Gift ring, Gift _) = await InsertGroupIntoDatabase();
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);

            Gift ringFromdb =  await service.FetchByIdAsync(ring.Id!.Value);
            User inigoOnRing = ringFromdb.User;
            
            Assert.AreEqual((ring.Id, SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl), 
                (ringFromdb.Id, ringFromdb.Title, ringFromdb.Description, ringFromdb.Url));
            Assert.IsNotNull(inigoOnRing);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), (inigoOnRing.FirstName, inigoOnRing.LastName));
        }

    }
}
