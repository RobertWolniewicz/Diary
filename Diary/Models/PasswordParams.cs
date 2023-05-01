using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Diary.Models
{
    public class PasswordParams
    {
        public Window Window { get; set; }
        public PasswordBox PasswordBox { get; set; }
        public PasswordBox ConfirmPasswordBox { get; set; }
    }
}
