using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    public abstract class EntityServiceBaseTest<TEntity> : TestBase where TEntity : EntityBase
    {
        public abstract (TEntity entity, TEntity secondEntity) GetEntities();

        public abstract EntityService<TEntity> GetService(ApplicationDbContext dbContext);

        public abstract void AssertEntitiesAreEqual(TEntity expected, TEntity actual);

        public abstract TEntity UpdateEntity(TEntity entity, string update);

        [TestMethod]
        public async Task InsertAsync_TwoEntities_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);
            (TEntity entity, TEntity secondEntity) = GetEntities();

            await service.InsertAsync(entity);
            await service.InsertAsync(secondEntity);

            Assert.IsNotNull(entity.Id);
            Assert.IsNotNull(secondEntity.Id);
        }

        [TestMethod]
        public async Task Update_EntityUpdated_ShouldSaveToDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);
            (TEntity entity, TEntity secondEntity) = GetEntities();

            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.Set<TEntity>().AddAsync(secondEntity);
            await dbContext.SaveChangesAsync();
            
            using var dbContextFetch = new ApplicationDbContext(Options);
            TEntity updateEntityFromDb = await dbContextFetch.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == entity.Id);

            updateEntityFromDb = UpdateEntity(updateEntityFromDb, "This was updated");

            await service.UpdateAsync(secondEntity.Id!.Value, updateEntityFromDb);

            using var dbContextAssert = new ApplicationDbContext(Options);
            TEntity entityFromDb = await dbContextAssert.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == entity.Id);
            TEntity secondEntityFromDb = await dbContextAssert.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == secondEntity.Id);

            AssertEntitiesAreEqual(entity, entityFromDb);
            AssertEntitiesAreEqual(updateEntityFromDb, secondEntityFromDb);
        }

        [TestMethod]
        public async Task Delete_OneEntity_RemovesOnlyOne()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);
            (TEntity entity, TEntity secondEntity) = GetEntities();

            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.Set<TEntity>().AddAsync(secondEntity);
            await dbContext.SaveChangesAsync();
            
            bool deleted = await service.DeleteAsync(entity.Id!.Value);
            using var dbContextAssert = new ApplicationDbContext(Options);
            TEntity entityFromDb =  await dbContextAssert.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == entity.Id);
            TEntity secondEntityFromDb = await dbContextAssert.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == secondEntity.Id);

            Assert.IsTrue(deleted);
            Assert.IsNull(entityFromDb);
            AssertEntitiesAreEqual(secondEntity, secondEntityFromDb);
        }

        [TestMethod]
        public async Task Delete_IdNotFound_ReturnsFalse()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);

            bool deleted = await service.DeleteAsync(42);

            Assert.IsFalse(deleted);
        }

        [TestMethod]
        public async Task FetchAll_RetrievesAllEntities_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);
            (TEntity entity, TEntity secondEntity) = GetEntities();

            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.Set<TEntity>().AddAsync(secondEntity);
            await dbContext.SaveChangesAsync();

            List<TEntity> entities = await service.FetchAllAsync();

            TEntity entityFromDb = entities[0];
            TEntity secondEntityFromDb = entities[1];

            AssertEntitiesAreEqual(entity, entityFromDb);
            AssertEntitiesAreEqual(secondEntity, secondEntityFromDb);
        }

        [TestMethod]
        public async Task FetchAll_EmptyDataBase_ReturnsEmpty()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);

            List<TEntity> entities = await service.FetchAllAsync();

            Assert.AreEqual(0, entities.Count);

            
        }

        [TestMethod]
        public async Task Fetch_RetrievesOneEntity_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);
            (TEntity entity, TEntity secondEntity) = GetEntities();

            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.Set<TEntity>().AddAsync(secondEntity);
            await dbContext.SaveChangesAsync();

            TEntity entityFromDb = await service.FetchByIdAsync(entity.Id!.Value);

            AssertEntitiesAreEqual(entity, entityFromDb);
        }

        [TestMethod]
        public async Task Fetch_IdNotFound_ReturnNulls()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<TEntity> service = GetService(dbContext);

            TEntity entity = await service.FetchByIdAsync(42);

            Assert.IsNull(entity);
        }
    }
}
