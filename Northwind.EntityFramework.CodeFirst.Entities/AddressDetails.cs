using System;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class AddressDetails : IEquatable<AddressDetails>
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public bool Equals(AddressDetails other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(this.Address, other.Address) 
                && Equals(this.City, other.City) 
                && Equals(this.Region, other.Region)
                && Equals(this.PostalCode, other.PostalCode)
                && Equals(this.Country, other.Country);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(AddressDetails)) return false;
            return Equals((AddressDetails)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Address.GetHashCode();
                result ^= this.City.GetHashCode();
                result ^= this.Region.GetHashCode();
                result ^= this.PostalCode.GetHashCode();
                result ^= this.Country.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(AddressDetails left, AddressDetails right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AddressDetails left, AddressDetails right)
        {
            return !Equals(left, right);
        }
    }
}