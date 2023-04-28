using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; } = null!;

    [BsonElement("name")]
    public string name { get; set; } = null!;

    [BsonElement("email")]
    public string email { get; set; } = null!;

    [BsonElement("password")]
    public string password { get; set; } = null!;

    [BsonElement("organization")]
    public string organization { get; set; } = null!;

    [BsonElement("role")]
    [Required]
    public Role role { get; set; }
}