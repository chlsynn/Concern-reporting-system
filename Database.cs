using Microsoft.Data.Sqlite;

namespace ConcernReportingSystem;

public static class Database
{
    private static readonly string connectionString =
        "Data Source=" + Path.Combine(AppContext.BaseDirectory, "concerns.db");

    public static void Initialize()
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS concerns (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            reporter_name TEXT,
            category TEXT NOT NULL,
            location TEXT NOT NULL,
            description TEXT NOT NULL,
            status TEXT NOT NULL DEFAULT 'Pending',
            admin_remarks TEXT,
            created_at TEXT NOT NULL)";
        cmd.ExecuteNonQuery();
    }

    public static void Add(Concern c)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO concerns
            (reporter_name, category, location, description, created_at)
            VALUES ($name, $category, $location, $description, $created)";
        cmd.Parameters.AddWithValue("$name", c.ReporterName);
        cmd.Parameters.AddWithValue("$category", c.Category);
        cmd.Parameters.AddWithValue("$location", c.Location);
        cmd.Parameters.AddWithValue("$description", c.Description);
        cmd.Parameters.AddWithValue("$created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.ExecuteNonQuery();
    }

    public static List<Concern> GetAll()
    {
        var list = new List<Concern>();

        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT id, reporter_name, category, location, description,
            status, admin_remarks, created_at FROM concerns ORDER BY id DESC";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Concern
            {
                Id = reader.GetInt32(0),
                ReporterName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                Category = reader.GetString(2),
                Location = reader.GetString(3),
                Description = reader.GetString(4),
                Status = reader.GetString(5),
                AdminRemarks = reader.IsDBNull(6) ? "" : reader.GetString(6),
                CreatedAt = DateTime.Parse(reader.GetString(7))
            });
        }
        return list;
    }

    public static void UpdateStatus(int id, string status, string remarks)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE concerns SET status = $status, admin_remarks = $remarks WHERE id = $id";
        cmd.Parameters.AddWithValue("$status", status);
        cmd.Parameters.AddWithValue("$remarks", remarks);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    public static void Delete(int id)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM concerns WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}