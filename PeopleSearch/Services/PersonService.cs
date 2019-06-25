using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeopleSearch.Entities;
using PeopleSearch.Interfaces;
using PeopleSearch.Models;

namespace PeopleSearch.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository _repository;
        
        public PersonService(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<List<PersonDto>> FindAsync(string query = "")
        {
            List<Person> people;
            
            // Simulate slow query
            Thread.Sleep(3000);
            
            var peopleFromRepo = await _repository.ListAsync<Person>();
            
            if (!string.IsNullOrEmpty(query))
            {
                // TODO: refactor search to improve performance with large data sets
                people = peopleFromRepo.Where(x => x.FirstName.ToLower().Contains(query)
                                                   || x.LastName.ToLower().Contains(query)
                                                   || (x.FirstName.ToLower() + " " + x.LastName.ToLower())
                                                   .Contains(query)).ToList();
            }
            else
            {
                people = peopleFromRepo;
            }

            return Mapper.Map<List<PersonDto>>(people);
        }

        public async Task<PersonDto> GetByIdAsync(int id)
        {
            var personFromRepo = await _repository.GetByIdAsync<Person>(id);

            return Mapper.Map<PersonDto>(personFromRepo);
        }

        public async Task<PersonDto> SaveAsync(PersonForManipulation person, int id = 0)
        {
            if (id == 0)
            {
                var personEntity = Mapper.Map<Person>(person);

                await _repository.AddAsync(personEntity);

                return Mapper.Map<PersonDto>(personEntity);
            }

            if (!await _repository.EntityExistsAsync<Person>(id))
            {
                
            }

            var personFromRepo = await _repository.GetByIdAsync<Person>(id);
            if (personFromRepo == null)
            {
                return null;
            }

            try
            {
                Mapper.Map(person, personFromRepo);

                await _repository.UpdateAsync(personFromRepo);

                return null;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.EntityExistsAsync<Person>(id))
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _repository.EntityExistsAsync<Person>(id))
            {
                return false;
            }

            var personFromRepo = await _repository.GetByIdAsync<Person>(id);
            if (personFromRepo == null)
            {
                return false;
            }

            await _repository.DeleteAsync(personFromRepo);

            return true;
        }
    }
}