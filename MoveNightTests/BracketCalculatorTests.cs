using System;
using System.Collections.Generic;
using MovieNight.Models;
using MovieNight.Services;
using NUnit.Framework;

namespace MoveNightTests;

public class BracketCalculatorTests
{
    private BracketCalculator calc;
    
    
    [SetUp]
    public void setUp()
    {
        calc = new BracketCalculator();
    }
    
    
    [Test]
    public void TwentyThreeMoviesShouldReturnNineInRo32()
    {
        List<Suggestion> movies = new List<Suggestion>();

        for (int i = 0; i < 23; i++)
        {
            Suggestion sugg = new Suggestion()
            {
                Movie = new Movie()
                {
                    Title = "Movie" + i
                }
            };
            movies.Add(sugg);
        }

        MovieBracket bracket = calc.GenerateBracket(movies);
        
        Assert.AreEqual(7, bracket.MoviePairs.Count);
        Assert.AreEqual(32, bracket.BracketNumber);
    }
    
    [Test]
    public void SimulateTwoRounds()
    {
        List<Suggestion> movies = new List<Suggestion>();

        for (int i = 0; i < 23; i++)
        {
            Suggestion sugg = new Suggestion()
            {
                Movie = new Movie()
                {
                    Title = "Movie" + i
                }
            };
            movies.Add(sugg);
        }

        MovieBracket bracket = calc.GenerateBracket(movies);

        foreach (var pair in bracket.MoviePairs)
        {
            pair.winnerFound = true;
            pair.winnerTop = new Random().NextDouble() >= 0.5;
        }

        MovieBracket nextBracket = calc.FillNextBracket(bracket, movies);
        
        Assert.AreEqual(8, nextBracket.MoviePairs.Count);
        Assert.AreEqual(16, nextBracket.BracketNumber);
    }
    
    [Test]
    public void SimulateFullGame()
    {
        List<Suggestion> movies = new List<Suggestion>();

        for (int i = 0; i < 16; i++)
        {
            movies.Add(new Suggestion());
        }

        MovieBracket bracket = calc.GenerateBracket(movies);


        while (bracket.MoviePairs.Count != 1)
        {
            foreach (var pair in bracket.MoviePairs)
            {
                pair.winnerFound = true;
                pair.winnerTop = new Random().NextDouble() >= 0.5;
            }

            bracket = calc.FillNextBracket(bracket, movies);
        }
        
        Assert.AreEqual(1, bracket.MoviePairs.Count);
        Assert.AreEqual(2, bracket.BracketNumber);
    }

    [Test]
    public void TestLogarithm()
    {
        Assert.AreEqual(true, calc.IsPowerOfTwo(32));
        Assert.AreEqual(true, calc.IsPowerOfTwo(16));
        Assert.AreEqual(false, calc.IsPowerOfTwo(0));
        Assert.AreEqual(false, calc.IsPowerOfTwo(15));
        Assert.AreEqual(false, calc.IsPowerOfTwo(-5));
        
        Assert.AreEqual(32, calc.FindHigherRound(17));
        Assert.AreEqual(32, calc.FindHigherRound(32));
        Assert.AreEqual(32, calc.FindHigherRound(31));
        Assert.AreEqual(16, calc.FindHigherRound(16));
        Assert.AreEqual(8, calc.FindHigherRound(5));
    }
}