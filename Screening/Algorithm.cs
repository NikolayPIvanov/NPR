using System.Drawing;
using System.Windows.Media.Imaging;
using Size = System.Windows.Size;

namespace Screening
{
    public abstract class Algorithm
    {
        protected Bitmap originalBitmap;
        protected Bitmap outputBitmap;
        protected int width, height;

        public BitmapSource OriginalBitmap => BitmapConverter.LoadBitmap(originalBitmap);
        public BitmapSource OutputBitmapSource => BitmapConverter.LoadBitmap(outputBitmap);

        public abstract (Size, BitmapSource) LoadImage(string filename);
    }
}