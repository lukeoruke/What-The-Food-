using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult<News>> Get()
        {
            var article = new News();
            article.title= "Valorant Gone Mobile";
            return Ok(article);
        }
    }
}
