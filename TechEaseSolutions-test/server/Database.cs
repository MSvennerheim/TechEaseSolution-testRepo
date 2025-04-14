namespace server.Properties;
using Npgsql;
using DotNetEnv;

public class Database
{

    // create a .env file in /server and add these rows with replacement for relevant local data
    //
    // PGHOST=localhost
    // PGPORT=myport
    // PGDATABASE=mydb
    // PGUSER=myuser
    // PGPASSWORD=mypassword
    //
    // for testing with local db
    
    private readonly string _host;
    private readonly string _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _database;

    private NpgsqlDataSource _connection;

    public NpgsqlDataSource Connection()
    {
        return _connection;
    }

    public Database()
    {
        Env.Load();
        _host = "217.76.56.135";
        _port = "5436";
        _username = "postgres";
        _password = "HealthyDealerSweats!";
        _database = "postgres";
        
        string connectionString = $"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database}";
        _connection = NpgsqlDataSource.Create(connectionString);
        
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("connected to db");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}
