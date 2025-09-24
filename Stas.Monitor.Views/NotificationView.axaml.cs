using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Stas.Monitor.Views;

public partial class NotificationView : Window
{
  public enum MessageBoxButtons
  {
    YesCancel
  }

  public enum MessageBoxResult
  {
    Ok,
    Cancel,
    Yes
  }

  private NotificationView()
  {
    AvaloniaXamlLoader.Load(this);
  }

  public static Task<MessageBoxResult> Show(Window? parent, string? text, string title,
    MessageBoxButtons buttons)
  {
    var notificationBox = new NotificationView() { Title = title };
    notificationBox.FindControl<TextBlock>("NotificationText")!.Text = "Message de l'exception : "+text;
    var buttonPanel = notificationBox.FindControl<StackPanel>("Buttons");

    var res = MessageBoxResult.Ok;

    void AddButton(string caption, MessageBoxResult r)
    {
      var btn = new Button { Content = caption, Foreground = Brushes.Chartreuse, FontSize = 27};
      btn.Click += (_, _) =>
      {
        res = r;
        notificationBox.Close();
      };
      buttonPanel?.Children.Add(btn);
    }

    if (buttons is MessageBoxButtons.YesCancel)
    {
      AddButton("Oui", MessageBoxResult.Yes);
      AddButton("Annuler", MessageBoxResult.Cancel);
    }

    var tcs = new TaskCompletionSource<MessageBoxResult>();
    notificationBox.Closed += delegate { tcs.TrySetResult(res); };
    if (parent != null)
    {
      notificationBox.ShowDialog(parent);
    }
    else
    {
      notificationBox.Show();
    }

    return tcs.Task;
  }
}
