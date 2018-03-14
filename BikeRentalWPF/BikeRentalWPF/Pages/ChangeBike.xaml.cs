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
    /// Interaction logic for ChangeBike.xaml
    /// </summary>
    public partial class ChangeBike : Window
    {

        private Bike bike;

        public ChangeBike(Bike bike)
        {
            InitializeComponent();

            this.bike = bike;

            Brand.Text = bike.Brand;
            PurchaseDate.Text = bike.PurchaseDate.ToLongDateString();
            Notes.Text = bike.Notes;
            LastService.Text = bike.LastService.ToLongDateString();
            PriceFirstHour.Text = bike.PriceFirstHour.ToString();
            PriceAdditionalHour.Text = bike.PriceAdditionalHour.ToString();
            Category.SelectedIndex = (int)bike.Category;

        }

        private async void ChangeAsync(object sender, RoutedEventArgs e)
        {

            int id = bike.ID;
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
                else if (Category.SelectedItem.ToString().Contains("Treckingbike"))
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

            double priceFirstHour = 0;
            double priceAdditionalHour = 0;
            try
            {
                priceFirstHour = Convert.ToDouble(PriceFirstHour.Text);
                priceAdditionalHour = Convert.ToDouble(PriceAdditionalHour.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Falsches Währungsformat");
            }

            Bike newBike = new Bike()
            {
                Brand = Brand.Text,
                PurchaseDate = PurchaseDate.DisplayDate,
                Notes = Notes.Text,
                LastService = LastService.DisplayDate,
                PriceFirstHour = priceFirstHour,
                PriceAdditionalHour = priceAdditionalHour,
                Category = category
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://bikerentalpublishbauer.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var stringContent = new StringContent(JsonConvert.SerializeObject(newBike), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("api/updateBike/" + id, stringContent);
            }

            this.Close();

        }

    }
}
