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
        
        if (!movieJson.SelectToken("Title").Value<string>().Equals("N/A"))
        {
            fetchedMovie.Title = movieJson.SelectToken("Title").Value<string>();
        }
        if (!movieJson.SelectToken("Year").Value<string>().Equals("N/A"))
        {
            fetchedMovie.ReleaseYear = movieJson.SelectToken("Year").Value<int>();
        }
        if (!movieJson.SelectToken("Genre").Value<string>().Equals("N/A"))
        {
            fetchedMovie.Genre = movieJson.SelectToken("Genre").Value<string>();
        }
        if (!movieJson.SelectToken("imdbRating").Value<string>().Equals("N/A"))
        {
            fetchedMovie.ImdbRating = movieJson.SelectToken("imdbRating").Value<float>();
        }
        if (!movieJson.SelectToken("Metascore").Value<string>().Equals("N/A"))
        {
            fetchedMovie.MetaScore = movieJson.SelectToken("Metascore").Value<int>();
        }
        if (!movieJson.SelectToken("Poster").Value<string>().Equals("N/A"))
        {
            fetchedMovie.PosterUrl = movieJson.SelectToken("Poster").Value<string>();
        }
        if (!movieJson.SelectToken("Plot").Value<string>().Equals("N/A"))
        {
            fetchedMovie.Plot = movieJson.SelectToken("Plot").Value<string>();
        }
        if (!movieJson.SelectToken("imdbID").Value<string>().Equals("N/A"))
        {
            fetchedMovie.ImdbId = movieJson.SelectToken("imdbID").Value<string>();
        }
        if (!movieJson.SelectToken("Runtime").Value<string>().Equals("N/A"))
        {
            fetchedMovie.Runtime = movieJson.SelectToken("Runtime").Value<string>();
        }
        
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