using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CartoonStyle
{
    public static class CartoonEffectFilterExtension
    {
        public static Bitmap CartoonEffectFilter(
            this Bitmap sourceBitmap,
            byte threshold = 0,
            SmoothingFilterType smoothFilter
                = SmoothingFilterType.None)
        {
            sourceBitmap = sourceBitmap.SmoothingFilter(smoothFilter);

            var sourceData =
                sourceBitmap.LockBits(new Rectangle(0, 0,
                        sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);


            var pixelBuffer = new byte[sourceData.Stride *
                                       sourceData.Height];


            var resultBuffer = new byte[sourceData.Stride *
                                        sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            var byteOffset = 0;
            int blueGradient, greenGradient, redGradient = 0;
            double blue = 0, green = 0, red = 0;


            var exceedsThreshold = false;


            for (var offsetY = 1;
                offsetY <
                sourceBitmap.Height - 1;
                offsetY++)
            for (var offsetX = 1;
                offsetX <
                sourceBitmap.Width - 1;
                offsetX++)
            {
                byteOffset = offsetY * sourceData.Stride +
                             offsetX * 4;


                blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);


                blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                                         pixelBuffer[byteOffset + sourceData.Stride]);


                byteOffset++;


                greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);


                greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                                          pixelBuffer[byteOffset + sourceData.Stride]);


                byteOffset++;


                redGradient =
                    Math.Abs(pixelBuffer[byteOffset - 4] -
                             pixelBuffer[byteOffset + 4]);


                redGradient +=
                    Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                             pixelBuffer[byteOffset + sourceData.Stride]);


                if (blueGradient + greenGradient + redGradient > threshold)
                {
                    exceedsThreshold = true;
                }
                else
                {
                    byteOffset -= 2;


                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] -
                                            pixelBuffer[byteOffset + 4]);
                    byteOffset++;


                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] -
                                             pixelBuffer[byteOffset + 4]);
                    byteOffset++;


                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4] -
                                           pixelBuffer[byteOffset + 4]);


                    if (blueGradient + greenGradient + redGradient > threshold)
                    {
                        exceedsThreshold = true;
                    }
                    else
                    {
                        byteOffset -= 2;


                        blueGradient =
                            Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                                     pixelBuffer[byteOffset + sourceData.Stride]);


                        byteOffset++;


                        greenGradient =
                            Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                                     pixelBuffer[byteOffset + sourceData.Stride]);


                        byteOffset++;


                        redGradient =
                            Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                                     pixelBuffer[byteOffset + sourceData.Stride]);


                        if (blueGradient + greenGradient +
                            redGradient > threshold)
                        {
                            exceedsThreshold = true;
                        }
                        else
                        {
                            byteOffset -= 2;


                            blueGradient =
                                Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] -
                                         pixelBuffer[byteOffset + 4 + sourceData.Stride]);


                            blueGradient +=
                                Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] -
                                         pixelBuffer[byteOffset + sourceData.Stride - 4]);


                            byteOffset++;


                            greenGradient =
                                Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] -
                                         pixelBuffer[byteOffset + 4 + sourceData.Stride]);


                            greenGradient +=
                                Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] -
                                         pixelBuffer[byteOffset + sourceData.Stride - 4]);


                            byteOffset++;


                            redGradient =
                                Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] -
                                         pixelBuffer[byteOffset + 4 + sourceData.Stride]);


                            redGradient +=
                                Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] -
                                         pixelBuffer[byteOffset + sourceData.Stride - 4]);


                            if (blueGradient + greenGradient + redGradient > threshold)
                                exceedsThreshold = true;
                            else
                                exceedsThreshold = false;
                        }
                    }
                }


                byteOffset -= 2;


                if (exceedsThreshold)
                {
                    blue = 0;
                    green = 0;
                    red = 0;
                }
                else
                {
                    blue = pixelBuffer[byteOffset];
                    green = pixelBuffer[byteOffset + 1];
                    red = pixelBuffer[byteOffset + 2];
                }


                blue = blue > 255 ? 255 :
                    blue < 0 ? 0 :
                    blue;


                green = green > 255 ? 255 :
                    green < 0 ? 0 :
                    green;


                red = red > 255 ? 255 :
                    red < 0 ? 0 :
                    red;


                resultBuffer[byteOffset] = (byte) blue;
                resultBuffer[byteOffset + 1] = (byte) green;
                resultBuffer[byteOffset + 2] = (byte) red;
                resultBuffer[byteOffset + 3] = 255;
            }


            var resultBitmap = new Bitmap(sourceBitmap.Width,
                sourceBitmap.Height);


            var resultData =
                resultBitmap.LockBits(new Rectangle(0, 0,
                        resultBitmap.Width, resultBitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }
    }
}