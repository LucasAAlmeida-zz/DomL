using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for MovieWindow.xaml
    /// </summary>
    public partial class MovieWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public MovieWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            UnitOfWork = unitOfWork;

            InfoMessage.Content =
                "Date:\t\t" + activity.Date.ToString("dd/MM/yyyy") + "\n" +
                "Category:\t" + activity.Category.Name + "\n" +
                "Status:\t\t" + activity.Status.Name;

            for (int index = 1; index < segments.Length; index++) {
                var segmento = segments[index];
                var dynLabel = new TextBox {
                    Text = segmento,
                    IsReadOnly = true,
                    Margin = new Thickness(5)
                };

                SegmentosStack.Children.Add(dynLabel);
            }

            var titles = BookService.GetAll(unitOfWork).Select(u => u.Title).ToList();
            var seriesNames = SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var numbers = Util.GetDefaultNumberList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[6];

            var indexesToAvoid = new int[] { 4 };

            // MOVIE; Title; (Director Name); (Series Name); (Number In Series); (Score); (Description)
            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];
                if (int.TryParse(searched, out int number)) {
                    searched = number.ToString("00");
                }

                if (titles.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else if (seriesNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 2, searched, indexesToAvoid);
                } else if (numbers.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 3, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(TitleCB, segments, titles, orderedSegments[0]);
            Util.SetComboBox(SeriesCB, segments, seriesNames, orderedSegments[1]);
            Util.SetComboBox(NumberCB, segments, numbers, orderedSegments[2]);
            Util.SetComboBox(DescriptionCB, segments, new List<string>(), orderedSegments[5]);

            TitleCB_LostFocus(null, null);
            DirectorCB_LostFocus(null, null);
            SeriesCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = TitleCB.Text;
            var movie = MovieService.GetByTitle(title, UnitOfWork);
            Util.ChangeInfoLabel(title, movie, TitleInfoLb);

            UpdateOptionalComboBoxes(movie);
        }

        private void DirectorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DirectorCB.IsKeyboardFocusWithin) {
                return;
            }

            var directorName = DirectorCB.Text;
        }

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SeriesCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, UnitOfWork);
            Util.ChangeInfoLabel(seriesName, series, SeriesInfoLb);
        }

        private void UpdateOptionalComboBoxes(Movie movie)
        {
            if (movie == null) {
                return;
            }

            if (movie.Person != null) {
                DirectorCB.Text = movie.Person;
                DirectorCB_LostFocus(null, null);
            }

            if (movie.Series != null) {
                SeriesCB.Text = movie.Series.Name;
                SeriesCB_LostFocus(null, null);
            }

            NumberCB.Text = movie.Number ?? NumberCB.Text;
            ScoreCB.Text = movie.Score ?? ScoreCB.Text;
        }
    }
}
