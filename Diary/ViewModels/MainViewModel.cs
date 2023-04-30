using Diary.Commands;
using Diary.Models;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            AddStudentsCommand = new RelayCommand(AddEditStudents);
            EditStudentsCommand = new RelayCommand(AddEditStudents, CanEditDeleteStudent);
            DeleteStudentsCommand = new AsyncRelayCommand(DeleteStudents, CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            RefreshDiary();
        }

        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand AddStudentsCommand { get; set; }
        public ICommand EditStudentsCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }

        Student _selectedStudent;

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
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

        void RefreshStudents(object obj)
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
            //usuwanie
            RefreshDiary();
        }

        private void AddEditStudents(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as Student);
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
        void RefreshDiary()
        {

        }
    }
}
