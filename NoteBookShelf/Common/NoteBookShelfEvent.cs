using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
namespace NoteBookShelf.Common
{
    public class NoteBookShelfEvent<TPayload> : PubSubEvent<TPayload> { }

    public class NoteBookShelfEventEventArgs<TPayload> : EventArgs
    {
        public TPayload Args { get; set; }
        public int ID { get; set; }
        public string StrId { get; set; }

        public NoteBookShelfEventEventArgs(int id, TPayload value)
        {
            this.Args = value;
            this.ID = id;
        }

        public NoteBookShelfEventEventArgs(string strid, TPayload value)
        {
            this.Args = value;
            this.StrId = strid;
        }
    }
}
