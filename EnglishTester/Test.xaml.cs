using EnglishTester.Scripts;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnglishTester
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Page
    {
        private static int now = 1;
        public static int skipped = 0;
        public static int currected = 0;
        private static string a = "";
        private static readonly List<Tuple<string, string>> s = new();
        public Test()
        {
            InitializeComponent();

            // Init
            Problem.Text = $"{now}/{MainWindow.TestC}";

            var random = new Random();
            var randomIndex = random.Next(Loader.Sentences.Count);

            var currectSentence = Loader.Sentences[randomIndex];
            var answer = new Regex(@"\[(.+)\]").Matches(currectSentence.Item1);

            var answerBlind = "";
            for (var i = 0; i < answer[0].Value.Length - 2; i++)
            {
                answerBlind += "_";
            }

            eng.Text = currectSentence.Item1.Replace(answer[0].Value, answerBlind);
            kor.Text = currectSentence.Item2;
            a = answer[0].Value.Replace("[", "").Replace("]", "");
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (input.Text == a)
            {
                is_true.Foreground = Brushes.Green;
                is_true.Text = "정답입니다!";
                currected++;
                newS();
            }
            else
            {
                is_true.Foreground = Brushes.Red;
                is_true.Text = "오답입니다!";
                if (MainWindow.is_autoSkip)
                {
                    skipped++;
                    newS();
                }
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            skipped++;
            newS();
        }

        private void newS()
        {
            if (now != MainWindow.TestC)
            {
                now++;
                Problem.Text = $"{now}/{MainWindow.TestC} ··· Skipped : {skipped}";

                input.Text = "";
                var random = new Random();
                var randomIndex = random.Next(Loader.Sentences.Count);

                var currectSentence = Loader.Sentences[randomIndex];
                var answer = new Regex(@"\[(.+)\]").Matches(currectSentence.Item1);

                var answerBlind = "";
                for (var i = 0; i < answer[0].Value.Length - 2; i++)
                {
                    answerBlind += "_";
                }

                eng.Text = currectSentence.Item1.Replace(answer[0].Value, answerBlind);
                kor.Text = currectSentence.Item2;
                a = answer[0].Value.Replace("[", "").Replace("]", "");
            }
            else
            {
                move.Navigate(new Uri("Finished.xaml", UriKind.Relative));
            }
        }
    }
}
