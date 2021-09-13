using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NoteBookShelf.Models
{
    public class NoteBook
    {
        public string Title { get; set; }
        public BitmapImage Image { get; set; }

        public string Date { get; set; }
        public NoteBook (string title, object image, string date)
        {
            Title = title;
            Image = LoadImage((Bitmap)image);
            Date = date;
        }
        public BitmapImage LoadImage(Bitmap image)
        {
           
            using (var memory = new MemoryStream())
            {
                image.Save(memory,image.RawFormat);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
