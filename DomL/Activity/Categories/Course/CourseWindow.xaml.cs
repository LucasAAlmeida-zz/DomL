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

            var names = CourseService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var personNames = PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var companyNames = CompanyService.GetAll(unitOfWork).Select(u => u.Name).ToList();
            var scoreValues = ScoreService.GetAll(unitOfWork).Select(u => u.Value.ToString()).ToList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[5];

            var indexesToAvoid = new int[] { 3 };

            // COURSE; Name; (School Name); (Teacher Name); (Score); (Description)
            while (remainingSegments.Length > 1 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[1];

                if (names.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 0, searched, indexesToAvoid);
                } else if (companyNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 1, searched, indexesToAvoid);
                } else if (personNames.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 2, searched, indexesToAvoid);
                } else if (scoreValues.Contains(searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, 3, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[1]).ToArray();
            }

            Util.SetComboBox(this.NameCB, segments, names, orderedSegments[0]);
            Util.SetComboBox(this.SchoolCB, segments, companyNames, orderedSegments[1]);
            Util.SetComboBox(this.TeacherCB, segments, personNames, orderedSegments[2]);
            Util.SetComboBox(this.ScoreCB, new string[1] { "" }, scoreValues, orderedSegments[3]);
            Util.SetComboBox(this.DescriptionCB, segments, new List<string>(), orderedSegments[4]);

            this.NameCB_LostFocus(null, null);
            this.TeacherCB_LostFocus(null, null);
            this.SchoolCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.NameCB.Text) || string.IsNullOrWhiteSpace(this.SchoolCB.Text)) {
                return;
            }
            this.DialogResult = true;
        }

        private void NameCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.NameCB.IsKeyboardFocusWithin) {
                return;
            }

            var name = this.NameCB.Text;
            var course = CourseService.GetCourseByName(name, this.UnitOfWork);
            Util.ChangeInfoLabel(name, course, this.NameInfoLb);
        }

        private void TeacherCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TeacherCB.IsKeyboardFocusWithin) {
                return;
            }

            var teacherName = this.TeacherCB.Text;
            var teacher = PersonService.GetByName(teacherName, this.UnitOfWork);
            Util.ChangeInfoLabel(teacherName, teacher, this.TeacherInfoLb);
        }

        private void SchoolCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.SchoolCB.IsKeyboardFocusWithin) {
                return;
            }

            var schoolName = this.SchoolCB.Text;
            var school = CompanyService.GetByName(schoolName, this.UnitOfWork);
            Util.ChangeInfoLabel(schoolName, school, this.SchoolInfoLb);
        }
    }
}
