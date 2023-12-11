namespace TestSample.PostgreSql.Models;

public class UserDataModel
{
    public int Id { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}