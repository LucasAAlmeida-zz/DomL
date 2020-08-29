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
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public BookWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            this.UnitOfWork = unitOfWork;

            this.InfoMessage.Content =
                "Date:\t\t" + activity.Date.ToString("dd/MM/yyyy") + "\n" +
                "Category:\t" + activity.Category.Name + "\n" +
                "Status:\t\t" + activity.Status.Name;

            for (int index2 = 1; index2 < segments.Length; index2++) {
                var segmento = segments[index2];
                var dynLabel = new TextBox {
                    Text = segmento,
                    IsReadOnly = true,
                    Margin = new Thickness(5)
                };

                this.SegmentosStack.Children.Add(dynLabel);
            }

            var titles = BookService.GetAll(unitOfWork).Select(u => u.Title).ToList();
            var numbers = Util.GetDefaultNumberList();
            var personNames = PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var seriesNames = SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var scoreValues = ScoreService.GetAll(unitOfWork).Select(u => u.Value.ToString()).ToList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[6];

            var indexesToAvoid = new int[] { 4 };

            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];
                if (int.TryParse(searched, out int number)) {
                    searched = number.ToString("00");
                }

                if (titles.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else if (personNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 1, searched, indexesToAvoid);
                } else if (seriesNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 2, searched, indexesToAvoid);
                } else if (numbers.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 3, searched, indexesToAvoid);
                } else if (scoreValues.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 4, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(this.TitleCB, segments, titles, orderedSegments[0]);
            Util.SetComboBox(this.AuthorCB, segments, personNames, orderedSegments[1]);
            Util.SetComboBox(this.SeriesCB, segments, seriesNames, orderedSegments[2]);
            Util.SetComboBox(this.NumberCB, segments, numbers, orderedSegments[3]);
            Util.SetComboBox(this.ScoreCB, new string[1] { "" }, scoreValues, orderedSegments[4]);
            Util.SetComboBox(this.DescriptionCB, segments, new List<string>(), orderedSegments[5]);

            this.TitleCB_LostFocus(null, null);
            this.AuthorCB_LostFocus(null, null);
            this.SeriesCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = this.TitleCB.Text;
            var book = BookService.GetByTitle(title, this.UnitOfWork);
            Util.ChangeInfoLabel(title, book, this.TitleInfoLb);

            UpdateOptionalComboBoxes(book);
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

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.SeriesCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = this.SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);
            Util.ChangeInfoLabel(seriesName, series, this.SeriesInfoLb);
        }

        private void UpdateOptionalComboBoxes(Book book)
        {
            if (book == null) {
                return;
            }

            if (book.Author != null) {
                this.AuthorCB.Text = book.Author.Name;
                this.AuthorCB_LostFocus(null, null);
            }

            if (book.Series != null) {
                this.SeriesCB.Text = book.Series.Name;
                this.SeriesCB_LostFocus(null, null);
            }

            this.NumberCB.Text = book.NumberInSeries ?? this.NumberCB.Text;
            this.ScoreCB.SelectedItem = book.Score ?? this.ScoreCB.SelectedItem;
        }
    }
}
