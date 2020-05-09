using System.Windows;

namespace DomL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClassificarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var classificarWindow = new ClassificarWindow();
            classificarWindow.Show();
            this.Close();
        }

        private void ConsolidarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var consolidarWindow = new ConsolidarWindow();
            consolidarWindow.Show();
            this.Close();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
