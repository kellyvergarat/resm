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

namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public User user;
        public Settings(User user)
        {
            InitializeComponent();
            this.user = user;
            DataContext = this;
            headerMsg.Text = "Welcome, " + user.FirstName + ".";

            // Populate user information
            emailTextBox.Text = user.Email;
            firstNameTextBox.Text = user.FirstName;
            lastNameTextBox.Text = user.LastName;

            // Select role in the roleComboBox
            ComboBoxItem selectedRoleItem = roleComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == user.Role);
            if (selectedRoleItem != null)
            {
                selectedRoleItem.IsSelected = true;
            }
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Get the input values from the form
            string email = emailTextBox.Text;
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string role = ((ComboBoxItem)roleComboBox.SelectedItem).Content.ToString();

            // Create a database connection
            using (NpgsqlConnection connection = CreateConnection())
            {
                connection.Open();

                // Update the user information in the database
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE users SET firstname = @FirstName, lastname = @LastName, role = @Role WHERE email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User information updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update user information.");
                    }
                }
            }
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void goToDashboard_Click(object sender, RoutedEventArgs e)
        {
            userDashboard window = new userDashboard(user);
            window.Show();
            Close();
        }

        private void goToSearch_Click(object sender, RoutedEventArgs e)
        {
            searchProperties newsearch = new searchProperties(user);
            newsearch.Show();
            Close();
        }

        private void goToPayment_Click(object sender, RoutedEventArgs e)
        {
            Payment window = new Payment(user);
            window.Show(); Close();
            Close();
        }

        private void goToSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(user);
            window.Show();
            Close();
        }

        private void goToMessages_Click(object sender, RoutedEventArgs e)
        {
            Messages window = new Messages(user);
            window.Show();
            Close();
        }
    }
}
