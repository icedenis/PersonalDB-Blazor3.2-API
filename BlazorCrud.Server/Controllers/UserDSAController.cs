using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gupta.SQLBase.Data;
using BlazorCrud.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using BlazorCrud.Shared.Data;
using BlazorCrud.Server.Services;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserDSAController : Controller
    {

        readonly IUserServices _iUser;

        public UserDSAController(IUserServices userServices)
        {
            _iUser = userServices;
        }

       [HttpGet("[action]")]
        public async Task<PagedResult<User>> GetUsersAsync([FromQuery]int page)
        {
            int pageSize = 10;

            var ul = await _iUser.GetUsers();

            return ul.AsQueryable()
                .OrderBy(p => p.Id)
                .GetPaged(page, pageSize);
        }

    }

}
