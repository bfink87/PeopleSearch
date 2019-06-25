using System.Collections.Generic;
using System.Threading.Tasks;
using PeopleSearch.Models;

namespace PeopleSearch.Interfaces
{
    public interface IPersonService
    {
        Task<List<PersonDto>> FindAsync(string query = "");
        Task<PersonDto> GetByIdAsync(int id);
        Task<PersonDto> SaveAsync(PersonForManipulation person, int id = 0);
        Task<bool> DeleteAsync(int id);
    }
}