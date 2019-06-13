using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RackSpaceWebService.Models;
using RackSpaceWebService.AzureService;

namespace RackSpaceWebService.Controllers
{
    public class WebRentalController : ApiController
    {
        private RackSpaceRepository _repo = null;
        private AzureApiReceiver _receiver = null;

        public WebRentalController()
        {
            _repo = new RackSpaceRepository();
            _receiver = new AzureApiReceiver();
        }

        public List<WebRentalModel> Get()
        {
            try
            {
                var webRental = _repo.GetAllWebRentals();
                return webRental.ToList();
            }
            catch
            {
                return null;
            }
        }

        public WebRentalModel Get(string id)
        {
            var record = _repo.GetAllWebRentals().FirstOrDefault(x => x.FirstName == id);
            return record;
        }

        [HttpPost]
        public int Post(QuoteModel quote)
        {
            var oid = quote.Oid;
            var organization = quote.Organization;
            var location = quote.Location;
            var organizationName = quote.OrganizationName;
            var locationName = quote.LocationName;
            var name = quote.Name;
            var firstName = quote.FirstName;
            var lastName = quote.LastName;
            var address = quote.Address;
            var city = quote.City;
            var state = quote.State;
            var zip = quote.Zip;
            var primaryPhone = quote.MobilePhonePrimary;
            var secondaryPhone = quote.HomePhoneSecondary;
            var emailAddress = quote.EmailAddress;
            var leaveOn = quote.LeaveOn;
            var returnOn = quote.ReturnOn;
            var destination = quote.Destination;
            var distance = quote.Distance;
            var adults = quote.Adults;
            var children = quote.Children;
            var webUserComments = quote.WebUserComments;
            var leadSourceOid = quote.LeadSourceOid;
            var leadSourceValue = quote.LeadSourceValue;
            var webUserSelectedClass = quote.WebUserSelectedClass;
            var classOid = quote.ClassOid;
            var vehicleName = quote.VehicleName;
            var vehicleOid = quote.VehicleId;
            var comments = quote.Comments;
            var country = quote.WebUserSelectedCountry;
            var equipments = quote.EquipmentTypeOids;
            var fees = quote.FeeOids;
            var equipmentNames = quote.EquipmentTypeNames;
            var feeNames = quote.FeeNames;
            var insuranceCompany = quote.LocationInsuranceCompanyOidsAndNames;

            var keyword = GetQuickFindKeyWordFromAzureWebApi(vehicleOid);
            var vehicleId = GetVehicleIdFromRackSpaceDatabase(keyword);

            var equipmentNameList = string.IsNullOrEmpty(equipmentNames) == false ? equipmentNames : "None";
            var feeNameList = string.IsNullOrEmpty(feeNames) == false ? feeNames : "None";

            var insuranceComment = "This quote is from the new website. They selected insurance : " 
                                    + GetInsuranceCompanyName(insuranceCompany);
            var insuranceAndUserComments = insuranceComment.Replace(insuranceComment, insuranceComment 
                                    + "\r\n\r\n") + "User Comments : " + webUserComments;
            
            var specialInstructions = insuranceAndUserComments.Replace(insuranceAndUserComments, insuranceAndUserComments
                                    +"\r\n\r\n") + "Optional Items : Equipments - " + equipmentNameList
                                    + " Fees - " + feeNameList; 

            var model = new WebRentalModel()
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = "",
                StreetAddress = address,
                City = city,
                State = state,
                PostalCode = zip,
                Phone = primaryPhone,
                Mobile = secondaryPhone,
                Email = emailAddress,
                DepartureDate = Convert.ToDateTime(leaveOn),
                ReturnDate = Convert.ToDateTime(returnOn),
                Notes = specialInstructions,
                AlternateDestinationName = destination,
                AlternateDestinationMileage = GetIntFromString(distance),
                AlternateAdvertiseSource = string.IsNullOrEmpty(leadSourceValue) == false ? leadSourceValue : "*New WebSite*",
                Adults = GetIntFromString(adults),
                Children = GetIntFromString(children),
                VehicleDescription = vehicleName,
                CountryCode = country,
                InsuranceCompany = GetInsuranceCompanyName(insuranceCompany),
                WebCallDateTime = DateTime.Now,
                DateTimeStamp = DateTime.Now,
                Status = "Open",
                AdvertizeID = 38,
                Passengers = GetPassengersResult(adults, children),
                VehicleID = vehicleId
            };

            var success = _repo.SaveWebRentalDataFromNewQuotePage(model);

            if (success)
                return 1;

            return 0;
        }

        private int? GetPassengersResult(string adults, string children)
        {
            var adultsValue = 0;
            var childrenValue = 0;

            if (!string.IsNullOrEmpty(adults))
                adultsValue = Convert.ToInt32(adults);

            if (!string.IsNullOrEmpty(children))
                childrenValue = Convert.ToInt32(children);

            var passengers = adultsValue + childrenValue;
            if (passengers == 0)
                return null;

            return passengers;
        }

        private int GetVehicleIdFromRackSpaceDatabase(string key)
        {
            var fleets = _repo.GetAllFleets().ToList();
            if (fleets == null || fleets.Count() <= 0)
                return -1;
            var keyValue = 0;
            if (!string.IsNullOrEmpty(key))
                keyValue = Convert.ToInt32(key);

            var selectedFleet = fleets.FirstOrDefault(x => x.WebsiteVehicleID == keyValue);
            if (selectedFleet == null)
                return 0;

            var vehicleId = selectedFleet.VehicleID;

            return vehicleId;
        }

        private string GetQuickFindKeyWordFromAzureWebApi(string id)
        {
            var guid = GetGuidFromString(id);
            try
            {
                var keyword = _receiver.GetVehicleKeyWord(guid);
                return keyword;
            }
            catch
            {
                return null;
            }
        }

        private Guid GetGuidFromString(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Guid.Empty;
            var guid = Guid.Parse(id);
            return guid;
        }

        private string GetInsuranceCompanyName(string str)
        {
            if (String.IsNullOrEmpty(str))
                return "Nothing";
            var name = str.Split(':').LastOrDefault();
            return name;
        }

        private int? GetIntFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            return Convert.ToInt32(str);
        }
    }
}
