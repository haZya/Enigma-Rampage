using EnigmaRampageLibrary.Models;

namespace EnigmaRampageLibrary.Helper
{
    /// <summary>
    /// Handles calculations of player stats
    /// </summary>
    public static class ScoreCalculator
    {
        /// <summary>
        /// Method for calculating the player stats
        /// </summary>
        /// <param name="currentLvl"></param>
        /// <param name="swaps"></param>
        /// <param name="time"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static PlayerStats Calculate(int currentLvl, int swaps, int time, string mode)
        {
            PlayerStats scores = new PlayerStats();

            if (mode == "Competitive")
            {
                if (currentLvl == 4)
                {
                    if (swaps > 5 || time > 10)
                    {
                        scores.XP = 2;
                        scores.SR = 1;
                        scores.Bronzes = 1;
                    }
                    else if (swaps > 3 || time > 7)
                    {
                        scores.XP = 4;
                        scores.SR = 2;
                        scores.Silvers = 1;
                    }
                    else
                    {
                        scores.XP = 6;
                        scores.SR = 3;
                        scores.Golds = 1;
                    }
                }
                else if (currentLvl == 9)
                {
                    if (swaps > 16 || time > 30)
                    {
                        scores.XP = 5;
                        scores.SR = 2;
                        scores.Bronzes = 1;
                    }
                    else if (swaps > 11 || time > 18)
                    {
                        scores.XP = 10;
                        scores.SR = 5;
                        scores.Silvers = 1;
                    }
                    else
                    {
                        scores.XP = 15;
                        scores.SR = 8;
                        scores.Golds = 1;
                    }
                }
                else if (currentLvl == 16)
                {
                    if (swaps > 28 || time > 52)
                    {
                        scores.XP = 10;
                        scores.SR = 5;
                        scores.Bronzes = 1;
                    }
                    else if (swaps > 19 || time > 35)
                    {
                        scores.XP = 20;
                        scores.SR = 10;
                        scores.Silvers = 1;
                    }
                    else
                    {
                        scores.XP = 30;
                        scores.SR = 15;
                        scores.Golds = 1;
                    }
                }
                else if (currentLvl == 25)
                {
                    if (swaps > 48 || time > 120)
                    {
                        scores.XP = 20;
                        scores.SR = 10;
                        scores.Bronzes = 1;
                    }
                    else if (swaps > 33 || time > 80)
                    {
                        scores.XP = 34;
                        scores.SR = 20;
                        scores.Silvers = 1;
                    }
                    else
                    {
                        scores.XP = 48;
                        scores.SR = 30;
                        scores.Golds = 1;
                    }
                }
            }
            else
            {
                if (currentLvl == 4)
                {
                    scores.XP = 4;
                }
                else if (currentLvl == 9)
                {
                    scores.XP = 10;
                }
                else if (currentLvl == 16)
                {
                    scores.XP = 20;
                }
                else if (currentLvl == 25)
                {
                    scores.XP = 34;
                }
            }
            return scores;
        }
    }
}