using BlazorCrud.Server.Helper;
using BlazorCrud.Shared.Models;
using Gupta.SQLBase.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCrud.Server.Services
{
    public class UserServices : IUserServices
    {


        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        

        public UserServices(IConfiguration configuration , IOptions<AppSettings> appSettings)
        {

            _configuration = configuration;
            _appSettings = appSettings.Value;

        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return connection;
        }


        
        
        public  async Task<List<User>> GetUsers()
        {
            var connectiongString = this.GetConnection();

            List<User> usersList = new List<User>();

            using (var conn =  new SQLBaseConnection(connectiongString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch ( SQLBaseException ex)
                {
                    Console.WriteLine($"Could not connect to SQLBase: {ex.Message}");
                    
                }
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    string SQL = "SELECT C_NAME FROM mitarbeiter";
                    SQLBaseCommand command = new SQLBaseCommand(SQL, conn);

                    using (SQLBaseDataReader dataReader = command.ExecuteReader())
                    {
                        
                        
                        User user = new User();
                        user.Username = Convert.ToString(dataReader["C_NAME"]);

                        usersList.Add(user);
                    }


                    
                }

                conn.Close();

            }

            return usersList.ToList();
        }
    }
}
