using System.Threading.Tasks;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;
using WorkerApp.Service.Interfaces;
using WorkerApp.Service.Utilities;

namespace WorkerApp.Service.ServicesImplementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<User> LogIn(User user)
        {
            user.Password = PasswordUtilities.CreateMD5Password(user.Password);
            return await userRepository.LogIn(user);
        }

        public async Task<User> Register(User user)
        {
            user.Rol = "Worker";
            user.Active = true;
            user.Password = PasswordUtilities.CreateMD5Password(user.Password);
            await userRepository.Register(user);

            return user;
        }
    }
}
