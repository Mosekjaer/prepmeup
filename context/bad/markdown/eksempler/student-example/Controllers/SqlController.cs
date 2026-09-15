using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace sql_demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SqlController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // **************************************************
        // OBS: Husk a erstatte dindatabase & ditpassword i connection string
        // **************************************************
        var connectionString = "Data Source=localhost;Database=dindatabase;User Id=sa;Password=ditpassword;TrustServerCertificate=True";
        var users = new List<User>();

        using (var conn = new SqlConnection(connectionString))
        {
            var sql = "SELECT * FROM Users";
            var command = new SqlCommand(sql, conn);
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    (
                       int.Parse(reader[0].ToString()),
                       reader[1].ToString(),
                       reader[2].ToString()
                    ));
                }
            }
        }
        return Ok(users);
    }
}

record User(int Id, string Name, string Email);