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
using Npgsql;

namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private bool showPassword;


        public Registration()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Clear" button click event
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtUsername.Focus();
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Validate that the username and password fields are not empty
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter a valid username and password.");
                return;
            }

            if (password != confirmPassword)
            {
                // Handle password mismatch error
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();

                    // Create a command with parameters
                    using (var cmd = new NpgsqlCommand("INSERT INTO credentials (username, password) VALUES (@username, @password)", conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Registration successful
                            MessageBox.Show("Registration successful!");
                            // Clear the input fields or navigate to the login page
                            // ...
                        }
                        else
                        {
                            // Handle registration failure
                            MessageBox.Show("Registration failed. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any database or connection errors
                MessageBox.Show("An error occurred while registering. Please try again later.");
                Console.WriteLine(ex.Message);
            }
        }

        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            showPassword = true;
            UpdatePasswordVisibility();
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            showPassword = false;
            UpdatePasswordVisibility();
        }

        private void UpdatePasswordVisibility()
        {
            if (showPassword)
            {
                txtPassword.PasswordChar = '\0';
                txtConfirmPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
                txtConfirmPassword.PasswordChar = '•';
            }
        }

        private void UpdatePasswordVisibility(object sender, RoutedEventArgs e)
        {
            UpdatePasswordVisibility();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
