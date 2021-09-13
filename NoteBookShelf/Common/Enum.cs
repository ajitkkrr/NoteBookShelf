using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookShelf.Common
{
    public enum NoteBookPopup
    {
      NewBook,
      NewCategory
    }
    public enum NewBook
    {
        Name,
        Date,
        Category
    }
    public enum NewCategory
    {
        Category
    }
}
