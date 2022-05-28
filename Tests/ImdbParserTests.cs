using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MovieNight.Models;
using NUnit.Framework;
using MovieNight.Services;

namespace Tests;

public class ImdbParserTests
{
    private readonly OmdbService _omdb;

    public ImdbParserTests()
    {
        _omdb = new OmdbService("YOUR-API-KEY");
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task MovieMatches() {
        Movie test = await _omdb.GetMovieInfo("tt4877122");
        Assert.AreEqual("The Emoji Movie", test.Title);
    }

    [Test]
    public void NotFoundReturnsNull() {
        Assert.That(async () => await _omdb.GetMovieInfo("tt9999999"), Throws.Exception);
    }
}