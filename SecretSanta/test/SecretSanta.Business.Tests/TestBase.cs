using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SecretSanta.Data;
using BlogEngine.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    public class TestBase : SecretSanta.Data.Tests.TestBase
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
