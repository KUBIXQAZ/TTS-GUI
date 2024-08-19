using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace TTS_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadWindowPosition();
        }

        private void LoadWindowPosition()
        {
            App app = (App)App.Current;

            Application.Current.MainWindow.Left = app.Settings.windowPosition.x;
            Application.Current.MainWindow.Top = app.Settings.windowPosition.y;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SaveWindowPosition();
        }

        private void SaveWindowPosition()
        {
            App app = (App)App.Current;

            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\TTS";
            string path = $"{folder}\\settings.json";

            app.Settings.windowPosition.x = Application.Current.MainWindow.Left;
            app.Settings.windowPosition.y = Application.Current.MainWindow.Top;

            string json = JsonConvert.SerializeObject(app.Settings);
            File.WriteAllText(path, json);
        }
    }
}