using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RackSpaceWebService.Models
{
    public class WebRentalModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> DepartureDate { get; set; }
        public Nullable<DateTime> ReturnDate { get; set; }
        public string Notes { get; set; }
        public string AlternateDestinationName { get; set; }
        public Nullable<int> AlternateDestinationMileage { get; set; }
        public string AlternateAdvertiseSource { get; set; }
        public Nullable<int> Adults { get; set; }
        public Nullable<int> Children { get; set; }
        public string VehicleDescription { get; set; }
        public string CountryCode { get; set; }
        public string InsuranceCompany { get; set; }
        public Nullable<DateTime> WebCallDateTime { get; set; }
        public Nullable<DateTime> DateTimeStamp { get; set; }
        public string Status { get; set; }
        public Nullable<int> AdvertizeID { get; set; }
        public Nullable<int> Passengers { get; set; }
        public Nullable<int> VehicleID { get; set; }
    }
}