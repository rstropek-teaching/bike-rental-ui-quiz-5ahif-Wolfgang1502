using BikeRentalWPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace BikeRentalWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();

        public MainWindow()
        {
            InitializeComponent();

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            lstPages.SelectionChanged += new SelectionChangedEventHandler(LstPages_SelectionChanged);
        }

        private void LstPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frmContent.Source = new Uri("Pages/" + ((ListBoxItem)lstPages.SelectedValue).Name + ".xaml", UriKind.Relative);
        }

    }
}
