using TTS_GUI.Models;
using TTS_GUI.MVVM;

namespace TTS_GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase ViewModel { get; set; }
        private SettingsModel settings;

        public MainWindowViewModel(ViewModelBase viewModel, SettingsModel settings)
        {
            ViewModel = viewModel;
            this.settings = settings;

            ViewModel = new MainControlPanelViewModel(ViewModel, settings);
        }
    }
}