using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerApp.Domain;

namespace WorkerApp.Service.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPeople();
        Task<Person> GetPerson(string id);
        Task<Person> GetPersonByUserId(string userId);
        Task<Person> CreatePerson(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(string id);
        Task<decimal?> GetProfileStatus(string id);
    }
}
