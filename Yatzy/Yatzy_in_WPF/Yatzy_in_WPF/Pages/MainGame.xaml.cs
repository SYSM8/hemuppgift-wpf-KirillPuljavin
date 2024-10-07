using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using static Yatzy_in_WPF.YatzyLogic;

namespace Yatzy_in_WPF.Pages
{
    public partial class MainGame : Page
    {
        public MainGame()
        {
            InitializeComponent();
            GameManager.InitializeGame(3); // Initialize game with 2 players
            Players = new ObservableCollection<YatzyLogic.Player>(GameManager.Players);
            DataContext = this;
        }

        public ObservableCollection<YatzyLogic.Player> Players { get; set; }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ScoreGridView == null) return;
            double totalWidth = e.NewSize.Width;

            // Adjust column widths based on percentage.
            // For example, let's set each column to a percentage of the total width.
            double playerNameWidth = totalWidth * 0.15;  // 15% for Player Name column
            double doublesWidth = totalWidth * 0.1;      // 10% for Doubles column
            double threeOfAKindWidth = totalWidth * 0.1; // 10% for Three of a Kind column
            double totalScoreWidth = totalWidth * 0.1;   // 10% for Total Score column

            // Set the calculated widths.
            ScoreGridView.Columns[0].Width = playerNameWidth;
            ScoreGridView.Columns[1].Width = doublesWidth;
            ScoreGridView.Columns[2].Width = threeOfAKindWidth;
            ScoreGridView.Columns[^1].Width = totalScoreWidth;

            // Distribute remaining space for other columns.
            double remainingWidth = totalWidth - (playerNameWidth + doublesWidth + threeOfAKindWidth + totalScoreWidth);
            int otherColumnsCount = ScoreGridView.Columns.Count - 4;

            if (otherColumnsCount > 0)
            {
                double otherColumnWidth = remainingWidth / otherColumnsCount;
                for (int i = 3; i < ScoreGridView.Columns.Count - 1; i++)
                {
                    ScoreGridView.Columns[i].Width = otherColumnWidth;
                }
            }
        }
    }
}