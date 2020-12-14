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

            UnitOfWork = unitOfWork;

            InfoMessage.Content =
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

                SegmentosStack.Children.Add(dynLabel);
            }

            var titles = BookService.GetAll(unitOfWork).Select(u => u.Title).ToList();
            var numbers = Util.GetDefaultNumberList();
            var seriesNames = SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList();

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
            Util.SetComboBox(SeriesCB, segments, seriesNames, orderedSegments[2]);
            Util.SetComboBox(NumberCB, segments, numbers, orderedSegments[3]);
            Util.SetComboBox(DescriptionCB, segments, new List<string>(), orderedSegments[5]);

            TitleCB_LostFocus(null, null);
            AuthorCB_LostFocus(null, null);
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
            var book = BookService.GetByTitle(title, UnitOfWork);
            Util.ChangeInfoLabel(title, book, TitleInfoLb);

            UpdateOptionalComboBoxes(book);
        }

        private void AuthorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AuthorCB.IsKeyboardFocusWithin) {
                return;
            }

            var authorName = AuthorCB.Text;
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

        private void UpdateOptionalComboBoxes(Book book)
        {
            if (book == null) {
                return;
            }

            if (book.Author != null) {
                AuthorCB.Text = book.Author;
                AuthorCB_LostFocus(null, null);
            }

            if (book.Series != null) {
                SeriesCB.Text = book.Series.Name;
                SeriesCB_LostFocus(null, null);
            }

            NumberCB.Text = book.Number ?? NumberCB.Text;
            ScoreCB.Text = book.Score ?? ScoreCB.Text;
        }
    }
}
