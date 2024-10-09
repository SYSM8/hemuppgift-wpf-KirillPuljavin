using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using static Yatzy_in_WPF.YatzyLogic;

namespace Yatzy_in_WPF.Pages
{
    public partial class MainGame : Page
    {
        public ObservableCollection<DiceState> DiceStates { get; set; } = [];
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
                if (!DiceStates[i].Fixed)
                {
                    int newDice = random.Next(1, 7);
                    YatzyLogic.GameManager.diceValues[i] = newDice;

                    double angle = random.NextDouble() * 90;
                    double offsetX = random.Next(-20, 20);

                    DiceStates[i].ImagePath = $"pack://application:,,,/imgs/dice{newDice}.png";
                    DiceStates[i].Angle = angle;
                    DiceStates[i].OffsetX = offsetX;
                }
            }
        }
        private void ToggleFixed(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is DiceState diceState)
                diceState.Fixed = !diceState.Fixed;
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ScoreGridView == null) return;
            double totalWidth = e.NewSize.Width;
            int columnsCount = ScoreGridView.Columns.Count;

            if (columnsCount > 1)
            {
                double otherColumnWidth = totalWidth / (columnsCount + 1);  // +1 for doubling the Player Name column
                ScoreGridView.Columns[0].Width = otherColumnWidth * 2;
                for (int i = 1; i < columnsCount; i++)
                    ScoreGridView.Columns[i].Width = otherColumnWidth;
            }
        }



        private void ScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tagValue && int.TryParse(tagValue, out int category))
            {
                if (Players[CurrentPlayerIndex].IsCategoryScored[category] == false)
                {
                    int score = GameManager.CalculateScore(category);
                    Players[CurrentPlayerIndex].IsCategoryScored[category] = true;
                    if (category < 6)
                    {
                        GameManager.CalculateBonus(Players[CurrentPlayerIndex]);
                    }
                    Players[CurrentPlayerIndex].ScoreCard[category] = score;
                    Players[CurrentPlayerIndex].TotalScore += score;
                    Players[CurrentPlayerIndex].GrandTotal = Players[CurrentPlayerIndex].TotalScore + Players[CurrentPlayerIndex].Bonus;

                    MessageBox.Show($"Category: {category} | Player: {Players[CurrentPlayerIndex].Name} scored {score}");
                    GameManager.EndTurn();
                }
            }

        }
    }

    public class DiceState : INotifyPropertyChanged
    {
        private string _imagePath;
        private double _angle;
        private double _offsetX;
        private bool _fixed;

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
        public bool Fixed
        {
            get => _fixed;
            set
            {
                _fixed = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}