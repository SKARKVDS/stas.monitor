using NSubstitute;
using Stas.Monitor.Domains.Factory;
using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Domains.Tests;

public class ConfigurationTest
{
  private IConfiguration? _configuration;
  private IConfigurationReader _configurationReader = null!;
  private string?[] _thermometers = null!;

  [SetUp]
  public void Setup()
  {
    _thermometers = new[] {"t1", "t2"};
    _configurationReader = Substitute.For<IConfigurationReader>();
    _configurationReader.Thermometers.Returns(_thermometers!);
    _configurationReader.DbConfig.Returns(new DbConfig("Admin", "Admin", "baseString;"));
    var configurationFactory = new ConfigurationFactory(_configurationReader);
    _configuration = configurationFactory.Create();
  }

  [Test]
  public void Should_Create_Configuration_Correctly() => Assert.That(_configuration?.AllThermometers, Is.EqualTo(_thermometers));

  [Test]
  public void Should_Return_Thermometer_When_It_Exists()
  {
    var thermometer = _configuration?.AllThermometers.FirstOrDefault(t => t == "t1");
    Assert.That(thermometer, Is.EqualTo("t1"));
  }

  [Test]
  public void Should_Return_Null_When_Thermometer_Does_Not_Exist()
  {
    var thermometer = _configuration?.AllThermometers.FirstOrDefault(t => t == "t3");
    Assert.That(thermometer, Is.Null);
  }

  [Test]
  public void Should_Return_ConnectionString_Correctly() => Assert.That(_configuration?.ConnectionString, Is.EqualTo("baseString;User id=Admin;Password=Admin;"));
}
