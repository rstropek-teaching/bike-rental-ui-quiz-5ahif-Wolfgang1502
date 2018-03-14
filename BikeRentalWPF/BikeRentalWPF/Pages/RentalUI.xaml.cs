using BikeRentalWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for RentalUI.xaml
    /// </summary>
    public partial class RentalUI : Page
    {

        private ObservableCollection<RentalData> rentals = new ObservableCollection<RentalData>();

        public RentalUI()
        {
            InitializeComponent();

            this.DataContext = this.rentals;

            string response = Get("http://bikerentalpublishbauer.azurewebsites.net/api/unpaidRentals");
            ExtractData(response);

        }

        public string Get(string uri)
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

            return responseFromServer;
        }

        public void ExtractData(string data)
        {

            string tmp = data.Substring(1, data.Length - 2);
            Console.WriteLine(tmp);
            string[] arr = tmp.Split(',');
            for(int i = 0; i < arr.Length; i = i + 7)
            {
                Console.WriteLine(i + " - " + arr[i]);
                Console.WriteLine(arr[i + 1]);


                RentalData rent = new RentalData()
                {
                    FirstName = arr[i + 1].Substring(1, arr[i + 1].Length - 2),
                    LastName = arr[i + 2].Substring(1, arr[i + 2].Length - 2),
                    RentalBegin = DateTime.Parse(arr[i + 4].Substring(1, arr[i + 4].Length - 10)),
                    RentalEnd = DateTime.Parse(arr[i + 5].Substring(1, arr[i + 4].Length - 10)),
                    TotalCosts = Convert.ToDouble(arr[i + 6]) / 10
                };

                rentals.Add(rent);

            }
        }

    }
}
