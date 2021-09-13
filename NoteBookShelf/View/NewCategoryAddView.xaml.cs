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
    /// Interaction logic for NewCategoryAddView.xaml
    /// </summary>
    public partial class NewCategoryAddView : Window
    {
        public NewCategoryAddView(List<string> categories)
        {
            InitializeComponent();
            NewCategoryAddViewModel newcategoryaddviewmodel = new NewCategoryAddViewModel(categories);
            this.DataContext = newcategoryaddviewmodel;
        }
    }
}
