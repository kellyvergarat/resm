using Npgsql;
using System;
using System.Windows;

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
                userDashboard userDash = new userDashboard();
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
    }
}
