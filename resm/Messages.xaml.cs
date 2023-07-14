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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Message = RealStateManagement.Model.Message;

namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for Messages.xaml
    /// </summary>
    public partial class Messages : Window
    {

        public ObservableCollection<Message> MessagesReceived { get; set; }
        public User User;

        public Messages(User user)
        {
            InitializeComponent();
            this.User = user;
            LoadMessagesFromDatabase();
            headerMsg.Text = "Welcome, " + user.FirstName + ".";
            DataContext = this;
        }

        private void goToInquiry_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button clickedButton = (System.Windows.Controls.Button)sender;
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

        private NpgsqlConnection CreateConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=resm;Username=postgres;Password=0000";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        private void LoadMessagesFromDatabase()
        {
            MessagesReceived = new ObservableCollection<Message>();

            using (NpgsqlConnection connection = CreateConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Inquiries WHERE toemail = @toEmail";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("toEmail", User.Email);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Message message = new Message
                            {
                                FromEmail = reader.GetString(reader.GetOrdinal("fromemail")),
                                ToEmail = reader.GetString(reader.GetOrdinal("toemail")),
                                MessageText = reader.GetString(reader.GetOrdinal("message")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Contact = reader.GetInt64(reader.GetOrdinal("contact"))
                            };

                            MessagesReceived.Add(message);
                        }
                    }
                }
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

        private void goToMessages_Click(object sender, RoutedEventArgs e)
        {
            Messages window = new Messages(User);
            window.Show();
            Close();
        }
    }
}
