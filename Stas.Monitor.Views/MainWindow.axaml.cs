using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Stas.Monitor.Presentations;
using static Stas.Monitor.Views.NotificationView;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
    public event EventHandler<string>? OnSelectedThermometer;
    public event EventHandler<int>? OnSelectedDuring;
    public event EventHandler<string>? OnSelectedView;

    private readonly List<MeasuresView> _measuresAndAlertsViews;

    private Task<MessageBoxResult>? _notificationView;
    public MainWindow()
    {
      InitializeComponent();
      _measuresAndAlertsViews = new List<MeasuresView>();
      _notificationView = null;
    }

    private void OnChangedThermometer(object? sender, SelectionChangedEventArgs e)
    {
        if(e.AddedItems.Count == 0)
        {
            return;
        }
        if ( sender is not ComboBox { SelectedItem: string selectedValue } )
        {
          return;
        }
        AvailableView.Children.Clear();
        _measuresAndAlertsViews.Clear();
        RefreshVisibleViews();
        OnSelectedThermometer?.Invoke(this, selectedValue);
    }

    public string[] ThermometerNames
    {
      set
      {
        Thermometers.Clear();
        foreach (var thermometer in value)
        {
          Thermometers.Items.Add(thermometer);
        }
        if(Thermometers.Items.Count == 0)
        {
          Thermometers.Items.Add("No thermometers available");
        }
        Thermometers.SelectedIndex = 0;
      }
    }

    public string[] AvailableViews
    {
        get => AvailableView.Children.OfType<CheckBox>()
            .Where(c => c.IsChecked == true)
            .Select(c => c.Content?.ToString()!)
            .ToArray();
        set
      {
        foreach (var name in value)
        {
            if (AvailableView.Children.Any(v => v is CheckBox { Content: string content } && content == name))
            {
                continue;
            }

            var checkBox = new CheckBox { Content = name, IsChecked = AvailableView.Children.Count < 2};
            checkBox.IsCheckedChanged += (_, _) => OnSelectedView?.Invoke(this, name);
            AvailableView.Children.Add(checkBox);
        }
        if(AvailableView.Children.Count == 0)
        {
          AvailableView.Children.Add(new Label { Content = "No series available", Foreground = Brushes.Red });
        }
      }
    }

    public IEnumerable<MeasureViewModel> MeasureViewModels
    {
      set
      {

        var type = value.FirstOrDefault()?.Type;
        if (type != null)
        {
            if (_measuresAndAlertsViews.Any(v => v.TypeOfMeasure == type))
            {
                var view = _measuresAndAlertsViews.First(v => v.TypeOfMeasure == type);
                view.MeasureViewModels = value;
            }
            else
            {
                var view = new MeasuresView { MeasureViewModels = value, TypeOfMeasure = type };
                _measuresAndAlertsViews.Add(view);
            }
        }
        RefreshVisibleViews();
      }
    }

    public async Task DisplayError(string? message)
    {
      Message.Foreground = Brushes.Red;
      MeasureAndAlertView.Children.Clear();
      Message.Content = message ?? "An error occured while loading the application";
      _notificationView ??= NotificationView.Show(this, message!, "An error occured while loading the application", MessageBoxButtons.YesCancel);
      var result = await _notificationView;
      switch (result)
      {
        case MessageBoxResult.Yes:
          Close();
          break;
        case MessageBoxResult.Cancel:
          _notificationView = null;
          break;
        case MessageBoxResult.Ok:
          Close();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void OnChangedDuring(object? sender, SelectionChangedEventArgs e)
    {
        if(e.AddedItems.Count == 0)
        {
          return;
        }
        var options = sender is ComboBox comboBox ? comboBox.SelectedIndex : 0;
        var seconds = options switch
        {
        0 => 30,
        1 => 60 * 1,
        2 => 60 * 5,
        _ => 60
        };
        OnSelectedDuring?.Invoke(this, seconds);
    }

    public void ChangeVisibilityOfView(string name)
    {
      var view = _measuresAndAlertsViews.FirstOrDefault(v => v.TypeOfMeasure == name);
      var checkBox = AvailableView.Children.OfType<CheckBox>().FirstOrDefault(c => c.Content?.ToString() == name);
      if ( view == null || checkBox == null)
      {
          return;
      }

      view.IsVisible = (bool) checkBox.IsChecked!;
      RefreshVisibleViews();
    }

    private void RefreshVisibleViews()
    {
      MeasureAndAlertView.Children.Clear();
      if (!_measuresAndAlertsViews.Any())
      {
        MeasureAndAlertView.Children.Add(new Label { Content = "No measures or alerts available for this room", Foreground = Brushes.Red });
      }else if (_measuresAndAlertsViews.All(v => !v.IsVisible))
      {
        MeasureAndAlertView.Children.Add(new Label { Content = "No series selected", Foreground = Brushes.White });
      }
      foreach (var view in _measuresAndAlertsViews.Where(v => v.IsVisible))
      {
        MeasureAndAlertView.Children.Add(view);
      }
    }
}
