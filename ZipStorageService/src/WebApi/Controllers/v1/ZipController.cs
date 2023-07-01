using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ZipController : ControllerBase
    {
        public ZipController()
        {

        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Test(string fileName)
        {
            return Ok(fileName);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFileCollection files)
        {
            return Ok(files.Count);
        }
    }
}
