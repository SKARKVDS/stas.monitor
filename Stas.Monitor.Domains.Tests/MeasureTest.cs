using static NUnit.Framework.Assert;

namespace Stas.Monitor.Domains.Tests;

public class MeasureTest
{
  private Measure? _measure;
  [SetUp]
  public void Setup() => _measure = new Measure(new DateTime(2023,1,1,12,20,02),20,25,"humidity");

  [Test]
  public void Should_be_correctly_initialized() =>
      Multiple(() =>
      {
          That(_measure?.Date, Is.EqualTo(new DateTime(2023,1,1,12,20,02)));
          That(_measure?.Observed, Is.EqualTo(20));
          That(_measure?.Expected, Is.EqualTo(25));
          That(_measure?.Type, Is.EqualTo("humidity"));
      });
}
