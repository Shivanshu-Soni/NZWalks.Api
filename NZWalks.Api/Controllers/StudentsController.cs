using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudets(){
            var students =new string[]{"jhon","marrk","maine","betty","cindy",};
            return Ok(students);
        }
    }
}