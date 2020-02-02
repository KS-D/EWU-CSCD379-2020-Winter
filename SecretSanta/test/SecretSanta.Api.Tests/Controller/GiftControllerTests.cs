using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controller
{
    [TestClass]
    public class GiftControllerTests
    {

        [TestMethod]
        public void Create_Controller_Success()
        {
            var controller = new GiftController(new Mock<IGiftService>().Object);

            Assert.IsNotNull(controller);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_PassedNullService_ThrowsError()
        {
            var giftController = new GiftController(null!);
        }

        [TestMethod]
        public async Task Get_EntitiesInDatabase_ReturnListOfEntities()
        {
            var service = new Mock<IGiftService>();
            service.Setup(service => service.FetchAllAsync())
                .Returns(Task.FromResult(new List<Gift>{SampleData.CreateRingGift(), SampleData.CreateGiftArduino()}));
            var giftController = new GiftController(service.Object);

            List<Gift> gifts = (List<Gift>)await giftController.Get();

            Assert.IsNotNull(gifts);
            Assert.AreEqual((SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl),
                (gifts[0].Title, gifts[0].Description, gifts[0].Url));
            Assert.AreEqual((SampleData.ArduinoTitle, SampleData.ArduinoDescription, SampleData.ArduinoUrl),
                (gifts[1].Title, gifts[1].Description, gifts[1].Url));
        }

        [TestMethod]
        public async Task Get_ItemById_Success()
        {
            var service = new Mock<IGiftService>();
            service.Setup(service => service.FetchByIdAsync(42))
                .Returns(Task.FromResult(SampleData.CreateRingGift()));
            var giftController = new GiftController(service.Object);

            ActionResult<Gift> giftResult = await giftController.Get(42);

            Assert.IsInstanceOfType(giftResult.Result, typeof(OkObjectResult));
            OkObjectResult ok = (OkObjectResult)giftResult.Result;
            Gift ringGift = (Gift)ok.Value;
            Assert.IsNotNull(ringGift);
            Assert.AreEqual((SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl),
                (ringGift.Title, ringGift.Description, ringGift.Url));
        }

        [TestMethod]
        public async Task Get_ItemNotFound_NotFoundStatusReturned()
        {
            var service = new Mock<IGiftService>();
            service.Setup(service => service.FetchByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Gift>(null!));
            var giftController = new GiftController(service.Object);

            ActionResult<Gift> giftResult = await giftController.Get(42);
    
            Assert.IsInstanceOfType(giftResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post_ItemPosted_Success()
        {
            var service = new Mock<IGiftService>();
            var gift = SampleData.CreateRingGift();
            Gift returnedGift = new MockGift(42, SampleData.CreateRingGift());
            service.Setup(service => service.InsertAsync(gift))
                .Returns(Task.FromResult(returnedGift));
            GiftController userController = new GiftController(service.Object);

            ActionResult<Gift> actionResult = await userController.Post(gift);
            Gift giftResult = actionResult.Value;
                
            Assert.IsNotNull(giftResult);
            Assert.AreEqual((SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl),
                (giftResult.Title, giftResult.Description, giftResult.Url));
            Assert.AreEqual(returnedGift.Id, giftResult.Id);
        }

      
        [TestMethod]
        public async Task Put_ItemUpdated_Success()
        {
            var service = new Mock<IGiftService>();
            Gift? gift = (Gift?) SampleData.CreateRingGift();
            service.Setup(service => service.UpdateAsync(42, gift))
                .Returns(Task.FromResult(gift));
            var giftController = new GiftController(service.Object);

            ActionResult<Gift> actionResult = await giftController.Put(42, gift);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            OkObjectResult ok = (OkObjectResult)actionResult.Result;
            Gift returnedGift = (Gift)ok.Value;
            Assert.IsNotNull(returnedGift);
            Assert.AreEqual((SampleData.RingTitle, SampleData.RingDescription, SampleData.RingUrl),
               (returnedGift.Title, returnedGift.Description, returnedGift.Url));
        }

        [TestMethod]
        public async Task Put_ItemNotFound_ReturnsNotFound()
        {
            var service = new Mock<IGiftService>();
            Gift gift = SampleData.CreateRingGift();
            service.Setup(service => service.UpdateAsync(42, gift))
                .Returns(Task.FromResult<Gift?>(null!));
            var giftController = new GiftController(service.Object);
            
            ActionResult<Gift> actionResult = await giftController.Put(42, gift);

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ItemRemoved_ReturnsTrue()
        {
            var service = new Mock<IGiftService>();
            service.Setup(service => service.DeleteAsync(42))
                .Returns(Task.FromResult(true));
            var giftController = new GiftController(service.Object);

            bool result = await giftController.Delete(42);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Delete_ItemNotRemoved_ReturnsFalse()
        {
            var service = new Mock<IGiftService>();
            service.Setup(service => service.DeleteAsync(42))
                .Returns(Task.FromResult(false));
            var giftController = new GiftController(service.Object);

            bool result = await giftController.Delete(42);

            Assert.IsFalse(result);
        }

        public class MockGift : Gift
        {
            public MockGift(int id, Gift gift)
            {
                Id = id;
                Title = gift.Title;
                Description = gift.Description;
                Url = gift.Url;
            }
        }
    }
}