using Npgsql;
using RealStateManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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

namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for userDashboard.xaml
    /// </summary>
    public partial class userDashboard : Window
    {
        public ObservableCollection<Property> Properties { get; set; }
        public User User;

        public userDashboard(User userLogged)
        {
            InitializeComponent();
            this.User = userLogged;
            DataContext = this;
            headerMsg.Text = "Welcome, " + userLogged.FirstName + ".";

            // Populate the Properties collection with actual property data
            Properties = new ObservableCollection<Property>();
            LoadPropertiesFromDatabase();

            string userRole = GetUserRole(User.Email);

            if (userRole == "owner")
            {
                sectionTitle.Text = "My properties listed";
            }
            else if (userRole == "tenant")
            {
                sectionTitle.Text = "Your active leases";
            }
            else if (userRole == "staff" || userRole == "admin")
            {
                sectionTitle.Text = "Your properties portfolio";
            }
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private void LoadPropertiesFromDatabase()
        {
            if (User != null && User.Email != null)
            {
                using (NpgsqlConnection connection = CreateConnection())
                {
                    string email = User.Email;
                    connection.Open();
                    string query = "SELECT imageurl, pricepermonth, bathrooms, rooms, owner, pourpose, status, address, city, province, realtor, id FROM property WHERE owner = @email";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("email", email);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string imageUrl = reader.GetString(0);
                                double pricePerMonth = reader.GetDouble(1);
                                int bathrooms = reader.GetInt32(2);
                                int rooms = reader.GetInt32(3);
                                string owner = reader.GetString(4);
                                string pourpose = reader.GetString(5);
                                bool status = reader.GetBoolean(6);
                                string addres = reader.GetString(7);
                                string city = reader.GetString(8);
                                string province = reader.GetString(9);
                                string realtor = reader.GetString(10);
                                int id = reader.GetInt32(11);
                                Property property = new Property
                                {
                                    Id = id,
                                    ImageUrl = imageUrl,
                                    PricePerMonth = pricePerMonth,
                                    Bathrooms = bathrooms,
                                    Rooms = rooms,
                                    Owner = owner,
                                    Pourpose = pourpose,
                                    Status = status,
                                    Address = addres,
                                    City = city,
                                    Province = province,
                                    Realtor = realtor,
                                };
                                Properties.Add(property);
                            }
                        }
                    }
                }
            }
        }



        private void goToInquiry_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            Grid parentGrid = (Grid)VisualTreeHelper.GetParent(clickedButton);
            TextBlock idText = parentGrid.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Name == "idText");
            if (idText != null)
            {
                int propertyId = int.Parse(idText.Text);

                // Open the inquiry window
                Inquiry newInquiry = new Inquiry(propertyId, User);
                newInquiry.Show();

                // Close the current window
                Close();
            }
        }

        private void goToSettingsClick(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(User);
            window.Show();
            Close();
        }


        private void goToPayment_Click(object sender, RoutedEventArgs e)
        {
            Payment window = new Payment(User);
            window.Show(); Close();
            Close();
        }

        private void goToDashboard_Click(object sender, RoutedEventArgs e)
        {
            userDashboard window = new userDashboard(User);
            window.Show();
            Close();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void goToSearch_Click(object sender, RoutedEventArgs e)
        {

            searchProperties newsearch = new searchProperties(User);
            newsearch.Show();
            Close();
        }

        private string GetUserRole(string userEmail)
        {
            string role = string.Empty;
            using (NpgsqlConnection connection = CreateConnection())
            {
                connection.Open();
                string query = "SELECT u.role FROM users u INNER JOIN property p ON u.email = p.owner WHERE p.owner = @UserEmail";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserEmail", userEmail);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = reader.GetString(0);
                        }
                    }
                }
            }
            return role;
        }

        private void goToMessages_Click(object sender, RoutedEventArgs e)
        {
            Messages window = new Messages(User);
            window.Show();
            Close();
        }
    }
}
