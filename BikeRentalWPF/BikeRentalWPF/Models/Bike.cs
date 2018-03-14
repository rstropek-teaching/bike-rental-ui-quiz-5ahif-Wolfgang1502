using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRentalWPF.Models
{
    public class Bike
    {

        public int ID { get; set; }

        public string Brand { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string Notes { get; set; }

        public DateTime LastService { get; set; }

        public double PriceFirstHour { get; set; }

        public double PriceAdditionalHour { get; set; }

        public Category Category { get; set; }

    }

    public enum Category
    {
        StandardBike,
        MountainBike,
        TreckingBike,
        RacingBike
    }

}
