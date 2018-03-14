using BikeRentalWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace BikeRentalWPF.Pages
{
    /// <summary>
    /// Interaction logic for ChangeCustomer.xaml
    /// </summary>
    public partial class ChangeCustomer : Window
    {

        private Customer customer;

        public ChangeCustomer(Customer customer)
        {
            InitializeComponent();

            this.customer = customer;
            
            Gender.SelectedIndex = (int)customer.Gender;
            FirstName.Text = customer.FirstName;
            LastName.Text = customer.LastName;
            Birthday.Text = customer.Birthday.ToLongDateString();
            Street.Text = customer.Street;
            HouseNumber.Text = customer.HouseNumber;
            ZipCode.Text = customer.ZipCode;
            Town.Text = customer.Town;
            
        }

        private async void ChangeAsync(object sender, RoutedEventArgs e)
        {

            int id = customer.ID;
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://bikerentalpublishbauer.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var stringContent = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("api/updateCustomer/" + id, stringContent);
            }

            this.Close();

        }
    }
}
