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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public GameWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            this.UnitOfWork = unitOfWork;

            this.InfoMessage.Content =
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

                this.SegmentosStack.Children.Add(dynLabel);
            }

            var titles = GameService.GetAll(unitOfWork).Select(u => u.Title).ToList();
            var platformTypes = MediaTypeService.GetAllPlatformTypes(unitOfWork).Select(u => u.Name).ToList();
            var seriesNames = SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var numbers = Util.GetDefaultNumberList();
            var personNames = PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var companyNames = CompanyService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var scoreValues = ScoreService.GetAll(unitOfWork).Select(u => u.Value.ToString()).ToList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[8];

            var indexesToAvoid = new int[] { 1, 6 };

            // GAME; Title; Platform Name; (Series Name); (Number In Series); (Director Name); (Publisher Name); (Score); (Description)
            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];
                if (int.TryParse(searched, out int number)) {
                    searched = number.ToString("00");
                }

                if (titles.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else if (platformTypes.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 1, searched, indexesToAvoid);
                } else if (seriesNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 2, searched, indexesToAvoid);
                } else if (numbers.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 3, searched, indexesToAvoid);
                } else if (personNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 4, searched, indexesToAvoid);
                } else if (companyNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 5, searched, indexesToAvoid);
                } else if (scoreValues.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 6, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(this.TitleCB, segments, titles, orderedSegments[0]);
            Util.SetComboBox(this.PlatformCB, new string[1] { "" }, platformTypes, orderedSegments[1]);
            Util.SetComboBox(this.SeriesCB, segments, seriesNames, orderedSegments[2]);
            Util.SetComboBox(this.NumberCB, segments, numbers, orderedSegments[3]);
            Util.SetComboBox(this.DirectorCB, segments, personNames, orderedSegments[4]);
            Util.SetComboBox(this.PublisherCB, segments, companyNames, orderedSegments[5]);
            Util.SetComboBox(this.ScoreCB, new string[1] { "" }, scoreValues, orderedSegments[6]);
            Util.SetComboBox(this.DescriptionCB, segments, new List<string>(), orderedSegments[7]);

            this.TitleCB_LostFocus(null, null);
            this.SeriesCB_LostFocus(null, null);
            this.DirectorCB_LostFocus(null, null);
            this.PublisherCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TitleCB.Text) || string.IsNullOrWhiteSpace(this.PlatformCB.Text)) {
                return;
            }
            this.DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = this.TitleCB.Text;
            var game = GameService.GetGameByTitleAndPlatformName(title, this.PlatformCB.Text, this.UnitOfWork);
            Util.ChangeInfoLabel(title, game, this.TitleInfoLb);

            UpdateOptionalComboBoxes(title);
        }

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.SeriesCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = this.SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);
            Util.ChangeInfoLabel(seriesName, series, this.SeriesInfoLb);
        }

        private void DirectorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DirectorCB.IsKeyboardFocusWithin) {
                return;
            }

            var directorName = this.DirectorCB.Text;
            var director = PersonService.GetByName(directorName, this.UnitOfWork);
            Util.ChangeInfoLabel(directorName, director, this.DirectorInfoLb);
        }

        private void PublisherCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.PublisherCB.IsKeyboardFocusWithin) {
                return;
            }

            var publisherName = this.PublisherCB.Text;
            var publisher = CompanyService.GetByName(publisherName, this.UnitOfWork);
            Util.ChangeInfoLabel(publisherName, publisher, this.PublisherInfoLb);
        }

        private void UpdateOptionalComboBoxes(string title)
        {
            var platform = this.PlatformCB != null ? (string)this.PlatformCB.SelectedItem : null;
            if (string.IsNullOrWhiteSpace(title) || platform == null) {
                return;
            }

            var game = GameService.GetGameByTitleAndPlatformName(title, platform, this.UnitOfWork);

            if (game == null) {
                return;
            }

            if (game.Series != null) {
                this.SeriesCB.Text = game.Series.Name;
                this.SeriesCB_LostFocus(null, null);
            }

            if (game.Director != null) {
                this.DirectorCB.Text = game.Director.Name;
                this.DirectorCB_LostFocus(null, null);
            }

            if (game.Publisher != null) {
                this.PublisherCB.Text = game.Publisher.Name;
                this.PublisherCB_LostFocus(null, null);
            }

            this.NumberCB.Text = game.NumberInSeries ?? this.NumberCB.Text;
            this.ScoreCB.Text = game.Score != null ? game.Score.Value.ToString() : this.ScoreCB.Text;
        }

    }
}
