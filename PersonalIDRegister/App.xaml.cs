using System;
using System.IO;
using System.Windows;

namespace PersonalIDRegister
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      var appName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
      if (appName.EndsWith(".vshost"))
        appName = appName.Substring(0, appName.Length - ".vshost".Length);

      var dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);

      if (!Directory.Exists(dataDirectory))
      {
        Directory.CreateDirectory(dataDirectory);
      }

      AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

      base.OnStartup(e);
    }
  }
}
