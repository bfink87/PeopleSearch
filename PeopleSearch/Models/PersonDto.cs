using System;
using System.ComponentModel.DataAnnotations;
using PeopleSearch.Entities;

namespace PeopleSearch.Models
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public int Age { get; set; }
        public string Interests { get; set; }
        public string Photo { get; set; }
    }
}