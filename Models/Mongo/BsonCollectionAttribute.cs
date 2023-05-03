namespace iBugged.Models.Mongo;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string collectionName;
    public BsonCollectionAttribute(string collectionName) =>
        this.collectionName = collectionName;
}