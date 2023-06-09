using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models.Enums;

public enum Role
{
    [BsonRepresentation(BsonType.String)]
    ProjectManager,
    [BsonRepresentation(BsonType.String)]
    Developer,
    [BsonRepresentation(BsonType.String)]
    TeamMember
}