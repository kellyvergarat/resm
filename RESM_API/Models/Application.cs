using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;
using RestAPI_farmersMarket.Models;
using RESM_API.Model;

namespace RestAPI_farmersMarket.Models
{
    public class Application
    {
        public Response GetAllProperties(NpgsqlConnection con)
        {
            string query = "SELECT * FROM property";
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            Response response = new Response();
            List<Property> listOfProperties = new List<Property>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Property property = new Property();

                    property.Id = (int)dt.Rows[i]["id"];
                    property.ImageUrl = (string)dt.Rows[i]["imageurl"];
                    property.PricePerMonth = (double)dt.Rows[i]["pricepermonth"];
                    property.Bathrooms = (int)dt.Rows[i]["bathrooms"];
                    property.Rooms = (int)dt.Rows[i]["rooms"];
                    property.Owner = (string)dt.Rows[i]["owner"];
                    property.Pourpose = (string)dt.Rows[i]["pourpose"];
                    property.Status = (bool)dt.Rows[i]["status"];
                    property.Address = (string)dt.Rows[i]["address"];
                    property.City = (string)dt.Rows[i]["city"];
                    property.Province = (string)dt.Rows[i]["province"];
                    property.Realtor = (string)dt.Rows[i]["realtor"];

                    listOfProperties.Add(property);
                }
            }

            if (listOfProperties.Count > 0)
            {
                response.statusCode = 200;
                response.statusMessage = "Data retrieval is successful";
                response.properties = listOfProperties;
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "No data retrieved.";
                response.properties = null;
            }

            return response;
        }

        public Response GetPropertyById(int id, NpgsqlConnection con)
        {
            Response response = new Response();
            string query = "SELECT * FROM property WHERE id = @Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Property property = new Property();
                property.Id = (int)dt.Rows[0]["id"];
                property.ImageUrl = (string)dt.Rows[0]["imageurl"];
                property.PricePerMonth = (double)dt.Rows[0]["pricepermonth"];
                property.Bathrooms = (int)dt.Rows[0]["bathrooms"];
                property.Rooms = (int)dt.Rows[0]["rooms"];
                property.Owner = (string)dt.Rows[0]["owner"];
                property.Pourpose = (string)dt.Rows[0]["pourpose"];
                property.Status = (bool)dt.Rows[0]["status"];
                property.Address = (string)dt.Rows[0]["address"];
                property.City = (string)dt.Rows[0]["city"];
                property.Province = (string)dt.Rows[0]["province"];
                property.Realtor = (string)dt.Rows[0]["realtor"];

                response.statusCode = 200;
                response.statusMessage = "The data retrieved successfully.";
                response.property = property;
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "No data retrieved";
                response.property = null;
            }

            return response;
        }

        public Response AddProperty(NpgsqlConnection con, Property property)
        {
            Response response = new Response();
            string query = "INSERT INTO property (imageurl, pricepermonth, bathrooms, rooms, owner, pourpose, status, address, city, province, realtor) " +
                "VALUES (@ImageUrl, @PricePerMonth, @Bathrooms, @Rooms, @Owner, @Pourpose, @Status, @Address, @City, @Province, @Realtor)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ImageUrl", property.ImageUrl);
            cmd.Parameters.AddWithValue("@PricePerMonth", property.PricePerMonth);
            cmd.Parameters.AddWithValue("@Bathrooms", property.Bathrooms);
            cmd.Parameters.AddWithValue("@Rooms", property.Rooms);
            cmd.Parameters.AddWithValue("@Owner", property.Owner);
            cmd.Parameters.AddWithValue("@Pourpose", property.Pourpose);
            cmd.Parameters.AddWithValue("@Status", property.Status);
            cmd.Parameters.AddWithValue("@Address", property.Address);
            cmd.Parameters.AddWithValue("@City", property.City);
            cmd.Parameters.AddWithValue("@Province", property.Province);
            cmd.Parameters.AddWithValue("@Realtor", property.Realtor);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                response.statusCode = 200;
                response.statusMessage = "Data entry successfully recorded.";
                response.property = property;
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "Data entry could not be recorded.";
            }

            return response;
        }

        public Response UpdateProperty(NpgsqlConnection con, Property property)
        {
            Response response = new Response();
            string query = "UPDATE property SET imageurl = @ImageUrl, pricepermonth = @PricePerMonth, bathrooms = @Bathrooms, " +
                "rooms = @Rooms, owner = @Owner, pourpose = @Pourpose, status = @Status, address = @Address, city = @City, province = @Province, " +
                "realtor = @Realtor WHERE id = @Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ImageUrl", property.ImageUrl);
            cmd.Parameters.AddWithValue("@PricePerMonth", property.PricePerMonth);
            cmd.Parameters.AddWithValue("@Bathrooms", property.Bathrooms);
            cmd.Parameters.AddWithValue("@Rooms", property.Rooms);
            cmd.Parameters.AddWithValue("@Owner", property.Owner);
            cmd.Parameters.AddWithValue("@Pourpose", property.Pourpose);
            cmd.Parameters.AddWithValue("@Status", property.Status);
            cmd.Parameters.AddWithValue("@Address", property.Address);
            cmd.Parameters.AddWithValue("@City", property.City);
            cmd.Parameters.AddWithValue("@Province", property.Province);
            cmd.Parameters.AddWithValue("@Realtor", property.Realtor);
            cmd.Parameters.AddWithValue("@Id", property.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                response.statusCode = 200;
                response.statusMessage = "The data is updated perfectly.";
                response.property = property;
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "No data updated properly.";
            }

            return response;
        }

        public Response DeleteProperty(NpgsqlConnection con, int id)
        {
            Response response = new Response();
            string query = "DELETE FROM property WHERE id = @Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                response.statusCode = 200;
                response.statusMessage = "The entry is deleted from the table.";
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "Delete could not be performed.";
            }

            return response;
        }
    }
}