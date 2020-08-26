using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WorkerApp.Domain
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PersonId { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Profession { get; set; }
        public string Photo { get; set; }
        public string UserId { get; set; }
        public IEnumerable<FileAttached> FilesAttached { get; set; }
    }
}
