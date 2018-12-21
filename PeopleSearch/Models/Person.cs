using System;
using System.Collections.Generic;

namespace PeopleSearch.Models
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        // TODO: calculate age from a birthdate instead
        public int Age { get; set; }
        // TODO: move interests to a seperate class to maintain them separately
        public string Interests { get; set; }
        public string Photo { get; set; }
    }
}
