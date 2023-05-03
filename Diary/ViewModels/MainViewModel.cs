using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        Repository _repository = new Repository();
        public MainViewModel()
        {

            AddStudentsCommand = new RelayCommand(AddEditStudents);
            EditStudentsCommand = new RelayCommand(AddEditStudents, CanEditDeleteStudent);
            DeleteStudentsCommand = new AsyncRelayCommand(DeleteStudents, CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            ChangeSettingsCommand = new RelayCommand(ChangeSettings);
            LoadedWindowCommand = new RelayCommand(LoadedWindow);

            LoadedWindow(null);

        }

        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand AddStudentsCommand { get; set; }
        public ICommand EditStudentsCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }
        public ICommand ChangeSettingsCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }

        StudentWrapper _selectedStudent;

        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<StudentWrapper> _students;

        public ObservableCollection<StudentWrapper> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
                RefreshDiary();
            }
        }

        ObservableCollection<Group> _group;

        public ObservableCollection<Group> Groups
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }
        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszyscy" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = 0;
        }
        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }
        private async Task DeleteStudents(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync
                ("Usuwanie ucznia", $"Czy napewno chcesz usunąć ucznia{SelectedStudent.FirstName}{SelectedStudent.LastName}?",
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
            {
                return;
            }

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        private void AddEditStudents(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }
        private void ChangeSettings(object obj)
        {
            var settingsWindow = new SettingsView();
            settingsWindow.ShowDialog();
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(
                _repository.GetStudents(SelectedGroupId));
        }
        private async void LoadedWindow(object obj)
        {
            if (!CanConnect())
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                var result = await metroWindow.ShowMessageAsync("Błąd połaczenia",
                    "Nie mozna połaczyć się z baza danych. Czy chcesz zmienić ustawienia połączenia?", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Negative)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    var settingsWindow = new SettingsView(false);
                    settingsWindow.ShowDialog();
                }
            }
            else
            {
                RefreshDiary();
                InitGroups();
            }
        }
        private bool CanConnect()
        {
           try
            {
                using(var context = new ApplicationDbContext())
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
