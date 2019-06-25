using System;
using System.Collections.Generic;

namespace PeopleSearch.Entities
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public List<Interest> Interests { get; set; }
        public string PhotoUri { get; set; }
    }
}
