using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Yatzy_in_WPF
{
    public static class YatzyLogic
    {
        public static int _currentPlayerIndex;
        public static int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            set
            {
                _currentPlayerIndex = value;
                OnStaticPropertyChanged(nameof(CurrentPlayerIndex));
            }
        }

        public static event PropertyChangedEventHandler PropertyChanged;
        private static void OnStaticPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public class Player : INotifyPropertyChanged
        {
            private string _name;
            private int _totalScore;
            private int _totalBonus;
            private int _grandTotal;
            private int _bonus;

            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public int TotalScore
            {
                get => _totalScore;
                set
                {
                    _totalScore = value;
                    OnPropertyChanged();
                }
            }

            public int TotalBonus
            {
                get => _totalBonus;
                set
                {
                    _totalBonus = value;
                    OnPropertyChanged();
                }
            }

            public int GrandTotal
            {
                get => _grandTotal;
                set
                {
                    _grandTotal = value;
                    OnPropertyChanged();
                }
            }

            public int Bonus
            {
                get => _bonus;
                set
                {
                    _bonus = value;
                    OnPropertyChanged();
                }
            }

            public int[] ScoreCard { get; set; }

            public Player(string name)
            {
                Name = name;
                ScoreCard = new int[15];
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static class GameManager
        {
            public static ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
            public static int[] diceValues = new int[5];
            public const int WinningScore = 100;
            public const int BonusThreshold = 63;
            public const int BonusPoints = 50;

            public static void InitializeGame(int numberOfPlayers)
            {
                Players.Clear();
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Players.Add(new Player($"Player {i + 1}"));
                }
            }

            public static void EndTurn()
            {
                // Check if any player has won
                if (Players[CurrentPlayerIndex].GrandTotal >= WinningScore)
                {
                    System.Windows.MessageBox.Show($"Game Over! {Players[CurrentPlayerIndex].Name} wins!");
                    return;
                }

                // Move to the next player
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
                System.Windows.MessageBox.Show($"It's {Players[CurrentPlayerIndex].Name}'s turn!");
            }

            public static void CalculateBonus(Player player)
            {
                int upperSectionScore = player.ScoreCard.Take(6).Sum();
                if (upperSectionScore >= BonusThreshold && player.Bonus == 0)
                {
                    player.Bonus = BonusPoints;
                    player.TotalBonus = player.Bonus;
                }
            }

            public static int CalculateScore(int category)
            {
                int score = 0;
                switch (category)
                {
                    case 1:
                        score = CalculateNumberScore(1);
                        break;
                    case 2:
                        score = CalculateNumberScore(2);
                        break;
                    case 3:
                        score = CalculateNumberScore(3);
                        break;
                    case 4:
                        score = CalculateNumberScore(4);
                        break;
                    case 5:
                        score = CalculateNumberScore(5);
                        break;
                    case 6:
                        score = CalculateNumberScore(6);
                        break;
                    case 7:
                        score = CalculateOnePair();
                        break;
                    case 8:
                        score = CalculateTwoPairs();
                        break;
                    case 9:
                        score = CalculateThreeOfAKind();
                        break;
                    case 10:
                        score = CalculateFourOfAKind();
                        break;
                    case 11:
                        score = CalculateSmallStraight();
                        break;
                    case 12:
                        score = CalculateLargeStraight();
                        break;
                    case 13:
                        score = CalculateFullHouse();
                        break;
                    case 14:
                        score = CalculateChance();
                        break;
                    case 15:
                        score = CalculateYatzy();
                        break;
                }
                return score;
            }

            private static int CalculateNumberScore(int number)
            {
                return diceValues.Where(d => d == number).Sum();
            }

            private static int CalculateOnePair()
            {
                var pair = diceValues.GroupBy(d => d).Where(g => g.Count() >= 2).OrderByDescending(g => g.Key).FirstOrDefault();
                return pair != null ? pair.Key * 2 : 0;
            }

            private static int CalculateTwoPairs()
            {
                var pairs = diceValues.GroupBy(d => d).Where(g => g.Count() >= 2).OrderByDescending(g => g.Key).Take(2).ToList();
                return pairs.Count == 2 ? pairs.Sum(p => p.Key * 2) : 0;
            }

            private static int CalculateThreeOfAKind()
            {
                var threeOfAKind = diceValues.GroupBy(d => d).FirstOrDefault(g => g.Count() >= 3);
                return threeOfAKind != null ? threeOfAKind.Key * 3 : 0;
            }

            private static int CalculateFourOfAKind()
            {
                var fourOfAKind = diceValues.GroupBy(d => d).FirstOrDefault(g => g.Count() >= 4);
                return fourOfAKind != null ? fourOfAKind.Key * 4 : 0;
            }

            private static int CalculateFullHouse()
            {
                var groups = diceValues.GroupBy(d => d).OrderByDescending(g => g.Count()).ToList();
                return groups.Count == 2 && (groups[0].Count() == 3 || groups[1].Count() == 3) ? diceValues.Sum() : 0;
            }

            private static int CalculateSmallStraight()
            {
                int[] smallStraight = { 1, 2, 3, 4, 5 };
                return diceValues.OrderBy(d => d).SequenceEqual(smallStraight) ? 15 : 0;
            }

            private static int CalculateLargeStraight()
            {
                int[] largeStraight = { 2, 3, 4, 5, 6 };
                return diceValues.OrderBy(d => d).SequenceEqual(largeStraight) ? 20 : 0;
            }

            private static int CalculateYatzy()
            {
                return diceValues.Distinct().Count() == 1 ? 50 : 0;
            }

            private static int CalculateChance()
            {
                return diceValues.Sum();
            }
        }
    }
}
