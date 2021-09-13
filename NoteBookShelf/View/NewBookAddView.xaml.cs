using NoteBookShelf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoteBookShelf.View
{
    /// <summary>
    /// Interaction logic for NewBookAddView.xaml
    /// </summary>
    public partial class NewBookAddView : Window
    {
        public NewBookAddView(List<string> categories,string currentCategory="")
        {
            InitializeComponent();
            this.DataContext = new NewBookAddViewModel(categories, currentCategory);
        }
    }
}
