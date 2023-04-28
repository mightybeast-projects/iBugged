using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public enum Role
{
    [BsonRepresentation(BsonType.String)]
    ProjectManager,
    [BsonRepresentation(BsonType.String)]
    TeamMember,
    [BsonRepresentation(BsonType.String)]
    Developer
}