using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Core.Models
{
    public class Debt
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string? Creditor { get; set; }
        public decimal Amount { get; set; }
    }
}

