using System;
using System.Collections.Generic;
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
        private static readonly List<Tuple<string, string, string, string>> Wrong = new();
        private static string _a = "";
        private static string _n = "";
        private static string _m = "";
        public Test()
        {
            InitializeComponent();

            // Init
            Problem.Text = $"{_now}/{MainWindow.TestC}";

            var random = new Random();
            var randomIndex = random.Next(MainWindow.SelectedSentences.Count);

            var currentSentence = MainWindow.SelectedSentences[randomIndex];
            MainWindow.SelectedSentences.RemoveAt(randomIndex);
            var answer = new Regex(@"\[(.*?)\]").Matches(currentSentence.Item1);
            _a = answer[0].Value.Replace("[", "").Replace("]", "");
            _n = currentSentence.Item1;
            
            if (MainWindow.IsToMean)
            {
                foreach (var t in Loader.Verbs.Where(t => _a.Contains(t.Item1)))
                {
                    _m = t.Item2;
                }
            }
            else
            {
                _m = "";
                for (var i = 0; i < _n.Length; i++)
                {
                    _m += "_";
                }
            }

            eng.Text = currentSentence.Item1.Replace(answer[0].Value, _m);
            kor.Text = currentSentence.Item2;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.Equals(input.Text, _a, StringComparison.CurrentCultureIgnoreCase))
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
                    Wrong.Add(new Tuple<string, string, string, string>(_n, _a, _m, input.Text));
                    Skipped++;
                    NewS();
                }
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Wrong.Add(new Tuple<string, string, string, string>(_n, _a, _m, "Null(Skipped)"));
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
                MainWindow.SelectedSentences.RemoveAt(randomIndex);
                var answer = new Regex(@"\[(.*?)\]").Matches(currentSentence.Item1);
                _a = answer[0].Value.Replace("[", "").Replace("]", "");
                _n = currentSentence.Item1;
                
                if (MainWindow.IsToMean)
                {
                    foreach (var t in Loader.Verbs.Where(t => _a.Contains(t.Item1)))
                    {
                        _m = t.Item2;
                    }
                }
                else
                {
                    _m = "";
                    for (var i = 0; i < _n.Length - 2; i++)
                    {
                        _m += "_";
                    }
                }

                eng.Text = currentSentence.Item1.Replace(answer[0].Value, _m);
                kor.Text = currentSentence.Item2;
            }
            else
            {
                var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                System.IO.File.WriteAllLines($"{Environment.CurrentDirectory}\\wrong-{unixTimestamp}.txt", Wrong.Select(a => $"문제 : \"{a.Item1}\" | 정답 : \"{a.Item2}\" | 뜻 : \"{a.Item3}\" | 입력한 값 : \"{a.Item4}\""));
                move.Navigate(new Uri("Finished.xaml", UriKind.Relative));
            }
        }
    }
}
