using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace CartoonStyle
{
    public partial class Form1 : Form
    {
        private Bitmap _inputBitmap;
        private Bitmap _outputBitmap;
        private readonly List<SmoothingFilterType> _types;
        private SmoothingFilterType _selectedSmoothingFilterType = SmoothingFilterType.None;
        private byte threshold = byte.MinValue;

        public Form1()
        {
            InitializeComponent();

            _types = Enum.GetValues(typeof(SmoothingFilterType)).Cast<SmoothingFilterType>().ToList();
            comboBox1.DataSource = _types;

            trackBar1.Maximum = byte.MaxValue;
            trackBar1.Minimum = byte.MinValue;

            textBox1.Text = this.threshold.ToString();
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            _inputBitmap = new Bitmap(Image.FromFile(openFileDialog1.FileName));
            pictureBox1.Size = new Size(_inputBitmap.Width, _inputBitmap.Height);
            pictureBox1.Image = _inputBitmap;

            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void saveImage_click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _outputBitmap.Save(dialog.FileName, ImageFormat.Png);
            }
        }
        

        private void cartoonizeImage_Click(object sender, EventArgs e)
        {
            _outputBitmap = _inputBitmap.CartoonEffectFilter(threshold, _selectedSmoothingFilterType);
            pictureBox1.Image = _outputBitmap;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _inputBitmap?.Dispose();
            _outputBitmap?.Dispose();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedSmoothingFilterType = _types.ElementAt(comboBox1.SelectedIndex);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.threshold = (byte)(trackBar1.Value);
            textBox1.Text = this.threshold.ToString();
        }

        private void resetToInputImage(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(_inputBitmap.Width, _inputBitmap.Height);
            pictureBox1.Image = _inputBitmap;
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (byte.TryParse(textBox1.Text, out byte newThreshhold))
            {
                this.threshold = newThreshhold;
                trackBar1.Value = newThreshhold;
            }
        }
    }
}