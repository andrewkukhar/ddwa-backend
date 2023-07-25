using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class DatabaseContext
{
  private readonly IMongoDatabase _db;
  public IMongoCollection<User> Users { get; }
  public IMongoCollection<Dataset> Datasets { get; }

  public DatabaseContext(IOptions<Settings> settings)
  {
    var client = new MongoClient(settings.Value.ConnectionString);
    _db = client.GetDatabase(settings.Value.Database);
    Users = _db.GetCollection<User>("Users");
    Datasets = _db.GetCollection<Dataset>("Datasets");
  }

  public class Settings
  {
    public string ConnectionString { get; set; }
    public string Database { get; set; }
  }
}
