using Npgsql;
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
using RealStateManagement.Model;
namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        public User user;
        public Dictionary<string, double> paymentAmounts;

        public Payment(User user)
        {
            InitializeComponent();
            this.user = user;
            DataContext = this;
            headerMsg.Text = "Welcome, "+this.user.FirstName+".";
            PopulatePaymentInfo();
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private void PopulatePaymentInfo()
        {
            string email = user.Email;
            Dictionary<string, double> paymentAmounts = new Dictionary<string, double>();

            using (NpgsqlConnection connection = CreateConnection())
            {
                connection.Open();

                // Query the payment table for the user's email
                string query = "SELECT month, amount FROM payment WHERE userid = @Email";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Clear the existing items in the months list
                        monthComboBox.Items.Clear();

                        // Populate the months list and payment amount for each month
                        while (reader.Read())
                        {
                            string month = reader.GetString(0);
                            double amount = reader.GetDouble(1);

                            // Add the month to the months list
                            monthComboBox.Items.Add(month);

                            // Store the payment amount for the month in the dictionary
                            paymentAmounts[month] = amount;
                        }
                    }
                }
            }

            this.paymentAmounts = paymentAmounts;
        }

       

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            string month = monthComboBox.SelectedItem as string;
            string creditCardHolder = creditCardHolderTextBox.Text;
            string creditCardNumber = creditCardNumberTextBox.Text;
            string expirationMonth = expirationMonthTextBox.Text;
            string expirationYear = expirationYearTextBox.Text;
            string cvv = cvvTextBox.Text;

            if (string.IsNullOrEmpty(month) || string.IsNullOrEmpty(creditCardHolder) ||
                string.IsNullOrEmpty(creditCardNumber) || string.IsNullOrEmpty(expirationMonth) ||
                string.IsNullOrEmpty(expirationYear) || string.IsNullOrEmpty(cvv))
            {
                MessageBox.Show("Please fill in all payment details.", "Payment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Payment successful!", "Payment", MessageBoxButton.OK, MessageBoxImage.Information);
                // Clear the payment fields
                monthComboBox.SelectedIndex = -1;
                creditCardHolderTextBox.Text = string.Empty;
                creditCardNumberTextBox.Text = string.Empty;
                expirationMonthTextBox.Text = string.Empty;
                expirationYearTextBox.Text = string.Empty;
                cvvTextBox.Text = string.Empty;
            }
        }

        private void monthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMonth = monthComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedMonth))
            {
                // Check if the selected month exists in the paymentAmounts dictionary
                if (paymentAmounts.ContainsKey(selectedMonth))
                {
                    // Retrieve the payment amount for the selected month and display it in the amountText TextBlock
                    double paymentAmount = paymentAmounts[selectedMonth];
                    amountText.Text = paymentAmount.ToString("C2"); // Format the amount as currency
                }
                else
                {
                    // Handle the case when the selected month does not exist in the paymentAmounts dictionary
                    amountText.Text = "N/A";
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

        private void goToSettingsClick(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(user);
            window.Show();
            Close();
        }

        private void goToPayment_Click(object sender, RoutedEventArgs e)
        {
            Payment window = new Payment(user);
            window.Show(); Close();
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
