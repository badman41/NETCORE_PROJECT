using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Address : IEntity<Address>
    {
        public Identity Id { get; }
        public City City { get; }
        public Country Country { get; }
        public District District { get; }
        public Lat Lat { get; }
        public Lng Lng { get; }
        public Street Street { get; }
        public StreetNumber StreetNumber { get; }

        public Address(Identity id, City city, Country country, District district, Lat lat, Lng lng, Street street, StreetNumber streetNumber)
        {
            Id = id;
            City = city;
            Country = country;
            District = district;
            Lat = lat;
            Lng = lng;
            Street = street;
            StreetNumber = streetNumber;
        }

        public bool sameIdentityAs(Address other)
        {
            return Id.Equals(Id);
        }
    }
}
