using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iBugged.Models;

public class Ticket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; } = null!;

    [BsonElement("title")]
    public string title { get; set; } = null!;

    [BsonElement("description")]
    public string description { get; set; } = null!;

    [BsonElement("ticketType")]
    public TicketType ticketType { get; set; }

    [BsonElement("priority")]
    public Priority priority { get; set; }

    [BsonElement("opened")]
    public bool isOpened { get; set; } = true;

    [BsonElement("project_id")]
    public string projectId { get; set; } = null!;

    [BsonElement("author_id")]
    public string authorId { get; set; } = null!;

    [BsonElement("assignee_id")]
    public string assigneeId { get; set; } = null!;

    [BsonElement("creation_date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime creationDate { get; set; } = DateTime.Now;
}