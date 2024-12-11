using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace DentistSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AppointmentController : ControllerBase
	{
		private IConfiguration _configuration;

		public AppointmentController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("GetAppointments")]
		public JsonResult GetAppointment()
		{
			string query = "select * from dbo.appointment";
			DataTable table = new DataTable();
			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDatasource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult(table);
		}

		[HttpPost]
		public IActionResult CreateAppointment([FromBody] Appointment newAppointment)
		{
			if (newAppointment == null)
				return BadRequest("Invalid appointment data.");

			string query = @"
                INSERT INTO dbo.appointment 
                (patient_id, dentist_id, appointment_time)
                VALUES (@PatientId, @DentistId, @AppointmentTime);
            ";

			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");

			try
			{
				using (SqlConnection connection = new SqlConnection(sqlDatasource))
				{
					connection.Open();

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@PatientId", newAppointment.PatientId);
						command.Parameters.AddWithValue("@DentistId", newAppointment.DentistId);
						command.Parameters.AddWithValue("@AppointmentTime", newAppointment.AppointmentTime);

						command.ExecuteNonQuery();
					}
				}

				return Ok("Appointment created successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("{appointmentId}")]
		public IActionResult GetAppointment(int appointmentId)
		{
			string query = @"
        SELECT * 
        FROM dbo.appointment
        WHERE appointment_id = @AppointmentId;
    	";

			DataTable table = new DataTable();
			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");

			try
			{
				using (SqlConnection connection = new SqlConnection(sqlDatasource))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@AppointmentId", appointmentId);

						SqlDataReader myReader = command.ExecuteReader();
						table.Load(myReader);
						myReader.Close();
					}
				}

				if (table.Rows.Count == 0)
				{
					return NotFound($"Appointment with ID {appointmentId} not found.");
				}

				return new JsonResult(table);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet]
		[Route("GetAllAppointments")]
		public IActionResult GetAllAppointments()
		{
			string query = @"
        SELECT * 
        FROM dbo.appointment
        ORDER BY appointment_time ASC;
    	";

			DataTable table = new DataTable();
			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");

			try
			{
				using (SqlConnection connection = new SqlConnection(sqlDatasource))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						SqlDataReader myReader = command.ExecuteReader();
						table.Load(myReader);
						myReader.Close();
					}
				}

				return new JsonResult(table);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpDelete("{appointmentId}")]
		public IActionResult DeleteAppointment(int appointmentId)
		{
			string query = @"
        DELETE FROM dbo.appointment
        WHERE appointment_id = @AppointmentId;
    	";

			string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");

			try
			{
				using (SqlConnection connection = new SqlConnection(sqlDatasource))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@AppointmentId", appointmentId);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							return Ok($"Appointment with ID {appointmentId} deleted successfully.");
						}
						else
						{
							return NotFound($"Appointment with ID {appointmentId} not found.");
						}
					}
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}









	}



	public class Appointment
	{
		public int PatientId { get; set; }
		public int DentistId { get; set; }
		public DateTime AppointmentTime { get; set; }
	}
}
