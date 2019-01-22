using System;

namespace PeopleSearch.Models
{
    public class PersonForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Interests { get; set; }
        public string Photo { get; set; }
    }
}