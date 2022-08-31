using System.Linq;
using WebApplication27.Models;

namespace WebApplication27.Clients
{
    /// <summary>
    /// client to call the movie API
    /// </summary>
    public class MovieClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<MovieClient> _logger;
        private readonly IConfiguration _configuration;

        public MovieClient(HttpClient client, ILogger<MovieClient> logger, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Get movies based on a search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>?> GetMovies(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return default;
            }

            int pageIndex = 1;
            var searchResult = await _client.GetFromJsonAsync<MovieSearchResult>(GetUrl(searchTerm, pageIndex));
            List<Movie> movies = new();

            if (searchResult != null)
            {
                movies.AddRange(searchResult.Movies);
                int totalPages = searchResult.TotalPages;
                
                while (pageIndex < totalPages)
                {
                    pageIndex++;
                    searchResult = await _client.GetFromJsonAsync<MovieSearchResult>(GetUrl(searchTerm, pageIndex));
                    if (searchResult != null)
                    {
                        movies.AddRange(searchResult.Movies);
                    }
                }
            }

            return movies;
        }

        private string GetUrl(string searchTerm, int pageIndex)
        {
            return $"{_configuration.GetValue<string>("SearchUrl")}?Title={searchTerm}&page={pageIndex}";
        }
    }
}
