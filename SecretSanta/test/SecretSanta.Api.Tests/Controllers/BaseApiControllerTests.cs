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
using System.Linq;
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
            Mock<TService> service = new Mock<TService>();
            List<TDto> getValues = new List<TDto>
            {
                CreateEntity(),
                CreateEntity(),
                CreateEntity()
            };
            service.Setup(service => service.FetchAllAsync())
                .Returns(Task.FromResult(getValues));
           
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IEnumerable<TDto> items = await controller.Get();

            Assert.IsNotNull(items);
            CollectionAssert.AreEqual(getValues.ToList(), items.ToList());
        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsNotFound()
        {
            Mock<TService> service = new Mock<TService>();
            service.Setup(service => service.FetchByIdAsync(42))
                .Returns(Task.FromResult<TDto>(null!));
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Get(42);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            Mock<TService> service = new Mock<TService>();
            TDto entity = CreateEntity();
            service.Setup(service => service.FetchByIdAsync(42))
                .Returns(Task.FromResult(entity));
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Get(42);

            OkObjectResult okResult = result as OkObjectResult;
            
            Assert.AreEqual(entity, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            TDto entity1 = CreateEntity();
            TDto entity2 = CreateEntity();
            TInputDto entityInput = Mapper.Map<TDto, TInputDto>(entity2);
            Mock<TService> service = new Mock<TService>();
            service.Setup(service => service.UpdateAsync(entity1.Id, entityInput))
                .Returns(Task.FromResult<TDto?>(entity2));
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            TDto? result = await controller.Put(entity1.Id, entityInput);

            Assert.AreEqual(entity2, result);
        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            TDto entity = CreateEntity();
            TInputDto entityInput = Mapper.Map<TDto, TInputDto>(entity);
            Mock<TService> service = new Mock<TService>();
            service.Setup(service => service.InsertAsync(entityInput))
                .Returns(Task.FromResult(entity));
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);
            
            TDto result = await controller.Post(entityInput);

            Assert.AreEqual(entity, result);
        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            Mock<TService> service = new Mock<TService>();
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Delete(1);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            TDto entity = CreateEntity();
            Mock<TService> service = new Mock<TService>();
            service.Setup(service => service.DeleteAsync(entity.Id))
                .Returns(Task.FromResult(true));
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Delete(entity.Id);

            Assert.IsTrue(result is OkResult);
        }

        private class ThrowingController : BaseApiController<TDto, TInputDto>
        {
            public ThrowingController() : base(null!)
            { }
        }
    }
}

