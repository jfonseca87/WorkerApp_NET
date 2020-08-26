using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerApp.Domain;

namespace WorkerApp.Repository.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPeople();
        Task<Person> GetPerson(string id);
        Task<Person> GetPersonByUserId(string userId);
        Task<Person> CreatePerson(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(string id);
        Task AddFile(string id, FileAttached file);
        Task DeleteFile(string personId, string fileId);
    }
}
