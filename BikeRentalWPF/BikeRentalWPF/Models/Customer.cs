using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRentalWPF.Models
{

    public class Customer
    {

        public int ID { get; set; }

        public Gender Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string ZipCode { get; set; }

        public string Town { get; set; }

    }

    public enum Gender
    {
        Male,
        Female,
        Unkown
    }

}
