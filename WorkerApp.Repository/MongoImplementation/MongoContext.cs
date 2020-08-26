using System.Web.Configuration;
using MongoDB.Driver;
using WorkerApp.Domain;

namespace WorkerApp.Repository.MongoImplementation
{
    public class MongoContext
    {
        private enum Collections
        {
            Files,
            People,
            Users
        }
        private readonly IMongoDatabase db;
        private readonly string dbServer = WebConfigurationManager.AppSettings["DbUrl"];
        private readonly string dbName = WebConfigurationManager.AppSettings["DbName"];

        public MongoContext()
        {
            MongoClient client = new MongoClient(dbServer);
            db = client.GetDatabase(dbName);
        }

        public IMongoCollection<File> Files { 
            get 
            {
                return db.GetCollection<File>(Collections.Files.ToString());
            } 
        }

        public IMongoCollection<Person> People
        {
            get
            {
                return db.GetCollection<Person>(Collections.People.ToString());
            }
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return db.GetCollection<User>(Collections.Users.ToString());
            }
        }
    }
}
