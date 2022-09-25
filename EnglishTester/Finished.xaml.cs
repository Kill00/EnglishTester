using System.Windows.Controls;

namespace EnglishTester
{

    public partial class Finished : Page
    {
        public Finished()
        {
            InitializeComponent();

            result.Text = $"정답 개수 : {Test.Corrected} | 스킵 개수 : {Test.Skipped}";
        }
    }
}
