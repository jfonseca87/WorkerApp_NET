using MongoDB.Bson.Serialization.Attributes;

namespace WorkerApp.Domain
{
    public class File
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string FileId { get; set; }
        public string FileType { get; set; }
        public string AllowedExtensions { get; set; }
    }
}
