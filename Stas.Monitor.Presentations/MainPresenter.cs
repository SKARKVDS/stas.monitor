using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  // fausse alerte
  private readonly IMainView? _mainView;
  private readonly IThermometerRepository _repository;
  private string? _currentThermometer;
  private int _during;

  public MainPresenter(IMainView? mainView, IThermometerRepository repository)
  {
    _mainView = mainView;
    _repository = repository;
    _currentThermometer = null;
    SubscribeToEvents();
  }

  public void Start()
  {
    _mainView!.ThermometerNames = _repository.AllThermometers!;
    _during = 60;
    RefreshViews();
  }

  private void SubscribeToEvents()
  {
    _mainView!.OnSelectedThermometer += OnSelectedThermometer;
    _mainView!.OnSelectedDuring += OnSelectedDuring;
    _mainView!.OnSelectedView += OnSelectedView;
  }

  private void OnSelectedView(object? sender, string e) => _mainView?.ChangeVisibilityOfView(e);

  private void OnSelectedDuring(object? sender, int during)
  {
    _during = during;
    RefreshViews();
  }

  private void OnSelectedThermometer(object? sender, string name)
  {
    _currentThermometer = _repository.GetThermometer(name);
    RefreshViews();
  }

  public void RefreshViews()
  {
      _mainView!.AvailableViews = _repository.GetAvailableViews(_currentThermometer!);
      UpdateMeasures();
  }

  private void UpdateMeasures()
  {
    foreach (var nameView in _mainView!.AvailableViews)
    {
        var lastDate = _repository.GetLastMeasureDate(_currentThermometer!, nameView).AddSeconds(_during * -1);
        _mainView!.MeasureViewModels = _repository.GetMeasures(_currentThermometer!, lastDate, nameView).Select(m => new MeasureViewModel(m));
    }
  }
}
