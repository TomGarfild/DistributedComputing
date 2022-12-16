using Microsoft.AspNetCore.Mvc;

namespace Lab9.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudioController : ControllerBase
    {

        private readonly ILogger<StudioController> _logger;
        private readonly Studio _studio;

        public StudioController(ILogger<StudioController> logger, Studio studio)
        {
            _logger = logger;
            _studio = studio;
        }

        [HttpGet("albums")]
        public ActionResult<List<Album>> GetAlbums()
        {
            return _studio.GetAlbums();
        }
    }
}