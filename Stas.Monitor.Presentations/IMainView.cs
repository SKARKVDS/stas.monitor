namespace Stas.Monitor.Presentations;

public interface IMainView
{
  event EventHandler<string> OnSelectedThermometer;

  event EventHandler<int> OnSelectedDuring;

  event EventHandler<string> OnSelectedView;

  string[] ThermometerNames { set; }

  string[] AvailableViews { get; set; }

  IEnumerable<MeasureViewModel> MeasureViewModels { set; }

  void ChangeVisibilityOfView(string name);
}
