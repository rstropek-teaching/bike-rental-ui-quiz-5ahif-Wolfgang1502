using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRentalWPF.Models
{
    public class Rental
    {

        public int ID { get; set; }

        public Customer Customer { get; set; }

        public Bike Bike { get; set; }

        public DateTime RentalBegin { get; set; }

        public DateTime? RentalEnd { get; set; }

        public double TotalCosts { get; set; }

        public Boolean Paid { get; set; }

    }
}
