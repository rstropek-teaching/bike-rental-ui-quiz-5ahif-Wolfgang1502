using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRentalWPF.Models
{
    public class RentalData
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RentalBegin { get; set; }

        public DateTime? RentalEnd { get; set; }

        public double TotalCosts { get; set; }

    }
}
