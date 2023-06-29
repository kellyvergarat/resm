using RealStateManagement.Model;
using System.Collections.ObjectModel;
using System.Windows;


namespace RealStateManagement
{
    /// <summary>
    /// Interaction logic for userDashboard.xaml
    /// </summary>
    public partial class userDashboard : Window
    {

        public ObservableCollection<Property> Properties { get; set; }


        public userDashboard()
        {
            InitializeComponent();
            DataContext = this;

            // Populate the Properties collection with actual property data
            Properties = new ObservableCollection<Property>
        {
            new Property
            {
                ImageUrl = "path_to_image1.jpg",
                PricePerMonth = 1500,
                Bathrooms = 2,
                Rooms = 3
            },
            new Property
            {
                ImageUrl = "path_to_image2.jpg",
                PricePerMonth = 2000,
                Bathrooms = 3,
                Rooms = 4
            },
            // Add more properties
        };
        }
    }
}
