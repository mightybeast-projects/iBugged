using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class Project : Document
{
    [BsonElement("name")]
    public string name { get; set; } = null!;

    [BsonElement("description")]
    public string description { get; set; } = null!;

    [BsonElement("members_id")]
    public List<string> membersId { get; set; } = new List<string>();
}