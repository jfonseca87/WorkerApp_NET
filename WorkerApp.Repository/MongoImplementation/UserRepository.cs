using System.Threading.Tasks;
using MongoDB.Driver;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;

namespace WorkerApp.Repository.MongoImplementation
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoContext db;

        public UserRepository()
        {
            db = new MongoContext();
        }

        public async Task<User> LogIn(User user)
        {
            return await db.Users.Find(x => 
                                       x.Email.Equals(user.Email) &&
                                       x.Password.Equals(user.Password)
                         ).FirstOrDefaultAsync();
        }

        public async Task<User> Register(User user)
        {
            await db.Users.InsertOneAsync(user);
            return user;
        }
    }
}
