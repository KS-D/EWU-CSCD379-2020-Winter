using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Business;
using SecretSanta.Business.Dto;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Gift = SecretSanta.Data.Gift;
using User = SecretSanta.Data.User;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests
    {
        private SecretSantaWebApplicationFactory Factory { get; set; }
        private HttpClient Client { get; set; }
        private IMapper Mapper { get; } = AutomapperConfigurationProfile.CreateMapper();

        [TestInitialize]
        public void TestSetup()
        {
            Factory = new SecretSantaWebApplicationFactory();

            using ApplicationDbContext context = Factory.GetDbContext();
            context.Database.EnsureCreated();

            Client = Factory.CreateClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Factory.Dispose();
        }

        [TestMethod]
        public async Task Get_ReturnsGift()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift gift = SampleData.CreateRingGift();
            context.Gifts.Add(gift);
            context.SaveChanges();
            var uri = new Uri("api/Gift", UriKind.RelativeOrAbsolute);

            HttpResponseMessage response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Business.Dto.Gift[] gifts =
                JsonSerializer.Deserialize<Business.Dto.Gift[]>(jsonData, options);
            Assert.AreEqual(1, gifts.Length);
            
            Assert.AreEqual(gift.Id, gifts[0].Id);
            Assert.AreEqual(gift.Title, gifts[0].Title);
            Assert.AreEqual(gift.Description, gifts[0].Description);
            Assert.AreEqual(gift.Url, gifts[0].Url);
        }

        [TestMethod]
        public async Task Get_ById_Ok()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift gift = SampleData.CreateRingGift();
            context.Gifts.Add(gift);
            context.SaveChanges();
            var uri = new Uri($"api/Gift/{gift.Id}", UriKind.RelativeOrAbsolute);

            HttpResponseMessage response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Business.Dto.Gift returnedGift 
                = JsonSerializer.Deserialize < Business.Dto.Gift>(jsonData, options);

            Assert.AreEqual(gift.Id, returnedGift.Id);
            Assert.AreEqual(gift.Title, returnedGift.Title);
            Assert.AreEqual(gift.Description, returnedGift.Description);
            Assert.AreEqual(gift.Url, returnedGift.Url);
        }
        
        [TestMethod]
        public async Task Get_ByIdNotFound_NotFound()
        {
            var uri = new Uri($"api/Gift/1", UriKind.RelativeOrAbsolute);

            HttpResponseMessage response = await Client.GetAsync(uri);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {

            GiftInput gift = Mapper.Map<Gift, Business.Dto.Gift>(SampleData.CreateRingGift());
            string jsonData = JsonSerializer.Serialize(gift);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var uri = new Uri("api/Gift/42", UriKind.RelativeOrAbsolute);

            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithId_200Ok()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift gift = SampleData.CreateRingGift();
            context.Gifts.Add(gift);
            context.SaveChanges();
            GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(gift);
            var uri = new Uri("api/Gift/1", UriKind.RelativeOrAbsolute);
            giftInput.Title += "changed";
            string jsonData = JsonSerializer.Serialize(giftInput);
            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string returnedJson = await response.Content.ReadAsStringAsync();
            using ApplicationDbContext assertContext = Factory.GetDbContext();
            Data.Gift giftFromDb = await assertContext.Gifts.Where(g => g.Id == gift.Id).SingleOrDefaultAsync();
            Business.Dto.Gift responseGift =
                JsonSerializer.Deserialize<Business.Dto.Gift>(returnedJson, options);
            Assert.AreEqual(giftInput.Title, responseGift.Title);
            Assert.AreEqual(giftInput.Description, responseGift.Description);
            Assert.AreEqual(giftInput.Url, responseGift.Url);
            Assert.AreEqual(giftInput.Description, responseGift.Description);
            Assert.AreEqual(giftFromDb.Title, responseGift.Title);
            Assert.AreEqual(giftFromDb.Description, responseGift.Description);
            Assert.AreEqual(giftFromDb.Url, responseGift.Url);
            Assert.AreEqual(giftFromDb.Description, responseGift.Description);
        }

        [TestMethod]
        [DataRow(nameof(Business.Dto.GiftInput.Title))]
        [DataRow(nameof(Business.Dto.GiftInput.UserId))]
        public async Task Put_WithoutRequiredField_BadRequest(string propertyName)
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift gift = SampleData.CreateRingGift();
            context.Gifts.Add(gift);
            context.SaveChanges();
            GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(gift);

            System.Type inputType = typeof(Business.Dto.GiftInput);
            PropertyInfo? propInfo = inputType.GetProperty(propertyName);
            propInfo!.SetValue(giftInput, null);

            var uri = new Uri($"api/Gift/{gift.Id}", UriKind.RelativeOrAbsolute);
            string jsonData = JsonSerializer.Serialize(giftInput);
            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [DataRow(nameof(Business.Dto.GiftInput.Title))]
        [DataRow(nameof(Business.Dto.GiftInput.UserId))]
        public async Task Post_WithoutRequiredField_NotFound(string propertyName)
        {
            Gift entity = SampleData.CreateRingGift();

            Business.Dto.GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(entity);
            System.Type inputType = typeof(Business.Dto.GiftInput);
            PropertyInfo? propInfo = inputType.GetProperty(propertyName);
            propInfo!.SetValue(giftInput, null);

            string jsonData = JsonSerializer.Serialize(giftInput);
            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var uri = new Uri($"api/Gift/", UriKind.RelativeOrAbsolute);

            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_ValidInput_Success()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            User user = SampleData.CreateUserInigo();
            context.Users.Add(user);
            context.SaveChanges();
            Gift gift = SampleData.CreateRingGift();

            Business.Dto.GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(gift);
            giftInput.UserId = user.Id;

            string jsonData = JsonSerializer.Serialize(giftInput);
            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var uri = new Uri($"api/Gift/", UriKind.RelativeOrAbsolute);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

             Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string returnedJson = await response.Content.ReadAsStringAsync();
            using ApplicationDbContext assertContext = Factory.GetDbContext();
            Business.Dto.Gift responseGift =
                JsonSerializer.Deserialize<Business.Dto.Gift>(returnedJson, options);
            Data.Gift giftFromDb = await assertContext.Gifts.Where(g => g.Id == responseGift.Id).SingleOrDefaultAsync();
            Assert.AreEqual(giftInput.Title, responseGift.Title);
            Assert.AreEqual(giftInput.Description, responseGift.Description);
            Assert.AreEqual(giftInput.Url, responseGift.Url);
            Assert.AreEqual(giftInput.Description, responseGift.Description);
            Assert.AreEqual(giftFromDb.Title, responseGift.Title);
            Assert.AreEqual(giftFromDb.Description, responseGift.Description);
            Assert.AreEqual(giftFromDb.Url, responseGift.Url);
            Assert.AreEqual(giftFromDb.Description, responseGift.Description);
        }

        [TestMethod]
        public async Task Delete_IdFound_DeleteSuccessful()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift gift = SampleData.CreateRingGift();
            context.Gifts.Add(gift);
            context.SaveChanges();
            var uri = new Uri($"api/Gift/{gift.Id}", UriKind.RelativeOrAbsolute);
            
            HttpResponseMessage response = await Client.DeleteAsync(uri);

            response.EnsureSuccessStatusCode();
            using ApplicationDbContext assertContext = Factory.GetDbContext();
            Gift giftFromDb = await assertContext.Gifts.Where(g => g.Id == gift.Id).SingleOrDefaultAsync();
            Assert.IsNull(giftFromDb);
        }

        [TestMethod]
        public async Task Delete_IdNotFound_NotFound()
        {
            var uri = new Uri("api/Gift/1", UriKind.RelativeOrAbsolute);

            HttpResponseMessage response = await Client.DeleteAsync(uri);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


    }
}
