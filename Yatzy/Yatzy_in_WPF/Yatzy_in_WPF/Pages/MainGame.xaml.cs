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
            GameManager.InitializeGame(3); // Initialize game with 2 players
            Players = new ObservableCollection<YatzyLogic.Player>(GameManager.Players);
            DataContext = this;
            ExampleArray = new int[] { 10, 20, 30 }; // Example array for frontend loop
        }

        public ObservableCollection<YatzyLogic.Player> Players { get; set; }
        public int[] ExampleArray { get; set; }
    }
}