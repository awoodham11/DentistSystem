using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DentistSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistAppController : ControllerBase
    {

        private IConfiguration _configuration;

        public DentistAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet]
        [Route("GetDentists")]
        public JsonResult GetDentists()
        {
            string query = "select * from dbo.dentist";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource)) { 
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) { 
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                } 
            }

            return new JsonResult(table);
        }


        [HttpPost]
        [Route("AddDentists")]
        public JsonResult AddDentists([FromForm] string newDentists)
        {
            string query = "insert into dbo.dentist values(@newDentists)";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@newDentists", newDentists);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpDelete]
        [Route("DeleteDentists")]
        public JsonResult DeleteDentists(int id)
        {
            string query = "delete from dbo.dentist where id=@id";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("dentistappDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }




    }
}
