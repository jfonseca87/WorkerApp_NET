using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.Service.ServicesImplementations
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IFileRepository fileRepository;

        public PersonService(IPersonRepository _personRepository, IFileRepository _fileRepository)
        {
            personRepository = _personRepository;
            fileRepository = _fileRepository;
        }

        public async Task<Person> CreatePerson(Person person)
        {
            await personRepository.CreatePerson(person);
            return person;
        }

        public async Task DeletePerson(string id)
        {
            await personRepository.DeletePerson(id);
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await personRepository.GetPeople();
        }

        public async Task<Person> GetPerson(string id)
        {
            return await personRepository.GetPerson(id);
        }

        public async Task<Person> GetPersonByUserId(string userId)
        {
            return await personRepository.GetPersonByUserId(userId);
        }

        public async Task<decimal?> GetProfileStatus(string id)
        {
            decimal percentage;
            Person personDB = await personRepository.GetPerson(id);

            if (personDB == null)
            {
                return null;
            }

            percentage = 35m;
            percentage += !string.IsNullOrEmpty(personDB.Photo) ? 15m : 0m;

            IEnumerable<File> fileTypes = await fileRepository.GetFiles();

            if (fileTypes.Any()) {
                decimal percentPerFile = 50m / fileTypes.Count();
                percentage += percentPerFile * personDB.FilesAttached.Count();
            }

            return percentage;
        }

        public async Task UpdatePerson(Person person)
        {
            var personUpdate = await personRepository.GetPerson(person.PersonId);

            personUpdate.Names = person.Names;
            personUpdate.Surnames = person.Surnames;
            personUpdate.Address = person.Address;
            personUpdate.PhoneNumber = person.PhoneNumber;
            personUpdate.Profession = person.Profession;

            await personRepository.UpdatePerson(personUpdate);
        }

    }
}
