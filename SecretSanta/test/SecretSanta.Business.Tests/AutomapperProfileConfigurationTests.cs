using AutoMapper;
using SecretSanta.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    class AutomapperProfileConfigurationTests
    {
        class MockUser : User
        {
            public MockUser(int id, string firstName, string lastName) : base(firstName, lastName)
            {
                base.Id = id;
            }
        }
    }
}
