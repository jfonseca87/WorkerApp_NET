using System.Threading.Tasks;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.Service.ServicesImplementations
{
    public class FileAttachedService : IFileAttachedService
    {
        private readonly IPersonRepository personRepository;

        public FileAttachedService(IPersonRepository _personRepository)
        {
            personRepository = _personRepository;
        }

        public async Task AddFile(string personId, FileAttached file)
        {
            file.Path = Utilities.File.SaveFile(personId, file.File);

            if (file.FileType == "Photo")
            {
                var person = await personRepository.GetPerson(personId);
                person.Photo = file.Path;
                await personRepository.UpdatePerson(person);
            }
            else
            {
                await personRepository.AddFile(personId, file);
            }
        }

        public async Task RemoveFile(string personId, string fileId)
        {
            await personRepository.DeleteFile(personId, fileId);
        }
    }
}
