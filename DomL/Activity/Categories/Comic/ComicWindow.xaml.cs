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
    /// Interaction logic for ComicWindow.xaml
    /// </summary>
    public partial class ComicWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public ComicWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
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
            var defaultChaptersList = Util.GetDefaultChaptersList();
            var personNames = PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var comicTypes = MediaTypeService.GetAllComicTypes(unitOfWork).Select(u => u.Name).ToList();
            var scoreValues = ScoreService.GetAll(unitOfWork).Select(u => u.Value.ToString()).ToList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[6];

            var indexesToAvoid = new int[] { 3, 4 };

            // COMIC; Series Name; Chapters; (Author Name); (Media Type Name); (Score); (Description)
            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];

                if (seriesNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else if ((defaultChaptersList.Contains(searched) || searched.Contains("~")) && orderedSegments[1] == null) {
                    Util.PlaceOrderedSegment(orderedSegments, 1, searched, indexesToAvoid);
                } else if (personNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 2, searched, indexesToAvoid);
                } else if (comicTypes.Contains(searched) ) {
                    Util.PlaceOrderedSegment(orderedSegments, 3, searched, indexesToAvoid);
                } else if (scoreValues.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 4, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(this.SeriesCB, segments, seriesNames, orderedSegments[0]);
            Util.SetComboBox(this.ChaptersCB, segments, defaultChaptersList, orderedSegments[1]);
            Util.SetComboBox(this.AuthorCB, segments, personNames, orderedSegments[2]);
            Util.SetComboBox(this.TypeCB, new string[1] { "" }, comicTypes, orderedSegments[3]);
            Util.SetComboBox(this.ScoreCB, new string[1] { "" }, scoreValues, orderedSegments[4]);
            Util.SetComboBox(this.DescriptionCB, segments, new List<string>(), orderedSegments[5]);

            this.SeriesCB_LostFocus(null, null);
            this.AuthorCB_LostFocus(null, null);
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

            UpdateOptionalComboBoxes(seriesName, this.ChaptersCB.Text);
        }

        private void AuthorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.AuthorCB.IsKeyboardFocusWithin) {
                return;
            }

            var authorName = this.AuthorCB.Text;
            var author = PersonService.GetByName(authorName, this.UnitOfWork);
            Util.ChangeInfoLabel(authorName, author, this.AuthorInfoLb);
        }

        private void UpdateOptionalComboBoxes(string seriesName, string chapters)
        {
            if (string.IsNullOrWhiteSpace(seriesName) || string.IsNullOrWhiteSpace(chapters)) {
                return;
            }

            var comicVolume = ComicService.GetComicVolumeBySeriesNameAndChapters(seriesName, chapters, this.UnitOfWork);

            if (comicVolume == null) {
                return;
            }

            if (comicVolume.Author != null) {
                this.AuthorCB.Text = comicVolume.Author.Name;
                this.AuthorCB_LostFocus(null, null);
            }

            this.TypeCB.Text = comicVolume.Type != null ? comicVolume.Type.Name.ToString() : this.TypeCB.Text;
            this.ScoreCB.Text = comicVolume.Score != null ? comicVolume.Score.Value.ToString() : this.ScoreCB.Text;
        }
    }
}
