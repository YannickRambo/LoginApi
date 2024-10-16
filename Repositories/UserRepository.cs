using MySql.Data.MySqlClient;

public class UserRepository
{
    private readonly Connection _connection;
    public UserRepository(Connection connection)
    {
        _connection = connection;
    }
    public List<User> GetUsers()
    {
        var users = new List<User>();

        string query = "SELECT * FROM users";

        using var command = new MySqlCommand(query, _connection.SqlConnection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var user = new User(reader.GetInt32("id"), reader.GetString("name"), reader.GetString("email"), reader.GetString("password"));
            users.Add(user);
        }

        return users;
    }

    public bool InsertUser(User user)
    {
        string query = "INSERT INTO users (name, email, password) VALUES (@name, @email, @password)";

        using var command = new MySqlCommand(query, _connection.SqlConnection);

        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", user.Password);

        int affectedRows = command.ExecuteNonQuery();

        return affectedRows > 0;
    }

    public User GetUserByEmail(User user)
    {
        User searchedUser = null;

        string query = "SELECT * FROM users WHERE email = @email";

        using var command = new MySqlCommand(query, _connection.SqlConnection);

        command.Parameters.AddWithValue("@email", user.Email);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            searchedUser = new User(reader.GetInt32("id"), reader.GetString("name"), reader.GetString("email"), reader.GetString("password"));
        }

        return searchedUser;
    }

    public User GetUserByEmailAndPassword(User user)
    {
        User searchedUser = null;

        string query = "SELECT * FROM users WHERE email = @email AND password = @password";

        using var command = new MySqlCommand(query, _connection.SqlConnection);

        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", user.Password);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            searchedUser = new User(reader.GetInt32("id"), reader.GetString("name"), reader.GetString("email"), reader.GetString("password"));
        }

        return searchedUser;
    }
}