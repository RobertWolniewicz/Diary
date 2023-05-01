using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Wrappers
{
    public class SettingsWrapper : IDataErrorInfo
    {
        public string ServerAddress { get; set; }
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }

        bool _isServerAddressValid;
        bool _isServerNameValid;
        bool _isDbNameValid;
        bool _isUserNameValid;
        bool _isPasswordValid;
        bool _isConfirmedPasswordValid;
        bool _isPasswordsAreSame;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(ServerAddress):
                        if (string.IsNullOrWhiteSpace(ServerAddress))
                        {
                            Error = "Pole Adres serwera jest wymagane";
                            _isServerAddressValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerAddressValid = true;
                        }
                        break;
                    case nameof(ServerName):
                        if (string.IsNullOrWhiteSpace(ServerName))
                        {
                            Error = "Pole Nazwa serwera jest wymagane";
                            _isServerNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerNameValid = true;
                        }
                        break;
                    case nameof(DbName):
                        if (string.IsNullOrWhiteSpace(ServerName))
                        {
                            Error = "Pole Nazwa bazy danych jest wymagane";
                            _isDbNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isDbNameValid = true;
                        }
                        break;
                    case nameof(UserName):
                        if (string.IsNullOrWhiteSpace(UserName))
                        {
                            Error = "Pole Użytkownik jest wymagane";
                            _isUserNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isUserNameValid = true;
                        }
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            Error = "Pole Hasło jest wymagane";
                            _isPasswordValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isPasswordValid = true;
                        }
                        break;
                    case nameof(ConfirmedPassword):
                        if (string.IsNullOrWhiteSpace(ConfirmedPassword))
                        {
                            Error = "Pole Powtórz hasło jest wymagane";
                            _isConfirmedPasswordValid = false;
                        }
                        else if (Password != ConfirmedPassword)
                        {
                            Error = "Hasła nie sa identyczne";
                            _isPasswordsAreSame = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isConfirmedPasswordValid = true;
                            _isPasswordsAreSame = true;
                        }
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }

        public string Error { get; set; }

        public bool IsValid
        {
            get
            {
                return _isServerAddressValid &&
                    _isServerNameValid &&
                    _isDbNameValid &&
                    _isUserNameValid; 
                    //&& _isPasswordValid && 
                    //_isConfirmedPasswordValid && 
                    //_isPasswordsAreSame;
            }
        }

    }
}
