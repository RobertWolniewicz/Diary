using Diary.Commands;
using Diary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class AddEditStudentViewModel : ViewModelBase
    {
        public AddEditStudentViewModel(Student student = null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);

            if (student == null)
            {
                Student = new Student();
            }
            else
            {
                Student = student;
                IsUpdate = true;
            }

        }



        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        Student _student;

        public Student Student
        {
            get { return _student; }
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        bool _isUpdate;

        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
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


        private void Confirm(object obj)
        {
            if (!IsUpdate)
                AddStudent();

            else
                UpdateStudent();

            CloseWindow(obj as Window);
        }

        private void AddStudent()
        {
            // baza
        }

        private void UpdateStudent()
        {
            //baza
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}