using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Api.Tests.Controller
{
    [TestClass]
    public class UserControllerTests 
    {

        [TestMethod]
        public void Create_UserController_Success()
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
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), (inigo.FirstName, inigo.LastName));
        }
    }
}
