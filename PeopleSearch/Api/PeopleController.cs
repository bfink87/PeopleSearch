using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleSearch.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PeopleSearch.Entities;
using PeopleSearch.Models;

namespace PeopleSearch.Api
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IRepository _repository;

        public PeopleController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("query={query}")]
        public async Task<IActionResult> Find([FromRoute] string query)
        {
            // Simulate slow query
            Thread.Sleep(3000);
            
            var peopleFromRepo = await _repository.ListAsync<Person>();

            // TODO: refactor search to improve performance with large data sets
            var filteredPeople = peopleFromRepo.Where(x => x.FirstName.ToLower().Contains(query)
                    || x.LastName.ToLower().Contains(query)
                    || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(query));
            
            var people = Mapper.Map<IEnumerable<PersonDto>>(filteredPeople);

            return Ok(people);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Simulate slow query
            Thread.Sleep(3000);

            var peopleFromRepo = await _repository.ListAsync<Person>();

            var people = Mapper.Map<IEnumerable<PersonDto>>(peopleFromRepo);
                
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var personFromRepo = await _repository.GetByIdAsync<Person>(id);

            if (personFromRepo == null)
            {
                return NotFound();
            }

            var person = Mapper.Map<PersonDto>(personFromRepo);

            return Ok(person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PersonForUpdateDto person)
        {
            if (person == null)
            {
                return BadRequest();
            }
            
            if (!await _repository.EntityExistsAsync<Person>(id))
            {
                return NotFound();
            }

            var personFromRepo = await _repository.GetByIdAsync<Person>(id);
            if (personFromRepo == null)
            {
                return NotFound();
            }

            try
            {
                Mapper.Map(person, personFromRepo);

                await _repository.UpdateAsync(personFromRepo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.EntityExistsAsync<Person>(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonForCreationDto person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            var personEntity = Mapper.Map<Person>(person);

            await _repository.AddAsync(personEntity);

            var personToReturn = Mapper.Map<PersonDto>(personEntity);

            return CreatedAtAction("GetById", new { id = personToReturn.Id }, personToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!await _repository.EntityExistsAsync<Person>(id))
            {
                return NotFound();
            }

            var personFromRepo = await _repository.GetByIdAsync<Person>(id);
            if (personFromRepo == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(personFromRepo);

            return NoContent();
        }
    }
}