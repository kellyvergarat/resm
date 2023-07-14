using Npgsql;
using RealStateManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for Inquiry.xaml
    /// </summary>
    public partial class Inquiry : Window
    {
        public User? user;
        public int? idProperty;
        public Inquiry(int id, User user)
        {       
            InitializeComponent();
            DataContext = this;
            this.user = user;
            this.idProperty = id;
            headerMsg.Text = "Welcome, " + user.FirstName + ".";
            inquiryBtn.Content = "Send message to "+ getPropertyRealtor();
            emailTextBox.Text = user.Email;
        }

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private string getPropertyRealtor()
        {
            string realtor=null;
            if (user != null && idProperty!= null)
            {
                using (NpgsqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT imageurl, pricepermonth, bathrooms, rooms, owner, pourpose, status, address, city, province, realtor, id FROM property WHERE id=@idProperty";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("idProperty", idProperty);
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
                                realtor = reader.GetString(10);
                                int id = reader.GetInt32(11);
                            }
                        }
                    }
                }
            }
            return realtor;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            string email = emailTextBox.Text;
            string toemail = getPropertyRealtor();
            BigInteger phone = BigInteger.Parse(phoneTextBox.Text);
            string message = messageTextBox.Text;
            using (NpgsqlConnection connection = CreateConnection())
            {
                connection.Open();
                // Find the highest ID in the "inquiries" table
                using (NpgsqlCommand maxIdCommand = new NpgsqlCommand("SELECT MAX(id) FROM inquiries", connection))
                {
                    int highestId = Convert.ToInt32(maxIdCommand.ExecuteScalar());
                    using (NpgsqlCommand insertCommand = new NpgsqlCommand("INSERT INTO inquiries (id, fromemail, toemail, message, name, contact) VALUES (@Id, @FromEmail, @ToEmail, @Message, @Name, @Contact)", connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Id", highestId + 1);
                        insertCommand.Parameters.AddWithValue("@FromEmail", email);
                        insertCommand.Parameters.AddWithValue("@ToEmail", toemail);
                        insertCommand.Parameters.AddWithValue("@Message", message);
                        insertCommand.Parameters.AddWithValue("@Name", name);
                        insertCommand.Parameters.AddWithValue("@Contact", phone);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
            MessageBox.Show("Message sent");
            nameTextBox.Text = string.Empty;
            emailTextBox.Text = string.Empty;
            phoneTextBox.Text = string.Empty;
            messageTextBox.Text = string.Empty;
        }


        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messages window = new Messages(user);
            window.Show();
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
