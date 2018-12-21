using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PeopleSearch.Data;
using PeopleSearch.Models;
using Xunit;

namespace PeopleSearch.Tests
{
    public class EfRepositoryShould
    {
        private AppDbContext _dbContext;

        private static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("PeopleSearch")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task AddPersonAndSetIdAsync()
        {
            var repository = GetRepository();
            var person = new PersonBuilder().Build();

            await repository.AddAsync(person);

            var people = await repository.ListAsync<Person>();
            var newPerson = people.FirstOrDefault();

            Assert.Equal(person, newPerson);
            Assert.True(newPerson?.Id > 0);
        }

        [Fact]
        public async Task UpdatePersonAfterAddingThemAsync()
        {
            // add a person
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var person = new PersonBuilder().FirstName(initialName).Build();

            await repository.AddAsync(person);

            // detach the person so we get a different instance
            _dbContext.Entry(person).State = EntityState.Detached;

            // fetch the person and update its first name
            var people = await repository.ListAsync<Person>();
            var newPerson = people.FirstOrDefault(i => i.FirstName == initialName);

            Assert.NotNull(newPerson);
            Assert.NotSame(person, newPerson);
            var newFirstName = Guid.NewGuid().ToString();
            newPerson.FirstName = newFirstName;

            // Update the person
            await repository.UpdateAsync(newPerson);
            var updatedPeople = await repository.ListAsync<Person>();
            var updatedPerson = updatedPeople.FirstOrDefault(i => i.FirstName == newFirstName);

            Assert.NotNull(updatedPerson);
            Assert.NotEqual(person.FirstName, updatedPerson.FirstName);
            Assert.Equal(newPerson.Id, updatedPerson.Id);
        }

        [Fact]
        public async Task DeletePersonAfterAddingItAsync()
        {
            // add a person
            var repository = GetRepository();
            var initialFirstName = Guid.NewGuid().ToString();
            var person = new PersonBuilder().FirstName(initialFirstName).Build();
            await repository.AddAsync(person);

            // delete the person
            await repository.DeleteAsync(person);

            var people = await repository.ListAsync<Person>();

            // verify it's no longer there
            Assert.DoesNotContain(people,
                i => i.FirstName == initialFirstName);
        }

        private EfRepository GetRepository()
        {
            var options = CreateNewContextOptions();

            _dbContext = new AppDbContext(options);
            return new EfRepository(_dbContext);
        }
    }
}
