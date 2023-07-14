using Microsoft.VisualBasic.ApplicationServices;
using Npgsql;
using RealStateManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using User = RealStateManagement.Model.User;

namespace RealStateManagement
{
    public partial class AddProperty : Window
    {

        public User User;
        private const string ConnectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000;";

        public AddProperty(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = this;
            headerMsg.Text = "Welcome, " + User.FirstName + ".";
        }

        private void addPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the entered property information from the text boxes and checkboxes
            string imageUrl = imageUrlTextBox.Text;
            double pricePerMonth = double.Parse(priceTextBox.Text);
            int bathrooms = int.Parse(bathroomsTextBox.Text);
            int rooms = int.Parse(roomsTextBox.Text);
            string owner = User.Email;
            string purpose = purposeTextBox.Text;
            bool status = false;
            string address = addressTextBox.Text;
            string city = cityTextBox.Text;
            string province = provinceTextBox.Text;
            string realtor = realtorTextBox.Text;

            Property property = new Property
            {
                ImageUrl = imageUrl,
                PricePerMonth = pricePerMonth,
                Bathrooms = bathrooms,
                Rooms = rooms,
                Owner = owner,
                Pourpose = purpose,
                Status = status,
                Address = address,
                City = city,
                Province = province,
                Realtor = realtor
            };

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Create a parameterized query to insert the property into the database
                    string query = "INSERT INTO property (imageurl, pricepermonth, bathrooms, rooms, owner, pourpose, status, address, city, province, realtor) " +
                        "VALUES (@ImageUrl, @PricePerMonth, @Bathrooms, @Rooms, @Owner, @Pourpose, @Status, @Address, @City, @Province, @Realtor)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("ImageUrl", property.ImageUrl);
                        command.Parameters.AddWithValue("PricePerMonth", property.PricePerMonth);
                        command.Parameters.AddWithValue("Bathrooms", property.Bathrooms);
                        command.Parameters.AddWithValue("Rooms", property.Rooms);
                        command.Parameters.AddWithValue("Owner", property.Owner);
                        command.Parameters.AddWithValue("Pourpose", property.Pourpose);
                        command.Parameters.AddWithValue("Status", property.Status);
                        command.Parameters.AddWithValue("Address", property.Address);
                        command.Parameters.AddWithValue("City", property.City);
                        command.Parameters.AddWithValue("Province", property.Province);
                        command.Parameters.AddWithValue("Realtor", property.Realtor);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Property added successfully
                            MessageBox.Show("Property added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            // Failed to add property
                            MessageBox.Show("Failed to add property!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding the property: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            userDashboard window = new userDashboard(User);
            window.Show();
            this.Close();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
