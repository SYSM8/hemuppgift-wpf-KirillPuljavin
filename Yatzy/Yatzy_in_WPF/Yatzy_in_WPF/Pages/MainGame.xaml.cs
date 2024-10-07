using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using static Yatzy_in_WPF.YatzyLogic;

namespace Yatzy_in_WPF.Pages
{
    public partial class MainGame : Page
    {
        public ObservableCollection<DiceState> DiceStates { get; set; } = new ObservableCollection<DiceState>();
        public ObservableCollection<YatzyLogic.Player> Players { get; set; }

        public MainGame()
        {
            InitializeComponent();
            InitializeDiceStates();
            DataContext = this;

            GameManager.InitializeGame(3); // Initialize game with 2 players
            Players = new ObservableCollection<YatzyLogic.Player>(GameManager.Players);
        }
        private void InitializeDiceStates()
        {
            for (int i = 0; i < 5; i++)
            {
                DiceStates.Add(new DiceState
                {
                    ImagePath = "pack://application:,,,/imgs/dice1.png", // Default to dice face 1
                    Angle = 0,
                    OffsetX = 0
                });
            }
        }

        private readonly Random random = new Random();
        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            // Update each dice with a random image, rotation, and offset
            for (int i = 0; i < DiceStates.Count; i++)
            {
                int newDice = random.Next(1, 7);
                YatzyLogic.GameManager.diceValues[i] = newDice;

                double angle = random.NextDouble() * 90;
                double offsetX = random.Next(-10, 11);

                DiceStates[i].ImagePath = $"pack://application:,,,/imgs/dice{newDice}.png";
                DiceStates[i].Angle = angle;
                DiceStates[i].OffsetX = offsetX;
            }
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ScoreGridView == null) return;
            double totalWidth = e.NewSize.Width;
            double playerNameWidth = totalWidth * 0.15;  // 15% for Player Name column
            double doublesWidth = totalWidth * 0.1;      // 10% for Doubles column
            double threeOfAKindWidth = totalWidth * 0.1; // 10% for Three of a Kind column
            double totalScoreWidth = totalWidth * 0.1;   // 10% for Total Score column

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

    public class DiceState : System.ComponentModel.INotifyPropertyChanged
    {
        private string _imagePath;
        private double _angle;
        private double _offsetX;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                OnPropertyChanged();
            }
        }

        public double OffsetX
        {
            get => _offsetX;
            set
            {
                _offsetX = value;
                OnPropertyChanged();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        // Helper method to trigger PropertyChanged for all properties (to refresh bindings).
        public void Refresh()
        {
            OnPropertyChanged(nameof(ImagePath));
            OnPropertyChanged(nameof(Angle));
            OnPropertyChanged(nameof(OffsetX));
        }
    }
}