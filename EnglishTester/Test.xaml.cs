using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnglishTester
{
    public partial class Test : Page
    {
        private static int _now = 1;
        public static int skipped = 0;
        public static int corrected = 0;
        private static string _a = "";
        private static readonly List<Tuple<string, string>> s = new();
        public Test()
        {
            InitializeComponent();

            // Init
            Problem.Text = $"{_now}/{MainWindow.TestC}";

            var random = new Random();
            var randomIndex = random.Next(MainWindow.SelectedSentences.Count);

            var currentSentence = MainWindow.SelectedSentences[randomIndex];
            var answer = new Regex(@"\[(.*?)\]").Matches(currentSentence.Item1);

            var answerBlind = "";
            Debug.WriteLine(answer[0].Value);
            for (var i = 0; i < answer[0].Value.Length - 2; i++)
            {
                answerBlind += "_";
            }

            eng.Text = currentSentence.Item1.Replace(answer[0].Value, answerBlind);
            kor.Text = currentSentence.Item2;
            _a = answer[0].Value.Replace("[", "").Replace("]", "");
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (input.Text == _a)
            {
                is_true.Foreground = Brushes.Green;
                is_true.Text = "정답입니다!";
                corrected++;
                NewS();
            }
            else
            {
                is_true.Foreground = Brushes.Red;
                is_true.Text = "오답입니다!";
                if (MainWindow.IsAutoSkip)
                {
                    is_true.Text = $"오답입니다! | 정답 : {_a}";
                    skipped++;
                    NewS();
                }
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            skipped++;
            NewS();
        }

        private void NewS()
        {
            if (_now != MainWindow.TestC)
            {
                _now++;
                Problem.Text = $"{_now}/{MainWindow.TestC} ··· Skipped : {skipped}";

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
