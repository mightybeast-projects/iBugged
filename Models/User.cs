using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    [BsonElement("name")]
    public string? name { get; set; }

    [BsonElement("email")]
    public string? email { get; set; }

    [BsonElement("password")]
    public string? password { get; set; }

    [BsonElement("organization")]
    public string? organization { get; set; }

    [BsonElement("role")]
    public string? role { get; set; }
}