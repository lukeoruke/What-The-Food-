using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        /// <summary>
        /// Get News for News Microservice
        /// </summary>
        //TODO: Finish writing both the get and put
        [HttpGet]
        public async Task<ActionResult<News>> Get()
        {
            var article = new News();
            article.title= "All Foods are now 100% fat free";
            return Ok(article);
        }
    }
}
