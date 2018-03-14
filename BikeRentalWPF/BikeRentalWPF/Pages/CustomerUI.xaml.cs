using BikeRentalWPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
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

namespace BikeRentalWPF.Pages
{
    /// <summary>
    /// Interaction logic for CustomerUI.xaml
    /// </summary>
    public partial class CustomerUI : Page
    {

        private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();

        public CustomerUI()
        {
            InitializeComponent();

            this.DataContext = this.customers;

            // Fill the customer list
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getCustomers");

        }

        public void Get(string uri)
        {
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(uri);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();

            var cust = JsonConvert.DeserializeObject<List<Customer>>(responseFromServer);
            customers.Clear();
            for (int i = 0; i < cust.Count; i++)
            {
                customers.Add(cust[i]);
                Console.WriteLine(cust[i].FirstName);

            }
        }

        public async Task PostAsync(Customer customer)
        {

            string json = JsonConvert.SerializeObject(customer);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://bikerentalpublishbauer.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/addCustomer", content);
            }

            // Refresh the customer list
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getCustomers");

        }

        private void SearchLastName(object sender, RoutedEventArgs e)
        {

            // Refresh the customer list
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getCustomers?lastName=" + InputLastName.Text);

        }

        private async void AddCustomerAsync(object sender, RoutedEventArgs e)
        {

            var gender = Models.Gender.Unkown;

            if (Gender.SelectedItem != null)
            {
                if (Gender.SelectedItem.ToString().Contains("Männlich"))
                {
                    gender = Models.Gender.Male;
                }
                else if (Gender.SelectedItem.ToString().Contains("Weiblich"))
                {
                    gender = Models.Gender.Female;
                }
                else
                {
                    gender = Models.Gender.Unkown;
                }
            }
            else
            {
                MessageBox.Show("Es müssen alle Pflichtfelder ausgefüllt werden!");
            }

            // Create new Customer with the inputs
            Customer newCustomer = new Customer()
            {
                Gender = gender,
                FirstName = FirstName.Text,
                LastName = LastName.Text,
                Birthday = Birthday.DisplayDate,
                Street = Street.Text,
                HouseNumber = HouseNumber.Text,
                ZipCode = ZipCode.Text,
                Town = Town.Text
            };

            await PostAsync(newCustomer);

        }

        private async void DeleteCustomerAsync(object sender, RoutedEventArgs e)
        {

            if (CustomerGrid.SelectedIndex >= 0)
            {
                Customer customer = CustomerGrid.SelectedItem as Customer;
                int id = customer.ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://bikerentalpublishbauer.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.DeleteAsync("api/deleteCustomer/" + id);
                }

                Get("http://bikerentalpublishbauer.azurewebsites.net/api/getCustomers");

            }

        }

        private void ChangeCustomer(object sender, RoutedEventArgs e)
        {

            if (CustomerGrid.SelectedIndex >= 0)
            {
                Customer customer = CustomerGrid.SelectedItem as Customer;

                ChangeCustomer c = new ChangeCustomer(customer);
                c.Show();

                // Refresh customer list after closing the change window
                c.Closed += new EventHandler(ChangeWindowClosed);

            }

        }

        private void ChangeWindowClosed(object sender, EventArgs e)
        {
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getCustomers");
        }

    }
}
