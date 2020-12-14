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

            var seriesNames = SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var defaultChaptersList = Util.GetDefaultChaptersList();

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
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(SeriesCB, segments, seriesNames, orderedSegments[0]);
            Util.SetComboBox(TitleCB, segments, defaultChaptersList, orderedSegments[1]);
            Util.SetComboBox(DescriptionCB, segments, new List<string>(), orderedSegments[5]);

            SeriesCB_LostFocus(null, null);
            AuthorCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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

        private void AuthorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AuthorCB.IsKeyboardFocusWithin) {
                return;
            }

            var authorName = AuthorCB.Text;
        }
    }
}
