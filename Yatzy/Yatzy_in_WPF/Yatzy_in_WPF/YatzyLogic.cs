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

            public ObservableCollection<int> ScoreCard { get; set; }
            public ObservableCollection<bool> IsCategoryScored { get; set; }


            public Player(string name)
            {
                Name = name;
                ScoreCard = new ObservableCollection<int>(new int[15]);
                IsCategoryScored = new ObservableCollection<bool>(new bool[15]);

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
                int upperSectionScore = player.ScoreCard[0] + player.ScoreCard[1] + player.ScoreCard[2] + player.ScoreCard[3] + player.ScoreCard[4] + player.ScoreCard[5];
                if (upperSectionScore >= BonusThreshold && player.Bonus == 0)
                {
                    player.Bonus = BonusPoints;
                }
            }

            public static int CalculateScore(int category)
            {
                int score = 0;
                switch (category)
                {
                    case 0:
                        score = CalculateNumberScore(1);
                        break;
                    case 1:
                        score = CalculateNumberScore(2);
                        break;
                    case 2:
                        score = CalculateNumberScore(3);
                        break;
                    case 3:
                        score = CalculateNumberScore(4);
                        break;
                    case 4:
                        score = CalculateNumberScore(5);
                        break;
                    case 5:
                        score = CalculateNumberScore(6);
                        break;
                    case 6:
                        score = CalculateOnePair();
                        break;
                    case 7:
                        score = CalculateTwoPairs();
                        break;
                    case 8:
                        score = CalculateThreeOfAKind();
                        break;
                    case 9:
                        score = CalculateFourOfAKind();
                        break;
                    case 10:
                        score = CalculateFullHouse();
                        break;
                    case 11:
                        score = CalculateSmallStraight();
                        break;
                    case 12:
                        score = CalculateLargeStraight();
                        break;
                    case 13:
                        score = CalculateChance();
                        break;
                    case 14:
                        score = CalculateYatzy();
                        break;
                }
                return score;
            }

            private static int CalculateNumberScore(int number)
            {
                int score = 0;
                foreach (int dice in diceValues)
                {
                    if (dice == number)
                    {
                        score += number;
                    }
                }
                return score;
            }

            private static int CalculateOnePair()
            {
                int[] counts = new int[7];

                foreach (int dice in diceValues)
                {
                    counts[dice]++;
                }

                for (int i = 6; i >= 1; i--)
                {
                    if (counts[i] >= 2)
                    {
                        return i * 2;
                    }
                }
                return 0;
            }


            private static int CalculateTwoPairs()
            {
                int pairsFound = 0;
                int score = 0;
                for (int i = 6; i >= 1; i--)
                {
                    int count = 0;
                    foreach (int dice in diceValues)
                    {
                        if (dice == i)
                        {
                            count++;
                        }
                    }
                    if (count >= 2)
                    {
                        score += i * 2;
                        pairsFound++;
                    }
                    if (pairsFound == 2)
                    {
                        break;
                    }
                }
                return pairsFound == 2 ? score : 0;
            }

            private static int CalculateThreeOfAKind()
            {
                for (int i = 6; i >= 1; i--)
                {
                    int count = 0;
                    foreach (int dice in diceValues)
                    {
                        if (dice == i)
                        {
                            count++;
                        }
                    }
                    if (count >= 3)
                    {
                        return i * 3;
                    }
                }
                return 0;
            }

            private static int CalculateFourOfAKind()
            {
                for (int i = 1; i <= 6; i++)
                {
                    int count = 0;
                    foreach (int dice in diceValues)
                    {
                        if (dice == i)
                        {
                            count++;
                        }
                    }
                    if (count >= 4)
                    {
                        return i * 4;
                    }
                }
                return 0;
            }

            private static int CalculateFullHouse()
            {
                int threeOfAKindValue = 0;
                int pairValue = 0;
                for (int i = 6; i >= 1; i--)
                {
                    int count = 0;
                    foreach (int dice in diceValues)
                    {
                        if (dice == i)
                        {
                            count++;
                        }
                    }
                    if (count == 3)
                    {
                        threeOfAKindValue = i * 3;
                    }
                    else if (count == 2)
                    {
                        pairValue = i * 2;
                    }
                }
                return (threeOfAKindValue > 0 && pairValue > 0) ? (threeOfAKindValue + pairValue) : 0;
            }

            private static int CalculateSmallStraight()
            {
                int[] smallStraight = { 1, 2, 3, 4, 5 };
                foreach (int value in smallStraight)
                {
                    if (!diceValues.Contains(value))
                    {
                        return 0;
                    }
                }
                return 15;
            }

            private static int CalculateLargeStraight()
            {
                int[] largeStraight = { 2, 3, 4, 5, 6 };
                foreach (int value in largeStraight)
                {
                    if (!diceValues.Contains(value))
                    {
                        return 0;
                    }
                }
                return 20;
            }

            private static int CalculateYatzy()
            {
                int firstValue = diceValues[0];
                foreach (int dice in diceValues)
                {
                    if (dice != firstValue)
                    {
                        return 0;
                    }
                }
                return 50;
            }

            private static int CalculateChance()
            {
                int score = 0;
                foreach (int dice in diceValues)
                {
                    score += dice;
                }
                return score;
            }
        }
    }
}