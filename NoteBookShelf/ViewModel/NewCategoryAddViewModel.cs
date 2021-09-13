using NoteBookShelf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NoteBookShelf.ViewModel
{
    public class NewCategoryAddViewModel:ViewModelBase
    {
        Dictionary<string, string> _errors = new Dictionary<string, string>();
        private List<string> Categories;

        private string _category;
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public NewCategoryAddViewModel(List<string> categories)
        {
            Categories = categories;
        }

        public void Save(Window w)
        {
            KeyValuePair<NewCategory, string> newcategorydata = new KeyValuePair<NewCategory, string>(NewCategory.Category, Category);
            NoteBookEventAggregator.EventInstance.GetEvent<NoteBookShelfEvent<KeyValuePair<NoteBookPopup, KeyValuePair<NewCategory,string>>>>().Publish(
                new KeyValuePair<NoteBookPopup, KeyValuePair<NewCategory, string>>(NoteBookPopup.NewCategory, newcategorydata));
            Cancel(w);
        }
        public void Cancel(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        public override string Validate(string propertyName)
        {
            string Error = string.Empty;
            switch (propertyName)
            {
                case "Category":
                    Error = string.IsNullOrWhiteSpace(Category) ? "Book Name is Mandatory" : string.Empty;
                    Error = string.IsNullOrWhiteSpace(Error) && Categories.Any(c=>c== Category)? "Category Already Exists":Error;
                    break;
            }
            if (!string.IsNullOrWhiteSpace(Error))
            {
                if (!_errors.ContainsKey(propertyName))
                {
                    _errors.Add(propertyName, Error);
                }
            }
            else
            {
                if (_errors.ContainsKey(propertyName))
                {
                    _errors.Remove(propertyName);
                }
            }
            return Error;
        }
        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(p => Save((Window)p), p => { return !_errors.Any(); });
                }
                return _saveCommand;
            }
        }
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(p => Cancel((Window)p));
                }
                return _cancelCommand;
            }
        }
    }
}
