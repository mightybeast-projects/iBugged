using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; } = null!;

    [BsonElement("name")]
    public string name { get; set; } = null!;

    [BsonElement("description")]
    public string description { get; set; } = null!;

    [BsonElement("members_id")]
    public List<string> membersId { get; set; } = new List<string>();
}