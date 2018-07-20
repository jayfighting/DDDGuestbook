using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
    }
}