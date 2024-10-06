









namespace Yatzy_in_WPF
{
    internal static class YatzyLogic
    {
        public class Player
        {
            public static string Name { get; set; }
            public static int[] ScoreCard { get; set; }
            public static int TotalScore { get; set; }
            public static int Bonus { get; set; }
            public static int TotalBonus { get; set; }
            public static int GrandTotal { get; set; }
        }

        private static Player[] players;

        public static Player[] Players { get => players; set => players = value; }

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
