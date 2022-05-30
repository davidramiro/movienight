using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MovieNight.Models;
using NUnit.Framework;
using MovieNight.Services;

namespace MoveNightTests;

public class ImdbParserTests
{
    private readonly OmdbService _omdb;

    public ImdbParserTests()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        _omdb = new OmdbService(config["OMDB:ApiKey"]);
    }

    [Test]
    public async Task MovieMatches() {
        Movie test = await _omdb.GetMovieInfo("tt4877122");
        Assert.AreEqual("The Emoji Movie", test.Title);
    }

    [Test]
    public void NotFoundReturnsNull() {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _omdb.GetMovieInfo("tt9999999"));
    }
}