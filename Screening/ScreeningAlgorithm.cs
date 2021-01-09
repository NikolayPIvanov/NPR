using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Size = System.Windows.Size;

namespace Screening
{
    public class ScreeningAlgorithm : Algorithm, IDisposable
    {
        private int _matrixSize, _fontSize;
        private string _text;
        private char[] _characters;
        private Graphics _graphics;
        private readonly Font _font;

        public ScreeningAlgorithm()
        {
            _matrixSize = 10;
            _font = new Font("Arial", 18, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            _characters = new char[1];
            _characters[0] = 'v';
        }
        
        public override (Size, BitmapSource) LoadImage(string filename)
        {
            originalBitmap = new Bitmap(Image.FromFile(filename));
            width = originalBitmap.Width / _matrixSize * _matrixSize;
            height = originalBitmap.Height / _matrixSize * _matrixSize;

            return (new System.Windows.Size(width, height), BitmapConverter.LoadBitmap(originalBitmap));
        }
        
        public BitmapSource Screening(string textBox, string size, string fontSize)
        {
            GetText(textBox);
            GetSize(size);
            GetFontSize(fontSize);

            if (_characters == null) return null;

            originalBitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(originalBitmap);

            for (var rowIndex = 0; rowIndex < height; rowIndex += _matrixSize)
            {
                var n = 0;
                for (var columnIndex = 0; columnIndex < width; columnIndex += _matrixSize)
                {
                    var pixel = originalBitmap.GetPixel(columnIndex, rowIndex);
                    if (pixel.R >= 0 && pixel.R < 63)
                    {
                        n = Draw(n, columnIndex, rowIndex, pixel, _fontSize);
                    }
                    else if (pixel.R >= 63 && pixel.R < 127)
                    {
                        n = Draw(n, columnIndex, rowIndex, pixel, _fontSize - 4);
                    }
                    else if (pixel.R >= 127 && pixel.R < 191)
                    {
                        n = Draw(n, columnIndex, rowIndex, pixel, _fontSize - 8);
                    }
                    else
                    {
                        n = Draw(n, columnIndex, rowIndex, pixel, _fontSize - 12);
                    }
                }
            }

            return BitmapConverter.LoadBitmap(outputBitmap);
        }

        private int Draw(int n, int i, int j, Color pixel, float fontSize)
        {
            using var f = new Font("Arial", fontSize, System.Drawing.FontStyle.Regular,
                GraphicsUnit.Pixel);
            _graphics.DrawString(_characters[n].ToString(), f, Brushes.Black, i + _matrixSize / 2 - 2, j + _matrixSize / 2 + 2);
            return Counter(n);
        }

        private void GetSize(string value)
        {
            try
            {
                _matrixSize = int.Parse(value);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                _matrixSize = 10;
            }
        }

        private void GetFontSize(string value)
        {
            try
            {
                _fontSize = int.Parse(value);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                _fontSize = 20;
            }
        }
        private void GetText(string value)
        {
            if (value != "")
            {
                _text = value;
                _text = _text.ToUpper();
                _text = _text.Replace(" ", "");
                _characters = _text.ToCharArray();
            }
            else
            {
                MessageBox.Show("Please, enter a short text");
            }
        }
        private int Counter(int n)
        {
            if (n < _characters.Length - 1)
            {
                n++;
            }
            else
            {
                n = 0;
            }
            return n;
        }

        public void Dispose()
        {
            originalBitmap?.Dispose();
            outputBitmap?.Dispose();
            _graphics?.Dispose();
            _font?.Dispose();
        }
    }
}