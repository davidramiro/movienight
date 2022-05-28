using MovieNight.Models;
using Newtonsoft.Json.Linq;

namespace MovieNight.Services;

public class OmdbService
{
    private const string OmdbApiBaseUrl = "http://www.omdbapi.com/?i=";
    private readonly string _apiKey;

    public OmdbService(IConfiguration configuration)
    {
        _apiKey = configuration["OMDB:ApiKey"];
    }
    
    public OmdbService(string apikey)
    {
        _apiKey = apikey;
    }

    public async Task<Movie> GetMovieInfo(string imdbId) {

        string queryId = ConvertIdInput(imdbId);
        string json = "";

        using (var httpClient = new HttpClient())
        {
            json = await httpClient.GetStringAsync(OmdbApiBaseUrl + queryId + "&apikey=" +
                                                       _apiKey);
        }

        JObject movieJson = JObject.Parse(json);

        if (!movieJson.SelectToken("Response").Value<bool>())
        {
            throw new KeyNotFoundException("Movie not found.");
        }

        Movie fetchedMovie = new Movie();

        fetchedMovie.Title = movieJson.SelectToken("Title").Value<string>();
        fetchedMovie.ReleaseYear = movieJson.SelectToken("Year").Value<int>();
        fetchedMovie.Genre = movieJson.SelectToken("Genre").Value<string>();
        fetchedMovie.ImdbRating = movieJson.SelectToken("imdbRating").Value<float>();
        fetchedMovie.MetaScore = movieJson.SelectToken("Metascore").Value<int>();
        fetchedMovie.PosterUrl = movieJson.SelectToken("Poster").Value<string>();
        fetchedMovie.Plot = movieJson.SelectToken("Plot").Value<string>();
        fetchedMovie.ImdbId = movieJson.SelectToken("imdbID").Value<string>();
        fetchedMovie.Runtime = movieJson.SelectToken("Runtime").Value<string>();

        return fetchedMovie;
    }
    
    public String ConvertIdInput(string input) {

        bool isUrl = (Uri.IsWellFormedUriString(input, UriKind.Absolute));
        string output = "";

        if (isUrl)
        {
            Uri uri = new Uri(input);
            output = uri.Segments.LastOrDefault().Split(new[]{';'}).First();
        }
        else
        {
            if (!input.StartsWith("tt")) 
            {
                output = "tt" + input;
            } 
            else 
            {
                output = input;
            }
        }
        
        return output.Replace("/", "");
    }
}