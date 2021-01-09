using System.Drawing;
using System.Windows.Media.Imaging;
using Size = System.Windows.Size;

namespace Screening
{
    public class OrderDithering : Algorithm
    {
        private int[,] dm;

        public OrderDithering()
        {
            originalBitmap = null;
            outputBitmap = null;
            dm = new int[2, 2];
            dm[0, 0] = 0;
            dm[0, 1] = 1;
            dm[1, 0] = 3;
            dm[1, 1] = 2;
        }

        public override (Size, BitmapSource) LoadImage(string filename)
        {
            originalBitmap = new Bitmap(Image.FromFile(filename));
            width = originalBitmap.Width;
            height = originalBitmap.Height;

            return (new Size(width, height), BitmapConverter.LoadBitmap(originalBitmap));
        }

        private int Approx2(int c)
        {
            if (c >= 0 && c < 63)
                return 0;
            if (c >= 63 && c < 127)
                return 63;
            if (c >= 127 && c < 191)
                return 127;
            if (c >= 191 && c < 255)
                return 191;
            else
                return 0;
        }


        private int Approx(int c)
        {
            if (c >= 0 && c < 63)
                return 0;
            if (c >= 63 && c < 127)
                return 1;
            if (c >= 127 && c < 191)
                return 2;
            if (c >= 191 && c < 255)
                return 3;
            else
                return 3;
        }

        public void Dithering()
        {
            int k;
            int m;

            outputBitmap = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    k = i % 2;
                    m = j % 2;
                    var pixel = originalBitmap.GetPixel(i, j);

                    if (Approx(pixel.R) < dm[k, m])
                    {
                        outputBitmap.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        var color = Color.FromArgb(pixel.ToArgb());
                        outputBitmap.SetPixel(i, j, color);
                    }
                }
            }
        }

    }
}