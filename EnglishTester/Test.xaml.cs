using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using EnglishTester.Scripts;

namespace EnglishTester
{
    public partial class Test : Page
    {
        private static int _now = 1;
        public static int Skipped;
        public static int Corrected;
        private static string _a = "";
        public Test()
        {
            InitializeComponent();

            // Init
            Problem.Text = $"{_now}/{MainWindow.TestC}";

            var random = new Random();
            var randomIndex = random.Next(MainWindow.SelectedSentences.Count);

            var currentSentence = MainWindow.SelectedSentences[randomIndex];
            var answer = new Regex(@"\[(.*?)\]").Matches(currentSentence.Item1);
            _a = answer[0].Value.Replace("[", "").Replace("]", "");

            var answerBlind = "";
            if (MainWindow.IsToMean)
            {
                foreach (var t in Loader.Verbs.Where(t => _a.Contains(t.Item1)))
                {
                    answerBlind = t.Item2;
                    break;
                }
            }
            else
            {
                for (var i = 0; i < answer[0].Value.Length - 2; i++)
                {
                    answerBlind += "_";
                }
            }

            eng.Text = currentSentence.Item1.Replace(answer[0].Value, answerBlind);
            kor.Text = currentSentence.Item2;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (input.Text == _a)
            {
                is_true.Foreground = Brushes.Green;
                is_true.Text = "정답입니다!";
                Corrected++;
                NewS();
            }
            else
            {
                is_true.Foreground = Brushes.Red;
                is_true.Text = "오답입니다!";
                if (MainWindow.IsAutoSkip)
                {
                    is_true.Text = $"오답입니다! | 정답 : {_a}";
                    Skipped++;
                    NewS();
                }
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Skipped++;
            NewS();
        }

        private void NewS()
        {
            if (_now != MainWindow.TestC)
            {
                _now++;
                Problem.Text = $"{_now}/{MainWindow.TestC} ··· Skipped : {Skipped}";

                input.Text = "";
                var random = new Random();
                var randomIndex = random.Next(MainWindow.SelectedSentences.Count);

                var currentSentence = MainWindow.SelectedSentences[randomIndex];
                var answer = new Regex(@"\[(.+)\]").Matches(currentSentence.Item1);

                var answerBlind = "";
                for (var i = 0; i < answer[0].Value.Length - 2; i++)
                {
                    answerBlind += "_";
                }

                eng.Text = currentSentence.Item1.Replace(answer[0].Value, answerBlind);
                kor.Text = currentSentence.Item2;
                _a = answer[0].Value.Replace("[", "").Replace("]", "");
            }
            else
            {
                move.Navigate(new Uri("Finished.xaml", UriKind.Relative));
            }
        }
    }
}
