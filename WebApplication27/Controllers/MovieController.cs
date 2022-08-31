using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApplication27.Clients;
using WebApplication27.Models;

namespace WebApplication27.Controllers
{
    /// <summary>
    /// Controller to get movies
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly MovieClient _movieClient;

        public MovieController(ILogger<MovieController> logger, MovieClient movieClient)
        {
            _logger = logger;
            _movieClient = movieClient;
        }

        /// <summary>
        /// Get movies filtered by title
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetMovies")]
        [ProducesResponseType(typeof(MovieSearchResult), 200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<Movie>?> Get([Required]string searchTerm)
        {
            var result = await _movieClient.GetMovies(searchTerm);

            return result;
        }
    }
}