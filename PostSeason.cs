using GameData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PostSeason : MonoBehaviour
{
    private TextMeshProUGUI CurrentDay;
    public Image[] Emblems;
    public Image KingEmblem;
    public TextMeshProUGUI[] Texts;

    void Start()
    {
        CurrentDay = GameObject.Find("CurrentDay").GetComponent<TextMeshProUGUI>();
        CurrentDay.text = GameDirector.currentDate.year.ToString() + "년 " + GameDirector.currentDate.month.ToString() + "월 " + GameDirector.currentDate.day.ToString() + "일 " + DataToString.DayOfWeekToString(GameDirector.currentDate.dayOfWeek);
        Emblems[0].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[0].DownTeam);
        Emblems[1].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[0].UpTeam);
        Emblems[3].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[2].UpTeam);
        Emblems[5].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[7].UpTeam);
        Emblems[7].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[12].UpTeam);
        if (GameDirector.postSchedule[1].isEnd || GameDirector.postSchedule[1].isPass)
        {
            if (GameDirector.postSchedule[2].DownTeam == GameDirector.postSchedule[0].UpTeam)
            {
                var image = Emblems[0];
                Color color = image.color;
                color.a = 33/255f;
                image.color = color;

            } else
            {
                var image = Emblems[1];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;
            }
            Emblems[2].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[2].DownTeam);
        }
        if (GameDirector.postSchedule[6].isEnd || GameDirector.postSchedule[6].isPass)
        {
            if (GameDirector.postSchedule[7].DownTeam == GameDirector.postSchedule[2].UpTeam)
            {
                var image = Emblems[2];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;

            }
            else
            {
                var image = Emblems[3];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;
            }
            Emblems[4].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[7].DownTeam);
        }
        if (GameDirector.postSchedule[11].isEnd || GameDirector.postSchedule[11].isPass)
        {
            if (GameDirector.postSchedule[12].DownTeam == GameDirector.postSchedule[7].UpTeam)
            {
                var image = Emblems[4];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;

            }
            else
            {
                var image = Emblems[5];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;
            }
            Emblems[6].sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[12].DownTeam);
        }
        if (GameDirector.postSchedule[18].isEnd || GameDirector.postSchedule[18].isPass)
        {
            if (GameDirector.KingTeam == GameDirector.postSchedule[12].UpTeam)
            {
                var image = Emblems[6];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;

            }
            else
            {
                var image = Emblems[7];
                Color color = image.color;
                color.a = 33 / 255f;
                image.color = color;
            }
            KingEmblem.sprite = TeamEmblem.GetEmblem(GameDirector.KingTeam);
        }
        int k = 0;
        for (int i = 0; i < 19; i++)
        {
            if (i == 2 || i == 7 || i == 12)
            {
                k = 0;
            }
            if (GameDirector.postSchedule[i].isPass)
            {
                Texts[i].text = "";
            }

            if (GameDirector.postSchedule[i].isEnd && !GameDirector.postSchedule[i].isPass)
            {
                Texts[i].text = GameDirector.postSchedule[i].dates.month.ToString() + "/" + GameDirector.postSchedule[i].dates.day.ToString() + " " + (++k).ToString() + "차전 " + GameDirector.postSchedule[i].homeScore.ToString() + " : " + GameDirector.postSchedule[i].awayScore.ToString();
                if (GameDirector.postSchedule[i].homeScore > GameDirector.postSchedule[i].awayScore)
                {
                    Texts[i].color = TeamColor.SetTeamColor(GameDirector.postSchedule[i].homeTeam);
                } else
                {
                    Texts[i].color = TeamColor.SetTeamColor(GameDirector.postSchedule[i].awayTeam);
                }

            } else if (!GameDirector.postSchedule[i].isEnd && !GameDirector.postSchedule[i].isPass)
            {
                Texts[i].text = GameDirector.postSchedule[i].dates.month.ToString() + "/" + GameDirector.postSchedule[i].dates.day.ToString() + " " + (++k).ToString() + "차전";
            }
            
        }
        
    }
}
