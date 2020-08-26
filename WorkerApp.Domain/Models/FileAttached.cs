using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace WorkerApp.Domain
{
    public class FileAttached
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string FileAttachedId { get; set; }
        public string FileType { get; set; }
        public string Path { get; set; }

        [BsonIgnore]
        public HttpPostedFileBase File { get; set; }

        [BsonIgnore]
        public string PersonId { get; set; }
    }
}
