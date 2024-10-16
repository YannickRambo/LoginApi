using MySql.Data.MySqlClient;

public class Connection : IDisposable
{
    public MySqlConnection SqlConnection { get; }
    private readonly IConfiguration _configuration;
    public Connection(IConfiguration configuration)
    {
        _configuration = configuration;
        SqlConnection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        SqlConnection.Open();
    }
    public void Dispose()
    {
        SqlConnection.Close();
    }
}