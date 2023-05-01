using Diary.Commands;
using Diary.Models;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly bool _canClose;
        public SettingsViewModel(bool canClose)
        {
            CloseCommand = new RelayCommand(Close, CanClose);
            ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
            _canClose = canClose;

            Settings = new SettingsWrapper
            {
                ServerAddress = Properties.Settings.Default.Address,
                ServerName = Properties.Settings.Default.ServerName,
                DbName = Properties.Settings.Default.DbName,
                UserName = Properties.Settings.Default.UserName,
            };
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        SettingsWrapper _settings;

        public SettingsWrapper Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

        private void Confirm(object obj)
        {
            var loginParams = obj as PasswordParams;

            UpdateSettings();

            Restart();
        }

        private void Restart()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private bool CanConfirm(object obj)
        {
            return Settings.IsValid;
        }

        private void UpdateSettings()
        {
                Properties.Settings.Default.Address = Settings.ServerAddress;
                Properties.Settings.Default.ServerName = Settings.ServerName;
                Properties.Settings.Default.DbName = Settings.DbName;
                Properties.Settings.Default.UserName = Settings.UserName;
                Properties.Settings.Default.Password = Settings.Password;
                Properties.Settings.Default.Save();
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        private bool CanClose(object obj)
        {
            return _canClose;
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
