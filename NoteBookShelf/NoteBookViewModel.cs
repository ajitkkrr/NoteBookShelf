using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteBookShelf.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Drawing;
using NoteBookShelf.Common;
using System.Windows;
using NoteBookShelf.View;
using NoteBookShelf.ViewModel;

namespace NoteBookShelf
{
    public class NoteBookViewModel : ViewModelBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public List<Object> Images;
        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }

        }
        private ObservableCollection<NoteBook> _noteBooks;
        public ObservableCollection<NoteBook> NoteBooks
        {
            get
            {
                if(_noteBooks == null)
                {
                    _noteBooks = new ObservableCollection<NoteBook>();
                }
                return _noteBooks;
            }
            set
            {
                _noteBooks = value;
                OnPropertyChanged("NoteBooks");
            }         
        }

        private ObservableCollection<NoteBookFolder> _noteBookFolders;
        public ObservableCollection<NoteBookFolder> NoteBookFolders
        {
            get
            {
                if(_noteBookFolders == null)
                {
                    _noteBookFolders = new ObservableCollection<NoteBookFolder>();
                }
                return _noteBookFolders;
            }
            set
            {
                _noteBookFolders = value;
                OnPropertyChanged("NoteBookFolders");
            }
        }
        public NoteBookViewModel()
        {
            SubscribeEvents();
            var images = Properties.Resources.ResourceManager
                       .GetResourceSet(CultureInfo.CurrentCulture, true, true)
                       .Cast<DictionaryEntry>()
                       .Where(x => x.Value.GetType() == typeof(Bitmap))
                       .Select(x => new { Name = x.Key.ToString(), Image = x.Value })
                       .ToList();
            Images = images.Select(x => x.Image).ToList();
            string date = DateTime.UtcNow.ToString("mm/dd/yyyy");
            int index = 0;
            List<string> bookNames = new List<string>();
            //foreach (var image in images)
            //{
            //    string name = string.Format("Book{0}",++index);
            //    NoteBooks.Add(new NoteBook(name, image.Image, date));
            //    bookNames.Add(name);
            //}
            NoteBookFolders.Add(new NoteBookFolder("Default", NoteBooks.ToList()));        
        }

        #region Methods

        public Bitmap GetRandomImage()
        {
            Random random = new Random();
            int index = random.Next(0, Images.Count);
            return (Bitmap)Images[index];
        }

        public void SubscribeEvents()
        {
            NoteBookEventAggregator.EventInstance.GetEvent<NoteBookShelfEvent<KeyValuePair<NoteBookPopup, Dictionary<NewBook, string>>>>().Subscribe(GetNewBookRecord);
            NoteBookEventAggregator.EventInstance.GetEvent<NoteBookShelfEvent<KeyValuePair<NoteBookPopup, KeyValuePair<NewCategory, string>>>>().Subscribe(GetNewCategoryRecord);
        }
        public void LoadFolderItems(string FolderName)
        {
            var notebooks = NoteBookFolders.Where(p => p.FolderName == FolderName).Select(i => i.NoteBooks).FirstOrDefault();
            NoteBooks = new ObservableCollection<NoteBook>(notebooks);
        }

        public void AddNewFolder()
        {
            NewCategoryAddView window = new NewCategoryAddView(NoteBookFolders.Select(b => b.FolderName).ToList());
            window.Title = "Add New Category";
            window.ResizeMode = ResizeMode.NoResize;
            window.Show();
        }
        public void AddNewBook(object category)
        {
            string currentCategory = category != null ? category.ToString() : string.Empty;
            NewBookAddView window = new NewBookAddView(NoteBookFolders.Select(b => b.FolderName).ToList(), currentCategory);
            window.Title = "Add New Book";
            window.ResizeMode = ResizeMode.NoResize;
            window.Show();
        }
        public void LoadBook(string bookName)
        {
            List<NoteBook> notebooks = new List<NoteBook>();
            NoteBookFolders.ToList().ForEach(folder =>
            {
                notebooks.AddRange(folder.NoteBooks);
            });
            NoteBooks = new ObservableCollection<NoteBook>(notebooks.Where(b=>b.Title == bookName).ToList());
        }
        public void GetNewBookRecord(KeyValuePair<NoteBookPopup,Dictionary<NewBook,string>> record)
        {
            if(record.Key == NoteBookPopup.NewBook)
            {
                NoteBook notebook = new NoteBook(record.Value[NewBook.Name],GetRandomImage(), record.Value[NewBook.Date]);
                NoteBooks.Add(notebook);
                var folder = NoteBookFolders.Where(p => p.FolderName == record.Value[NewBook.Category]).Select(x => x.NoteBooks).FirstOrDefault();
                if(folder!= null)
                {
                    folder.Add(notebook);
                    NoteBookFolders = new ObservableCollection<NoteBookFolder>(_noteBookFolders);
                }
                else
                {
                    NoteBookFolders.Add(new NoteBookFolder(record.Value[NewBook.Category], new List<NoteBook>() { notebook }));
                }
                OnPropertyChanged("NoteBookFolders");
            }
        }

        public void GetNewCategoryRecord(KeyValuePair<NoteBookPopup, KeyValuePair<NewCategory, string>> record)
        {
            if(record.Key == NoteBookPopup.NewCategory)
            {
                NoteBookFolders.Add(new NoteBookFolder(record.Value.Value, new List<NoteBook>()));
            }
        }
        #endregion

        #region Command

        private RelayCommand _showNavigationCommand;
        public RelayCommand ShowNavigationCommand
        {
            get
            {
                if(_showNavigationCommand == null)
                {
                    _showNavigationCommand = new RelayCommand(p => { IsVisible = IsVisible ? false : true; OnPropertyChanged("IsVisble"); });
                }
                return _showNavigationCommand;
            }

        }

        private RelayCommand _loadFolderItemsCommand;
        public RelayCommand LoadFolderItemsCommand
        {
            get
            {
                if (_loadFolderItemsCommand == null)
                {
                    _loadFolderItemsCommand = new RelayCommand(p => LoadFolderItems(p.ToString()));
                }
                return _loadFolderItemsCommand;
            }
        }

        private RelayCommand _addNewFolderCommand;
        public RelayCommand AddNewFolderCommand
        {
            get
            {
                if (_addNewFolderCommand == null)
                {
                    _addNewFolderCommand = new RelayCommand(p => AddNewFolder());
                }
                return _addNewFolderCommand;
            }
        }

        private RelayCommand _addNewBookCommand;
        public RelayCommand AddNewBookCommand
        {
            get
            {
                if (_addNewBookCommand == null)
                {
                    _addNewBookCommand = new RelayCommand(p => AddNewBook(p));
                }
                return _addNewBookCommand;
            }
        }

        private RelayCommand _loadBookCommand;
        public RelayCommand LoadBookCommand
        {
            get
            {
                if (_loadBookCommand == null)
                {
                    _loadBookCommand = new RelayCommand(p => LoadBook(p.ToString()));
                }
                return _loadBookCommand;
            }
        }
        #endregion
 
    }
}
