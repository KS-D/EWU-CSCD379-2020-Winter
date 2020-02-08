using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public abstract class BaseApiControllerTests<TService, TDto, TInputDto> 
        where TService : class, IEntityService<TDto, TInputDto>
        where TInputDto : class
        where TDto : class, TInputDto, IEntity 

    {
        protected abstract BaseApiController<TDto, TInputDto> CreateController(TService service);

        protected abstract TDto CreateEntity();
        private IMapper Mapper { get; } = AutomapperConfigurationProfile.CreateMapper();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RequiresService()
        {
            new ThrowingController();
        }

        [TestMethod]
        public async Task Get_FetchesAllItems()
        {
            var service = new Mock<TService>();
            List<TDto> getValues = new List<TDto>
            {

            };
           
            var controller = CreateController(service.Object);

            var items = await controller.Get();

        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsNotFound()
        {
            var service = new Mock<TService>();
            var controller = CreateController(service.Object);

            var result = await controller.Get(1);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            var service = new Mock<TService>();
            var entity = CreateEntity();
            
            var controller = CreateController(service.Object);

            var result = await controller.Get(entity.Id);

            var okResult = result as OkObjectResult;
            
            Assert.AreEqual(entity, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            var service = new Mock<TService>();
            var entity1 = CreateEntity();
            var entity2 = CreateEntity();
            var controller = CreateController(service.Object);
            var entityInput = Mapper.Map<TDto, TInputDto>(entity2);

            var result = await controller.Put(entity1.Id, entityInput);

            Assert.AreEqual(entity2, result);
        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            var service = new Mock<TService>();
            var entity = CreateEntity();
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);
            var entityInput = Mapper.Map<TDto, TInputDto>(entity);
            
            var result = await controller.Post(entityInput);

            Assert.AreEqual(entity, result);
        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            var service = new Mock<TService>();
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            var result = await controller.Delete(1);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            var service = new Mock<TService>();
            var entity = CreateEntity();
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            var result = await controller.Delete(entity.Id);

            Assert.IsTrue(result is OkResult);
        }

        private class ThrowingController : BaseApiController<TDto, TInputDto>
        {
            public ThrowingController() : base(null!)
            { }
        }
    }
}

