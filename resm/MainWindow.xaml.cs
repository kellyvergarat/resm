using System;
﻿using Npgsql;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RealStateManagement.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void goRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            Close();
        }

        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            // Handle the "Show Password" checkbox checked event
            txtPassword.Visibility = System.Windows.Visibility.Visible;
            txtPassword.Clear();
            txtPassword.Focus();
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            // Handle the "Show Password" checkbox unchecked event
            txtPassword.Visibility = System.Windows.Visibility.Collapsed;
            txtPassword.Clear();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Clear" button click event
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Perform database query to check if the credentials are correct
            if (CheckCredentials(username, password))
            {
                // Successful login
                MessageBox.Show("Login successful!");

                // Navigate to the dashboard control
                User user = GetUserByUsername(username);
                userDashboard userDash = new userDashboard(user);
                userDash.Show();
                Close();
            }
            else
            {
                // Failed login
                MessageBox.Show("Invalid username or password.");
            }
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private bool CheckCredentials(string username, string password)
        {
            // database query to check if the credentials exist and are correct

            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM credentials WHERE username = @Username AND password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private User GetUserByUsername(string username)
        {
            User user=null;
            using (var connection = CreateConnection())
            {
                connection.Open();
                string query = "SELECT email, firstname, lastname, role FROM users WHERE email = @username";
                using(var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    using(var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string email = reader.GetString(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string role = reader.GetString(3);
                            user = new User(email,firstName,lastName,role);
                        }
                    }
                }
            }
            return user;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
