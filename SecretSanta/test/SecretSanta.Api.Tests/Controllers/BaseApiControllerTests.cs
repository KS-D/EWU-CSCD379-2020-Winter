using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretSanta.Business;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public abstract class BaseApiControllerTests<TEntity, TService, TDto, TInputDto> 
        where TEntity : EntityBase
        where TService : InMemoryEntityService<TEntity, TDto,TInputDto>, new()
        where TDto : class, TInputDto 
        where TInputDto : class

    {
        protected abstract BaseApiController<TEntity, TDto, TInputDto> CreateController(TService service);

        protected abstract TEntity CreateEntity();
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
            var service = new TService();
            service.Items.Add(CreateEntity());
            service.Items.Add(CreateEntity());
            service.Items.Add(CreateEntity());

            var controller = CreateController(service);

            var items = await controller.Get();

            CollectionAssert.AreEqual(service.Items.ToList(), items.ToList());
        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsNotFound()
        {
            var service = new TService();
            var controller = CreateController(service);

            var result = await controller.Get(1);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            var service = new TService();
            var entity = CreateEntity();
            service.Items.Add(entity);
            var controller = CreateController(service);

            var result = await controller.Get(entity.Id);

            var okResult = result as OkObjectResult;
            
            Assert.AreEqual(entity, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            var service = new TService();
            var entity1 = CreateEntity();
            service.Items.Add(entity1);
            var entity2 = CreateEntity();
            var controller = CreateController(service);
            var entityInput = Mapper.Map<TEntity, TInputDto>(entity2);

            var result = await controller.Put(entity1.Id, entityInput);

            Assert.AreEqual(entity2, result);
            Assert.AreEqual(entity2, service.Items.Single());
        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            var service = new TService();
            var entity = CreateEntity();
            BaseApiController<TEntity, TDto, TInputDto> controller = CreateController(service);
            var entityInput = Mapper.Map<TEntity, TInputDto>(entity);
            
            var result = await controller.Post(entityInput);

            Assert.AreEqual(entity, result);
            Assert.AreEqual(entity, service.Items.Single());
        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            var service = new TService();
            BaseApiController<TEntity, TDto, TInputDto> controller = CreateController(service);

            var result = await controller.Delete(1);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            var service = new TService();
            var entity = CreateEntity();
            service.Items.Add(entity);
            BaseApiController<TEntity, TDto, TInputDto> controller = CreateController(service);

            var result = await controller.Delete(entity.Id);

            Assert.IsTrue(result is OkResult);
        }

        private class ThrowingController : BaseApiController<TEntity, TDto, TInputDto>
        {
            public ThrowingController() : base(null!)
            { }
        }
    }

    public class InMemoryEntityService<TEntity, TDto, TInputDto> : IEntityService<TDto, TInputDto>  
        where TEntity : EntityBase
        where TInputDto : class
        where TDto : class, TInputDto

    {
        private IEntityService<TDto, TInputDto> _entityServiceImplementation;
        public IList<TEntity> Items { get; } = new List<TEntity>();

        public Task<TDto?> UpdateAsync(int id, TInputDto entity)
        {
            return _entityServiceImplementation.UpdateAsync(id, entity);
        }

        public Task<bool> DeleteAsync(int id)
        {
            if (Items.FirstOrDefault(x => x.Id == id) is { } found)
            {
                return Task.FromResult(Items.Remove(found));
            }
            return Task.FromResult(false);
        }

        public Task<List<TEntity>> FetchAllAsync()
        {
            return Task.FromResult(Items.ToList());
        }

        Task<TDto> IEntityService<TDto, TInputDto>.FetchByIdAsync(int id)
        {
            return _entityServiceImplementation.FetchByIdAsync(id);
        }

        public Task<TDto> InsertAsync(TInputDto entity)
        {
            return _entityServiceImplementation.InsertAsync(entity);
        }

        Task<List<TDto>> IEntityService<TDto, TInputDto>.FetchAllAsync()
        {
            return _entityServiceImplementation.FetchAllAsync();
        }

        public Task<TEntity> FetchByIdAsync(int id)
        {
            return Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
        }

        public Task<TEntity> InsertAsync(TEntity entity)
        {
            Items.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            if (Items.FirstOrDefault(x => x.Id == id) is { } found)
            {
                Items[Items.IndexOf(found)] = entity;
                return Task.FromResult<TEntity>(entity);
            }
            return Task.FromResult(default(TEntity));
        }
    }
}
