using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ServerApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [Route("GetUploadToken")]
        [HttpGet]
        public async Task<IActionResult> GetUploadToken()
        {
            return Ok();
        }

        [Route("UploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            return Ok();
        }
    }
}
