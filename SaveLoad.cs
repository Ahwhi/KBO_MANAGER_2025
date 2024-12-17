using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaveLoad
{
    public static void SaveData()
    {
        string fileName = Path.Combine(Application.persistentDataPath, GameDirector.currentFile); 
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine("DATE");
            writer.WriteLine($"{GameDirector.currentDate.year}-{GameDirector.currentDate.month}-{GameDirector.currentDate.day}-{(int)GameDirector.currentDate.dayOfWeek}");

            writer.WriteLine("MYTEAM");
            writer.WriteLine($"{(int)GameDirector.myTeam}");

            writer.WriteLine("MONEY");
            writer.WriteLine($"{(int)GameDirector.money}");

            writer.WriteLine("TEAM");
            for (int t = 0; t < 10; t++)
            {
                writer.WriteLine($"{GameDirector.Teams[t].teamCode}, {GameDirector.Teams[t].win}, {GameDirector.Teams[t].draw}, {GameDirector.Teams[t].lose}, {GameDirector.Teams[t].currentSP}");
            }

            writer.WriteLine("SCHEDULE");
            for (int i = 0; i < GameDirector.totalMatchCount; i++)
            {
                writer.WriteLine($"{(int)GameDirector.schedule[i].homeTeam}, {(int)GameDirector.schedule[i].awayTeam}, {GameDirector.schedule[i].dates.year}-{GameDirector.schedule[i].dates.month}-{GameDirector.schedule[i].dates.day}-{(int)GameDirector.schedule[i].dates.dayOfWeek}, {GameDirector.schedule[i].homeScore}:{GameDirector.schedule[i].awayScore}, {GameDirector.schedule[i].isEnd}");
            }

            writer.WriteLine("POSTSEASON");
            writer.WriteLine($"{GameDirector.isPostSeason}");
            writer.WriteLine($"{GameDirector.UpTeamWin}");
            writer.WriteLine($"{GameDirector.DownTeamWin}");
            writer.WriteLine($"{(int)GameDirector.KingTeam}");
            if (GameDirector.isPostSeason)
            {
                for (int i = 0; i < 19; i++)
                {
                    writer.WriteLine($"{(int)GameDirector.postSchedule[i].homeTeam}, {(int)GameDirector.postSchedule[i].awayTeam}, {GameDirector.postSchedule[i].dates.year}-{GameDirector.postSchedule[i].dates.month}-{GameDirector.postSchedule[i].dates.day}-{(int)GameDirector.postSchedule[i].dates.dayOfWeek}, {GameDirector.postSchedule[i].homeScore}:{GameDirector.postSchedule[i].awayScore}, {GameDirector.postSchedule[i].isEnd}, {GameDirector.postSchedule[i].isPass}, {(int)GameDirector.postSchedule[i].UpTeam}, {(int)GameDirector.postSchedule[i].DownTeam}");
                }
            }

            writer.WriteLine("PITCHER");
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                writer.WriteLine($"{GameDirector.pitcher[i].playerId},{GameDirector.pitcher[i].name},{(int)GameDirector.pitcher[i].hand},{(int)GameDirector.pitcher[i].team},{(int)GameDirector.pitcher[i].born},{(int)GameDirector.pitcher[i].pos},{GameDirector.pitcher[i].game}," +
                                 $"{GameDirector.pitcher[i].inningsPitched1},{GameDirector.pitcher[i].inningsPitched2},{GameDirector.pitcher[i].win},{GameDirector.pitcher[i].lose}," +
                                 $"{GameDirector.pitcher[i].hold},{GameDirector.pitcher[i].save},{GameDirector.pitcher[i].strikeOut},{GameDirector.pitcher[i].baseOnBall}," +
                                 $"{GameDirector.pitcher[i].hitAllowed},{GameDirector.pitcher[i].homerunAllowed},{GameDirector.pitcher[i].earnedRuns},{GameDirector.pitcher[i].earnedRunAverage}," +
                                 $"{GameDirector.pitcher[i].WHIP},{GameDirector.pitcher[i].HP},{GameDirector.pitcher[i].SPEED},{GameDirector.pitcher[i].COMMAND},{GameDirector.pitcher[i].BREAKING},{GameDirector.pitcher[i].posInTeam},{GameDirector.pitcher[i].isForeign}");
            }

            writer.WriteLine("BATTER");
            for (int i = 0; i < GameDirector.batterCount; i++)
            {
                writer.WriteLine($"{GameDirector.batter[i].playerId},{GameDirector.batter[i].name},{(int)GameDirector.batter[i].hand},{(int)GameDirector.batter[i].team},{(int)GameDirector.batter[i].born},{(int)GameDirector.batter[i].pos},{GameDirector.batter[i].game}," +
                                 $"{GameDirector.batter[i].plateAppearance},{GameDirector.batter[i].atBat},{GameDirector.batter[i].hit},{GameDirector.batter[i].homerun}," +
                                 $"{GameDirector.batter[i].totalBase},{GameDirector.batter[i].RBI},{GameDirector.batter[i].baseOnBall},{GameDirector.batter[i].runScored}," +
                                 $"{GameDirector.batter[i].battingAverage},{GameDirector.batter[i].SLG},{GameDirector.batter[i].OBP},{GameDirector.batter[i].OPS}," +
                                 $"{GameDirector.batter[i].stolenBased},{GameDirector.batter[i].POWER},{GameDirector.batter[i].CONTACT},{GameDirector.batter[i].EYE},{ GameDirector.batter[i].posInTeam},{GameDirector.batter[i].isForeign}");
            }

            writer.WriteLine("FPITCHER");
            for (int i = 0; i < GameDirector.foreignPitcherCandidateCount; i++)
            {
                writer.WriteLine($"{GameDirector.Fpitcher[i].playerId},{GameDirector.Fpitcher[i].name},{(int)GameDirector.Fpitcher[i].nation},{(int)GameDirector.Fpitcher[i].hand},{GameDirector.Fpitcher[i].born}," +
                                 $"{(int)GameDirector.Fpitcher[i].pos},{GameDirector.Fpitcher[i].SPEED},{GameDirector.Fpitcher[i].COMMAND},{GameDirector.Fpitcher[i].BREAKING}");
            }

            writer.WriteLine("FBATTER");
            for (int i = 0; i < GameDirector.foreignBatterCandidateCount; i++)
            {
                writer.WriteLine($"{GameDirector.Fbatter[i].playerId},{GameDirector.Fbatter[i].name},{(int)GameDirector.Fbatter[i].nation},{(int)GameDirector.Fbatter[i].hand},{GameDirector.Fbatter[i].born}," +
                                 $"{(int)GameDirector.Fbatter[i].pos},{GameDirector.Fbatter[i].POWER},{GameDirector.Fbatter[i].CONTACT},{GameDirector.Fbatter[i].EYE}");
            }

            writer.WriteLine("MAIL");
            for (int i = 0; i < GameDirector.mail.Count; i++)
            {
                writer.WriteLine($"{GameDirector.mail[i].Title},{GameDirector.mail[i].Sender},{GameDirector.mail[i].Dates.year}-{GameDirector.mail[i].Dates.month}-{GameDirector.mail[i].Dates.day}-{(int)GameDirector.mail[i].Dates.dayOfWeek},{GameDirector.mail[i].isRead},{GameDirector.mail[i].Detail.Replace("\n", ";")}");
            }
        }
    }

    public static void LoadData(string FileName)
    {
        if (!File.Exists(FileName))
        {
            Debug.LogError("파일이 존재하지 않습니다.");
            return;
        }

        CreateData.CreateDate();
        CreateData.CreateTeam();
        CreateData.CreatePlayer();
        CreateData.CreateForeignCandidatePlayer();
        CreateData.CreateSchedule();

        using (StreamReader reader = new StreamReader(FileName))
        {
            string line;

            //currentDate
            reader.ReadLine();
            line = reader.ReadLine();
            string[] dateParts = line.Split('-');
            GameDirector.currentDate.year = int.Parse(dateParts[0]);
            GameDirector.currentDate.month = int.Parse(dateParts[1]);
            GameDirector.currentDate.day = int.Parse(dateParts[2]);
            GameDirector.currentDate.dayOfWeek = (GameData.DayOfWeek)Enum.Parse(typeof(GameData.DayOfWeek), dateParts[3]);

            //myTeam
            reader.ReadLine();
            line = reader.ReadLine();
            GameDirector.myTeam = (TeamName)Enum.Parse(typeof(TeamName), line);

            //Money
            reader.ReadLine();
            line = reader.ReadLine();
            GameDirector.money = long.Parse(line);

            //Teams
            reader.ReadLine();
            for (int t = 0; t < 10; t++)
            {
                line = reader.ReadLine();
                string[] teamParts = line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                GameDirector.Teams[t].teamCode = int.Parse(teamParts[0]);
                GameDirector.Teams[t].win = int.Parse(teamParts[1]);
                GameDirector.Teams[t].draw = int.Parse(teamParts[2]);
                GameDirector.Teams[t].lose = int.Parse(teamParts[3]);
                GameDirector.Teams[t].currentSP = int.Parse(teamParts[4]);
            }

            //schedule
            reader.ReadLine();
            for (int i = 0; i < GameDirector.totalMatchCount; i++)
            {
                line = reader.ReadLine();
                string[] scheduleParts = line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                GameDirector.schedule[i].homeTeam = (TeamName)Enum.Parse(typeof(TeamName), scheduleParts[0]);
                GameDirector.schedule[i].awayTeam = (TeamName)Enum.Parse(typeof(TeamName), scheduleParts[1]);
                string[] datePartsSchedule = scheduleParts[2].Split('-');
                GameDirector.schedule[i].dates.year = int.Parse(datePartsSchedule[0]);
                GameDirector.schedule[i].dates.month = int.Parse(datePartsSchedule[1]);
                GameDirector.schedule[i].dates.day = int.Parse(datePartsSchedule[2]);
                GameDirector.schedule[i].dates.dayOfWeek = (GameData.DayOfWeek)Enum.Parse(typeof(GameData.DayOfWeek), datePartsSchedule[3]);
                string[] scoreParts = scheduleParts[3].Split(':');
                GameDirector.schedule[i].homeScore = int.Parse(scoreParts[0]);
                GameDirector.schedule[i].awayScore = int.Parse(scoreParts[1]);
                GameDirector.schedule[i].isEnd = bool.Parse(scheduleParts[4]);
            }

            //postSchedule
            reader.ReadLine();
            line = reader.ReadLine();
            GameDirector.isPostSeason = bool.Parse(line);
            line = reader.ReadLine();
            GameDirector.UpTeamWin = int.Parse(line);
            line = reader.ReadLine();
            GameDirector.DownTeamWin = int.Parse(line);
            line = reader.ReadLine();
            GameDirector.KingTeam = (TeamName)Enum.Parse(typeof(TeamName), line);
            if (GameDirector.isPostSeason)
            {
                CreateData.CreatePostSeasonSchedule(0, 0, 0, 0, 0);
                for (int i = 0; i < 19; i++)
                {
                    line = reader.ReadLine();
                    string[] postScheduleParts = line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    GameDirector.postSchedule[i].homeTeam = (TeamName)Enum.Parse(typeof(TeamName), postScheduleParts[0]);
                    GameDirector.postSchedule[i].awayTeam = (TeamName)Enum.Parse(typeof(TeamName), postScheduleParts[1]);
                    string[] datePartsSchedule = postScheduleParts[2].Split('-');
                    GameDirector.postSchedule[i].dates.year = int.Parse(datePartsSchedule[0]);
                    GameDirector.postSchedule[i].dates.month = int.Parse(datePartsSchedule[1]);
                    GameDirector.postSchedule[i].dates.day = int.Parse(datePartsSchedule[2]);
                    GameDirector.postSchedule[i].dates.dayOfWeek = (GameData.DayOfWeek)Enum.Parse(typeof(GameData.DayOfWeek), datePartsSchedule[3]);
                    string[] scoreParts = postScheduleParts[3].Split(':');
                    GameDirector.postSchedule[i].homeScore = int.Parse(scoreParts[0]);
                    GameDirector.postSchedule[i].awayScore = int.Parse(scoreParts[1]);
                    GameDirector.postSchedule[i].isEnd = bool.Parse(postScheduleParts[4]);
                    GameDirector.postSchedule[i].isPass = bool.Parse(postScheduleParts[5]);
                    GameDirector.postSchedule[i].UpTeam = (TeamName)Enum.Parse(typeof(TeamName), postScheduleParts[6]);
                    GameDirector.postSchedule[i].DownTeam = (TeamName)Enum.Parse(typeof(TeamName), postScheduleParts[7]);
                }
            }

            //pitcher
            reader.ReadLine();
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                line = reader.ReadLine();
                string[] pitcherParts = line.Split(',');
                GameDirector.pitcher[i].playerId = int.Parse(pitcherParts[0]);
                GameDirector.pitcher[i].name = pitcherParts[1];
                GameDirector.pitcher[i].hand = (Hand)Enum.Parse(typeof(Hand), pitcherParts[2]);
                GameDirector.pitcher[i].team = (TeamName)Enum.Parse(typeof(TeamName), pitcherParts[3]);
                GameDirector.pitcher[i].born = int.Parse(pitcherParts[4]);
                GameDirector.pitcher[i].pos = (PitcherPosition)Enum.Parse(typeof(PitcherPosition), pitcherParts[5]);
                GameDirector.pitcher[i].game = int.Parse(pitcherParts[6]);
                GameDirector.pitcher[i].inningsPitched1 = int.Parse(pitcherParts[7]);
                GameDirector.pitcher[i].inningsPitched2 = int.Parse(pitcherParts[8]);
                GameDirector.pitcher[i].win = int.Parse(pitcherParts[9]);
                GameDirector.pitcher[i].lose = int.Parse(pitcherParts[10]);
                GameDirector.pitcher[i].hold = int.Parse(pitcherParts[11]);
                GameDirector.pitcher[i].save = int.Parse(pitcherParts[12]);
                GameDirector.pitcher[i].strikeOut = int.Parse(pitcherParts[13]);
                GameDirector.pitcher[i].baseOnBall = int.Parse(pitcherParts[14]);
                GameDirector.pitcher[i].hitAllowed = int.Parse(pitcherParts[15]);
                GameDirector.pitcher[i].homerunAllowed = int.Parse(pitcherParts[16]);
                GameDirector.pitcher[i].earnedRuns = int.Parse(pitcherParts[17]);
                GameDirector.pitcher[i].earnedRunAverage = float.Parse(pitcherParts[18]);
                GameDirector.pitcher[i].WHIP = float.Parse(pitcherParts[19]);
                GameDirector.pitcher[i].HP = int.Parse(pitcherParts[20]);
                GameDirector.pitcher[i].SPEED = int.Parse(pitcherParts[21]);
                GameDirector.pitcher[i].COMMAND = int.Parse(pitcherParts[22]);
                GameDirector.pitcher[i].BREAKING = int.Parse(pitcherParts[23]);
                GameDirector.pitcher[i].posInTeam = int.Parse(pitcherParts[24]);
                GameDirector.pitcher[i].isForeign = bool.Parse(pitcherParts[25]);
            }

            //batter
            reader.ReadLine();
            for (int i = 0; i < GameDirector.batterCount; i++)
            {
                line = reader.ReadLine();
                string[] batterParts = line.Split(',');

                GameDirector.batter[i].playerId = int.Parse(batterParts[0]);
                GameDirector.batter[i].name = batterParts[1];
                GameDirector.batter[i].hand = (Hand)Enum.Parse(typeof(Hand), batterParts[2]);
                GameDirector.batter[i].team = (TeamName)Enum.Parse(typeof(TeamName), batterParts[3]);
                GameDirector.batter[i].born = int.Parse(batterParts[4]);
                GameDirector.batter[i].pos = (BatterPosition)Enum.Parse(typeof(BatterPosition), batterParts[5]);
                GameDirector.batter[i].game = int.Parse(batterParts[6]);
                GameDirector.batter[i].plateAppearance = int.Parse(batterParts[7]);
                GameDirector.batter[i].atBat = int.Parse(batterParts[8]);
                GameDirector.batter[i].hit = int.Parse(batterParts[9]);
                GameDirector.batter[i].homerun = int.Parse(batterParts[10]);
                GameDirector.batter[i].totalBase = int.Parse(batterParts[11]);
                GameDirector.batter[i].RBI = int.Parse(batterParts[12]);
                GameDirector.batter[i].baseOnBall = int.Parse(batterParts[13]);
                GameDirector.batter[i].runScored = int.Parse(batterParts[14]);
                GameDirector.batter[i].battingAverage = float.Parse(batterParts[15]);
                GameDirector.batter[i].SLG = float.Parse(batterParts[16]);
                GameDirector.batter[i].OBP = float.Parse(batterParts[17]);
                GameDirector.batter[i].OPS = float.Parse(batterParts[18]);
                GameDirector.batter[i].stolenBased = int.Parse(batterParts[19]);
                GameDirector.batter[i].POWER = int.Parse(batterParts[20]);
                GameDirector.batter[i].CONTACT = int.Parse(batterParts[21]);
                GameDirector.batter[i].EYE = int.Parse(batterParts[22]);
                GameDirector.batter[i].posInTeam = int.Parse(batterParts[23]);
                GameDirector.batter[i].isForeign = bool.Parse(batterParts[24]);
            }

            //Fpitcher
            reader.ReadLine();
            for (int i = 0; i < GameDirector.foreignPitcherCandidateCount; i++)
            {
                line = reader.ReadLine();
                string[] pitcherParts = line.Split(',');
                GameDirector.Fpitcher[i].playerId = int.Parse(pitcherParts[0]);
                GameDirector.Fpitcher[i].name = pitcherParts[1];
                GameDirector.Fpitcher[i].nation = (Nation)Enum.Parse(typeof(Nation), pitcherParts[2]);
                GameDirector.Fpitcher[i].hand = (Hand)Enum.Parse(typeof(Hand), pitcherParts[3]);
                GameDirector.Fpitcher[i].born = int.Parse(pitcherParts[4]);
                GameDirector.Fpitcher[i].pos = (PitcherPosition)Enum.Parse(typeof(PitcherPosition), pitcherParts[5]);
                GameDirector.Fpitcher[i].SPEED = int.Parse(pitcherParts[6]);
                GameDirector.Fpitcher[i].COMMAND = int.Parse(pitcherParts[7]);
                GameDirector.Fpitcher[i].BREAKING = int.Parse(pitcherParts[8]);
            }

            //Fbatter
            reader.ReadLine();
            for (int i = 0; i < GameDirector.foreignBatterCandidateCount; i++)
            {
                line = reader.ReadLine();
                string[] pitcherParts = line.Split(',');
                GameDirector.Fbatter[i].playerId = int.Parse(pitcherParts[0]);
                GameDirector.Fbatter[i].name = pitcherParts[1];
                GameDirector.Fbatter[i].nation = (Nation)Enum.Parse(typeof(Nation), pitcherParts[2]);
                GameDirector.Fbatter[i].hand = (Hand)Enum.Parse(typeof(Hand), pitcherParts[3]);
                GameDirector.Fbatter[i].born = int.Parse(pitcherParts[4]);
                GameDirector.Fbatter[i].pos = (BatterPosition)Enum.Parse(typeof(BatterPosition), pitcherParts[5]);
                GameDirector.Fbatter[i].POWER = int.Parse(pitcherParts[6]);
                GameDirector.Fbatter[i].CONTACT = int.Parse(pitcherParts[7]);
                GameDirector.Fbatter[i].EYE = int.Parse(pitcherParts[8]);
            }

            //mail
            reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                string[] mailData = line.Split(',');
                if (mailData.Length >= 5)
                {
                    Mail mail = new Mail();
                    mail.Title = mailData[0];
                    mail.Sender = mailData[1];
                    string[] dateParts2 = mailData[2].Split('-');
                    if (dateParts2.Length == 4)
                    {
                        int year = int.Parse(dateParts2[0]);
                        int month = int.Parse(dateParts2[1]);
                        int day = int.Parse(dateParts2[2]);
                        int dayOfWeek = int.Parse(dateParts2[3]);
                        mail.Dates = new Date(year, month, day, (GameData.DayOfWeek)dayOfWeek);
                    }
                    mail.isRead = bool.Parse(mailData[3]);
                    mail.Detail = mailData[4].Replace(";", "\n");
                    GameDirector.mail.Add(mail);
                }
                else
                {
                    Debug.LogError("잘못된 형식의 데이터가 있습니다: " + line);
                }
            }
        }
        TeamColor.SetMyTeamColor();
        GameDirector.currentFile = FileName;
        SceneManager.LoadScene("Main");
    }
}
    
