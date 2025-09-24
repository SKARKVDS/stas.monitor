using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ClpNet;
using Serilog;
using Stas.Monitor.Domains.Factory;
using Stas.Monitor.Domains.Interfaces;
using Stas.Monitor.Infrastructures;
using Stas.Monitor.Infrastructures.Reader;
using Stas.Monitor.Presentations;
using Stas.Monitor.Views;

namespace Stas.Monitor.App;

public class App : Application
{
    private MainWindow? _mainWindow;
    private MainPresenter? _mainPresenter;
    private string[]? _args;
    private DispatcherTimer? _thread;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = log;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _args = desktop.Args ?? Array.Empty<string>();
            _mainWindow = new MainWindow();
            desktop.MainWindow = _mainWindow;
            desktop.MainWindow.Opened += MainWindow_Opened;
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void MainWindow_Opened(object? sender, EventArgs eventArgs)
    {
        try
        {
            SetupApp(_args);
            _mainPresenter?.Start();
            _thread = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _thread.Tick += ThreadTick;
            _thread.Start();
        }
        catch ( Exception e )
        {
            Log.Logger.Error("monitor: {EMessage}", e.Message);
            _mainWindow?.DisplayError(e.Message);
            _thread?.Stop();
        }
    }

    private void ThreadTick(object? sender, EventArgs e)
    {
        try
        {
            _mainPresenter?.RefreshViews();
        }
        catch (Exception ex)
        {
            if(sender is DispatcherTimer timer)
            {
                timer.Stop();
            }
            Log.Logger.Error("monitor: {EMessage}", ex.Message);
            _mainWindow?.DisplayError(ex.Message);
        }
    }

    private void SetupApp(string[]? args)
    {
        var parser = new CommandLineParser(args);
        var parameters = parser.Parameters;
        var parametersMap = parameters.ToDictionary(p => p.Key, p => p.Value);
        var configurationReader = new IniConfigurationReader(parametersMap);
        var configurationFactory = new ConfigurationFactory(configurationReader);
        var configuration = configurationFactory.Create();
        IThermometerRepository repository = new Repository(configuration);
        _mainPresenter = new MainPresenter(_mainWindow!, repository);
    }

}
