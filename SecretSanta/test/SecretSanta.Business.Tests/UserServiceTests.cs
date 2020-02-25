using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserServiceTests : EntityServiceTests<Dto.User, Dto.UserInput, Data.User>
    {
        protected override Data.User CreateEntity()
            => new Data.User(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());

        protected override UserInput CreateInputDto()
        {
            return new UserInput
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
        }

        protected override IEntityService<Dto.User, Dto.UserInput> GetService(ApplicationDbContext dbContext,
            IMapper mapper)
            => new UserService(dbContext, mapper);

        public override async Task FetchAllAsync_ReturnsAllItems()
        {
            var item1 = CreateEntity();
            var item2 = CreateEntity();
            var item3 = CreateEntity();
            using var setupContext = new ApplicationDbContext(Options);
            setupContext.Add(item1);
            setupContext.Add(item2);
            setupContext.Add(item3);
            setupContext.SaveChanges();

            using var dbContext = new ApplicationDbContext(Options);
            IEntityService<Dto.User, UserInput> service = GetService(dbContext, Mapper);

            // Act
            List<Dto.User> items = await service.FetchAllAsync();

            // Assert
            CollectionAssert.AreEquivalent(new[]
                {
                    1,
                    item1.Id,
                    item2.Id,
                    item3.Id
                },
                items.Select(x => x.Id).ToArray());
        }
    }
}