using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DentistSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private IConfiguration _configuration;

		public PatientController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("GetPatients")]
		public JsonResult GetPatients()
		{
			string query = "select * from dbo.patient";
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
        [Route("RegisterPatient")]
        public JsonResult RegisterPatient([FromForm] Patient newPatient)
		{
            string query = @"INSERT INTO dbo.patient " +
				"(first_name, last_name, dob, patient_address, contact_number, email, password) " +
				"VALUES(@FirstName, @LastName, @Dob, @PatientAddress, @ContactNumber, @Email, @Password)";

			DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
					myCommand.Parameters.AddWithValue("@FirstName", newPatient.FirstName);
					myCommand.Parameters.AddWithValue("@LastName", newPatient.LastName);
					myCommand.Parameters.AddWithValue("@Dob", newPatient.Dob);
					myCommand.Parameters.AddWithValue("@PatientAddress", newPatient.PatientAddress);
					myCommand.Parameters.AddWithValue("@ContactNumber", newPatient.ContactNumber);
					myCommand.Parameters.AddWithValue("@Email", newPatient.Email);
					myCommand.Parameters.AddWithValue("@Password", newPatient.Password);
					myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

    }

	public class Patient
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Date Dob { get; set; }
		public string PatientAddress { get; set; }
		public string ContactNumber { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}



}
