using System;
using System.Collections.Generic;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles tracking of player achievements
    /// </summary>
    public class AchievementsTracker
    {
        /// <summary>
        /// Method for tracking the player achievements progress
        /// </summary>
        /// <param name="achievementsCompleted"></param>
        /// <param name="username"></param>
        /// <param name="stats"></param>
        /// <param name="currentMode"></param>
        /// <param name="currentLvl"></param>
        /// <returns></returns>
        public static List<AchievementsCompleted> UpdateAchievements(List<AchievementsCompleted> achievementsCompleted, string username, PlayerStats stats, string currentMode, int currentLvl)
        {
            for (int i = 0; i < achievementsCompleted.Count; i++)
            {
                string title = String.Empty;
                string description = String.Empty;
                string image = String.Empty;
                int progress = achievementsCompleted[i].Progress;
                bool status = achievementsCompleted[i].Status;
                if (!status)
                {
                    if (currentMode == "Casual")
                    {
                        if (i == 0)
                        {
                            title = achievementsCompleted[i].Title;
                            description = achievementsCompleted[i].Description;
                            image = achievementsCompleted[i].Image;
                            progress = 1;
                            status = true;
                        }
                        else if (i == 1)
                        {
                            progress = achievementsCompleted[i].Progress + 1;
                            if (progress >= 5)
                            {
                                title = achievementsCompleted[i].Title;
                                description = achievementsCompleted[i].Description;
                                image = achievementsCompleted[i].Image;
                                status = true;
                            }
                        }
                        else if (i == 2)
                        {
                            progress = achievementsCompleted[i].Progress + 1;
                            if (progress >= 20)
                            {
                                title = achievementsCompleted[i].Title;
                                description = achievementsCompleted[i].Description;
                                image = achievementsCompleted[i].Image;
                                status = true;
                            }
                        }
                    }
                    else if (currentMode == "Competitive")
                    {
                        if (i == 3)
                        {
                            title = achievementsCompleted[i].Title;
                            description = achievementsCompleted[i].Description;
                            image = achievementsCompleted[i].Image;
                            progress = 1;
                            status = true;
                        }
                        else if (i == 4)
                        {
                            progress = achievementsCompleted[i].Progress + 1;
                            if (progress >= 5)
                            {
                                title = achievementsCompleted[i].Title;
                                description = achievementsCompleted[i].Description;
                                image = achievementsCompleted[i].Image;
                                status = true;
                            }
                        }
                        else if (i == 5)
                        {
                            progress = achievementsCompleted[i].Progress + 1;
                            if (progress >= 20)
                            {
                                title = achievementsCompleted[i].Title;
                                description = achievementsCompleted[i].Description;
                                image = achievementsCompleted[i].Image;
                                status = true;
                            }
                        }
                    }

                    if (i == 6 && stats.XP / 100 == 5)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 7 && stats.XP / 100 == 20)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 8 && stats.XP / 100 == 50)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 9 && stats.SR >= 50)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 10 && stats.SR >= 200)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 11 && stats.SR >= 500)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 12 && stats.PlayTime >= TimeSpan.Parse("00:10:00"))
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 13 && stats.PlayTime >= TimeSpan.Parse("00:30:00"))
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 14 && stats.PlayTime >= TimeSpan.Parse("01:00:00"))
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 15 && stats.Bronzes >= 5)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 16 && stats.Silvers >= 10)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }
                    else if (i == 17 && stats.Golds >= 15)
                    {
                        title = achievementsCompleted[i].Title;
                        description = achievementsCompleted[i].Description;
                        image = achievementsCompleted[i].Image;
                        status = true;
                    }

                    if (i == 18 && currentLvl == 25)
                    {
                        progress = achievementsCompleted[i].Progress + 1;
                        if (progress >= 5)
                        {
                            title = achievementsCompleted[i].Title;
                            description = achievementsCompleted[i].Description;
                            image = achievementsCompleted[i].Image;
                            status = true;
                        }
                    }
                }

                achievementsCompleted[i].Username = username;
                achievementsCompleted[i].Title = title;
                achievementsCompleted[i].Description = description;
                achievementsCompleted[i].Image = image;
                achievementsCompleted[i].Progress = progress;
                achievementsCompleted[i].Status = status;
            }
            return achievementsCompleted;
        }

        /// <summary>
        /// Method for adding achievements for a player
        /// </summary>
        /// <param name="achievementsCompleted"></param>
        /// <param name="username"></param>
        /// <param name="currentMode"></param>
        /// <returns></returns>
        public static List<AchievementsCompleted> InsertAchievements(List<AchievementsCompleted> achievementsCompleted, string username, string currentMode)
        {
            achievementsCompleted = new List<AchievementsCompleted>();
            for (int i = 1; i < 20; i++)
            {
                int progress = 0;
                bool status = false;
                if (currentMode == "Casual")
                {
                    if (i == 1)
                    {
                        progress = 1;
                        status = true;
                    }
                    else if (i == 2 || i == 3)
                    {
                        progress = 1;
                        status = false;
                    }
                }
                else if (currentMode == "Competitive")
                {
                    if (i == 4)
                    {
                        progress = 1;
                        status = true;
                    }
                    else if (i == 5 || i == 6)
                    {
                        progress = 1;
                        status = false;
                    }
                }

                achievementsCompleted.Add(new AchievementsCompleted()
                {
                    Username = username,
                    Progress = progress,
                    Status = status
                });
            }
            return achievementsCompleted;
        }
    }
}