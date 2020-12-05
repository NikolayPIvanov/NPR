using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Screening
{
    public class BitmapConverter
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        
        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            var ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, 
                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }
    }
}