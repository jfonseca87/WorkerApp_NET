using System.Threading.Tasks;
using WorkerApp.Domain;

namespace WorkerApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> LogIn(User user);
        Task<User> Register(User user);
    }
}
