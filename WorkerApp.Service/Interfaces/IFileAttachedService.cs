using System.Threading.Tasks;
using WorkerApp.Domain;

namespace WorkerApp.Service.Interfaces
{
    public interface IFileAttachedService
    {
        Task AddFile(string personId, FileAttached file);
        Task RemoveFile(string personId, string fileId);
    }
}
