using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Gupta.SQLBase.Data;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public PersonalController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET: api/Personal
        [HttpGet(Name = "Count")]
        public IActionResult Count()
        {
            int count = -1;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (var conn = new SQLBaseConnection(connectionString))
            {
                // Open the connection, each of these is a session.
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    // Create a new command object for executing a statement, each of these is a cursor.
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM employee";
                        // Get the result. Since the result set is a single value, ExecuteScalar is ideal.
                        count = (int)cmd.ExecuteScalar();
                    }
                }
            } // The connection is automatically closed when execution leaves this block. Alternative is to call conn.Close().

            if (count < 0)
            {
                return Problem();
            }

            return Ok ( new { Count = count } );
        }


        // GET: api/Personal
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET: api/Personal/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Personal
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Personal/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
