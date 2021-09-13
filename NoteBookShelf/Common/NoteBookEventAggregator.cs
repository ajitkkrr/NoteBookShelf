using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookShelf.Common
{
    public class NoteBookEventAggregator 
    {
        private static IEventAggregator _eventAggregator;
        static NoteBookEventAggregator()
        {
            _eventAggregator = new EventAggregator();
        } 
        public static IEventAggregator EventInstance
        {
            get { return _eventAggregator; }
        }
    }
}
