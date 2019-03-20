using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RackSpaceWebService.Models
{
    public class RackSpaceRepository
    {
        private string _connectionString;

        public RackSpaceRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["RackSpaceConnection"].ConnectionString;
        }

        // to get records of dbo.tbl_WebRental table - test module
        public IQueryable<WebRentalModel> GetAllWebRentals()
        {
            var queryString = "select FirstName, LastName, MiddleName, StreetAddress, City, State, PostalCode, Phone, Mobile, email, DepartureDate, ReturnDate, " +
                              "Notes, AlternateDestinationName, AlternateDestinationMileage, Adults, Children, VehicleDescription, CountryCode, InsuranceCompany " +
                              "from dbo.tbl_WebRental";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<WebRentalModel>();
                    while (reader.Read())
                    {
                        var record = new WebRentalModel();
                        record.FirstName = reader.GetValue(0).ToString();
                        record.LastName = reader.GetValue(1).ToString();
                        record.MiddleName = reader.GetValue(2).ToString();
                        record.StreetAddress = reader.GetValue(3).ToString();
                        record.City = reader.GetValue(4).ToString();
                        record.State = reader.GetValue(5).ToString();
                        record.PostalCode = reader.GetValue(6).ToString();
                        record.Phone = reader.GetValue(7).ToString();
                        record.Mobile = reader.GetValue(8).ToString();
                        record.Email = reader.GetValue(9).ToString();
                        record.DepartureDate = GetDateTimeValue(reader, 10);
                        record.ReturnDate = GetDateTimeValue(reader, 11);
                        record.Notes = reader.GetValue(12).ToString();
                        record.AlternateDestinationName = reader.GetValue(13).ToString();
                        record.AlternateDestinationMileage = GetIntValue(reader, 14);
                        record.Adults = GetIntValue(reader, 15);
                        record.Children = GetIntValue(reader, 16);
                        record.VehicleDescription = reader.GetValue(17).ToString();
                        record.CountryCode = reader.GetValue(18).ToString();
                        record.InsuranceCompany = reader.GetValue(19).ToString();

                        list.Add(record);
                    }
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        // to get records of dbo.tbl_Fleet table
        public IQueryable<FleetModel> GetAllFleets()
        {
            var queryString = "select VehicleID, WebSiteVehicleID from dbo.tbl_Fleet where WebSiteVehicleID is not null and WebSiteVehicleID != 0";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<FleetModel>();
                    while (reader.Read())
                    {
                        var record = new FleetModel();
                        record.VehicleID = Convert.ToInt32(reader.GetValue(0));
                        record.WebsiteVehicleID = GetIntValue(reader, 1);

                        list.Add(record);
                    }
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        // to get posting data
        public bool SaveWebRentalDataFromNewQuotePage(WebRentalModel model)
        {
            var queryString = "insert into dbo.tbl_WebRental (FirstName, LastName, MiddleName, StreetAddress, City, State, PostalCode, Phone, Mobile, " +
                                    "Email, DepartureDate, ReturnDate, Notes, AlternateDestinationName, AlternateDestinationMileage, AlternateAdvertiseSource, Adults, Children, " +
                                    "VehicleDescription, CountryCode, InsuranceCompany, WebCallDateTime, DateTimeStamp, Status, AdvertizeID, Passengers, VehicleID) " +
                                    "values(@FirstName, @LastName, @MiddleName, @StreetAddress, @City, @State, @PostalCode, @Phone, @Mobile, @Email, " +
                                    "@DepartureDate, @ReturnDate, @Notes, @AlternateDestinationName, @AlternateDestinationMileage, @AlternateAdvertiseSource, @Adults, @Children, " +
                                    "@VehicleDescription, @CountryCode, @InsuranceCompany, @WebCallDateTime, @DateTimeStamp, @Status, @AdvertizeID, @Passengers, @VehicleID)";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@MiddleName", model.MiddleName);
                    command.Parameters.AddWithValue("@StreetAddress", model.StreetAddress);
                    command.Parameters.AddWithValue("@City", model.City);
                    command.Parameters.AddWithValue("@State", model.State);
                    command.Parameters.AddWithValue("@PostalCode", model.PostalCode);
                    command.Parameters.AddWithValue("@Phone", model.Phone);
                    command.Parameters.AddWithValue("@Mobile", model.Mobile);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@DepartureDate", model.DepartureDate);
                    command.Parameters.AddWithValue("@ReturnDate", model.ReturnDate);
                    command.Parameters.AddWithValue("@Notes", model.Notes);
                    command.Parameters.AddWithValue("@AlternateDestinationName", model.AlternateDestinationName);
                    command.Parameters.AddWithValue("@AlternateDestinationMileage", IsSetDBNullFromInt(model.AlternateDestinationMileage));
                    command.Parameters.AddWithValue("@AlternateAdvertiseSource", model.AlternateAdvertiseSource);
                    command.Parameters.AddWithValue("@Adults", IsSetDBNullFromInt(model.Adults));
                    command.Parameters.AddWithValue("@Children", IsSetDBNullFromInt(model.Children));
                    command.Parameters.AddWithValue("@VehicleDescription", model.VehicleDescription);
                    command.Parameters.AddWithValue("@CountryCode", model.CountryCode);
                    command.Parameters.AddWithValue("@InsuranceCompany", model.InsuranceCompany);
                    command.Parameters.AddWithValue("@WebCallDateTime", model.WebCallDateTime);
                    command.Parameters.AddWithValue("@DateTimeStamp", model.DateTimeStamp);
                    command.Parameters.AddWithValue("@Status", model.Status);
                    command.Parameters.AddWithValue("@AdvertizeID", model.AdvertizeID);
                    command.Parameters.AddWithValue("@Passengers", IsSetDBNullFromInt(model.Passengers));
                    command.Parameters.AddWithValue("@VehicleID", model.VehicleID);

                    int id = command.ExecuteNonQuery();
                    if (id > 0)
                        return true;

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        private int? GetIntValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetInt32(index);

            return null;
        }

        private DateTime? GetDateTimeValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetDateTime(index);

            return null;
        }

        public object IsSetDBNull(Guid? id)
        {
            if (id == null)
                return DBNull.Value;

            return id;
        }

        private object IsSetDBNullFromInt(int? val)
        {
            if (val == null)
                return DBNull.Value;

            return val;
        }

        private object IsSetDBNullFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return DBNull.Value;

            return str;
        }
    }
}