using GameData;
using MailData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    private TextMeshProUGUI CurrentDay, Match1Text, Match2Text, Match3Text, Match4Text, Match5Text;
    public GameObject NewsInfoPanel, SavePanel, HomeSPPanel, AwaySPPanel, CannotPanel;
    public TextMeshProUGUI TodayHomeSP, TodayAwaySP, CurrentRank, ResentResult, ResentScore, MailResult, SeasonText;
    public Image M1L, M1R, M2L, M2R, M3L, M3R, M4L, M4R, M5L, M5R;
    public Image MyImage, EnemyImage, HomeSPImage, AwaySPImage;
    public Color CHighLight;
    public Color CBasic;
    public GameObject mailPrefab;
    public Transform content;
    public GridLayoutGroup TodayMatchLayoutGroup;
    public static bool isUpdateMain = false;
    public static bool isCannot = false;
    Schedule ResentScedule;

    void Start()
    {
        isUpdateMain = true;
        RectTransform rectTransform = TodayMatchLayoutGroup.GetComponent<RectTransform>();
        float panelHeight = rectTransform.rect.height;
        TodayMatchLayoutGroup.cellSize = new Vector2(TodayMatchLayoutGroup.cellSize.x, panelHeight / 8);
    }

    void Update()
    {
        if (isUpdateMain)
        {
            SetUI();
            isUpdateMain = false;
        }
        if (isCannot)
        {
            CannotPanel.SetActive(true);
            isCannot = false;
        }
    }

    public void OnSave()
    {
        SavePanel.SetActive(true);
    }

    public void OnSaveConfirm()
    {
        SavePanel.SetActive(false);
        SaveLoad.SaveData();
    }

    public void OnSaveCancle()
    {
        SavePanel.SetActive(false);
    }

    public void LoadMail()
    {
        int UnreadM = 0;
        int ReadM = 0;
        // �̸� ������ �������� ����
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("Mail");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        // ����
        for (int i = 0; i < GameDirector.mail.Count; i++)
        {
            Mail currentMail = GameDirector.mail[i];
            GenerateMailPrefab(currentMail);
            if (currentMail.isRead) {
                ReadM++;
            } else
            {
                UnreadM++;
            }
        }
        if (ReadM == 0 && UnreadM == 0)
        {
            MailResult.text = "�ҽ��� �����ϴ�.";
        }
        else if (ReadM != 0 && UnreadM == 0)
        {
            MailResult.text = "��� �ҽ��� �о����ϴ�.";
        }
        else
        {
            MailResult.text = UnreadM.ToString() + "���� ���� ���� �ҽ��� �ֽ��ϴ�.";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void GenerateMailPrefab(Mail newMail)
    {
        GameObject mailItem = Instantiate(mailPrefab, content);
        TMP_Text[] texts = mailItem.GetComponentsInChildren<TMP_Text>();
        Button mailButton = mailItem.GetComponentInChildren<Button>();
        Image[] ReadIcon = mailItem.GetComponentsInChildren<Image>();
        mailButton.onClick.AddListener(() => OnMailButtonClick(newMail));
        texts[0].text = newMail.Title;
        texts[1].text = newMail.Sender;
        if (newMail.isRead)
        {
            ReadIcon[1].sprite = Resources.Load<Sprite>("Mail/Read");
        }
    }

    private void OnMailButtonClick(Mail clickedMail)
    {
        NewsInfoPanel.SetActive(true);
        clickedMail.isRead = true;
        TMP_Text[] textArray;
        textArray = NewsInfoPanel.GetComponentsInChildren<TextMeshProUGUI>();
        textArray[0].text = clickedMail.Title;
        textArray[1].text = clickedMail.Detail;
        textArray[2].text = $"{clickedMail.Dates.year}�� {clickedMail.Dates.month}�� {clickedMail.Dates.day}�� {DataToString.DayOfWeekToString(clickedMail.Dates.dayOfWeek)} {clickedMail.Sender} �帲";
        Button[] buttonArray = NewsInfoPanel.GetComponentsInChildren<Button>();
        buttonArray[0].onClick.RemoveAllListeners();
        buttonArray[0].onClick.AddListener(() => OnMailRemove(clickedMail));
    }

    public void OnOK()
    {
        CannotPanel.SetActive(false);
    }

    public void OnMailRemove(Mail clickedMail)
    {
        GameDirector.mail.Remove(clickedMail);
        SceneManager.LoadScene("Main");
    }

    public void OnMailReturn()
    {
        NewsInfoPanel.SetActive(false);
        SetUI();
    }

    public void SetUI()
    {
        ResentResult.text = "";
        List<Team> sortedTeam = new List<Team>(GameDirector.Teams);
        sortedTeam.Sort((team1, team2) => team2.WinRate().CompareTo(team1.WinRate()));
        for (int i = 0; i <10; i++)
        {
            if (sortedTeam[i].teamCode == (int)GameDirector.myTeam)
            {
                CurrentRank.text = "[" + (i+1) + "��] " + sortedTeam[i].win + "�� " + sortedTeam[i].lose + "�� " + sortedTeam[i].draw + "�� " + sortedTeam[i].WinRate().ToString("F3");
                break;
            }
        }

        CurrentDay = GameObject.Find("CurrentDay").GetComponent<TextMeshProUGUI>();
        Match1Text = GameObject.Find("Match1Text").GetComponent<TextMeshProUGUI>();
        Match2Text = GameObject.Find("Match2Text").GetComponent<TextMeshProUGUI>();
        Match3Text = GameObject.Find("Match3Text").GetComponent<TextMeshProUGUI>();
        Match4Text = GameObject.Find("Match4Text").GetComponent<TextMeshProUGUI>();
        Match5Text = GameObject.Find("Match5Text").GetComponent<TextMeshProUGUI>();
        CurrentDay.text = GameDirector.currentDate.year.ToString() + "�� " + GameDirector.currentDate.month.ToString() + "�� " + GameDirector.currentDate.day.ToString() + "�� " + DataToString.DayOfWeekToString(GameDirector.currentDate.dayOfWeek);
        CurrentDay.color = Color.white;

        int todayGameCount = 0;
        int textCount = 0;
        for (int t = 0; t < GameDirector.totalMatchCount; t++)
        {
            // �ֱ� ���
            if (GameDirector.schedule[t].isEnd && (GameDirector.schedule[t].homeTeam == GameDirector.myTeam || GameDirector.schedule[t].awayTeam == GameDirector.myTeam))
            {
                ResentScedule = GameDirector.schedule[t];
            }

            // ������ ���
            if (GameDirector.schedule[t].dates.year == GameDirector.currentDate.year &&
                GameDirector.schedule[t].dates.month == GameDirector.currentDate.month &&
                GameDirector.schedule[t].dates.day == GameDirector.currentDate.day)
            {
                if (GameDirector.schedule[t].homeTeam == GameDirector.myTeam || GameDirector.schedule[t].awayTeam == GameDirector.myTeam)
                {
                    Match1Text.color = CHighLight;
                    Match1Text.text = "(Ȩ) " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " vs " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " (����)";
                    M1L.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
                    M1R.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);

                    // ������ ����
                    HomeSPImage.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
                    AwaySPImage.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);
                    for (int p = 0; p < GameDirector.pitcherCount; p++)
                    {
                        if (GameDirector.pitcher[p].team == GameDirector.schedule[t].homeTeam && GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].currentSP == GameDirector.pitcher[p].posInTeam)
                        {
                            TodayHomeSP.text = GameDirector.pitcher[p].name.ToString() + " (" + GameDirector.pitcher[p].win.ToString() + "�� " + GameDirector.pitcher[p].lose + "�� " + GameDirector.pitcher[p].earnedRunAverage.ToString("F2") + ")";
                            HomeSPPanel.GetComponent<Image>().color = TeamColor.SetTeamColor(GameDirector.pitcher[p].team);
                        }
                        if (GameDirector.pitcher[p].team == GameDirector.schedule[t].awayTeam && GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].currentSP == GameDirector.pitcher[p].posInTeam)
                        {
                            TodayAwaySP.text = GameDirector.pitcher[p].name.ToString() + " (" + GameDirector.pitcher[p].win.ToString() + "�� " + GameDirector.pitcher[p].lose + "�� " + GameDirector.pitcher[p].earnedRunAverage.ToString("F2") + ")";
                            AwaySPPanel.GetComponent<Image>().color = TeamColor.SetTeamColor(GameDirector.pitcher[p].team);
                        }
                    }
                }
                else
                {
                    textCount++;
                    if (textCount == 1)
                    {
                        Match2Text.text = "(Ȩ) " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " vs " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " (����)";
                        M2L.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
                        M2R.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);
                    }
                    else if (textCount == 2)
                    {
                        Match3Text.text = "(Ȩ) " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " vs " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " (����)";
                        M3L.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
                        M3R.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);
                    }
                    else if (textCount == 3)
                    {
                        Match4Text.text = "(Ȩ) " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " vs " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " (����)";
                        M4L.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
                        M4R.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);
                    }
                    else if (textCount == 4)
                    {
                        Match5Text.text = "(Ȩ) " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " vs " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " (����)";
                        M5L.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
                        M5R.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);
                    }
                }
                todayGameCount++;
            }
        }

        // ����Ʈ ����
        if (GameDirector.isPostSeason)
        {
            for (int t = 0; t < 19; t++)
            {
                // �ֱ� ���
                if (GameDirector.postSchedule[t].isEnd && (GameDirector.postSchedule[t].homeTeam == GameDirector.myTeam || GameDirector.postSchedule[t].awayTeam == GameDirector.myTeam))
                {
                    ResentScedule = GameDirector.postSchedule[t];
                }

                // ������ ���
                if (GameDirector.postSchedule[t].dates.year == GameDirector.currentDate.year &&
                    GameDirector.postSchedule[t].dates.month == GameDirector.currentDate.month &&
                    GameDirector.postSchedule[t].dates.day == GameDirector.currentDate.day &&
                    !GameDirector.postSchedule[t].isPass)
                {
                    if (GameDirector.postSchedule[t].homeTeam == GameDirector.myTeam || GameDirector.postSchedule[t].awayTeam == GameDirector.myTeam)
                    {
                        Match1Text.color = CHighLight;
                        // ������ ����
                        HomeSPImage.sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[t].homeTeam);
                        AwaySPImage.sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[t].awayTeam);
                        for (int p = 0; p < GameDirector.pitcherCount; p++)
                        {
                            if (GameDirector.pitcher[p].team == GameDirector.postSchedule[t].homeTeam && GameDirector.Teams[(int)GameDirector.postSchedule[t].homeTeam].currentSP == GameDirector.pitcher[p].posInTeam)
                            {
                                TodayHomeSP.text = GameDirector.pitcher[p].name.ToString() + " (" + GameDirector.pitcher[p].win.ToString() + "�� " + GameDirector.pitcher[p].lose + "�� " + GameDirector.pitcher[p].earnedRunAverage.ToString("F2") + ")";
                                HomeSPPanel.GetComponent<Image>().color = TeamColor.SetTeamColor(GameDirector.pitcher[p].team);
                            }
                            if (GameDirector.pitcher[p].team == GameDirector.postSchedule[t].awayTeam && GameDirector.Teams[(int)GameDirector.postSchedule[t].awayTeam].currentSP == GameDirector.pitcher[p].posInTeam)
                            {
                                TodayAwaySP.text = GameDirector.pitcher[p].name.ToString() + " (" + GameDirector.pitcher[p].win.ToString() + "�� " + GameDirector.pitcher[p].lose + "�� " + GameDirector.pitcher[p].earnedRunAverage.ToString("F2") + ")";
                                AwaySPPanel.GetComponent<Image>().color = TeamColor.SetTeamColor(GameDirector.pitcher[p].team);
                            }
                        }

                    } else
                    {
                        Match1Text.color = Color.white;
                    }
                    if (t < 2)
                    {
                        SeasonText.text = "������ ���(���ϵ�ī��)";
                    } else if (t >= 2 && t <= 6)
                    {
                        SeasonText.text = "������ ���(���÷��̿���)";
                    } else if (t >= 7 && t <= 11)
                    {
                        SeasonText.text = "������ ���(�÷��̿���)";
                    } else if (t >= 12 && t <= 18)
                    {
                        SeasonText.text = "������ ���(�ѱ��ø���)";
                    }
                    Match1Text.text = "(Ȩ) " + DataToString.TeamToString(GameDirector.postSchedule[t].homeTeam) + " vs " + DataToString.TeamToString(GameDirector.postSchedule[t].awayTeam) + " (����)";
                    M1L.sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[t].homeTeam);
                    M1R.sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[t].awayTeam);
                    Destroy(M2L.gameObject);
                    Destroy(M2L.gameObject);
                    Destroy(M2R.gameObject);
                    Destroy(M3L.gameObject);
                    Destroy(M3R.gameObject);
                    Destroy(M4L.gameObject);
                    Destroy(M4R.gameObject);
                    Destroy(M5L.gameObject);
                    Destroy(M5R.gameObject);
                    Match2Text.text = "";
                    Match3Text.text = "";
                    Match4Text.text = "";
                    Match5Text.text = "";
                    todayGameCount++;
                }
            }
        }

        MyImage.sprite = TeamEmblem.GetEmblem(GameDirector.myTeam);
        if (ResentScedule != null)
        {
            if (ResentScedule.homeScore > ResentScedule.awayScore && ResentScedule.homeTeam == GameDirector.myTeam)
            {
                ResentResult.color = Color.green;
                ResentResult.text = "�¸�";
                ResentScore.text = ResentScedule.homeScore + ":" + ResentScedule.awayScore;
                EnemyImage.sprite = TeamEmblem.GetEmblem(ResentScedule.awayTeam);
            }
            else if (ResentScedule.homeScore < ResentScedule.awayScore && ResentScedule.awayTeam == GameDirector.myTeam)
            {
                ResentResult.color = Color.green;
                ResentResult.text = "�¸�";
                EnemyImage.sprite = TeamEmblem.GetEmblem(ResentScedule.homeTeam);
                ResentScore.text = ResentScedule.awayScore + ":" + ResentScedule.homeScore;
            }
            else if (ResentScedule.homeScore == ResentScedule.awayScore)
            {
                ResentResult.color = Color.yellow;
                ResentResult.text = "��";
                if (ResentScedule.homeTeam == GameDirector.myTeam)
                {
                    EnemyImage.sprite = TeamEmblem.GetEmblem(ResentScedule.awayTeam);
                    ResentScore.text = ResentScedule.homeScore + ":" + ResentScedule.awayScore;
                }
                else
                {
                    EnemyImage.sprite = TeamEmblem.GetEmblem(ResentScedule.homeTeam);
                    ResentScore.text = ResentScedule.awayScore + ":" + ResentScedule.homeScore;
                }
            }
            else
            {
                ResentResult.color = Color.red;
                ResentResult.text = "�й�";
                if (ResentScedule.homeTeam == GameDirector.myTeam)
                {
                    EnemyImage.sprite = TeamEmblem.GetEmblem(ResentScedule.awayTeam);
                    ResentScore.text = ResentScedule.homeScore + ":" + ResentScedule.awayScore;
                }
                else
                {
                    EnemyImage.sprite = TeamEmblem.GetEmblem(ResentScedule.homeTeam);
                    ResentScore.text = ResentScedule.awayScore + ":" + ResentScedule.homeScore;
                }
            }
        } else // ù�����϶� 
        {
            MyImage.sprite = TeamEmblem.GetEmblem((TeamName)999);
            EnemyImage.sprite = TeamEmblem.GetEmblem((TeamName)999);
            ResentScore.text = "";
        }
        

        if (todayGameCount == 0)
        {
            HomeSPImage.sprite = TeamEmblem.GetEmblem((TeamName)999);
            AwaySPImage.sprite = TeamEmblem.GetEmblem((TeamName)999);
            HomeSPPanel.GetComponent<Image>().color = Color.gray;
            AwaySPPanel.GetComponent<Image>().color = Color.gray;
            M1L.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M1R.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M2L.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M2R.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M3L.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M3R.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M4L.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M4R.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M5L.sprite = TeamEmblem.GetEmblem((TeamName)999);
            M5R.sprite = TeamEmblem.GetEmblem((TeamName)999);
            TodayHomeSP.text = "��Ⱑ �����ϴ�.";
            TodayAwaySP.text = "��Ⱑ �����ϴ�.";
            Match1Text.color = CBasic;
            Match1Text.text = "��Ⱑ �����ϴ�.";
            Match2Text.text = "";
            Match3Text.text = "";
            Match4Text.text = "";
            Match5Text.text = "";
        }
        LoadMail();
    }

}
