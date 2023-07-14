using Npgsql;
using RealStateManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for searchProperties.xaml
    /// </summary>
    public partial class searchProperties : Window
    {
        public ObservableCollection<Property> Properties { get; set; }
        public User User;
        public List<string> PropertyTypes { get; set; }
        public List<string> Provinces { get; set; }
        public List<string> Cities { get; set; }

        public searchProperties(User userLogged)
        {
            InitializeComponent();
            this.User = userLogged;
            DataContext = this;
            headerMsg.Text = "Welcome, " + userLogged.FirstName + ".";
            Properties = new ObservableCollection<Property>();
            LoadPropertiesFromDatabase();
            // Initialize data sources
            PropertyTypes = new List<string> { "rent", "sell"};
            Provinces = new List<string> { "Ontario", "Quebec", "British Columbia" };
            Cities = new List<string> { "Toronto", "Montreal", "Vancouver" };
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private void LoadPropertiesFromDatabase()
        {
            if (User != null)
            {
                using (NpgsqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT imageurl, pricepermonth, bathrooms, rooms, owner, pourpose, status, address, city, province, realtor, id FROM property";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
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


        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected filter values
            string budget = budgetTextBox.Text;
            string propertyType = propertyTypeComboBox.SelectedItem?.ToString();
            string province = provinceComboBox.SelectedItem?.ToString();
            string city = cityComboBox.SelectedItem?.ToString();

            // Filter the properties based on the selected filters and budget
            List<Property> filteredProperties = Properties
                .Where(p => (string.IsNullOrEmpty(budget) || p.PricePerMonth <= double.Parse(budget))
                         && (string.IsNullOrEmpty(propertyType) || p.Pourpose == propertyType)
                         && (string.IsNullOrEmpty(province) || p.Province == province)
                         && (string.IsNullOrEmpty(city) || p.City == city))
                .ToList();

            // Update the ItemsSource of the property card list
            propertyCardList.ItemsSource = filteredProperties;
        }

        private void clearFilters_Click(object sender, RoutedEventArgs e)
        {
            propertyTypeComboBox.SelectedItem = null;
            provinceComboBox.SelectedItem = null;
            cityComboBox.SelectedItem = null;
            budgetTextBox.Text = "";
            propertyCardList.ItemsSource = Properties;
        }

        private void goToInquiry_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            Grid parentGrid = (Grid)VisualTreeHelper.GetParent(clickedButton);
            TextBlock idText = parentGrid.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Name == "idText");
            if (idText != null)
            {
                int propertyId = int.Parse(idText.Text);
                Inquiry newInquiry = new Inquiry(propertyId, User);
                newInquiry.Show();
                Close();
            }
        }

        private void goToDashboard_Click(object sender, RoutedEventArgs e)
        {
            userDashboard window = new userDashboard(User);
            window.Show();
            Close();
        }

        private void goToSearch_Click(object sender, RoutedEventArgs e)
        {
            searchProperties newsearch = new searchProperties(User);
            newsearch.Show();
            Close();
        }

        private void goToPayment_Click(object sender, RoutedEventArgs e)
        {
            Payment window = new Payment(User);
            window.Show(); Close();
            Close();
        }

        private void goToSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(User);
            window.Show();
            Close();
        }

        private void goToMessages_Click(object sender, RoutedEventArgs e)
        {
            Messages window = new Messages(User);
            window.Show();
            Close();
        }
    }
}
