using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace CartoonStyle
{
    public static class ConvolutionFilterExtension
    {   
        public static Bitmap ConvolutionFilter(this Bitmap sourceBitmap,
            double[,] filterMatrix,
            double factor = 1,
            int bias = 0)
        {
            var sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                    sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);


            var pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            var resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            var blue = 0.0;
            var green = 0.0;
            var red = 0.0;


            var filterWidth = filterMatrix.GetLength(1);
            var filterHeight = filterMatrix.GetLength(0);


            var filterOffset = (filterWidth - 1) / 2;
            var calcOffset = 0;


            var byteOffset = 0;


            for (var offsetY = filterOffset;
                offsetY <
                sourceBitmap.Height - filterOffset;
                offsetY++)
                for (var offsetX = filterOffset;
                    offsetX <
                    sourceBitmap.Width - filterOffset;
                    offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;


                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;


                    for (var filterY = -filterOffset;
                        filterY <= filterOffset;
                        filterY++)
                        for (var filterX = -filterOffset;
                            filterX <= filterOffset;
                            filterX++)
                        {
                            calcOffset = byteOffset +
                                         filterX * 4 +
                                         filterY * sourceData.Stride;


                            blue += pixelBuffer[calcOffset] *
                                    filterMatrix[filterY + filterOffset,
                                        filterX + filterOffset];


                            green += pixelBuffer[calcOffset + 1] *
                                     filterMatrix[filterY + filterOffset,
                                         filterX + filterOffset];


                            red += pixelBuffer[calcOffset + 2] *
                                   filterMatrix[filterY + filterOffset,
                                       filterX + filterOffset];
                        }


                    blue = factor * blue + bias;
                    green = factor * green + bias;
                    red = factor * red + bias;


                    blue = blue > 255 ? 255 :
                        blue < 0 ? 0 :
                        blue;


                    green = green > 255 ? 255 :
                        green < 0 ? 0 :
                        green;


                    red = red > 255 ? 255 :
                        red < 0 ? 0 :
                        red;


                    resultBuffer[byteOffset] = (byte)blue;
                    resultBuffer[byteOffset + 1] = (byte)green;
                    resultBuffer[byteOffset + 2] = (byte)red;
                    resultBuffer[byteOffset + 3] = 255;
                }


            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            var resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                    resultBitmap.Width, resultBitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }
    }
}