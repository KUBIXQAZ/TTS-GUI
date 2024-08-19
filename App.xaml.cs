using Newtonsoft.Json;
using System.IO;
using System.Windows;
using TTS_GUI.Models;
using TTS_GUI.MVVM;
using TTS_GUI.ViewModels;

namespace TTS_GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ViewModelBase ViewModel { get; set; }
        public SettingsModel Settings { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoadSettings();

            MainWindow mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(ViewModel, Settings),
                Topmost = true
            };
            mainWindow.Show();
        }

        private void LoadSettings()
        {
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\TTS";
            string path = $"{folder}\\settings.json";

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            if(File.Exists(path))
            {
                string file = File.ReadAllText(path);
                Settings = JsonConvert.DeserializeObject<SettingsModel>(file);
            } 
            else
            {
                Settings = new SettingsModel();
                Settings.volume = 50;

                PositionModel position = new PositionModel
                {
                    x = 0,
                    y = 0,
                };

                Settings.windowPosition = position;

                Settings.speechRate = 0;

                string json = JsonConvert.SerializeObject(Settings);
                File.WriteAllText(path, json);
            }
        }
    }

}