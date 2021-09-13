using NoteBookShelf.Common;
using NoteBookShelf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NoteBookShelf.ViewModel
{
    public class NewBookAddViewModel:ViewModelBase
    {
        
        Dictionary<string,string> _errors = new Dictionary<string, string>();


        private bool _canModifyCategory;
        public bool CanModifyCategory
        {
            get { return _canModifyCategory; }
            set
            {
                _canModifyCategory = value;
                OnPropertyChanged("CanModifyCategory");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private DateTime? _bookDate;
        public DateTime? BookDate
        {
            get { return _bookDate; }
            set
            {
                _bookDate = value;
                OnPropertyChanged("BookDate");
            }
        }
        private ObservableCollection<string> _categories;
        public ObservableCollection<string> Categories
        {
            get
            {
                if(_categories == null)
                {
                    _categories = new ObservableCollection<string>();
                }
                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        public NewBookAddViewModel(List<string> categories, string currentCategory)
        {
            Categories = new ObservableCollection<string>(categories);
            SelectedCategory = Categories.Where(p => p == currentCategory).FirstOrDefault();
            CanModifyCategory = !string.IsNullOrWhiteSpace(currentCategory) && !string.IsNullOrWhiteSpace(currentCategory) ? false : true;
        }

        public void Save(Window w)
        {
            Dictionary<NewBook,string> newbookdata = new Dictionary<NewBook, string>();
            newbookdata.Add(NewBook.Name, Name);
            newbookdata.Add(NewBook.Date, BookDate.Value.ToString("MM/dd/yyyy"));
            newbookdata.Add(NewBook.Category, SelectedCategory);
            NoteBookEventAggregator.EventInstance.GetEvent<NoteBookShelfEvent<KeyValuePair<NoteBookPopup, Dictionary<NewBook, string>>>>().Publish(
                new KeyValuePair<NoteBookPopup, Dictionary<NewBook, string>>(NoteBookPopup.NewBook, newbookdata));
            Cancel(w);
        }
        public void Cancel(Window window)
        {
            if (window !=null)
            {
                window.Close();
            }
        }
        public override string Validate(string propertyName)
        {
            string Error = string.Empty;
            switch(propertyName)
            {
                case "Name":
                    Error = string.IsNullOrWhiteSpace(Name) ? "Book Name is Mandatory" : string.Empty;
                    break;
                case "BookDate":
                    Error = !BookDate.HasValue ? "Book Date is Mandatory" : string.Empty;
                    break;
                case "SelectedCategory":
                    Error = string.IsNullOrWhiteSpace(SelectedCategory) ? "Category must be selected" : string.Empty;
                    break;
            }
            if(!string.IsNullOrWhiteSpace(Error))
            {
               if(! _errors.ContainsKey(propertyName))
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
                if(_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(p=>Save((Window)p), p => { return !_errors.Any(); });
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
