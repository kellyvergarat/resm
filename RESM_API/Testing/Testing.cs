using Moq;
using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using RESM_API.Model;
using RestAPI_farmersMarket.Controllers;
using RestAPI_farmersMarket.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RESM_API.Testing
{
    [TestFixture]
    public class Testing
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000;";
        private Mock<IConfiguration> _configurationMock;
        private PropertiesController _propertiesController;
        HttpClient httpClient;

        [SetUp]
        public void Setup()
        {
            // Create a mock configuration object
            _configurationMock = new Mock<IConfiguration>();

            // Create an instance of the PropertiesController and pass the mock configuration object
            _propertiesController = new PropertiesController(_configurationMock.Object);
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7103/Properties");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Test]
        public void InsertDataUsingADO_Success()
        {
            // Arrange
            var property = new Property
            {
                Id = 15,
                ImageUrl = "https://example.com/image.jpg",
                PricePerMonth = 1000,
                Bathrooms = 2,
                Rooms = 3,
                Owner = "John Doe",
                Pourpose = "Residential",
                Status = true,
                Address = "123 Main St",
                City = "New York",
                Province = "NY",
                Realtor = "Jane Smith"
            };

            // Act
            var connection = new NpgsqlConnection(ConnectionString);
            var application = new Application();
            var response = application.AddProperty(connection, property);

            // Assert
            Assert.AreEqual(200, response.statusCode);

            // Retrieve the inserted data and assert its values
            var insertedProperty = RetrievePropertyFromDatabase(connection, property.Id);
            Assert.AreEqual(property.Id, insertedProperty.Id);
            Assert.AreEqual(property.ImageUrl, insertedProperty.ImageUrl);
            Assert.AreEqual(property.PricePerMonth, insertedProperty.PricePerMonth);
            Assert.AreEqual(property.Bathrooms, insertedProperty.Bathrooms);
            Assert.AreEqual(property.Rooms, insertedProperty.Rooms);
            Assert.AreEqual(property.Owner, insertedProperty.Owner);
            Assert.AreEqual(property.Pourpose, insertedProperty.Pourpose);
            Assert.AreEqual(property.Status, insertedProperty.Status);
            Assert.AreEqual(property.Address, insertedProperty.Address);
            Assert.AreEqual(property.City, insertedProperty.City);
            Assert.AreEqual(property.Province, insertedProperty.Province);
            Assert.AreEqual(property.Realtor, insertedProperty.Realtor);
        }

        [Test]
        public void InsertDataUsingAPI_Success()
        {
            // Arrange
            var property = new Property
            {
                Id = 17,
                ImageUrl = "https://example.com/image2.jpg",
                PricePerMonth = 2000,
                Bathrooms = 3,
                Rooms = 4,
                Owner = "Jane Doe",
                Pourpose = "Commercial",
                Status = true,
                Address = "456 Elm St",
                City = "Los Angeles",
                Province = "CA",
                Realtor = "John Smith"
            };

            // Act
            var response = httpClient.PostAsJsonAsync("AddProperty", property).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Assert
            var retrievedResponse = httpClient.GetAsync($"GetPropertyByID/{property.Id}").Result;
            Assert.IsTrue(retrievedResponse.IsSuccessStatusCode);

            var content = retrievedResponse.Content.ReadAsStringAsync().Result;
            var retrievedProperty = JsonConvert.DeserializeObject<Property>(content);

            Assert.AreEqual(property.Id, retrievedProperty.Id);
            Assert.AreEqual(property.ImageUrl, retrievedProperty.ImageUrl);
            Assert.AreEqual(property.PricePerMonth, retrievedProperty.PricePerMonth);
            Assert.AreEqual(property.Bathrooms, retrievedProperty.Bathrooms);
            Assert.AreEqual(property.Rooms, retrievedProperty.Rooms);
            Assert.AreEqual(property.Owner, retrievedProperty.Owner);
            Assert.AreEqual(property.Pourpose, retrievedProperty.Pourpose);
            Assert.AreEqual(property.Status, retrievedProperty.Status);
            Assert.AreEqual(property.Address, retrievedProperty.Address);
            Assert.AreEqual(property.City, retrievedProperty.City);
            Assert.AreEqual(property.Province, retrievedProperty.Province);
            Assert.AreEqual(property.Realtor, retrievedProperty.Realtor);
        }


        [Test]
        public void UpdateDataUsingADO_Success()
        {
            // Arrange
            var property = new Property
            {
                Id = 1,
                ImageUrl = "https://example.com/updated.jpg",
                PricePerMonth = 1500,
                Bathrooms = 2,
                Rooms = 3,
                Owner = "Updated Owner",
                Pourpose = "Updated Purpose",
                Status = true,
                Address = "Updated Address",
                City = "Updated City",
                Province = "Updated Province",
                Realtor = "Updated Realtor"
            };

            // Act
            var connection = new NpgsqlConnection(ConnectionString);
            var application = new Application();
            var response = application.UpdateProperty(connection, property);

            // Assert
            Assert.AreEqual(200, response.statusCode);

            // Retrieve the updated data and assert its values
            var updatedProperty = RetrievePropertyFromDatabase(connection, property.Id);
            Assert.AreEqual(property.Id, updatedProperty.Id);
            Assert.AreEqual(property.ImageUrl, updatedProperty.ImageUrl);
            Assert.AreEqual(property.PricePerMonth, updatedProperty.PricePerMonth);
            Assert.AreEqual(property.Bathrooms, updatedProperty.Bathrooms);
            Assert.AreEqual(property.Rooms, updatedProperty.Rooms);
            Assert.AreEqual(property.Owner, updatedProperty.Owner);
            Assert.AreEqual(property.Pourpose, updatedProperty.Pourpose);
            Assert.AreEqual(property.Status, updatedProperty.Status);
            Assert.AreEqual(property.Address, updatedProperty.Address);
            Assert.AreEqual(property.City, updatedProperty.City);
            Assert.AreEqual(property.Province, updatedProperty.Province);
            Assert.AreEqual(property.Realtor, updatedProperty.Realtor);
        }

        [Test]
        public void UpdateDataUsingAPI_Success()
        {
            // Arrange
            var property = new Property
            {
                Id = 2,
                ImageUrl = "https://example.com/updated2.jpg",
                PricePerMonth = 2500,
                Bathrooms = 3,
                Rooms = 4,
                Owner = "Updated Owner 2",
                Pourpose = "Updated Purpose 2",
                Status = true,
                Address = "Updated Address 2",
                City = "Updated City 2",
                Province = "Updated Province 2",
                Realtor = "Updated Realtor 2"
            };

            // Convert the property to JSON
            var jsonProperty = JsonConvert.SerializeObject(property);
            var httpContent = new StringContent(jsonProperty, Encoding.UTF8, "application/json");

            // Set the content type header
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = httpClient.PutAsync("UpdateProperty", httpContent).Result;
            var responseData = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Response>(responseData);

            // Assert
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(200, result.statusCode);

            // Retrieve the updated data using GET request and assert its values
            var retrievedResponse = _propertiesController.GetPropertyById(property.Id);
            Assert.AreEqual(200, retrievedResponse.statusCode);
            Assert.AreEqual(property.Id, retrievedResponse.property.Id);
            Assert.AreEqual(property.ImageUrl, retrievedResponse.property.ImageUrl);
            Assert.AreEqual(property.PricePerMonth, retrievedResponse.property.PricePerMonth);
            Assert.AreEqual(property.Bathrooms, retrievedResponse.property.Bathrooms);
            Assert.AreEqual(property.Rooms, retrievedResponse.property.Rooms);
            Assert.AreEqual(property.Owner, retrievedResponse.property.Owner);
            Assert.AreEqual(property.Pourpose, retrievedResponse.property.Pourpose);
            Assert.AreEqual(property.Status, retrievedResponse.property.Status);
            Assert.AreEqual(property.Address, retrievedResponse.property.Address);
            Assert.AreEqual(property.City, retrievedResponse.property.City);
            Assert.AreEqual(property.Province, retrievedResponse.property.Province);
            Assert.AreEqual(property.Realtor, retrievedResponse.property.Realtor);
        }

        [Test]
        public void DeleteDataUsingADO_Success()
        {
            // Arrange
            var propertyId = 1;

            // Act
            var connection = new NpgsqlConnection(ConnectionString);
            var application = new Application();
            var response = application.DeleteProperty(connection, propertyId);

            // Assert
            Assert.AreEqual(200, response.statusCode);

            // Verify the non-existence of the deleted data using SELECT query
            var deletedProperty = RetrievePropertyFromDatabase(connection, propertyId);
            Assert.IsNull(deletedProperty);
        }
        [Test]
        public void DeleteDataUsingAPI_Success()
        {
            // Arrange
            int propertyId = 2;

            // Act
            var response = httpClient.DeleteAsync($"DeleteProperty/{propertyId}").Result;
            var responseData = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Response>(responseData);

            // Assert
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(200, result.statusCode);

            // Verify the non-existence of the deleted data using GET request
            var retrievedResponse = httpClient.GetAsync($"GetPropertyByID/{propertyId}").Result;
            var retrievedData = retrievedResponse.Content.ReadAsStringAsync().Result;
            var retrievedResult = JsonConvert.DeserializeObject<Response>(retrievedData);

            Assert.AreEqual(100, (int)retrievedResponse.StatusCode);
            Assert.IsNull(retrievedResult.property);
        }

        private Property RetrievePropertyFromDatabase(NpgsqlConnection connection, int propertyId)
        {
            var query = $"SELECT * FROM property WHERE id = {propertyId}";

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var property = new Property
                        {
                            Id = (int)reader["id"],
                            ImageUrl = (string)reader["imageurl"],
                            PricePerMonth = (double)reader["pricepermonth"],
                            Bathrooms = (int)reader["bathrooms"],
                            Rooms = (int)reader["rooms"],
                            Owner = (string)reader["owner"],
                            Pourpose = (string)reader["pourpose"],
                            Status = (bool)reader["status"],
                            Address = (string)reader["address"],
                            City = (string)reader["city"],
                            Province = (string)reader["province"],
                            Realtor = (string)reader["realtor"]
                        };

                        return property;
                    }
                }
            }

            return null;
        }
    }
}
