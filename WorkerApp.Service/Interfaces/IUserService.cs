using System.Threading.Tasks;
using WorkerApp.Domain;

namespace WorkerApp.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> LogIn(User user);
        Task<User> Register(User user);
    }
}
