using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Runtime.CompilerServices;

namespace NoteBookShelf.Common
{
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public List<string> Errors { get; private set;}
        public string this[string PropertyName]
        {
            get
            {
                return Validate(PropertyName);
            }
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string Validate(string propertyName)
        {
            return string.Empty;
        }
        public virtual void AddError(string propertyName)
        {
            if(!Errors.Contains(propertyName))
            {
                Errors.Add(propertyName);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
