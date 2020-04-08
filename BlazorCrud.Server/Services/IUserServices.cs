using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCrud.Shared.Models;



namespace BlazorCrud.Server.Services
{
    public interface IUserServices
    {

        Task<List<User>> GetUsers();




    }
}
