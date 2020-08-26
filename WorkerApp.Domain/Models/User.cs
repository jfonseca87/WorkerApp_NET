using MongoDB.Bson.Serialization.Attributes;

namespace WorkerApp.Domain
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public string Rol { get; set; }
    }
}
