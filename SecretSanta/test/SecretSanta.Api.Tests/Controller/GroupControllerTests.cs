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
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controller
{
    [TestClass]
    public class GroupControllerTests
    {
        [TestMethod]
        public void Create_Controller_Success()
        {
            var groupController = new GroupController(new Mock<IGroupService>().Object);

            Assert.IsNotNull(groupController);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_PassedNullService_ThrowsError()
        {
            var groupController = new GroupController(null!);
        }

        [TestMethod]
        public async Task Get_EntitiesInDatabase_ReturnListOfEntities()
        {
            var service = new Mock<IGroupService>();
            service.Setup(service => service.FetchAllAsync())
                .Returns(Task.FromResult(new List<Group> { SampleData.CreateEnchantedForestGroup(), SampleData.CreateCastleGroup() }));
            var groupController = new GroupController(service.Object);

            List<Group> groups = (List<Group>)await groupController.Get();

            Assert.IsNotNull(groups);
            Assert.AreEqual(SampleData.ForestGroupTitle, groups[0].Title);
            Assert.AreEqual(SampleData.CastleGroupTitle, groups[1].Title);
        }

        [TestMethod]
        public async Task Get_ItemById_Success()
        {
            var service = new Mock<IGroupService>();
            service.Setup(service => service.FetchByIdAsync(42))
                .Returns(Task.FromResult(SampleData.CreateCastleGroup()));
            var groupController = new GroupController(service.Object);

            ActionResult<Group> groupResult = await groupController.Get(42);

            Assert.IsInstanceOfType(groupResult.Result, typeof(OkObjectResult));
            OkObjectResult ok = (OkObjectResult)groupResult.Result;
            Group group = (Group)ok.Value;
            Assert.IsNotNull(group);
            Assert.AreEqual(SampleData.CastleGroupTitle, group.Title);
        }

        [TestMethod]
        public async Task Get_ItemNotFound_NotFoundStatusReturned()
        {
            var service = new Mock<IGroupService>();
            service.Setup(service => service.FetchByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Group>(null!));
            var groupController = new GroupController(service.Object);

            ActionResult<Group> groupResult = await groupController.Get(42);

            Assert.IsInstanceOfType(groupResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post_ItemPosted_Success()
        {
            var service = new Mock<IGroupService>();
            var group = SampleData.CreateCastleGroup();
            Group returnedGroup = new MockGroup(42, SampleData.CastleGroupTitle);
            service.Setup(service => service.InsertAsync(group))
                .Returns(Task.FromResult(returnedGroup));
            GroupController groupController = new GroupController(service.Object);

            ActionResult<Group> actionResult = await groupController.Post(group);
            Group groupResult = actionResult.Value;

            Assert.IsNotNull(groupResult);
            Assert.AreEqual(SampleData.CastleGroupTitle, groupResult.Title);
            Assert.AreEqual(returnedGroup.Id, groupResult.Id);
        }


        [TestMethod]
        public async Task Put_ItemUpdated_Success()
        {
            var service = new Mock<IGroupService>();
            Group? group = (Group?)SampleData.CreateCastleGroup();
            service.Setup(service => service.UpdateAsync(42, group))
                .Returns(Task.FromResult(group));
            var groupController = new GroupController(service.Object);

            ActionResult<Group> actionResult = await groupController.Put(42, group);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            OkObjectResult ok = (OkObjectResult)actionResult.Result;
            Group returnedGroup = (Group)ok.Value;
            Assert.IsNotNull(returnedGroup);
            Assert.AreEqual(SampleData.CastleGroupTitle, returnedGroup.Title);
        }

        [TestMethod]
        public async Task Put_ItemNotFound_ReturnsNotFound()
        {
            var service = new Mock<IGroupService>();
            Group group = SampleData.CreateCastleGroup();
            service.Setup(service => service.UpdateAsync(42, group))
                .Returns(Task.FromResult<Group?>(null!));
            var groupController = new GroupController(service.Object);

            ActionResult<Group> actionResult = await groupController.Put(42, group);

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ItemRemoved_ReturnsTrue()
        {
            var service = new Mock<IGroupService>();
            service.Setup(service => service.DeleteAsync(42))
                .Returns(Task.FromResult(true));
            var groupController = new GroupController(service.Object);

            bool result = await groupController.Delete(42);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Delete_ItemNotRemoved_ReturnsFalse()
        {
            var service = new Mock<IGroupService>();
            service.Setup(service => service.DeleteAsync(42))
                .Returns(Task.FromResult(false));
            var groupController = new GroupController(service.Object);

            bool result = await groupController.Delete(42);

            Assert.IsFalse(result);
        }


        private class MockGroup : Group
        {
            public MockGroup(int id, string title) : base(title)
            {
                Id = id;
            }
        }
    }

}