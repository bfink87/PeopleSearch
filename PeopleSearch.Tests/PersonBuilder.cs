using PeopleSearch.Models;

namespace PeopleSearch.Tests
{
    public class PersonBuilder
    {
        private readonly Person _person = new Person();

        public PersonBuilder Id(int id)
        {
            _person.Id = id;
            return this;
        }

        public PersonBuilder FirstName(string firstName)
        {
            _person.FirstName = firstName;
            return this;
        }

        public Person Build() => _person;
    }
}
