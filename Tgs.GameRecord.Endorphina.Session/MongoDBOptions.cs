namespace Tgs.GameRecord.Endorphina.Session;

public record MongoDBOptions
{
	public string ConnectionString { get; set; } = "mongodb://localhost";
}
