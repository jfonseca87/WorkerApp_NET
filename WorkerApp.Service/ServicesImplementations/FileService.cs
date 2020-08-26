using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.Service.ServicesImplementations
{
    public class FileService : IFileService
    {
        private readonly IFileRepository fileRepository;

        public FileService(IFileRepository _fileRepository)
        {
            fileRepository = _fileRepository;
        }

        public async Task<File> CreateFile(File file)
        {
            await fileRepository.CreateFile(file);
            return file;
        }

        public async Task DeleteFile(string id)
        {
            await fileRepository.DeleteFile(id);
        }

        public async Task<File> GetFile(string id)
        {
            return await fileRepository.GetFile(id);
        }

        public async Task<IEnumerable<File>> GetFiles()
        {
            return await fileRepository.GetFiles();
        }

        public async Task UpdateFile(File file)
        {
            await fileRepository.UpdateFile(file);
        }
    }
}
