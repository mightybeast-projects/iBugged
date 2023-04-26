using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    [BsonElement("name")]
    public string? name { get; set; }

    [BsonElement("description")]
    public string? description { get; set; }

    [BsonElement("members")]
    public List<string>? members { get; set; }
}