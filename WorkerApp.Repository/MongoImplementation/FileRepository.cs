using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using WorkerApp.Domain;
using WorkerApp.Repository.Interfaces;

namespace WorkerApp.Repository.MongoImplementation
{
    public class FileRepository : IFileRepository
    {
        private readonly MongoContext db;

        public FileRepository()
        {
            db = new MongoContext();
        }

        public async Task<File> CreateFile(File file)
        {
            await db.Files.InsertOneAsync(file);
            return file;
        }

        public async Task DeleteFile(string id)
        {
            FilterDefinition<File> filter = Builders<File>.Filter.Eq("FileId", id);
            await db.Files.DeleteOneAsync(filter);
        }

        public async Task<File> GetFile(string id)
        {
            FilterDefinition<File> filter = Builders<File>.Filter.Eq("FileId", id);
            return await db.Files.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<File>> GetFiles()
        {
            return await db.Files.Find(FilterDefinition<File>.Empty).ToListAsync(); 
        }

        public async Task UpdateFile(File file)
        {
            FilterDefinition<File> filter = Builders<File>.Filter.Eq("FileId", file.FileId);
            await db.Files.ReplaceOneAsync(filter, file);
        }
    }
}
