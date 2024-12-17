using GameData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamRank : MonoBehaviour
{
    public GameObject Line1, Line2, Line3, Line4, Line5, Line6, Line7, Line8, Line9, Line10;
    private TextMeshProUGUI CurrentDay;
    TMP_Text[] textArray;
    Image[] LogoImages;

    void Start()
    {
        List<Team> sortedTeam = new List<Team>(GameDirector.Teams);
        sortedTeam.Sort((team1, team2) => team2.WinRate().CompareTo(team1.WinRate()));
        int[] streak = new int[10];
        for (int i = 0; i < GameDirector.totalMatchCount; i++)
        {
            if (GameDirector.schedule[i].isEnd == true)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (GameDirector.schedule[i].homeTeam == (TeamName)sortedTeam[j].teamCode || GameDirector.schedule[i].awayTeam == (TeamName)sortedTeam[j].teamCode)
                    {
                        if (GameDirector.schedule[i].homeScore > GameDirector.schedule[i].awayScore && GameDirector.schedule[i].homeTeam == (TeamName)sortedTeam[j].teamCode)
                        {
                            if (streak[j] >= 1)
                            {
                                streak[j]++;
                            }
                            else
                            {
                                streak[j] = 1;
                            }
                        }
                        else if (GameDirector.schedule[i].homeScore < GameDirector.schedule[i].awayScore && GameDirector.schedule[i].homeTeam == (TeamName)sortedTeam[j].teamCode)
                        {
                            if (streak[j] <= -1)
                            {
                                streak[j]--;
                            }
                            else
                            {
                                streak[j] = -1;
                            }
                        }
                        else if (GameDirector.schedule[i].homeScore > GameDirector.schedule[i].awayScore && GameDirector.schedule[i].awayTeam == (TeamName)sortedTeam[j].teamCode)
                        {
                            if (streak[j] <= -1)
                            {
                                streak[j]--;
                            }
                            else
                            {
                                streak[j] = -1;
                            }
                        }
                        else if (GameDirector.schedule[i].homeScore < GameDirector.schedule[i].awayScore && GameDirector.schedule[i].awayTeam == (TeamName)sortedTeam[j].teamCode)
                        {
                            if (streak[j] >= 1)
                            {
                                streak[j]++;
                            }
                            else
                            {
                                streak[j] = 1;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject currentLine = GetLineObject(i);
            textArray = currentLine.GetComponentsInChildren<TMP_Text>();
            LogoImages = currentLine.GetComponentsInChildren<Image>();
            currentLine.GetComponent<Image>().color = TeamColor.SetTeamColor((TeamName)sortedTeam[i].teamCode);
            LogoImages[1].sprite = TeamEmblem.GetEmblem((TeamName)sortedTeam[i].teamCode);
            textArray[1].text = DataToString.TeamToString((TeamName)sortedTeam[i].teamCode);
            textArray[2].text = (sortedTeam[i].win + sortedTeam[i].lose + sortedTeam[i].draw).ToString();
            textArray[3].text = (sortedTeam[i].win).ToString();
            textArray[4].text = (sortedTeam[i].lose).ToString();
            textArray[5].text = (sortedTeam[i].draw).ToString();
            textArray[6].text = (sortedTeam[i].WinRate()).ToString("F3");
            textArray[7].text = ((float)((sortedTeam[0].win - sortedTeam[0].lose) - (sortedTeam[i].win - sortedTeam[i].lose)) / 2).ToString();
            if (sortedTeam[i].teamCode == (int)GameDirector.myTeam)
            {
                for (int u = 0; u < 9; u++)
                {
                    textArray[u].color = Color.green;
                }
            }
            string streakText = "기록 없음";
            if (streak[i] <= -2)
            {
                streakText = Mathf.Abs(streak[i]).ToString() + "연패";
            }
            else if (streak[i] == -1)
            {
                streakText = Mathf.Abs(streak[i]).ToString() + "패";
            }
            else if (streak[i] == 1)
            {
                streakText = streak[i].ToString() + "승";
            }
            else if (streak[i] >= 2)
            {
                streakText = streak[i].ToString() + "연승";
            }
            textArray[8].text = streakText;
        }

        CurrentDay = GameObject.Find("CurrentDay").GetComponent<TextMeshProUGUI>();
        CurrentDay.text = GameDirector.currentDate.year.ToString() + "년 " + GameDirector.currentDate.month.ToString() + "월 " + GameDirector.currentDate.day.ToString() + "일 " + DataToString.DayOfWeekToString(GameDirector.currentDate.dayOfWeek);
    }

    GameObject GetLineObject(int index)
    {
        switch (index)
        {
            case 0: return Line1;
            case 1: return Line2;
            case 2: return Line3;
            case 3: return Line4;
            case 4: return Line5;
            case 5: return Line6;
            case 6: return Line7;
            case 7: return Line8;
            case 8: return Line9;
            case 9: return Line10;
            default: return null;
        }
    }
}
