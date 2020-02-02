﻿using Microsoft.AspNetCore.Mvc;
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
    public class UserControllerTests
    {

        [TestMethod]
        public void Create_Controller_Success()
        {
            var userController = new UserController(new Mock<IUserService>().Object);

            Assert.IsNotNull(userController);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_PassedNullService_ThrowsError()
        {
            var userController = new UserController(null!);
        }

        [TestMethod]
        public async Task Get_EntitiesInDatabase_ReturnListOfEntities()
        {
            var service = new Mock<IUserService>();
            service.Setup(service => service.FetchAllAsync())
                .Returns(Task.FromResult(new List<User>{SampleData.CreateUserInigo(), SampleData.CreateUserButtercup()}));
            var userController = new UserController(service.Object);

            List<User> users = (List<User>)await userController.Get();

            Assert.IsNotNull(users);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), (users[0].FirstName, users[0].LastName));
            Assert.AreEqual((SampleData.Princess, SampleData.Buttercup), (users[1].FirstName, users[1].LastName));
        }

        [TestMethod]
        public async Task Get_ItemById_Success()
        {
            var service = new Mock<IUserService>();
            service.Setup(service => service.FetchByIdAsync(42))
                .Returns(Task.FromResult(SampleData.CreateUserInigo()));
            var userController = new UserController(service.Object);

            ActionResult<User> inigoResult = await userController.Get(42);

            Assert.IsInstanceOfType(inigoResult.Result, typeof(OkObjectResult));
            OkObjectResult ok = (OkObjectResult)inigoResult.Result;
            User inigo = (User)ok.Value;
            Assert.IsNotNull(inigo);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), (inigo.FirstName, inigo.LastName));
        }

        [TestMethod]
        public async Task Get_ItemNotFound_NotFoundStatusReturned()
        {
            var service = new Mock<IUserService>();
            service.Setup(service => service.FetchByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(null!));
            var userController = new UserController(service.Object);

            ActionResult<User> inigoResult = await userController.Get(42);
    
            Assert.IsInstanceOfType(inigoResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post_ItemPosted_Success()
        {
            var service = new Mock<IUserService>();
            var user = SampleData.CreateUserInigo();
            User returnedUser = new MockUser(42, SampleData.Inigo, SampleData.Montoya);
            service.Setup(service => service.InsertAsync(user))
                .Returns(Task.FromResult(returnedUser));
            UserController userController = new UserController(service.Object);

            ActionResult<User> actionResult = await userController.Post(user);
            User userResult = actionResult.Value;
                
            Assert.IsNotNull(userResult);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya),
                (userResult.FirstName, userResult.LastName));
            Assert.AreEqual(returnedUser.Id, userResult.Id);
        }


        [TestMethod]
        public async Task Put_ItemUpdated_Success()
        {
            var service = new Mock<IUserService>();
            User? user = (User?) SampleData.CreateUserInigo();
            service.Setup(service => service.UpdateAsync(42, user))
                .Returns(Task.FromResult(user));
            var userController = new UserController(service.Object);

            ActionResult<User> actionResult = await userController.Put(42, user);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            OkObjectResult ok = (OkObjectResult)actionResult.Result;
           // IHttpActionResult actionResult = controller.Delete(10);
            User returnedUser = (User)ok.Value;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya),
               (returnedUser.FirstName, returnedUser.LastName));
        }

        [TestMethod]
        public async Task Put_ItemNotFound_ReturnsNotFound()
        {
            var service = new Mock<IUserService>();
            User user = SampleData.CreateUserInigo();
            service.Setup(service => service.UpdateAsync(42, user))
                .Returns(Task.FromResult<User?>(null!));
            var userController = new UserController(service.Object);
            
            ActionResult<User> actionResult = await userController.Put(42, user);

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ItemRemoved_ReturnsTrue()
        {
            var service = new Mock<IUserService>();
            service.Setup(service => service.DeleteAsync(42))
                .Returns(Task.FromResult(true));
            var userController = new UserController(service.Object);

            bool result = await userController.Delete(42);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Delete_ItemNotRemoved_ReturnsFalse()
        {
            var service = new Mock<IUserService>();
            service.Setup(service => service.DeleteAsync(42))
                .Returns(Task.FromResult(false));
            var userController = new UserController(service.Object);

            bool result = await userController.Delete(42);

            Assert.IsFalse(result);
        }


        private class MockUser : User
        {
            public MockUser(int id ,string firstName, string lastName) : base(firstName, lastName)
            {
                Id = id;
            }
        }

    }
}
