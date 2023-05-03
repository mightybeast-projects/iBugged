using iBugged.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class User : Document
{
    [BsonElement("name")]
    public string name { get; set; } = null!;

    [BsonElement("email")]
    public string email { get; set; } = null!;

    [BsonElement("password")]
    public string password { get; set; } = null!;

    [BsonElement("role")]
    public Role role { get; set; }
}