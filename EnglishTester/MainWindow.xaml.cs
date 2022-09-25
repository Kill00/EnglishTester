using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using EnglishTester.Scripts;

namespace EnglishTester
{
public partial class MainWindow
    {
        public static int TestC;
        public static bool IsAutoSkip;
        public static List<Tuple<string, string>> SelectedSentences = new ();

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = System.Windows.ResizeMode.NoResize;

            // Loaders Setup
            Loader.Setup();
        }
        
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Debug.WriteLine(e.Text);
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Start_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Init
            SelectedSentences = new List<Tuple<string, string>>();
            if (L2.IsChecked == true)
            {
                for (var i = Loader.Classify[0]; i <= Loader.Classify[1]; i++)
                {
                    SelectedSentences.Add(new Tuple<string,string>(Loader.Sentences[i].Item1, Loader.Sentences[i].Item2));
                }
            }
            if (L3.IsChecked == true)
            {
                for (var i = Loader.Classify[2]; i <= Loader.Classify[3]; i++)
                {
                    SelectedSentences.Add(new Tuple<string,string>(Loader.Sentences[i].Item1, Loader.Sentences[i].Item2));
                }
                Debug.WriteLine(SelectedSentences[^1].Item1);
            }
            if (MockExam.IsChecked == true)
            {
                for (var i = Loader.Classify[4]; i <= Loader.Sentences.Count - 1; i++)
                {
                    SelectedSentences.Add(new Tuple<string,string>(Loader.Sentences[i].Item1, Loader.Sentences[i].Item2));
                }
            }
            if (L2.IsChecked == false && L3.IsChecked == false && MockExam.IsChecked == false)
            {
                SelectedSentences = Loader.Sentences;
            }
            // Init
            
            Debug.WriteLine(SelectedSentences.Count);
            if (string.IsNullOrEmpty(TestCount.Text) || Convert.ToInt32(TestCount.Text) >= SelectedSentences.Count || Convert.ToInt32(TestCount.Text) <= 0)
            {
                ErrorIsTooHigh.Text = $"테스트할 문항이 너무 많거나 읽어올수 없습니다. (최대 : {SelectedSentences.Count}개)";
                TestCount.Text = "";
            } else
            {
                TestC = Convert.ToInt32(TestCount.Text);
                IsAutoSkip = AutoSkip.IsChecked == true;
                TestPage.Navigate(new Uri("Test.xaml", UriKind.Relative));
            }
           
        }
    }
}