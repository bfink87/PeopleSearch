using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleSearch.Interfaces;
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
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("query={query}")]
        public async Task<IActionResult> Find([FromRoute] string query)
        {
            var people = await _personService.FindAsync(query);

            return Ok(people);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var people = await _personService.FindAsync();
                
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var person = await _personService.GetByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            
            return Ok(person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PersonForManipulation person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            await _personService.SaveAsync(person, id);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonForManipulation person)
        {
            if (person == null)
            {
                return BadRequest();
            }
            
            var personToReturn = await _personService.SaveAsync(person);

            return CreatedAtAction("GetById", new { id = personToReturn.Id }, personToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _personService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}