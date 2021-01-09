using System.Drawing;

namespace CartoonStyle
{
    public static class SmoothingFilterExtension
    {
        public static Bitmap SmoothingFilter(this Bitmap sourceBitmap,
           SmoothingFilterType smoothFilter =
               SmoothingFilterType.None)
        {
            Bitmap inputBitmap = null;


            switch (smoothFilter)
            {
                case SmoothingFilterType.None:
                    {
                        inputBitmap = sourceBitmap;
                    }
                    break;
                case SmoothingFilterType.Gaussian3x3:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.Gaussian3x3, 1.0 / 16.0);
                    }
                    break;
                case SmoothingFilterType.Gaussian5x5:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.Gaussian5x5, 1.0 / 159.0);
                    }
                    break;
                case SmoothingFilterType.Gaussian7x7:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.Gaussian7x7, 1.0 / 136.0);
                    }
                    break;
                case SmoothingFilterType.Median3x3:
                    {
                        inputBitmap = sourceBitmap.MedianFilter(3);
                    }
                    break;
                case SmoothingFilterType.Median5x5:
                    {
                        inputBitmap = sourceBitmap.MedianFilter(5);
                    }
                    break;
                case SmoothingFilterType.Median7x7:
                    {
                        inputBitmap = sourceBitmap.MedianFilter(7);
                    }
                    break;
                case SmoothingFilterType.Median9x9:
                    {
                        inputBitmap = sourceBitmap.MedianFilter(9);
                    }
                    break;
                case SmoothingFilterType.Mean3x3:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.Mean3x3, 1.0 / 9.0);
                    }
                    break;
                case SmoothingFilterType.Mean5x5:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.Mean5x5, 1.0 / 25.0);
                    }
                    break;
                case SmoothingFilterType.LowPass3x3:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.LowPass3x3, 1.0 / 16.0);
                    }
                    break;
                case SmoothingFilterType.LowPass5x5:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.LowPass5x5, 1.0 / 60.0);
                    }
                    break;
                case SmoothingFilterType.Sharpen3x3:
                    {
                        inputBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.Sharpen3x3, 1.0 / 8.0);
                    }
                    break;
            }


            return inputBitmap;
        }
    }
}