using System.Collections.Generic;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views.Interfaces;

public interface IMeasuresAndAlertsView
{
  IEnumerable<MeasureViewModel> MeasureViewModels { set; }
}
