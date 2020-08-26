using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;

namespace WorkerApp.Repository.MongoImplementation
{
    public class PersonRepository : IPersonRepository
    {
        private readonly MongoContext db;

        public PersonRepository()
        {
            db = new MongoContext();
        }

        public async Task AddFile(string id, FileAttached file)
        {
            file.FileAttachedId = ObjectId.GenerateNewId().ToString();

            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("PersonId", id);
            UpdateDefinition<Person> objUpdate = Builders<Person>.Update.Push<FileAttached>(x => x.FilesAttached, file);

            await db.People.UpdateOneAsync(filter, objUpdate);
        }

        public async Task<Person> CreatePerson(Person person)
        {
            await db.People.InsertOneAsync(person);
            return person;
        }

        public async Task DeleteFile(string personId, string fileId)
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("PersonId", personId);
            UpdateDefinition<Person> objUpdate = Builders<Person>.Update.PullFilter("FilesAttached", Builders<FileAttached>.Filter.Eq("FileAttachedId", fileId));

            await db.People.UpdateOneAsync(filter, objUpdate);
        }

        public async Task DeletePerson(string id)
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("PersonId", id);
            await db.People.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await db.People.Find(FilterDefinition<Person>.Empty).ToListAsync();
        }

        public async Task<Person> GetPerson(string id)
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("PersonId", id);
            return await db.People.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Person> GetPersonByUserId(string userId)
        {
            return await db.People.Find(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task UpdatePerson(Person person)
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("PersonId", person.PersonId);
            await db.People.ReplaceOneAsync(filter, person);
        }
    }
}
