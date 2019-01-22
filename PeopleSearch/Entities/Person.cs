using System;

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
        // TODO: move interests to a separate class to maintain them separately
        public string Interests { get; set; }
        public string Photo { get; set; }
    }
}
