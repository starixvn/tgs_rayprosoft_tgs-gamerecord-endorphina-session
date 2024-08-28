using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Tgs.GameRecord.Endorphina.Session.Core;

namespace Tgs.GameRecord.Endorphina.Session.Repositories.Models;

[Table("session_forms")]
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

	[BsonElement("created_on")]
	public DateTime CreatedOn { get; set; }
	[BsonElement("update_on")]
	public DateTime UpdateOn { get; set; }
}
