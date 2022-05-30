using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using MovieNight.Services;

namespace MoveNightTests;

public class UrlParserTests
{
    private readonly OmdbService _omdb;

    public UrlParserTests()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        _omdb = new OmdbService(config["OMDB:ApiKey"]);
    }
    
    [Test]
    public void UrlConvertsCorrectly() {
        Assert.AreEqual("tt4877122",
            _omdb.ConvertIdInput("https://www.imdb.com/title/tt4877122"));
    }

    [Test]
    public void UrlWithTrailingSlashConvertsCorrectly() {
        Assert.AreEqual("tt4877122",
            _omdb.ConvertIdInput("https://www.imdb.com/title/tt4877122/"));
    }

    [Test]
    public void UrlWithParamConvertsCorrectly() {
        Assert.AreEqual("tt4877122",
            _omdb.ConvertIdInput("https://www.imdb.com/title/tt4877122/?ref_=nv_sr_srsg_3"));
    }

    [Test]
    public void IdWithSlashConvertsCorrectly() {
        Assert.AreEqual("tt4877122",
            _omdb.ConvertIdInput("tt4877122/"));
    }

    [Test]
    public void UrlWithoutTTConvertsCorrectly() {
        Assert.AreEqual("tt4877122",
            _omdb.ConvertIdInput("4877122"));
    }
}