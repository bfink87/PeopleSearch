using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleSearch.Interfaces;
using PeopleSearch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IRepository _repository;

        public PeopleController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("query={query}")]
        public async Task<IEnumerable<Person>> SearchAsync([FromRoute] string query)
        {
            // TODO: refactor search to improve performance with large datasets
            var people = await _repository.ListAsync<Person>();

            // Simulate slow query
            Thread.Sleep(3000);

            var filteredList = people.Where(x => x.FirstName.ToLower().Contains(query)
                    || x.LastName.ToLower().Contains(query)
                    || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(query));

            return filteredList;
        }

        // GET: api/People
        [HttpGet]
        public async Task<IEnumerable<Person>> GetPeople()
        {
            // Simulate slow query
            Thread.Sleep(3000);

            return await _repository.ListAsync<Person>();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _repository.GetByIdAsync<Person>(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson([FromRoute] int id, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(person);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _repository.EntityExistsAsync<Person>(id);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/People
        [HttpPost]
        public async Task<IActionResult> PostPerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(person);

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _repository.GetByIdAsync<Person>(id);
            if (person == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(person);

            return Ok(person);
        }
    }
}
