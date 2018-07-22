using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        readonly ILogger<TodoController> _log;

        public TodoController(ILogger<TodoController> log)
        {
            _log = log;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            _log.LogInformation("TODO started");
            return Ok("OK");
        }

    }
}