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
    /// Interaction logic for CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public CourseWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
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

            var names = CourseService.GetAll(unitOfWork).Select(u => u.Title).ToList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[5];

            var indexesToAvoid = new int[] { 3 };

            // COURSE; Name; (School Name); (Teacher Name); (Score); (Description)
            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];

                if (names.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(TitleCB, segments, names, orderedSegments[0]);
            Util.SetComboBox(DescriptionCB, segments, new List<string>(), orderedSegments[4]);

            NameCB_LostFocus(null, null);
            TeacherCB_LostFocus(null, null);
            SchoolCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleCB.Text) || string.IsNullOrWhiteSpace(SchoolCB.Text)) {
                return;
            }
            DialogResult = true;
        }

        private void NameCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var name = TitleCB.Text;
            var course = CourseService.GetByName(name, UnitOfWork);
            Util.ChangeInfoLabel(name, course, NameInfoLb);
        }

        private void TeacherCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ProfessorCB.IsKeyboardFocusWithin) {
                return;
            }

            var teacherName = ProfessorCB.Text;
        }

        private void SchoolCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SchoolCB.IsKeyboardFocusWithin) {
                return;
            }

            var schoolName = SchoolCB.Text;
        }
    }
}
