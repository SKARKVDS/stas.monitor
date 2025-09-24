using static NUnit.Framework.Assert;
using Stas.Monitor.Domains.Factory;
using Stas.Monitor.Infrastructures.Except;
using Stas.Monitor.Infrastructures.Reader;

namespace Stas.Monitor.Infrastructures.Tests;

public class IniConfigurationReaderTest
{
    [Test]
    public void Should_Load_Ini_File_Correctly()
    {
        var args = new Dictionary<string, string>{ {"config-file","ValidConfig.ini"} };
        var iniConfigurationReader = new IniConfigurationReader(args);
        var factory = new ConfigurationFactory(iniConfigurationReader);
        var configuration = factory.Create();
        That(configuration.AllThermometers.Any(), Is.True);
    }

    [Test]
    public void Should_Throw_Exception_When_Ini_File_Is_Empty()
    {
        // Arrange
        var args = new Dictionary<string, string>{ {"config-file","NoDatabaseConfig.ini"} };

        // Act
        var exception = Throws<EmptyDatabaseSectionException>(() =>
        {
          _ = new ConfigurationFactory(new IniConfigurationReader(args));
        });

        // Assert
        That(exception?.Message, Is.EqualTo("missing required section database"));
    }

    [Test]
    public void Should_Throw_Exception_When_Ini_File_Not_Found()
    {
        // Arrange
        var args = new Dictionary<string, string>{ {"config-file","NotExisting.ini"} };

        // Act
        var exception = Throws<NoConfigurationFileFoundException>(() =>
        {
          _ = new ConfigurationFactory(new IniConfigurationReader(args));
        });

        // Assert
        That(exception?.Message, Is.EqualTo("configuration file not found"));
    }

    [Test]
    public void Should_Throw_Exception_When_File_Had_No_Thermometer_Names()
    {
      var args = new Dictionary<string, string>{ {"config-file","NoThermometerConfig.ini"} };
      var exception = Throws<EmptyThermometersSectionException>(() =>
      {
          _ = new ConfigurationFactory(new IniConfigurationReader(args));
      });
      That(exception?.Message, Is.EqualTo("no thermometer found in configuration file"));
    }

    [Test]
    public void Should_Throw_Exception_When_The_Load_Of_Thermometers_Returns_No_Values()
    {
        var exception = Throws<EmptyThermometersSectionException>(() =>
        {
            _ = new ThermometersIniReader("NoThermometerConfig.ini");
        });
        That(exception?.Message, Is.EqualTo("missing required section thermometers"));
    }

    [Test]
    public void Should_Throw_Exception_When_The_Load_Of_Database_Returns_No_Values()
    {
        var exception = Throws<EmptyDatabaseSectionException>(() =>
        {
            _ = new DbConfigIniReader("NoDatabaseConfig.ini");
        });
        That(exception?.Message, Is.EqualTo("missing required section database"));
    }
}
