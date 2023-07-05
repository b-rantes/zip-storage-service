using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Ping()
        {
            return Ok("pong");
        }
    }
}
