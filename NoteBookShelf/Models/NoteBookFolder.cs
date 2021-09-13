using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookShelf.Models
{
    public class NoteBookFolder
    {
        public string FolderName { get; set; }
        public List<NoteBook> NoteBooks { get; set; }
        public NoteBookFolder(string foldername, List<NoteBook> notebooks)
        {
            FolderName = foldername;
            NoteBooks = notebooks;
        }
    }
}
