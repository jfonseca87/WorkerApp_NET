using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerApp.Domain;

namespace WorkerApp.Service.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<File>> GetFiles();
        Task<File> GetFile(string id);
        Task<File> CreateFile(File file);
        Task UpdateFile(File file);
        Task DeleteFile(string id);
    }
}
