using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;


namespace Screening
{
    public partial class MainWindow : Window
    {
        private readonly ScreeningAlgorithm _screening;

        public MainWindow()
        {
            InitializeComponent();

            _screening = new ScreeningAlgorithm();
        }

        private void screeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessedImage.Source = _screening.Screening(Text.Text, MatrixSizeSlider.Value.ToString(CultureInfo.InvariantCulture));
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            var (size, source) = _screening.LoadImage(openFileDialog.FileName);
            ProcessedImage.RenderSize = size;
            ProcessedImage.Source = source;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _screening?.Dispose();
            this.Close();
        }

        private void originalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessedImage.Source = _screening.OriginalBitmap;
        }

        private void MatrixSizeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CurrentMatrixSize ??= new Label();
            CurrentMatrixSize.Content = e.NewValue;
        }
    }
}