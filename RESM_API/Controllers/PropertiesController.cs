using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestAPI_farmersMarket.Models;
using RESM_API.Model;

namespace RestAPI_farmersMarket.Controllers
{
    public class PropertiesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PropertiesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProperties")]
        public Response GetAllProperties()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("PropertyConnection").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.GetAllProperties(connection);
            return response;
        }

        [HttpGet]
        [Route("GetPropertyById/{id}")]
        public Response GetPropertyById(int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("PropertyConnection").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.GetPropertyById(id, connection);
            return response;
        }

        [HttpPost]
        [Route("AddProperty")]
        public Response AddProperty( Property property)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("PropertyConnection").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.AddProperty(connection, property);
            return response;
        }

        [HttpPut]
        [Route("UpdateProperty")]
        public Response UpdateProperty( Property property)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("PropertyConnection").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.UpdateProperty(connection, property);
            return response;
        }

        [HttpDelete]
        [Route("DeleteProperty/{id}")]
        public Response DeleteProperty(int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("PropertyConnection").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.DeleteProperty(connection, id);
            return response;
        }

    }
}
