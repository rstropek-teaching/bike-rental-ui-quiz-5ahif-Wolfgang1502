using BikeRentalWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BikeRentalWPF.Pages
{
    /// <summary>
    /// Interaction logic for BikeUI.xaml
    /// </summary>
    public partial class BikeUI : Page
    {

        private ObservableCollection<Bike> bikes = new ObservableCollection<Bike>();

        public BikeUI()
        {
            InitializeComponent();

            this.DataContext = this.bikes;

            // Fill the bike list
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes");
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

            var bike = JsonConvert.DeserializeObject<List<Bike>>(responseFromServer);
            bikes.Clear();
            for (int i = 0; i < bike.Count; i++)
            {
                bikes.Add(bike[i]);
                Console.WriteLine(bike[i].Notes);
            }
        }

        public async Task PostAsync(Bike bike)
        {

            string json = JsonConvert.SerializeObject(bike);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://bikerentalpublishbauer.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/addBike", content);
            }

            // Refresh the bike list
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes");

        }

        private void Sorting(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(SortingBy.Text);

            if(SortingBy.Text.Equals("Preis erste Stunde (aufsteigend)"))
            {
                Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes?sorting=FirstHour");
            }
            else if (SortingBy.Text.Equals("Preis zusätzliche Stunden (aufsteigend)"))
            {
                Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes?sorting=AdditionalHour");
            }
            else if(SortingBy.Text.Equals("Kaufdatum (absteigend)"))
            {
                Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes?sorting=PurchaseDate");
            }

        }

        private async void AddBikeAsync(object sender, RoutedEventArgs e)
        {

            var category = Models.Category.StandardBike;

            if (Category.SelectedItem != null)
            {
                if (Category.SelectedItem.ToString().Contains("Standardbike"))
                {
                    category = Models.Category.StandardBike;
                }
                else if (Category.SelectedItem.ToString().Contains("Mountainbike"))
                {
                    category = Models.Category.MountainBike;
                }
                else if(Category.SelectedItem.ToString().Contains("Treckingbike"))
                {
                    category = Models.Category.TreckingBike;
                }
                else
                {
                    category = Models.Category.RacingBike;
                }
            }
            else
            {
                MessageBox.Show("Es müssen alle Pflichtfelder ausgefüllt werden!");
            }

            // Create new Bike with the inputs
            Bike newBike = new Bike()
            {
                Brand = Brand.Text,
                PurchaseDate = PurchaseDate.DisplayDate,
                Notes = Notes.Text,
                LastService = LastService.DisplayDate,
                PriceFirstHour = Convert.ToDouble(PriceFirstHour.Text.Substring(1).Replace(',', '.')) / 100,
                PriceAdditionalHour = Convert.ToDouble(PriceAdditionalHour.Text.Substring(1).Replace(',', '.')) / 100,
                Category = category
            };

            await PostAsync(newBike);

        }

        private async void DeleteBikeAsync(object sender, RoutedEventArgs e)
        {

            if (BikeGrid.SelectedIndex >= 0)
            {
                Bike bike = BikeGrid.SelectedItem as Bike;
                int id = bike.ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://bikerentalpublishbauer.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.DeleteAsync("api/deleteBike/" + id);
                }

                Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes");

            }

        }

        private void ChangeBike(object sender, RoutedEventArgs e)
        {

            if (BikeGrid.SelectedIndex >= 0)
            {
                Bike bike = BikeGrid.SelectedItem as Bike;

                ChangeBike c = new ChangeBike(bike);
                c.Show();

                // Refresh bike list after closing the change window
                c.Closed += new EventHandler(ChangeWindowClosed);

            }

        }

        private void ChangeWindowClosed(object sender, EventArgs e)
        {
            Get("http://bikerentalpublishbauer.azurewebsites.net/api/getBikes");
        }

    }
}
