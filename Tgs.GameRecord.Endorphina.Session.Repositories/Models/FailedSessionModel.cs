using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tgs.GameRecord.Endorphina.Session.Repositories.Models;

public record FailedSessionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = default!;

    [BsonElement("brand_code")]
    [BsonRepresentation(BsonType.String)]
    public string BrandCode { get; set; } = default!;

    [BsonElement("player_code")]
    [BsonDefaultValue(false)]
    [BsonRepresentation(BsonType.String)]
    public string PlayerCode { get; set; } = default!;
}
