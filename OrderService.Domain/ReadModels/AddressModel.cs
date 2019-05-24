using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class AddressModel
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public AddressModel() { }
        public AddressModel(int id, string city, string country, string district, string street, string streetNumber, double lat, double lng)
        {
            ID = id;
            City = city;
            Country = country;
            District = district;
            Street = street;
            StreetNumber = streetNumber;
            Lat = lat;
            Lng = lng;
        }
    }
}
