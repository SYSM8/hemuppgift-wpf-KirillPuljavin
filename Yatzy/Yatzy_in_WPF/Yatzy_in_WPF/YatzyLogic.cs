using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Yatzy_in_WPF
{
    public static class YatzyLogic
    {
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
            public static List<Player> Players { get; set; } = new List<Player>();
            public static int[] diceValues = new int[5];
            public static int currentPlayerIndex = 0;
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

            public static void OnScoreButtonClick(int[] dice, int category)
            {
                int score = CalculateScore(dice, category);
                if (Players[currentPlayerIndex].ScoreCard[category - 1] == 0)
                {
                    Players[currentPlayerIndex].ScoreCard[category - 1] = score;
                    Players[currentPlayerIndex].TotalScore += score;
                }
                CalculateBonus(Players[currentPlayerIndex]);
                Players[currentPlayerIndex].GrandTotal = Players[currentPlayerIndex].TotalScore + Players[currentPlayerIndex].TotalBonus;
                EndTurn();
            }

            public static void EndTurn()
            {
                if (Players[currentPlayerIndex].GrandTotal >= WinningScore)
                {
                    System.Windows.MessageBox.Show($"Game Over! {Players[currentPlayerIndex].Name} wins!");
                    return;
                }

                currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
                System.Windows.MessageBox.Show($"It's {Players[currentPlayerIndex].Name}'s turn!");
            }

            private static void CalculateBonus(Player player)
            {
                int upperSectionScore = player.ScoreCard.Take(6).Sum();
                if (upperSectionScore >= BonusThreshold && player.Bonus == 0)
                {
                    player.Bonus = BonusPoints;
                    player.TotalBonus = player.Bonus;
                }
            }
        }

        public static int CalculateScore(int[] dice, int category)
        {
            int score = 0;
            switch (category)
            {
                case 1:
                    score = CalculateNumberScore(dice, 1);
                    break;
                case 2:
                    score = CalculateNumberScore(dice, 2);
                    break;
                case 3:
                    score = CalculateNumberScore(dice, 3);
                    break;
                case 4:
                    score = CalculateNumberScore(dice, 4);
                    break;
                case 5:
                    score = CalculateNumberScore(dice, 5);
                    break;
                case 6:
                    score = CalculateNumberScore(dice, 6);
                    break;
                case 7:
                    score = CalculateOnePair(dice);
                    break;
                case 8:
                    score = CalculateTwoPairs(dice);
                    break;
                case 9:
                    score = CalculateThreeOfAKind(dice);
                    break;
                case 10:
                    score = CalculateFourOfAKind(dice);
                    break;
                case 11:
                    score = CalculateSmallStraight(dice);
                    break;
                case 12:
                    score = CalculateLargeStraight(dice);
                    break;
                case 13:
                    score = CalculateFullHouse(dice);
                    break;
                case 14:
                    score = CalculateChance(dice);
                    break;
                case 15:
                    score = CalculateYatzy(dice);
                    break;
            }
            return score;
        }

        private static int CalculateYatzy(int[] dice)
        {
            return dice.Distinct().Count() == 1 ? 50 : 0;
        }

        private static int CalculateChance(int[] dice)
        {
            return dice.Sum();
        }

        private static int CalculateFullHouse(int[] dice)
        {
            var groups = dice.GroupBy(d => d).OrderByDescending(g => g.Count()).ToList();
            return groups.Count == 2 && (groups[0].Count() == 3 || groups[1].Count() == 3) ? dice.Sum() : 0;
        }

        private static int CalculateLargeStraight(int[] dice)
        {
            int[] largeStraight = { 2, 3, 4, 5, 6 };
            return dice.OrderBy(d => d).SequenceEqual(largeStraight) ? 20 : 0;
        }

        private static int CalculateSmallStraight(int[] dice)
        {
            int[] smallStraight = { 1, 2, 3, 4, 5 };
            return dice.OrderBy(d => d).SequenceEqual(smallStraight) ? 15 : 0;
        }

        private static int CalculateFourOfAKind(int[] dice)
        {
            var groups = dice.GroupBy(d => d).FirstOrDefault(g => g.Count() >= 4);
            return groups != null ? groups.Key * 4 : 0;
        }

        private static int CalculateThreeOfAKind(int[] dice)
        {
            var groups = dice.GroupBy(d => d).FirstOrDefault(g => g.Count() >= 3);
            return groups != null ? groups.Key * 3 : 0;
        }

        private static int CalculateTwoPairs(int[] dice)
        {
            var pairs = dice.GroupBy(d => d).Where(g => g.Count() >= 2).Take(2).ToList();
            return pairs.Count == 2 ? pairs.Sum(p => p.Key * 2) : 0;
        }

        private static int CalculateOnePair(int[] dice)
        {
            var pair = dice.GroupBy(d => d).FirstOrDefault(g => g.Count() >= 2);
            return pair != null ? pair.Key * 2 : 0;
        }

        private static int CalculateNumberScore(int[] dice, int v)
        {
            return dice.Where(d => d == v).Sum();
        }
    }
}