using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DentistSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {

            if (ValidateUser(model.Username, model.Password))
            {
                var token = GenerateJwtToken(model.Username, "Patient");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }

        [HttpPost("dentistlogin")]
        public IActionResult DentistLogin([FromBody] LoginModel model)
        {
			
			if (ValidateDentistLogin(model.Username, model.Password))
			{
				var token = GenerateJwtToken(model.Username, "Dentist");
				return Ok(new { Token = token });
			}

			return Unauthorized("Invalid credentials");
		}

		[HttpPost("managerlogin")]
		public IActionResult ManagerLogin([FromBody] LoginModel model)
		{
			
			if (ValidateManagerLogin(model.Username, model.Password))
			{
				var token = GenerateJwtToken(model.Username, "Manager");
				return Ok(new { Token = token });
			}

			return Unauthorized("Invalid credentials");
		}

		[HttpPost("receptionistlogin")]
		public IActionResult ReceptionistLogin([FromBody] LoginModel model)
		{
			
			if (ValidateReceptionistLogin(model.Username, model.Password))
			{
				var token = GenerateJwtToken(model.Username, "Receptionist");
				return Ok(new { Token = token });
			}

			return Unauthorized("Invalid credentials");
		}

		private bool ValidateUser(string username, string password)
        {
            string query = @"SELECT COUNT(1) FROM dbo.patient WHERE email = @Email AND password = @Password";
            string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
            using (SqlConnection con = new SqlConnection(sqlDatasource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 1;
                }
            }
        }

		private bool ValidateDentistLogin(string username, string password)
		{
			string query = @"SELECT COUNT(1) FROM dbo.dentist WHERE email = @Email AND password = @Password";
			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
			using (SqlConnection con = new SqlConnection(sqlDatasource))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@Email", username);
					cmd.Parameters.AddWithValue("@Password", password);

					int count = Convert.ToInt32(cmd.ExecuteScalar());
					return count == 1;
				}
			}
		}

		private bool ValidateManagerLogin(string username, string password)
		{
			string query = @"SELECT COUNT(1) FROM dbo.practicemanager WHERE email = @Email AND password = @Password";
			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
			using (SqlConnection con = new SqlConnection(sqlDatasource))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@Email", username);
					cmd.Parameters.AddWithValue("@Password", password);

					int count = Convert.ToInt32(cmd.ExecuteScalar());
					return count == 1;
				}
			}
		}

		private bool ValidateReceptionistLogin(string username, string password)
		{
			string query = @"SELECT COUNT(1) FROM dbo.receptionist WHERE email = @Email AND password = @Password";
			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
			using (SqlConnection con = new SqlConnection(sqlDatasource))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@Email", username);
					cmd.Parameters.AddWithValue("@Password", password);

					int count = Convert.ToInt32(cmd.ExecuteScalar());
					return count == 1;
				}
			}
		}



		private string GenerateJwtToken(string username, string role)
        {
            // Create the claims for the token (you can add more claims if needed)
            var claims = new[]
            {
				new Claim(JwtRegisteredClaimNames.Sub, username),
				new Claim("role", role),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

            Console.WriteLine($"Generated role claim: {claims.FirstOrDefault(c => c.Type == "role")?.Value}");

            // Get the secret key from appsettings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Sign the token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60), // Token expires after 60 minutes
                signingCredentials: creds);

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("claims")]
        [Authorize] // This ensures the user is authenticated before accessing this endpoint
        public IActionResult GetClaims()
        {
            var claims = User.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToList();
            return Ok(claims);
        }

    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
