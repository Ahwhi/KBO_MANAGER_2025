using GameData;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScheduleScreen : MonoBehaviour
{
    GameObject[] SchedulePrefabs;
    TMP_Text[] textArray;
    Image[] imageArray;
    public GameObject SchedulePrefab, DatePrefab;
    public Transform content, CalenderPanel;
    public Color WinColor, LoseColor, DrawColor, DefalutColor;
    public Material WinMTR, LoseMTR, DrawMTR;
    public TextMeshProUGUI MonthText;
    private int ViewMonth;
    private TextMeshProUGUI CurrentDay;

    void Start()
    {
        SchedulePrefabs = new GameObject[GameDirector.totalMatchCount];
        GetSchedulePrefab();
        ViewMonth = GameDirector.currentDate.month;
        MonthText.text = ViewMonth.ToString() + "월";
        GetCalenderPrefab();
        CurrentDay = GameObject.Find("CurrentDay").GetComponent<TextMeshProUGUI>();
        CurrentDay.text = GameDirector.currentDate.year.ToString() + "년 " + GameDirector.currentDate.month.ToString() + "월 " + GameDirector.currentDate.day.ToString() + "일 " + DataToString.DayOfWeekToString(GameDirector.currentDate.dayOfWeek);
    }

    void GetCalenderPrefab()
    {
        Date date = new Date(2025, 3, 1, DayOfWeek.SATURDAY);

        // 현재 달 1일까지 이동
        while (date.month < ViewMonth)
        {
            date = CreateData.UpdateDate(date);
        }

        // 1일 앞에 빈 요일은 빈 프리팹 채워넣기
        if (date.day == 1 && date.dayOfWeek != DayOfWeek.SUNDAY)
        {
            for (int i = 0; i < (int)date.dayOfWeek + 1; i++)
            {
                GameObject CurrentPrefab = Instantiate(DatePrefab, CalenderPanel);
                textArray = CurrentPrefab.GetComponentsInChildren<TextMeshProUGUI>();
                imageArray = CurrentPrefab.GetComponentsInChildren<Image>();
                textArray[0].text = "";
                imageArray[0].color = new Color(0f, 0f, 0f, 0f);
                imageArray[1].color = new Color(0f, 0f, 0f, 0f);
            }
        }

        // 3월이면 프리시즌 반영
        if (ViewMonth == 3)
        {
            for (int i = 1; i < 25; i++)
            {
                GameObject CurrentPrefab = Instantiate(DatePrefab, CalenderPanel);
                textArray = CurrentPrefab.GetComponentsInChildren<TextMeshProUGUI>();
                textArray[0].text = "프리시즌";
                textArray[0].color = Color.white;
            }
            date = new Date(2025, 3, 25, DayOfWeek.TUESDAY);
        }

        if (date.dayOfWeek == DayOfWeek.MONDAY)
        {
            GameObject CurrentPrefab2 = Instantiate(DatePrefab, CalenderPanel);
            textArray = CurrentPrefab2.GetComponentsInChildren<TextMeshProUGUI>();
            imageArray = CurrentPrefab2.GetComponentsInChildren<Image>();
            textArray[0].text = "휴식";
            imageArray[0].color = DefalutColor;
            date = CreateData.UpdateDate(date);
        }

        for (int s = 0; s < GameDirector.totalMatchCount; s++)
        {
            if (GameDirector.schedule[s].dates.year == date.year &&
                GameDirector.schedule[s].dates.month == date.month &&
                GameDirector.schedule[s].dates.day == date.day &&
                date.dayOfWeek != DayOfWeek.MONDAY)
            {
                if (GameDirector.schedule[s].homeTeam == GameDirector.myTeam || GameDirector.schedule[s].awayTeam == GameDirector.myTeam)
                {
                    GameObject CurrentPrefab = Instantiate(DatePrefab, CalenderPanel);
                    textArray = CurrentPrefab.GetComponentsInChildren<TextMeshProUGUI>();
                    imageArray = CurrentPrefab.GetComponentsInChildren<Image>();
                    if (GameDirector.schedule[s].homeTeam == GameDirector.myTeam)
                    {
                        imageArray[1].sprite = TeamEmblem.GetEmblem(GameDirector.schedule[s].awayTeam);
                    } else
                    {
                        imageArray[1].sprite = TeamEmblem.GetEmblem(GameDirector.schedule[s].homeTeam);
                    }

                    if (!GameDirector.schedule[s].isEnd)
                    {
                        textArray[0].text = "경기 전";
                        imageArray[0].color = DefalutColor;
                    } else
                    {
                        textArray[0].text = GameDirector.schedule[s].homeScore + " : " + GameDirector.schedule[s].awayScore;
                        if ((GameDirector.schedule[s].homeTeam == GameDirector.myTeam && GameDirector.schedule[s].homeScore > GameDirector.schedule[s].awayScore) ||
                            (GameDirector.schedule[s].awayTeam == GameDirector.myTeam && GameDirector.schedule[s].homeScore < GameDirector.schedule[s].awayScore))
                        {
                            imageArray[0].material = WinMTR;
                            textArray[0].color = Color.cyan;
                        } else if (GameDirector.schedule[s].homeScore == GameDirector.schedule[s].awayScore) {
                            imageArray[0].material = DrawMTR;
                            textArray[0].color = Color.yellow;
                        }
                        else
                        {
                            imageArray[0].material = LoseMTR;
                            textArray[0].color = Color.magenta;
                        }
                    }
                    date = CreateData.UpdateDate(date);
                    if (date.dayOfWeek == DayOfWeek.MONDAY)
                    {
                        if (date.month != 9 && date.day != 8)
                        {
                            GameObject CurrentPrefab2 = Instantiate(DatePrefab, CalenderPanel);
                            textArray = CurrentPrefab2.GetComponentsInChildren<TextMeshProUGUI>();
                            imageArray = CurrentPrefab2.GetComponentsInChildren<Image>();
                            textArray[0].text = "휴식";
                            imageArray[0].color = DefalutColor;
                            date = CreateData.UpdateDate(date);
                        }
                    }
                    if (date.month != ViewMonth)
                    {
                        break;
                    }
                }
            }
        } 
    }

    void GetSchedulePrefab()
    {
        int completedCount = 0;
        for (int s = 0; s < GameDirector.totalMatchCount; s++)
        {
            if (GameDirector.schedule[s].homeTeam == GameDirector.myTeam || GameDirector.schedule[s].awayTeam == GameDirector.myTeam)
            {
                SchedulePrefabs[s] = Instantiate(SchedulePrefab, content);
                textArray = SchedulePrefabs[s].GetComponentsInChildren<TextMeshProUGUI>();
                imageArray = SchedulePrefabs[s].GetComponentsInChildren<Image>();
                textArray[0].text = GameDirector.schedule[s].dates.year.ToString();
                textArray[1].text = GameDirector.schedule[s].dates.month.ToString();
                textArray[2].text = GameDirector.schedule[s].dates.day.ToString();
                textArray[3].text = DataToString.DayOfWeekToString(GameDirector.schedule[s].dates.dayOfWeek);
                if (GameDirector.schedule[s].dates.dayOfWeek == (DayOfWeek)6)
                {
                    textArray[3].color = Color.red;
                }
                else if (GameDirector.schedule[s].dates.dayOfWeek == (DayOfWeek)5)
                {
                    textArray[3].color = Color.blue;
                }
                imageArray[1].sprite = TeamEmblem.GetEmblem(GameDirector.schedule[s].homeTeam);
                imageArray[2].sprite = TeamEmblem.GetEmblem(GameDirector.schedule[s].awayTeam);
                if (GameDirector.schedule[s].isEnd)
                {
                    textArray[4].text = GameDirector.schedule[s].homeScore + ":" + GameDirector.schedule[s].awayScore;
                    if (GameDirector.schedule[s].homeScore > GameDirector.schedule[s].awayScore && GameDirector.schedule[s].homeTeam == GameDirector.myTeam)
                    {
                        SchedulePrefabs[s].GetComponent<Image>().color = WinColor;
                        textArray[4].text += " 승";
                        textArray[4].color = Color.cyan;
                    } else if (GameDirector.schedule[s].homeScore < GameDirector.schedule[s].awayScore && GameDirector.schedule[s].awayTeam == GameDirector.myTeam)
                        {
                            SchedulePrefabs[s].GetComponent<Image>().color = WinColor;
                        textArray[4].text += " 승";
                        textArray[4].color = Color.cyan;
                    }
                    else if (GameDirector.schedule[s].homeScore == GameDirector.schedule[s].awayScore)
                    {
                        SchedulePrefabs[s].GetComponent<Image>().color = DrawColor;
                        textArray[4].text += " 무";
                        textArray[4].color = Color.yellow;
                    } else
                    {
                        SchedulePrefabs[s].GetComponent<Image>().color = LoseColor;
                        textArray[4].text += " 패";
                        textArray[4].color = Color.magenta;
                    }
                    completedCount++;
                } else
                {
                    SchedulePrefabs[s].GetComponent<Image>().color = DefalutColor;
                    textArray[4].text = "-";
                }
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        float scrollPosition = (float)completedCount / 144f;
        scrollRect.verticalNormalizedPosition = 1f - scrollPosition;
    }
}
