using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WinForms
{
    public partial class Form1 : Form
    {
        private Bitmap bm;
        private Bitmap bmOut;
        private int w, h;
        private int matrix; //размер на блока от пиксели
        private string text; //текст използван за screening
        private char[] sym; //представяне на текса като масив от символи
        private Graphics gr; //графичен обект, необходим за изчертаване на символи 
        private Font font; //използван шрифт при изписването на символите от текста

        public Form1()
        {
            InitializeComponent();

            matrix = 10; //размер на блока от пиксели по подразбиране
            font = new Font("Arial", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            sym = new char[1]; //текст по подразбиране, състоящ се от 1 символ
            sym[0] = 'v'; //символ по подразбиране

        }

        private void GetSize()
        {

            if (textBox2.Text != "")
            {
                try
                {
                    matrix = int.Parse(textBox2.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    matrix = 10;
                }
            }
            else
            {
                MessageBox.Show("Please, enter size of the square");
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            font?.Dispose();
            this.Close();
        }

        //връщане към оригиналното изображение
        private void originalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = bm;
        }

        //метод за извличане на текста, въведен от потребителя в полето с етикет Text for Screening, използван за screening with text
        private void GetText()
        {
            if (textBox1.Text != "")
            {
                text = textBox1.Text;
                text = text.ToUpper(); //преобразуване на всички букви от текста в големи
                text = text.Replace(" ", ""); //премахване на празните интервали в текста
                sym = text.ToCharArray(); //преобразуване на текста в масив от символи
            }
            else
            {
                MessageBox.Show("Please, enter a short text");
            }
        }

        //метод за указване на номера на поредния символ в масива от символи
        private int Counter(int n)
        {
            if (n < sym.Length - 1)
            {
                n++;
            }
            else
            {
                n = 0;
            }
            return n;
        }
        //реализация на алгоритъма за screening with text
        private void screeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int c;
            int n;

            GetText();
            GetSize();
            if (sym != null)
            {
                bmOut = new Bitmap(w, h);
                gr = Graphics.FromImage(bmOut);
                for (int j = 0; j < h; j += matrix)
                {
                    n = 0;
                    for (int i = 0; i < w; i += matrix)
                    {
                        c = bm.GetPixel(i, j).R;
                        if (c >= 0 && c < 63)
                        {
                            gr.DrawString(sym[n].ToString(), font, Brushes.Black, i, j);
                            n = Counter(n);
                        }
                        else
                        {
                            if (c >= 63 && c < 127)
                            {
                                using (Font f = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel))
                                {
                                    gr.DrawString(sym[n].ToString(), f, Brushes.Black, i + matrix / 2 - 2, j + matrix / 2 + 2);
                                    n = Counter(n);
                                }
                            }
                            else
                            {
                                if (c >= 127 && c < 191)
                                {
                                    using (Font f = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                                    {

                                        gr.DrawString(sym[n].ToString(), f, Brushes.Black, i + matrix / 2 - 2, j + matrix / 2 + 2);
                                        n = Counter(n);
                                    }
                                }
                                else
                                {
                                    using (Font f = new Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Pixel))
                                    {
                                        gr.DrawString(sym[n].ToString(), f, Brushes.Black, i + matrix / 2 - 2, j + matrix / 2 + 2);
                                        n = Counter(n);
                                    }
                                }
                            }
                        }
                    }
                }
                pictureBox1.Image = bmOut;
            }
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bm = new Bitmap(Image.FromFile(openFileDialog1.FileName));
                w = bm.Width / matrix * matrix;
                h = bm.Height / matrix * matrix;
                pictureBox1.Size = new Size(w, h);
                pictureBox1.Image = bm;
            }
        }
    }
}
