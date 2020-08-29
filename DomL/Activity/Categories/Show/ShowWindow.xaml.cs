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
    /// Interaction logic for ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public ShowWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
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

            var seriesNames = SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var defaultSeasonsList = Util.GetDefaultSeasonsList();
            var showTypes = MediaTypeService.GetAllShowTypes(unitOfWork).Select(u => u.Name).ToList();
            var personNames = PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var publisherNames = CompanyService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var scoreValues = ScoreService.GetAll(unitOfWork).Select(u => u.Value.ToString()).ToList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[6];

            var indexesToAvoid = new int[] { 2, 4 };

            // SHOW; Series Name; Season; (Media Type Name); (Director Name); (Score); (Description)
            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];

                if (seriesNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else if ((defaultSeasonsList.Contains(searched) || searched.Contains("~")) && orderedSegments[1] == null) {
                    Util.PlaceOrderedSegment(orderedSegments, 1, searched, indexesToAvoid);
                } else if (showTypes.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 2, searched, indexesToAvoid);
                } else if (personNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 3, searched, indexesToAvoid);
                } else if (scoreValues.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 4, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(this.SeriesCB, segments, seriesNames, orderedSegments[0]);
            Util.SetComboBox(this.SeasonCB, segments, defaultSeasonsList, orderedSegments[1]);
            Util.SetComboBox(this.TypeCB, new string[1] { "" }, showTypes, orderedSegments[2]);
            Util.SetComboBox(this.DirectorCB, segments, personNames, orderedSegments[3]);
            Util.SetComboBox(this.ScoreCB, new string[1] { "" }, scoreValues, orderedSegments[4]);
            Util.SetComboBox(this.DescriptionCB, segments, new List<string>(), orderedSegments[5]);

            this.SeriesCB_LostFocus(null, null);
            this.DirectorCB_LostFocus(null, null);
            this.TypeCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.SeriesCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = this.SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);
            Util.ChangeInfoLabel(seriesName, series, this.SeriesInfoLb);

            UpdateOptionalComboBoxes(seriesName, this.SeasonCB.Text);
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

        private void TypeCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TypeCB.IsKeyboardFocusWithin) {
                return;
            }

            var typeName = this.TypeCB.Text;
            var mediaType = MediaTypeService.GetByName(typeName, this.UnitOfWork);
            Util.ChangeInfoLabel(typeName, mediaType, this.TypeInfoLb);
        }

        private void UpdateOptionalComboBoxes(string seriesName, string season)
        {
            if (string.IsNullOrWhiteSpace(seriesName) || string.IsNullOrWhiteSpace(season)) {
                return;
            }

            var showSeason = ShowService.GetShowSeasonBySeriesNameAndSeason(seriesName, season, this.UnitOfWork);

            if (showSeason == null) {
                return;
            }

            if (showSeason.Director != null) {
                this.DirectorCB.Text = showSeason.Director.Name;
                this.DirectorCB_LostFocus(null, null);
            }

            if (showSeason.Type != null) {
                this.TypeCB.Text = showSeason.Type.Name;
                this.TypeCB_LostFocus(null, null);
            }

            //this.ScoreCB.Text = showSeason.Score ?? this.ScoreCB.Text;
        }
    }
}
