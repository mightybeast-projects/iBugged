using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models.Enums;

public enum TicketType
{
    [BsonRepresentation(BsonType.String)]
    Bug,
    [BsonRepresentation(BsonType.String)]
    Task,
    [BsonRepresentation(BsonType.String)]
    Support,
    [BsonRepresentation(BsonType.String)]
    Other
}