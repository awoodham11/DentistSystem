using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace DentistSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualPatientController : ControllerBase
    {
        private IConfiguration _configuration;

        public ActualPatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
    }
}
