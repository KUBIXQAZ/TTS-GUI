using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TTS_GUI.MVVM;
using Newtonsoft.Json;
using TTS_GUI.Models;
using System.IO;
using System.Windows;

namespace TTS_GUI.ViewModels
{
    public class MainControlPanelViewModel : ViewModelBase
    {
        private ViewModelBase viewModel;
        private SettingsModel settings;

        private SpeechSynthesizer speechSynthesizer;

        private string _ttsText;
        public string TtsText
        {
            get
            {
                return _ttsText;
            }
            set
            {
                _ttsText = value;
                OnPropertyChanged(nameof(TtsText));
            }
        }

        private int _volume;
        public int Volume
        {
            get 
            { 
                return _volume; 
            } 
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        private int _speechRate;
        public int SpeechRate
        {
            get
            {
                return _speechRate;
            }
            set
            {
                _speechRate = value;
                OnPropertyChanged(nameof(SpeechRate));
            }
        }

        public RelayCommand StartTtsCommand => new RelayCommand(execute => StartTts());
        public RelayCommand PauseResumeCommand => new RelayCommand(execute => PauseResume());
        public RelayCommand ChangeVolumeCommand => new RelayCommand(execute => ChangeVolume());
        public RelayCommand ChangeSpeechRateCommand => new RelayCommand(execute => ChangeSpeechRate());
        public RelayCommand PasteCommand => new RelayCommand(execute => PasteText());

        public MainControlPanelViewModel(ViewModelBase viewModel, SettingsModel settings)
        {
            this.viewModel = viewModel;
            this.settings = settings;

            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("pl-pl"));

            _volume = settings.volume;
            speechSynthesizer.Volume = _volume;

            _speechRate = settings.speechRate;
            speechSynthesizer.Rate = _speechRate;
        }

        private void StartTts()
        {
            if (TtsText == null) return;

            speechSynthesizer.SpeakAsyncCancelAll();
            if (speechSynthesizer.State == SynthesizerState.Paused) speechSynthesizer.Resume();

            speechSynthesizer.SpeakAsync(_ttsText);
        }

        private void PauseResume()
        {
            if (speechSynthesizer.State == SynthesizerState.Speaking)
            {
                speechSynthesizer.Pause();
            } 
            else if (speechSynthesizer.State == SynthesizerState.Paused)
            {
                speechSynthesizer.Resume();
            }
        }

        private void ChangeVolume()
        {
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\TTS";
            string path = $"{folder}\\settings.json";

            settings.volume = _volume;

            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(path, json);

            speechSynthesizer.Volume = _volume;
            StartTts();
        }

        private void ChangeSpeechRate()
        {
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\TTS";
            string path = $"{folder}\\settings.json";

            settings.speechRate = _speechRate;

            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(path, json);

            speechSynthesizer.Rate = _speechRate;
            StartTts();
        }

        private void PasteText()
        {
            TtsText = Clipboard.GetText();
        }
    }
}
