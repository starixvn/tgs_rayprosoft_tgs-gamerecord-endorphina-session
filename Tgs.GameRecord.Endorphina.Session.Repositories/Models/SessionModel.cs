using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Tgs.GameRecord.Endorphina.Session.Core;

namespace Tgs.GameRecord.Endorphina.Session.Repositories.Models;

public record SessionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = default!;

    [BsonElement("brand_code")]
    [BsonRepresentation(BsonType.String)]
    public string BrandCode { get; set; } = default!;

    [BsonElement("player_code")]
    [BsonRepresentation(BsonType.String)]
    public string PlayerCode { get; set; } = default!;

    [BsonElement("state")]
    [BsonRepresentation(BsonType.String)]
    public TokenState State { get; set; }

    [BsonElement("retry")]
    [BsonRepresentation(BsonType.Int32)]
    public int Retry { get; set; }

    [BsonElement("merchant_id")]
    [BsonRepresentation(BsonType.String)]
    public string MerchantId { get; set; } = default!;
}
