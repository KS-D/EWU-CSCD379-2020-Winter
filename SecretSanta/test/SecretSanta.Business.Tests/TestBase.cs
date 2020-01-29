
using AutoMapper;
using BlogEngine.Business;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Business.Tests
{
    public class TestBase : Data.Tests.TestBase
    {
#nullable disable
        protected IMapper Mapper { get; private set; }
#nullable enable

        [TestInitialize]
        public override void InitializeTests()
        {
            base.InitializeTests();
            Mapper = AutomapperProfileConfiguration.CreateMapper();
        }
    }
}
