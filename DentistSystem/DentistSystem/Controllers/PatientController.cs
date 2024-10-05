using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

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

	}
}
