using System.Collections;
using Stas.Monitor.Domains;
using Stas.Monitor.Infrastructures.Interfaces;
using Stas.Monitor.Infrastructures.SqlStorages;

namespace Stas.Monitor.Infrastructures.Tests;

using System.Data.SQLite;

public class SqlStorageTest
{
    private ISqlStorage? _storage;
    private const string? ConnectionString = "Data Source=database.sqlite;Version=3;";
    private readonly IList<Measure> _measures = new List<Measure>
    {
        new(new DateTime(2023, 1, 1,18,20,0), 0.5, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,0), 20.0, null, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,2), 0.6, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,2), 22.0, null, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,4), 29.0, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,4), 29.0, null, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,6), 29.0, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,6), 29.0, null, "temperature")
    };
    private readonly IList<Measure> _measuresWithAlerts = new List<Measure>
    {
        new(new DateTime(2023, 1, 1,18,20,8), 0.7, 0.5, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,8), 29.0, 20.0, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,10), 0.8, 0.4, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,10), 28.0, 20.0, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,12), 0, 0.5, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,12), 10.0, 20.0, "temperature")
    };
    private readonly IList<Measure> _newMeasures = new List<Measure>
    {
        new(new DateTime(2023, 1, 1,18,20,14), 0.9, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,14), 27.0, null, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,16), 0.9, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,16), 27.0, null, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,18), 0.9, null, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,18), 27.0, null, "temperature")
    };
    private readonly IList<Measure> _newMeasuresWithAlerts = new List<Measure>
    {
        new(new DateTime(2023, 1, 1,18,20,20), 1.0, 0.5, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,20), 27.0, 20.0, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,22), 1.0, 0.4, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,22), 27.0, 20.0, "temperature"),
        new(new DateTime(2023, 1, 1,18,20,24), 1.0, 0.5, "humidity"),
        new(new DateTime(2023, 1, 1,18,20,24), 27.0, 20.0, "temperature")
    };
    private const string InsertMeasure = "INSERT INTO Measures (thermometerName, type, observed, date) VALUES (@thermometerName, @type, @observed, @date)";
    private const string InsertMeasureWithAlert = "BEGIN;" +
                                                  "INSERT INTO Measures (thermometerName, type, observed, date) VALUES (@thermometerName, @type, @observed, @date);" +
                                                  "INSERT INTO Alerts (expected, measureId) VALUES (@expected, LAST_INSERT_ROWID());" +
                                                  "COMMIT;";

    private const string Query = @"
            DROP TABLE IF EXISTS `Measures`;
            DROP TABLE IF EXISTS `Alerts`;

            CREATE TABLE Measures
            (
                id              INTEGER
                    primary key AUTOINCREMENT,
                thermometerName varchar(50)  null,
                type            varchar(50)  null,
                observed        double(4, 2) null,
                date            datetime     null
            );

            CREATE TABLE Alerts
            (
                id        INTEGER
                    primary key AUTOINCREMENT,
                expected  double(4, 2) null,
                measureId int          not null,
                constraint measureId
                    foreign key (measureId) references Measures (id)
            );";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var factory = new SqlStorageFactory("System.Data.SQLite", ConnectionString, SQLiteFactory.Instance);
        _storage = (SqlStorage)factory.NewStorage();
    }

    [SetUp]
    public void Setup()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        using var command = new SQLiteCommand(Query, connection);
        command.ExecuteNonQuery();
    }

    private static void FillDataBase(IEnumerable<Measure> measures, string thermometerName = "Chambre")
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        foreach (var measure in measures)
        {
            using var command = new SQLiteCommand(connection);
            if (measure.Expected is null)
            {
                command.CommandText = InsertMeasure;
                command.Parameters.AddWithValue("@thermometerName", thermometerName);
                command.Parameters.AddWithValue("@type", measure.Type);
                command.Parameters.AddWithValue("@observed", measure.Observed);
                command.Parameters.AddWithValue("@date", measure.Date);
            }
            else
            {
                command.CommandText = InsertMeasureWithAlert;
                command.Parameters.AddWithValue("@thermometerName", thermometerName);
                command.Parameters.AddWithValue("@type", measure.Type);
                command.Parameters.AddWithValue("@observed", measure.Observed);
                command.Parameters.AddWithValue("@date", measure.Date);
                command.Parameters.AddWithValue("@expected", measure.Expected);
            }
            command.ExecuteNonQuery();
        }
    }

    [Test]
    public void Should_Not_Get_Measures_When_Thermometer_Dont_Exist()
    {
        FillDataBase(_measures);
        var measures = _storage?.GetMeasures("test", DateTime.MinValue, "temperature");
        Assert.That(measures, Is.Empty);
    }

    [Test]
    public void Should_Get_Measures_When_Thermometer_Exist()
    {
        FillDataBase(_measures);
        var measures = _storage?.GetMeasures("Chambre", DateTime.MinValue, "humidity");
        Assert.That(measures, Is.Not.Empty);
    }

    [Test]
    public void Should_Get_Measures_When_New_Measures_Are_Created()
    {
        FillDataBase(_measures);
        var measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,0), "humidity");
        Assert.That(measures, Is.Not.Empty);
        FillDataBase(_newMeasures);
        measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,10), "humidity");
        Assert.That(measures, Is.Not.Empty);
    }

    [Test]
    public void Should_Get_Measures_When_New_Measures_Are_Created_With_Alert()
    {
        FillDataBase(_measures);
        var measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,0), "temperature");
        Assert.That(measures, Is.Not.Empty);
        FillDataBase(_measuresWithAlerts);
        measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,10), "temperature");
        Assert.That(measures?.First().Expected, Is.Not.Null);
    }

    [Test]
    public void Should_Get_Measures_When_New_Measures_Are_Created_Without_Alert()
    {
        FillDataBase(_measures);
        var measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,0), "humidity");
        Assert.That(measures, Is.Not.Empty);
        FillDataBase(_newMeasures);
        measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,10), "humidity");
        Assert.That(measures?.Last().Expected, Is.Null);
    }

    [Test]
    public void Should_Get_Measures_When_New_Measures_Are_Created_With_New_Alert()
    {
        FillDataBase(_measures);
        var measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,0), "temperature");
        Assert.That(measures, Is.Not.Empty);
        FillDataBase(_measuresWithAlerts);
        measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,10), "temperature");
        Assert.That(measures?.First().Expected, Is.Not.Null);
        FillDataBase(_newMeasuresWithAlerts);
        measures = _storage?.GetMeasures("Chambre", new DateTime(2023, 1, 1,18,20,20), "temperature");
        Assert.That(measures?.First().Expected, Is.Not.Null);
    }

    [Test]
    public void Should_Get_Last_Measure_Date_When_Thermometer_Exist()
    {
        FillDataBase(_measures);
        var lastMeasureDate = _storage?.GetLastMeasureDate("Chambre", "humidity");
        Assert.That(lastMeasureDate, Is.EqualTo(new DateTime(2023, 1, 1,18,20,6)));
    }

    [Test]
    public void Should_Get_Last_Measure_Date_When_Thermometer_Exist_With_Alert()
    {
        FillDataBase(_measuresWithAlerts);
        var lastMeasureDate = _storage?.GetLastMeasureDate("Chambre", "temperature");
        Assert.That(lastMeasureDate, Is.EqualTo(new DateTime(2023, 1, 1,18,20,12)));
    }

    [Test]
    public void Should_Get_Type_Of_Views_For_Temperature_Correctly()
    {
        FillDataBase(_measures);
        var typeOfViews = _storage?.GetTypeOfViews("Chambre");
        Assert.Multiple(() =>
        {
            Assert.That(typeOfViews!, Does.Contain("temperature"));
            Assert.That(typeOfViews!, Does.Contain("humidity"));
        });
    }

    [Test]
    public void Should_Get_Type_Of_Views_For_Humidity_Correctly()
    {
        FillDataBase(_measures, "Salon");
        var typeOfViews = _storage?.GetTypeOfViews("Salon");
        Assert.Multiple(() =>
        {
            Assert.That(typeOfViews!, Does.Contain("temperature"));
            Assert.That(typeOfViews!, Does.Contain("humidity"));
        });
    }
}
