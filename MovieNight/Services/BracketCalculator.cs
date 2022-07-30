using MovieNight.Models;
using MovieNight.Models.Exceptions;

namespace MovieNight.Services;

public class BracketCalculator
{
    

    public MovieBracket GenerateBracket(List<Suggestion> movies)
    {
        int higherBracket = FindHigherRound(movies.Count);

        MovieBracket bracket = new MovieBracket
        {
            BracketNumber = higherBracket,
            Date = DateTime.Today
        };

        int moviesInBracket = movies.Count - (higherBracket - movies.Count);

        List<MoviePair> moviePairList = new List<MoviePair>();
        
        for (int i = 0; i < moviesInBracket / 2; i++)
        {
            MoviePair moviePair = new MoviePair();
            int upperRandomIndex = new Random().Next(0, movies.Count);
            moviePair.TopSuggestion = movies[upperRandomIndex];
            int bottomRandomIndex = new Random().Next(0, movies.Count);
            moviePair.BottomSuggestion = movies[bottomRandomIndex];

            moviePairList.Add(moviePair);
        }

        bracket.MoviePairs = moviePairList;

        return bracket;
    }

    public MovieBracket FillNextBracket(MovieBracket bracket, List<Suggestion> movies)
    {
        if (bracket.MoviePairs.Count == 1)
        {
            return bracket;
        }
        
        foreach (var pair in bracket.MoviePairs)
        {
            if (!pair.winnerFound)
            {
                throw new VotingNotFinishedException();
            }
            movies.Remove(pair.winnerTop ? pair.BottomSuggestion : pair.TopSuggestion);
        }
        
        List<MoviePair> moviePairList = new List<MoviePair>();
        
        for (int i = 0; i < movies.Count / 2; i++)
        {
            MoviePair moviePair = new MoviePair();
            int upperRandomIndex = new Random().Next(0, movies.Count);
            moviePair.TopSuggestion = movies[upperRandomIndex];
            int bottomRandomIndex = new Random().Next(0, movies.Count);
            moviePair.BottomSuggestion = movies[bottomRandomIndex];
            moviePairList.Add(moviePair);
        }
        
        MovieBracket newBracket = new MovieBracket
        {
            BracketNumber = movies.Count,
            Date = DateTime.Today,
            MoviePairs = moviePairList
        };

        return newBracket;
    }

    public int FindHigherRound(int number)
    {
        while (Math.Log2(number) % 1 != 0)
        {
            number++;
        }

        return number;
    }

    public bool IsPowerOfTwo(int number)
    {
        return number == FindHigherRound(number);
    }
}