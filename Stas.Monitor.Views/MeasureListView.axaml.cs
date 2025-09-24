using System;
using Avalonia.Controls;
using Avalonia.Media;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasureListView : UserControl
{
  public MeasureListView()
  {
    InitializeComponent();
  }


  public MeasureViewModel ViewModel
  {
    init
    {
      DateView.Text = value.Date?.ToString("dddd dd MMMM yyyy HH:mm:ss");
      MeasureValueView.Text = value.MeasureValue!;
      if (value.Difference == null)
      {
          ExpectedValueView.IsVisible = false;
          return;
      }

      if (value.Difference! >= 0)
      {
          Container.Background = Brushes.Red;
          ExpectedValueView.Text = "Erreur : +"+value.Difference!+value.Unit;
      }
      else
      {
          Container.Background = Brushes.Blue;
          ExpectedValueView.Text = "Erreur : "+value.Difference!+value.Unit;
      }
    }
  }

  public DateTime Date => DateTime.Parse(DateView.Text!);
}
