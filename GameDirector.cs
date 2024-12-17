using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameData;
using TMPro;
using UnityEngine.SceneManagement;
using MailData;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance;
    public static bool isPostSeason;
    public static int UpTeamWin;
    public static int DownTeamWin;
    public static TeamName KingTeam;
    public static string currentFile;
    public static TeamName myTeam;
    public static TeamName tempMyTeam;
    public static Color myColor;
    public static Date currentDate;
    public static int currentGame;
    public static int remainGames = 0;
    public static long money = 0;
    public static List<Team> Teams = new List<Team>();
    public static List<Pitcher> pitcher = new List<Pitcher>();
    public static List<Batter> batter = new List<Batter>();
    public static List<ForeignPitcher> Fpitcher = new List<ForeignPitcher>();
    public static List<ForeignBatter> Fbatter = new List<ForeignBatter>();
    public static List<Schedule> schedule = new List<Schedule>();
    public static List<PostSchedule> postSchedule = new List<PostSchedule>();
    public static List<Mail> mail = new List<Mail>();
    public static int pitcherCount = 0;
    public static int batterCount = 0;
    public static int foreignPitcherCandidateCount = 0;
    public static int foreignBatterCandidateCount = 0;
    public static int totalPlayerCount = 0;
    public static int totalMatchCount = 0;
    public static bool isSimulation = false;
    public static bool isLogo = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Title" && scene.name != "SelectTeam" && scene.name != "GameModule")
        {
            Image MyPanel = GameObject.Find("TopPanel").GetComponent<Image>();
            MyPanel.color = myColor;
            TextMeshProUGUI TeamName = GameObject.Find("TeamName").GetComponent<TextMeshProUGUI>();
            TeamName.text = DataToString.TeamToString(myTeam);
            Image TeamEmblemPanel = GameObject.Find("TeamEmblemPanel").GetComponent<Image>();
            TeamEmblemPanel.sprite = TeamEmblem.GetEmblem(myTeam);
        }
    }

    public static void DayToMail()
    {
        if (currentDate.year == 2025 && currentDate.month == 3 && currentDate.day == 23) { GetStockMail.M2(); }
        if (currentDate.year == 2025 && currentDate.month == 3 && currentDate.day == 24) { GetStockMail.M3(); }
        if (currentDate.year == 2025 && currentDate.month == 3 && currentDate.day == 25) { GetStockMail.M4(); }
        if ((currentDate.year == 2025 && currentDate.month == 4 && currentDate.day == 1) ||
            (currentDate.year == 2025 && currentDate.month == 5 && currentDate.day == 1) ||
            (currentDate.year == 2025 && currentDate.month == 6 && currentDate.day == 1) ||
            (currentDate.year == 2025 && currentDate.month == 7 && currentDate.day == 1) ||
            (currentDate.year == 2025 && currentDate.month == 8 && currentDate.day == 1)) { 
            GetStockMail.M5();
            money += 100000000;
        }
        if (currentDate.year == 2025 && currentDate.month == 6 && currentDate.day == 20) { GetStockMail.M6(); }
        if (currentDate.year == 2025 && currentDate.month == 7 && currentDate.day == 10) { GetStockMail.M7(); }
        if (currentDate.year == 2025 && currentDate.month == 9 && currentDate.day == 8) { GetStockMail.M8(); }
        if (currentDate.year == 2025 && currentDate.month == 10 && currentDate.day == 10) { GetStockMail.M9(); }
    }

    public static void GetMail(string title, string detail, string sender)
    {
        Mail newMail = new Mail();
        newMail.Title = title;
        newMail.Detail = detail;
        newMail.Sender = sender;
        newMail.Dates = new Date(currentDate.year, currentDate.month, currentDate.day, currentDate.dayOfWeek);
        newMail.isRead = false;
        mail.Add(newMail);
    }

    public static void PlayGame()
    {
        currentGame = GameCheck();
        if (currentDate.year == 2025 && currentDate.month == 9 && currentDate.day == 8)
        {
            isPostSeason = true;
            List<Team> sortedTeam = new List<Team>(Teams);
            sortedTeam.Sort((team1, team2) => team2.WinRate().CompareTo(team1.WinRate()));
            CreateData.CreatePostSeasonSchedule((TeamName)sortedTeam[4].teamCode, (TeamName)sortedTeam[3].teamCode, (TeamName)sortedTeam[2].teamCode, (TeamName)sortedTeam[1].teamCode, (TeamName)sortedTeam[0].teamCode);
        }

        if (currentDate.year == 2025 && currentDate.month == 10 && currentDate.day == 11)
        {
            SceneManager.LoadScene("Main");
        }

        if (currentGame == -1)
        {
            DayToMail();
            currentDate = CreateData.UpdateDate(currentDate);
            MainScreen.isUpdateMain = true;
        }
        else
        {
            SceneManager.LoadScene("GameModule");
        }
    }

    public static int GameCheck()
    {
        if (!isPostSeason)
        {
            for (int t = 0; t < totalMatchCount; t++)
            {
                if (schedule[t].dates.year == currentDate.year &&
                    schedule[t].dates.month == currentDate.month &&
                    schedule[t].dates.day == currentDate.day)
                {
                    if ((schedule[t].homeTeam == myTeam) || (schedule[t].awayTeam == myTeam))
                    {
                        return t;
                    }
                }
            }
        } else
        {
            for (int t = 0; t < 19; t++)
            {
                if (postSchedule[t].dates.year == currentDate.year &&
                    postSchedule[t].dates.month == currentDate.month &&
                    postSchedule[t].dates.day == currentDate.day &&
                    !postSchedule[t].isPass)
                {
                    if ((postSchedule[t].homeTeam == myTeam) || (postSchedule[t].awayTeam == myTeam))
                    {
                        tempMyTeam = myTeam;
                        return t;
                    } else
                    {
                        tempMyTeam = myTeam;
                        isSimulation = true;
                        return t;
                    }
                }
            }
        }
        return -1;
    }

    public static bool CheckBatterPosAvailable()
    {
        int OK_DH = 0, OK_C = 0, OK_1B = 0, OK_2B = 0, OK_SS = 0, OK_3B = 0, OK_LF = 0, OK_CF = 0, OK_RF = 0;
        for (int i = 0; i < batterCount; i++)
        {
            if (batter[i].team == myTeam && batter[i].posInTeam >= 101 && batter[i].posInTeam <= 109)
            {
                switch (batter[i].pos)
                {
                    case BatterPosition.DH: OK_DH++; break;
                    case BatterPosition.C: OK_C++; break;
                    case BatterPosition._1B: OK_1B++; break;
                    case BatterPosition._2B: OK_2B++; break;
                    case BatterPosition._3B: OK_3B++; break;
                    case BatterPosition.SS: OK_SS++; break;
                    case BatterPosition.LF: OK_LF++; break;
                    case BatterPosition.CF: OK_CF++; break;
                    case BatterPosition.RF: OK_RF++; break;
                }
            }
        }
        if (OK_DH != 1 || OK_C != 1 || OK_1B != 1 || OK_2B != 1 || OK_SS != 1 || OK_3B != 1 || OK_LF != 1 || OK_CF != 1 || OK_RF != 1)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
