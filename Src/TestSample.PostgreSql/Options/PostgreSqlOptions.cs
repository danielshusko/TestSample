namespace TestSample.PostgreSql.Options;

public class PostgreSqlOptions
{
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public string? Server { get; set; }
    public int Port { get; set; }
    public string? Database { get; set; }
    public string? Schema { get; set; }
    public bool RunMigrationOnStart { get; set; }
}