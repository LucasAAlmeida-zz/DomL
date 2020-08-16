using DomL.Business.Services;
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
using System.Windows.Shapes;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for RestoreFullWindow.xaml
    /// </summary>
    public partial class RestoreFullWindow : Window
    {
        public RestoreFullWindow()
        {
            InitializeComponent();
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreBooksFromFile();
            } catch (Exception exception) {
                
            }
        }

        private void ComicButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreComicsFromFile();
            } catch (Exception exception) {

            }
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreGamesFromFile();
            } catch (Exception exception) {

            }
        }

        private void SeriesButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreSeriesFromFile();
            } catch (Exception exception) {

            }
        }

        private void WatchButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreWatchsFromFile();
            } catch (Exception exception) {

            }
        }
    }
}
