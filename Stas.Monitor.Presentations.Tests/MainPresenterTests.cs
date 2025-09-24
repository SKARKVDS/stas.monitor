using NSubstitute;
using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Presentations.Tests;

public class Tests
{
    private IMainView? _mainView;
    private IThermometerRepository? _repository;
    private MainPresenter? _presenter;
    [SetUp]
    public void Setup()
    {
        _mainView = Substitute.For<IMainView>();
        _repository = Substitute.For<IThermometerRepository>();
        _presenter = new MainPresenter(_mainView, _repository);
    }

    [Test]
    public void Should_Provide_His_View_With_Thermometer_Name()
    {
      var toReceive = new [] { "Thermometer 1", "Thermometer 2" };
      var mockedView = Substitute.For<IMainView>();
      var mockedRepository = Substitute.For<IThermometerRepository>();
      mockedRepository.AllThermometers.Returns(toReceive);
      var presenter = new MainPresenter(mockedView, mockedRepository);

      presenter.Start();

      mockedView.Received(1).ThermometerNames = toReceive;
    }

    [Test]
    public void Should_Select_Thermometer_Correctly()
    {
      _repository?.GetThermometer("Thermometer 1").Returns("Thermometer 1");
      _presenter?.Start();
      _mainView!.OnSelectedThermometer += Raise.Event<EventHandler<string>>(_mainView, "Thermometer 1");
      _repository.Received(1)?.GetThermometer("Thermometer 1");
    }

    [Test]
    public void Should_Modify_Visibility_Of_View()
    {
      _presenter?.Start();
      _mainView!.OnSelectedView += Raise.Event<EventHandler<string>>(_mainView, "Thermometer 1");
      _mainView.Received(1).ChangeVisibilityOfView("Thermometer 1");
    }
}
