using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public enum Priority
{
    [BsonRepresentation(BsonType.String)]
    Low,
    [BsonRepresentation(BsonType.String)]
    Medium,
    [BsonRepresentation(BsonType.String)]
    High
}