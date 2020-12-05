using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Size = System.Windows.Size;

namespace Screening
{
    public class ScreeningAlgorithm : IDisposable
    {
        private Bitmap _originalBitmap;
        private Bitmap _outputBitmap;
        private int _width, _height, _matrixSize;
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

        public BitmapSource OriginalBitmap => BitmapConverter.LoadBitmap(_originalBitmap);
        public BitmapSource OutputBitmapSource => BitmapConverter.LoadBitmap(_outputBitmap);
        
        public (Size, BitmapSource) LoadImage(string filename)
        {
            _originalBitmap = new Bitmap(Image.FromFile(filename));
            _width = _originalBitmap.Width / _matrixSize * _matrixSize;
            _height = _originalBitmap.Height / _matrixSize * _matrixSize;

            return (new System.Windows.Size(_width, _height), BitmapConverter.LoadBitmap(_originalBitmap));
        }

        public BitmapSource Screening(string textBox, string size)
        {
            GetText(textBox);
            GetSize(size);
            if (_characters == null) return null;

            _outputBitmap = new Bitmap(_width, _height);
            _graphics = Graphics.FromImage(_outputBitmap);

            for (var j = 0; j < _height; j += _matrixSize)
            {
                var n = 0;
                for (var i = 0; i < _width; i += _matrixSize)
                {
                    int c = _originalBitmap.GetPixel(i, j).R;
                    if (c >= 0 && c < 63)
                    {
                        _graphics.DrawString(_characters[n].ToString(), _font, Brushes.Black, i, j);
                        n = Counter(n);
                    }
                    else if (c >= 63 && c < 127)
                    {
                        n = Draw(n, i, j, 16);
                    }
                    else if (c >= 127 && c < 191)
                    {
                        n = Draw(n, i, j, 12);
                    }
                    else
                    {
                        n = Draw(n, i, j, 8);
                    }
                }
            }

            return BitmapConverter.LoadBitmap(_outputBitmap);
        }

        private int Draw(int n, int i, int j, float fontSize)
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
            _originalBitmap?.Dispose();
            _outputBitmap?.Dispose();
            _graphics?.Dispose();
            _font?.Dispose();
        }
    }
}