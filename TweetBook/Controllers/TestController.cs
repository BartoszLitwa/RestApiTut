using Microsoft.AspNetCore.Mvc;

namespace TweetBook.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("api/user")]
        public IActionResult Index()
        {
            return Ok(new { name = "test"});
        }
    }
}
