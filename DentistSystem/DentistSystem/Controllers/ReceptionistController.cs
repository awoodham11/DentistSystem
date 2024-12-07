using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DentistSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {

        private IConfiguration _configuration;

        public ReceptionistController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetReceptionists")]
        public JsonResult GetReceptionists()
        {
            string query = "select * from dbo.receptionist";
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


        [HttpGet]
        [Route("GetPatientAppointment")]
        public JsonResult GetPatientAppointment()
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
        


	}
}
