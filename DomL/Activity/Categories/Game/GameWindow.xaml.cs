using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;
using System;
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

        enum NamedIndices
        {
            title = 0,
            type = 1,
            series = 2,
            number = 3,
            person = 4,
            company = 5,
            year = 6,
            score = 7,
            description = 8
        }

        public GameWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            UnitOfWork = unitOfWork;

            InfoMessage.Content = activity.GetInfoMessage();
            Util.FillSegmentosStack(segments, SegmentosStack);

            var games = GameService.GetAll(unitOfWork);

            var titleList = games.Select(u => u.Title).Distinct().ToList();
            var typeList = games.Where(u => u.Type != null).Select(u => u.Type).Distinct().ToList();
            var seriesList = games.Where(u => u.Series != null).Select(u => u.Series.Name).Distinct().ToList();
            var numberList = Util.GetDefaultNumberList();
            var personList = games.Where(u => u.Person != null).Select(u => u.Person).Distinct().ToList();
            var companyList = games.Where(u => u.Company != null).Select(u => u.Company).Distinct().ToList();
            var yearList = Util.GetDefaultYearList();
            var scoreList = Util.GetDefaultScoreList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[Enum.GetValues(typeof(NamedIndices)).Length];

            var indexesToAvoid = new int[] { (int)NamedIndices.year };
            Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.title, remainingSegments[1], indexesToAvoid);
            Util.SetComboBox(TitleCB, segments, titleList, orderedSegments[(int)NamedIndices.title]);
            TitleCB_LostFocus(null, null);

            // GAME; Title; Type; Series; Number; Person; Company; Year; Score; Description
            while (remainingSegments.Length > 2 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[2];
                if (int.TryParse(searched, out int number)) {
                    searched = number.ToString("00");
                }

                if (Util.ListContainsText(typeList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.type, searched, indexesToAvoid);
                } else if (Util.ListContainsText(seriesList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.series, searched, indexesToAvoid);
                } else if (Util.ListContainsText(numberList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.number, searched, indexesToAvoid);
                } else if (Util.ListContainsText(personList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.person, searched, indexesToAvoid);
                } else if (Util.ListContainsText(companyList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.company, searched, indexesToAvoid);
                } else if (Util.ListContainsText(yearList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.year, searched, indexesToAvoid);
                } else if (Util.ListContainsText(scoreList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.score, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[2]).ToArray();
            }

            Util.SetComboBox(TypeCB, segments, typeList, orderedSegments[(int)NamedIndices.type]);
            Util.SetComboBox(SeriesCB, segments, seriesList, orderedSegments[(int)NamedIndices.series]);
            Util.SetComboBox(NumberCB, segments, numberList, orderedSegments[(int)NamedIndices.number]);
            Util.SetComboBox(PersonCB, segments, personList, orderedSegments[(int)NamedIndices.person]);
            Util.SetComboBox(CompanyCB, segments, companyList, orderedSegments[(int)NamedIndices.company]);
            Util.SetComboBox(YearCB, segments, yearList, orderedSegments[(int)NamedIndices.year]);
            Util.SetComboBox(ScoreCB, segments, scoreList, orderedSegments[(int)NamedIndices.score]);
            Util.SetComboBox(DescriptionCB, segments, new List<string>(), orderedSegments[(int)NamedIndices.description]);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleCB.Text) || string.IsNullOrWhiteSpace(TypeCB.Text)) {
                return;
            }
            DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = TitleCB.Text;
            var game = GameService.GetByTitle(title, UnitOfWork);
            Util.ChangeInfoLabel(title, game, TitleInfoLb);

            if (game != null) {
                UpdateOptionalComboBoxes(game);
            }
        }

        private void UpdateOptionalComboBoxes(Game game)
        {
            if (game.Type != null) {
                TypeCB.Text = game.Type;
            }

            if (game.Series != null) {
                SeriesCB.Text = game.Series.Name;
            }

            if (game.Number != null) {
                NumberCB.Text = game.Number;
            }

            if (game.Person != null) {
                PersonCB.Text = game.Person;
            }

            if (game.Company != null) {
                CompanyCB.Text = game.Company;
            }

            if (game.Year != 0) {
                YearCB.Text = game.Year.ToString();
            }

            if (game.Score != null) {
                ScoreCB.Text = game.Score;
            }
        }
    }
}
