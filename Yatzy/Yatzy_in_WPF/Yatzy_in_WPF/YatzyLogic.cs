namespace Yatzy_in_WPF
{
    internal static class YatzyLogic
    {
        public class Player
        {
            public string Name { get; set; }
            public int[] ScoreCard = new int[15]; // This is the score list of the player
            public int TotalScore { get; set; } // This is the total score of the player
            public int Bonus { get; set; }
            public int TotalBonus { get; set; }
            public int GrandTotal { get; set; } // This is the total score of the player including bonus
            public Player(string name)
            {
                Name = name;
            }
        }

        public static class GameManager
        {
            public static List<Player> Players { get; set; } = new List<Player>();
            private const int WinningScore = 100; // Winning score threshold

            public static void InitializeGame(int numberOfPlayers)
            {
                Players.Clear(); // Clear the list in case it's a new game

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Players.Add(new Player($"Player {i + 1}"));
                }
            }

            public static void GameLoop()
            {
                // While (any of the Players GrandTotal value is BELOW winning value of 100)
                while (Players.All(player => player.GrandTotal < WinningScore))
                {
                    // Game is running
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
            int score = 0;
            for (int i = 6; i > 0; i--)
            {
                int count = 0;
                foreach (int die in dice)
                {
                    if (die == i)
                    {
                        count++;
                    }
                }
                if (count == 5)
                {
                    score = 50;
                    break;
                }
            }
            return score;
        }

        private static int CalculateChance(int[] dice)
        {
            return dice.Sum();
        }

        private static int CalculateFullHouse(int[] dice)
        {
            int score = 0;
            int[] sortedDice = dice.OrderBy(d => d).ToArray();
            if (sortedDice[0] == sortedDice[1] && sortedDice[3] == sortedDice[4] && (sortedDice[2] == sortedDice[1] || sortedDice[2] == sortedDice[3]))
            {
                score = dice.Sum();
            }
            return score;
        }

        private static int CalculateLargeStraight(int[] dice)
        {
            int score = 0;
            int[] sortedDice = dice.OrderBy(d => d).ToArray();
            if (sortedDice[0] == 2 && sortedDice[1] == 3 && sortedDice[2] == 4 && sortedDice[3] == 5 && sortedDice[4] == 6)
            {
                score = 20;
            }
            return score;
        }

        private static int CalculateSmallStraight(int[] dice)
        {
            int score = 0;
            int[] sortedDice = dice.OrderBy(d => d).ToArray();
            if (sortedDice[0] == 1 && sortedDice[1] == 2 && sortedDice[2] == 3 && sortedDice[3] == 4 && sortedDice[4] == 5)
            {
                score = 15;
            }
            return score;
        }

        private static int CalculateFourOfAKind(int[] dice)
        {
            int score = 0;
            for (int i = 6; i > 0; i--)
            {
                int count = 0;
                foreach (int die in dice)
                {
                    if (die == i)
                    {
                        count++;
                    }
                }
                if (count >= 4)
                {
                    score = i * 4;
                    break;
                }
            }
            return score;
        }

        private static int CalculateThreeOfAKind(int[] dice)
        {
            int score = 0;
            for (int i = 6; i > 0; i--)
            {
                int count = 0;
                foreach (int die in dice)
                {
                    if (die == i)
                    {
                        count++;
                    }
                }
                if (count >= 3)
                {
                    score = i * 3;
                    break;
                }
            }
            return score;
        }

        private static int CalculateTwoPairs(int[] dice)
        {
            int score = 0;
            int pairCount = 0;
            for (int i = 6; i > 0; i--)
            {
                int count = 0;
                foreach (int die in dice)
                {
                    if (die == i)
                    {
                        count++;
                    }
                }
                if (count >= 2)
                {
                    score += i * 2;
                    pairCount++;
                }
                if (pairCount == 2)
                {
                    break;
                }
            }
            if (pairCount < 2)
            {
                score = 0;
            }
            return score;
        }

        private static int CalculateOnePair(int[] dice)
        {
            int score = 0;
            for (int i = 6; i > 0; i--)
            {
                int count = 0;
                foreach (int die in dice)
                {
                    if (die == i)
                    {
                        count++;
                    }
                }
                if (count >= 2)
                {
                    score = i * 2;
                    break;
                }
            }
            return score;
        }

        private static int CalculateNumberScore(int[] dice, int v)
        {
            int score = 0;
            foreach (int die in dice)
            {
                if (die == v)
                {
                    score += v;
                }
            }
            return score;
        }
    }
}
