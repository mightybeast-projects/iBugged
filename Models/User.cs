using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }
    public string? name { get; set; }
    public string? email { get; set; }
    public string? password { get; set; }
    public string? organization { get; set; }
    public string? role { get; set; }
}