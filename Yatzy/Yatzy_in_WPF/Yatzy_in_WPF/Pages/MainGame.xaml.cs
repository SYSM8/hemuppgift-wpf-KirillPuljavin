using System.Collections.ObjectModel;
using System.Windows.Controls;
using static Yatzy_in_WPF.YatzyLogic;

namespace Yatzy_in_WPF.Pages
{
    public partial class MainGame : Page
    {
        public MainGame()
        {
            InitializeComponent();
            GameManager.InitializeGame(2); // Initialize game with 2 players
            Players = new ObservableCollection<YatzyLogic.Player>
            {
                new("Test Player 1") { TotalScore = 100, GrandTotal = 150 },
                new("Test Player 2") { TotalScore = 120, GrandTotal = 150 }
            };
            DataContext = this;

            foreach (Player player in Players)
            {
                player.ScoreCard = new int[15];
            }
        }

        public ObservableCollection<YatzyLogic.Player> Players { get; set; }
    }
}