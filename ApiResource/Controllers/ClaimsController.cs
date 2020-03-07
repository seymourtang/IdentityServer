using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiResource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok(User.Claims.Select(o => new
            {
                o.Type,
                o.Value
            }).ToList());
        }
    }
}