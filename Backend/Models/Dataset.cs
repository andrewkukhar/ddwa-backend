using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Dataset
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string? Name { get; set; }
}
