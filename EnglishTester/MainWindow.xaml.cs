using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using EnglishTester.Scripts;

namespace EnglishTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static int TestC;
        public static bool is_autoSkip;
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
            if (String.IsNullOrEmpty(TestCount.Text) || Convert.ToInt32(TestCount.Text) >= Loader.Sentences.Count || Convert.ToInt32(TestCount.Text) <= 0)
            {
                Error_Is_Too_High.Text = $"테스트할 문항이 너무 많거나 읽어올수 없습니다. (최대 : {Loader.Sentences.Count}개)";
                TestCount.Text = "";
                TestCount.Foreground = Brushes.Red;
            } else
            {
                TestC = Convert.ToInt32(TestCount.Text);
                is_autoSkip = autoSkip.IsChecked == true;
                testPage.Navigate(new Uri("Test.xaml", UriKind.Relative));
            }
           
        }
    }
}