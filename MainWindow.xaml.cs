using System.Windows;

namespace color_converter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonTask1_Click(object sender, RoutedEventArgs e)
        {
            // Task1Window task1 = new Task1Window();
            // task1.Show();
            // this.Close();
            MessageBox.Show("Задание 1 в процессе");
        }

        private void ButtonTask2_Click(object sender, RoutedEventArgs e)
        {
            RGBHystograms task2 = new RGBHystograms();
            task2.Show();
            this.Close();
        }

        private void ButtonTask3_Click(object sender, RoutedEventArgs e)
        {
            // Task3Window task3 = new Task3Window();
            // task3.Show();
            // this.Close();
            MessageBox.Show("Задание 3 в процессе");
        }
    }
}