using static NUnit.Framework.Assert;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations.Tests;

public class MeasureViewModelTests
{
    [Test]
    public void Should_Provide_Correctly_His_Parameters_With_Negative_Difference_For_Temperature()
    {
        var date = new DateTime(2023, 1, 1, 18, 0, 0);
        var measure = new Measure(date, 20, 26,"temperature");
        var measureViewModel = new MeasureViewModel(measure);

        Multiple(() =>
        {
            That(measureViewModel.MeasureValue, Is.EqualTo("20°C"));
            That(measureViewModel.Date, Is.EqualTo(date));
            That(measureViewModel.Unit, Is.EqualTo("°C"));
            That(measureViewModel.Difference, Is.Negative);
        });
    }

    [Test]
    public void Should_Provide_Correctly_His_Parameters_With_Positive_Difference_For_Temperature()
    {
        var date = new DateTime(2023, 1, 1, 18, 0, 0);
        var measure = new Measure(date, 20, 17,"temperature");
        var measureViewModel = new MeasureViewModel(measure);

        Multiple(() =>
        {
            That(measureViewModel.MeasureValue, Is.EqualTo("20°C"));
            That(measureViewModel.Date, Is.EqualTo(date));
            That(measureViewModel.Unit, Is.EqualTo("°C"));
            That(measureViewModel.Difference, Is.Positive);
        });
    }
  [Test]
  public void Should_Provide_Correctly_His_Parameters_With_Negative_Difference_For_Humidity()
  {
    var date = new DateTime(2023, 1, 1, 18, 0, 0);
    var measure = new Measure(date, 0.2, 0.4,"humidity");
    var measureViewModel = new MeasureViewModel(measure);

    Multiple(() =>
    {
      That(measureViewModel.MeasureValue, Is.EqualTo("20%"));
      That(measureViewModel.Date, Is.EqualTo(date));
      That(measureViewModel.Unit, Is.EqualTo("%"));
      That(measureViewModel.Difference, Is.Negative);
    });
  }

  [Test]
  public void Should_Provide_Correctly_His_Parameters_With_Positive_Difference_For_Humidity()
  {
      var date = new DateTime(2023, 1, 1, 18, 0, 0);
      var measure = new Measure(date, 0.2, 0,"humidity");
      var measureViewModel = new MeasureViewModel(measure);

      Multiple(() =>
      {
          That(measureViewModel.MeasureValue, Is.EqualTo("20%"));
          That(measureViewModel.Date, Is.EqualTo(date));
          That(measureViewModel.Unit, Is.EqualTo("%"));
          That(measureViewModel.Difference, Is.Positive);
      });
  }

  [Test]
  public void Should_Provide_Correctly_His_Parameters_When_There_Is_No_Expected_Value()
  {
      var date = new DateTime(2023, 1, 1, 18, 0, 0);
      var measure = new Measure(date, 0.2, null,"humidity");
      var measureViewModel = new MeasureViewModel(measure);

      Multiple(() =>
      {
          That(measureViewModel.MeasureValue, Is.EqualTo("20%"));
          That(measureViewModel.Date, Is.EqualTo(date));
          That(measureViewModel.Unit, Is.EqualTo("%"));
          That(measureViewModel.Difference, Is.Null);
      });
  }

  [Test]
  public void Should_Provide_Correctly_His_Parameters_When_There_Is_No_Correct_Type()
  {
      var date = new DateTime(2023, 1, 1, 18, 0, 0);
      var measure = new Measure(date, 0.2, 0,"pressure");
      var measureViewModel = new MeasureViewModel(measure);

      Multiple(() =>
      {
          That(measureViewModel.MeasureValue, Is.Null);
          That(measureViewModel.Date, Is.EqualTo(date));
          That(measureViewModel.Unit, Is.EqualTo(string.Empty));
          That(measureViewModel.Difference, Is.Null);
      });
  }

  [Test]
    public void Should_Provide_Correctly_His_Parameters_When_There_Is_No_Expected_value()
    {
        var date = new DateTime(2023, 1, 1, 18, 0, 0);
        var measure = new Measure(date, 0.2, null,"humidity");
        var measureViewModel = new MeasureViewModel(measure);

        Multiple(() =>
        {
            That(measureViewModel.MeasureValue, Is.EqualTo("20%"));
            That(measureViewModel.Date, Is.EqualTo(date));
            That(measureViewModel.Unit, Is.EqualTo("%"));
            That(measureViewModel.Difference, Is.Null);
        });
    }
}
