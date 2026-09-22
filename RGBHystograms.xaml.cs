using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace color_converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RGBHystograms : Window
    {

        Bitmap originalBitmap;

        int[] histR;
        int[] histB;
        int[] histG;

        public RGBHystograms()
        {
            InitializeComponent();
        }

        private void ButtonLoad_Click(object seneder, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    originalBitmap = new Bitmap(openFileDialog.FileName);

                    OriginalImage.Source = convertToBitmapSource(originalBitmap);

                    buttonProcess.IsEnabled = true;

                } catch (Exception ex)
                {

                    MessageBox.Show("Ошибка загрузки изображения " + ex.Message);
                }
            }
        }

        private void ButtonProcess_Click(object sender, RoutedEventArgs e) {

            if (originalBitmap == null) {

                return;
            }

            int width = originalBitmap.Width;
            int height = originalBitmap.Height;

            Bitmap bitmapR = new Bitmap(width, height);
            Bitmap bitmapG = new Bitmap(width, height);
            Bitmap bitmapB = new Bitmap(width, height);

            histR = new int[256];
            histB = new int[256];
            histG = new int[256];

            for (int h = 0; h < height; ++h) {
                for (int w = 0; w < width; ++w) {

                    System.Drawing.Color pixel = originalBitmap.GetPixel(w, h);

                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;

                    bitmapR.SetPixel(w, h, System.Drawing.Color.FromArgb(r, 0, 0));
                    bitmapG.SetPixel(w, h, System.Drawing.Color.FromArgb(0, g, 0));
                    bitmapB.SetPixel(w, h, System.Drawing.Color.FromArgb(0, 0, b));

                    ++histR[r];
                    ++histG[g];
                    ++histB[b];

                }
            }

            imageRed.Source = convertToBitmapSource(bitmapR);
            imageGreen.Source = convertToBitmapSource(bitmapG);
            imageBlue.Source = convertToBitmapSource(bitmapB);


            drawHystogram(canvasHistR, histR, System.Windows.Media.Brushes.Red);
            drawHystogram(canvasHistG, histG, System.Windows.Media.Brushes.Green);
            drawHystogram(canvasHistB, histB, System.Windows.Media.Brushes.Blue);
        }

        private void drawHystogram(Canvas canvas, int[] hystogram, System.Windows.Media.Brush color) {
            canvas.Children.Clear();

            double w = canvas.ActualWidth;
            double h = canvas.ActualHeight;

            if (w == 0 || h == 0) {

                return;
            }

            int maxVal = 0;
            for (int i = 0; i < 256; ++i) {

                if (hystogram[i] > maxVal) {
                    maxVal = hystogram[i];
                }
            }

            if (maxVal == 0) {
                maxVal = 1;
            }

            double barWidth = w / 256.0;

            for (int i = 0; i < 256; ++i) {

                double barHeight = ((double)hystogram[i] / maxVal) * h;

                Line line = new Line
                {
                    X1 = i * barWidth,
                    Y1 = h,
                    X2 = i * barWidth,
                    Y2 = h - barHeight,
                    Stroke = color,
                    StrokeThickness = barWidth
                };

                canvas.Children.Add(line);

            }

            
        }

        private BitmapSource convertToBitmapSource(Bitmap bitmap) {

            IntPtr ptrBitmap = bitmap.GetHbitmap();

            try {
                BitmapSource res = Imaging.CreateBitmapSourceFromHBitmap(ptrBitmap,
                                    IntPtr.Zero,
                                    Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());


                res.Freeze();

                return res;
            
            } finally {
                DeleteObject(ptrBitmap);
            }

            
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);



        private void ButtonBack_Click(object sender, RoutedEventArgs e) {

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();
            this.Close();
        }
    }
}
