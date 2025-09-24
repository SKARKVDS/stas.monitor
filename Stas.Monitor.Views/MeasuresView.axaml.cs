using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using Stas.Monitor.Presentations;
using Stas.Monitor.Views.Interfaces;

namespace Stas.Monitor.Views;

public partial class MeasuresView : UserControl, IMeasuresAndAlertsView
{
  private readonly List<MeasureListView> _measureListViews;

  public MeasuresView()
  {
    InitializeComponent();
    _measureListViews = new List<MeasureListView>();
  }

  public IEnumerable<MeasureViewModel> MeasureViewModels
  {
    set
    {
      _measureListViews.Clear();
      foreach ( var vm in value )
      {
        var measureViewModel = new MeasureListView { ViewModel = vm };
        _measureListViews.Add(measureViewModel);
      }
      UpdateListView();
    }
  }

  private void UpdateListView()
  {
    MeasuresAndAlertsListView?.Children.Clear();
    if (_measureListViews.Any())
    {
        _measureListViews.Sort((u1, u2) => u2.Date.CompareTo(u1.Date));
      foreach ( var userControl in _measureListViews )
      {
        MeasuresAndAlertsListView?.Children.Add(userControl);
      }
    }
    else
    {
      var textBlock = new TextBlock { Text = "Aucune mesure ou alerte" };
      textBlock.Foreground = Brushes.Red;
      MeasuresAndAlertsListView?.Children.Add(textBlock);
    }
  }

  public string TypeOfMeasure
  {
    init => TypeOfRelease.Content = value;
    get => TypeOfRelease.Content?.ToString()!;
  }
}
