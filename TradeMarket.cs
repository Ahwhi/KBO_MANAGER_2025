using GameData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradeMarket : MonoBehaviour
{
    public Button[] TeamButtons;
    public Button EnemyPitButton, EnemyBatButton, MyPitButton, MyBatButton, SendButton;
    public GameObject PopUpCanvas, EnemyContent, MyContent, EnemyLContent, MyLContent, TradeListPrefab, TradeCandidatePrefab, EnemyTooltipB, EnemyTooltipP, MyTooltipB, MyTooltipP, EnemyC, MyC;
    public Image MyImage, EnemyImage;
    public Material DefaultMaterial;
    public Slider ProcessSlider;
    public TextMeshProUGUI ProcessText, Moneytext;
    bool isEnemyPB, isMyPB; // True = 투수, False = 타자
    int enemyCandidateP = 0;
    int myCandidateP = 0;
    int enemyCandidateB = 0;
    int myCandidateB = 0;
    int myBatterCount = 0;
    int myPitcherCount = 0;
    int enemyBatterCount = 0;
    int enemyPitcherCount = 0;
    Transform EnemyContentTransform, MyContentTransform, EnemyContentLTransform, MyContentLTransform;
    Color EnemyColor, MyColor;
    TMP_Text[] textArray;
    TeamName currentTeam;


    void Start()
    {
        Moneytext.text = "예산: " + GameDirector.money.ToString("N0") + "원";
        int MyTeam = (int)GameDirector.myTeam;
        int unCheck = -1;
        for (int i = 0; i < 9; i++)
        {
            if (GameDirector.Teams[i].teamCode != MyTeam)
            {
                TeamName teamName = (TeamName)i;
                Image[] TeamImage = TeamButtons[i].GetComponentsInChildren<Image>();
                TeamImage[0].color = TeamColor.SetTeamColor((TeamName)GameDirector.Teams[i].teamCode);
                TeamImage[1].sprite = TeamEmblem.GetEmblem((TeamName)GameDirector.Teams[i].teamCode);
                TeamButtons[i].onClick.AddListener(() => OnSelectTeam((TeamName)teamName));
            } else
            {
                unCheck = i;
            }
        }
        if (unCheck != -1)
        {
            Image[] TeamImage = TeamButtons[unCheck].GetComponentsInChildren<Image>();
            TeamImage[0].color = TeamColor.SetTeamColor((TeamName)GameDirector.Teams[9].teamCode);
            TeamImage[1].sprite = TeamEmblem.GetEmblem((TeamName)GameDirector.Teams[9].teamCode);
            TeamButtons[unCheck].onClick.AddListener(() => OnSelectTeam((TeamName)GameDirector.Teams[9].teamCode));
        }
        isEnemyPB = true;
        isMyPB = true;
    }

    public void OnEnemyPitButton()
    {
        isEnemyPB = true;
        OnSelectTeam(currentTeam);
    }

    public void OnEnemyBatButton()
    {
        isEnemyPB = false;
        OnSelectTeam(currentTeam);
    }

    public void OnMyPitButton()
    {
        isMyPB = true;
        OnSelectTeam(currentTeam);
    }

    public void OnMyBatButton()
    {
        isMyPB = false;
        OnSelectTeam(currentTeam);
    }

    public void OnResetButton()
    {
        for (int i = 0; i < GameDirector.pitcherCount; i++)
        {
            GameDirector.pitcher[i].isTrade = false;
        }
        for (int i = 0; i < GameDirector.batterCount; i++)
        {
            GameDirector.batter[i].isTrade = false;
        }
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("TradeCandidate");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
        PopUpCanvas.SetActive(false);
        enemyCandidateP = 0;
        myCandidateP = 0;
        enemyCandidateB = 0;
        myCandidateB = 0;
        myBatterCount = 0;
        myPitcherCount = 0;
        enemyBatterCount = 0;
        enemyPitcherCount = 0;
        OnSelectTeam(currentTeam);
    }

    public void OnSendButton()
    {
        if (GetAvaliableTrade())
        {
            List<Pitcher> mySelectedPitchers = new List<Pitcher>();
            List<Batter> mySelectedBatters = new List<Batter>();
            List<Pitcher> enemySelectedPitchers = new List<Pitcher>();
            List<Batter> enemySelectedBatters = new List<Batter>();

            foreach (var pitcher in GameDirector.pitcher)
            {
                if (pitcher.isTrade && pitcher.team == GameDirector.myTeam)
                {
                    mySelectedPitchers.Add(pitcher);
                }
            }
            foreach (var batter in GameDirector.batter)
            {
                if (batter.isTrade && batter.team == GameDirector.myTeam)
                {
                    mySelectedBatters.Add(batter);
                }
            }
            foreach (var pitcher in GameDirector.pitcher)
            {
                if (pitcher.isTrade && pitcher.team == currentTeam)
                {
                    enemySelectedPitchers.Add(pitcher);
                }
            }
            foreach (var batter in GameDirector.batter)
            {
                if (batter.isTrade && batter.team == currentTeam)
                {
                    enemySelectedBatters.Add(batter);
                }
            }

            foreach (var pitcher in mySelectedPitchers)
            {
                pitcher.team = currentTeam; // 상대팀으로 트레이드
                pitcher.isTrade = false; // 트레이드 상태 해제
            }
            foreach (var batter in mySelectedBatters)
            {
                batter.team = currentTeam; // 상대팀으로 트레이드
                batter.isTrade = false; // 트레이드 상태 해제
            }

            foreach (var pitcher in enemySelectedPitchers)
            {
                pitcher.team = GameDirector.myTeam; // 내 팀으로 트레이드
                pitcher.isTrade = false; // 트레이드 상태 해제
            }
            foreach (var batter in enemySelectedBatters)
            {
                batter.team = GameDirector.myTeam; // 내 팀으로 트레이드
                batter.isTrade = false; // 트레이드 상태 해제
            }

            int newPos = 1;
            for (int u = 0; u < GameDirector.pitcherCount; u++)
            {
                if (GameDirector.pitcher[u].team == GameDirector.myTeam)
                {
                    GameDirector.pitcher[u].posInTeam = newPos++;
                }
            }
            newPos = 1;
            for (int u = 0; u < GameDirector.pitcherCount; u++)
            {
                if (GameDirector.pitcher[u].team == currentTeam)
                {
                    GameDirector.pitcher[u].posInTeam = newPos++;
                }
            }
            newPos = 101;
            for (int u = 0; u < GameDirector.batterCount; u++)
            {
                if (GameDirector.batter[u].team == GameDirector.myTeam)
                {
                    GameDirector.batter[u].posInTeam = newPos++;
                }
            }
            newPos = 101;
            for (int u = 0; u < GameDirector.batterCount; u++)
            {
                if (GameDirector.batter[u].team == currentTeam)
                {
                    GameDirector.batter[u].posInTeam = newPos++;
                }
            }
            myCandidateP = 0;
            enemyCandidateP = 0;
            myCandidateB = 0;
            enemyCandidateB = 0;
            SceneDirector.GoToScene("Main");
        }
    }

    public void OnCancleButton()
    {
        for (int i = 0; i < GameDirector.pitcherCount; i++)
        {
            GameDirector.pitcher[i].isTrade = false;
        }
        for (int i = 0; i < GameDirector.batterCount; i++)
        {
            GameDirector.batter[i].isTrade = false;
        }
        SceneDirector.GoToScene("TradeMarket");
    }

    bool GetAvaliableTrade()
    {
        int MyALL = 0;
        int EnemyALL = 0;

        foreach (var pitcher in GameDirector.pitcher)
        {
            if (pitcher.isTrade && pitcher.team == GameDirector.myTeam)
            {
                MyALL += pitcher.SPEED;
                MyALL += pitcher.COMMAND;
                MyALL += pitcher.BREAKING;
            }
        }
        foreach (var batter in GameDirector.batter)
        {
            if (batter.isTrade && batter.team == GameDirector.myTeam)
            {
                MyALL += batter.POWER;
                MyALL += batter.CONTACT;
                MyALL += batter.EYE;
            }
        }
        foreach (var pitcher in GameDirector.pitcher)
        {
            if (pitcher.isTrade && pitcher.team == currentTeam)
            {
                EnemyALL += pitcher.SPEED;
                EnemyALL += pitcher.COMMAND;
                EnemyALL += pitcher.BREAKING;
            }
        }
        foreach (var batter in GameDirector.batter)
        {
            if (batter.isTrade && batter.team == currentTeam)
            {
                EnemyALL += batter.POWER;
                EnemyALL += batter.CONTACT;
                EnemyALL += batter.EYE;
            }
        }

        if (myCandidateB + myCandidateP != 0 && enemyCandidateB + enemyCandidateP != 0)
        {
            if (MyALL - EnemyALL <= -3)
            {
                ProcessSlider.value = 0;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.red;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.red;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == -2)
            {
                ProcessSlider.value = 10;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.red;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.red;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == -1)
            {
                ProcessSlider.value = 20;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.red;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.red;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 0 && myCandidateB + myCandidateP != 0 && enemyCandidateB + enemyCandidateP != 0)
            {
                ProcessSlider.value = 30;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.red;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.red;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 1)
            {
                ProcessSlider.value = 40;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.red;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.red;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 2)
            {
                ProcessSlider.value = 50;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.yellow;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 3)
            {
                ProcessSlider.value = 60;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.yellow;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 4)
            {
                ProcessSlider.value = 70;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.yellow;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 5)
            {
                ProcessSlider.value = 80;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.yellow;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL == 6)
            {
                ProcessSlider.value = 90;
                ProcessSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                ProcessText.text = "추가 선수 요구";
                ProcessText.color = Color.yellow;
                SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                return false;
            }
            else if (MyALL - EnemyALL >= 7)
            {
                if (myBatterCount + enemyCandidateB >= 16 &&
                    myPitcherCount + enemyCandidateP >= 14 &&
                    enemyBatterCount + myCandidateB >= 16 &&
                    enemyPitcherCount + myCandidateP >= 14 &&
                    myBatterCount + enemyCandidateB <= 30 &&
                    myPitcherCount + enemyCandidateP <= 30 &&
                    enemyBatterCount + myCandidateB <= 30 &&
                    enemyPitcherCount + myCandidateP <= 30 &&
                    myCandidateB + myCandidateP <= 5 && // 각 팀 5명제한
                    enemyCandidateB + enemyCandidateP <= 5)
                {
                    ProcessSlider.value = 100;
                    ProcessSlider.fillRect.GetComponent<Image>().color = Color.green;
                    ProcessText.text = "제안 가능";
                    ProcessText.color = Color.cyan;
                    SceneDirector.SetButton(SendButton, DefaultMaterial, true);
                    return true;
                }
                else
                {
                    ProcessText.text = "제안 불가 (KBO 룰 위반)";
                    ProcessText.color = Color.red;
                    ProcessSlider.value = 0;
                    ProcessSlider.fillRect.GetComponent<Image>().color = Color.red;
                    SceneDirector.SetButton(SendButton, DefaultMaterial, false);
                    return false;
                }
            }
        } else
        {
            ProcessText.text = "제안 시작";
            ProcessText.color = Color.gray;
            ProcessSlider.value = 0;
            ProcessSlider.fillRect.GetComponent<Image>().color = Color.gray;
            SceneDirector.SetButton(SendButton, DefaultMaterial, false);
            return false;
        }
        ProcessText.text = "제안 불가 (이유를 알 수 없음)";
        ProcessText.color = Color.red;
        ProcessSlider.value = 0;
        SceneDirector.SetButton(SendButton, DefaultMaterial, false);
        return false;
    }

    public void OnSelectPitcher(Pitcher pitcher, GameObject prefab)
    {
        pitcher.isTrade = true;
        if (pitcher.team == GameDirector.myTeam)
        {
            GameObject currentPrefab = Instantiate(TradeCandidatePrefab, MyContentLTransform);
            int OVR = pitcher.SPEED + pitcher.COMMAND + pitcher.BREAKING;
            currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text = pitcher.name;
            if (OVR <= 5)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

            }
            else if (OVR >= 6 && OVR <= 10)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
            }
            else if (OVR >= 11 && OVR <= 14)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
            }
            else if (OVR >= 15)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★★</color>";
            }
            
            currentPrefab.GetComponent<Image>().color = MyColor;
            myCandidateP++;
            myPitcherCount--;
        } else
        {
            GameObject currentPrefab = Instantiate(TradeCandidatePrefab, EnemyContentLTransform);
            int OVR = pitcher.SPEED + pitcher.COMMAND + pitcher.BREAKING;
            currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text = pitcher.name;
            if (OVR <= 5)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

            }
            else if (OVR >= 6 && OVR <= 10)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
            }
            else if (OVR >= 11 && OVR <= 14)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
            }
            else if (OVR >= 15)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★★</color>";
            }
            currentPrefab.GetComponent<Image>().color = EnemyColor;
            enemyCandidateP++;
            enemyPitcherCount--;
        }
        Destroy(prefab);
        GetAvaliableTrade();
    }

    public void OnSelectBatter(Batter batter, GameObject prefab)
    {
        batter.isTrade = true;
        if (batter.team == GameDirector.myTeam)
        {
            GameObject currentPrefab = Instantiate(TradeCandidatePrefab, MyContentLTransform);
            currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text = batter.name;
            int OVR = batter.POWER + batter.CONTACT + batter.EYE;
            if (OVR <= 5)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

            }
            else if (OVR >= 6 && OVR <= 10)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
            }
            else if (OVR >= 11 && OVR <= 14)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
            }
            else if (OVR >= 15)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★★</color>";
            }
            currentPrefab.GetComponent<Image>().color = MyColor;
            myCandidateB++;
            myBatterCount--;
        }
        else
        {
            GameObject currentPrefab = Instantiate(TradeCandidatePrefab, EnemyContentLTransform);
            currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text = batter.name;
            int OVR = batter.POWER + batter.CONTACT + batter.EYE;
            if (OVR <= 5)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

            }
            else if (OVR >= 6 && OVR <= 10)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
            }
            else if (OVR >= 11 && OVR <= 14)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
            }
            else if (OVR >= 15)
            {
                currentPrefab.GetComponentInChildren<TextMeshProUGUI>().text += " <color=#FFFF00>★★★★</color>";
            }
            currentPrefab.GetComponent<Image>().color = EnemyColor;
            enemyCandidateB++;
            enemyBatterCount--;
        }
        Destroy(prefab);
        GetAvaliableTrade();
    }

    public void OnSelectTeam(TeamName teamName)
    {
        currentTeam = teamName;
        EnemyColor = TeamColor.SetTeamColor(teamName);
        MyColor = TeamColor.SetTeamColor(GameDirector.myTeam);
        PopUpCanvas.SetActive(true);

        EnemyC.GetComponent<Image>().color = EnemyColor;
        MyC.GetComponent<Image>().color = MyColor;

        MyImage.sprite = TeamEmblem.GetEmblem(GameDirector.myTeam);
        EnemyImage.sprite = TeamEmblem.GetEmblem(teamName);
        ProcessSlider.value = 0;

        GetAvaliableTrade();

        EnemyContentTransform = EnemyContent.GetComponent<Transform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(EnemyContent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect1 = EnemyContent.GetComponentInParent<ScrollRect>();
        scrollRect1.verticalNormalizedPosition = 1f;

        MyContentTransform = MyContent.GetComponent<Transform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(MyContent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect2 = MyContent.GetComponentInParent<ScrollRect>();
        scrollRect2.verticalNormalizedPosition = 1f;

        EnemyContentLTransform = EnemyLContent.GetComponent<Transform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(EnemyLContent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect3 = EnemyLContent.GetComponentInParent<ScrollRect>();
        scrollRect3.verticalNormalizedPosition = 1f;

        MyContentLTransform = MyLContent.GetComponent<Transform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(MyLContent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect4 = MyLContent.GetComponentInParent<ScrollRect>();
        scrollRect4.verticalNormalizedPosition = 1f;

        if (isEnemyPB)
        {
            EnemyPitButton.interactable = false;
            EnemyBatButton.interactable = true;
            EnemyTooltipP.SetActive(true);
            EnemyTooltipB.SetActive(false);
        } else
        {
            EnemyPitButton.interactable = true;
            EnemyBatButton.interactable = false;
            EnemyTooltipP.SetActive(false);
            EnemyTooltipB.SetActive(true);
        }

        if (isMyPB)
        {
            MyPitButton.interactable = false;
            MyBatButton.interactable = true;
            MyTooltipP.SetActive(true);
            MyTooltipB.SetActive(false);
        } else
        {
            MyPitButton.interactable = true;
            MyBatButton.interactable = false;
            MyTooltipP.SetActive(false);
            MyTooltipB.SetActive(true);
        }

        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("TradeList");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        int EPC = 0;
        int MPC = 0;
        for (int i = 0; i < GameDirector.pitcherCount; i++)
        {
            if (GameDirector.pitcher[i].team == teamName && !GameDirector.pitcher[i].isTrade)
            {
                EPC++;
            }
            if (GameDirector.pitcher[i].team == GameDirector.myTeam && !GameDirector.pitcher[i].isTrade)
            {
                MPC++;
            }
            if (GameDirector.pitcher[i].team == teamName && !GameDirector.pitcher[i].isTrade && !GameDirector.pitcher[i].isForeign)
            {
                if (isEnemyPB)
                {
                    GameObject currentPrefab = Instantiate(TradeListPrefab, EnemyContentTransform);
                    int index = i;
                    currentPrefab.GetComponent<Button>().onClick.AddListener(() => OnSelectPitcher(GameDirector.pitcher[index], currentPrefab));
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    textArray[0].text = GameDirector.pitcher[i].name;
                    textArray[1].text = DataToString.AgeToString(GameDirector.pitcher[i].born);
                    textArray[2].text = GameDirector.pitcher[i].pos.ToString();

                    int OVR = GameDirector.pitcher[i].SPEED + GameDirector.pitcher[i].COMMAND + GameDirector.pitcher[i].BREAKING;
                    if (OVR <= 5)
                    {
                        textArray[3].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                    }
                    else if (OVR >= 6 && OVR <= 10)
                    {
                        textArray[3].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                    }
                    else if (OVR >= 11 && OVR <= 14)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                    }
                    else if (OVR >= 15)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★★</color>";
                    }

                    textArray[4].text = GameDirector.pitcher[i].SPEED.ToString();
                    textArray[5].text = GameDirector.pitcher[i].COMMAND.ToString();
                    textArray[6].text = GameDirector.pitcher[i].BREAKING.ToString();
                    if (GameDirector.pitcher[i].SPEED == 3)
                    {
                        textArray[4].color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].SPEED == 4)
                    {
                        textArray[4].color = Color.cyan;
                    }
                    else if (GameDirector.pitcher[i].SPEED == 5)
                    {
                        textArray[4].color = Color.magenta;
                    }
                    else
                    {
                        textArray[4].color = Color.white;
                    }
                    if (GameDirector.pitcher[i].COMMAND == 3)
                    {
                        textArray[5].color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].COMMAND == 4)
                    {
                        textArray[5].color = Color.cyan;
                    }
                    else if (GameDirector.pitcher[i].COMMAND == 5)
                    {
                        textArray[5].color = Color.magenta;
                    }
                    else
                    {
                        textArray[5].color = Color.white;
                    }
                    if (GameDirector.pitcher[i].BREAKING == 3)
                    {
                        textArray[6].color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].BREAKING == 4)
                    {
                        textArray[6].color = Color.cyan;
                    }
                    else if (GameDirector.pitcher[i].BREAKING == 5)
                    {
                        textArray[6].color = Color.magenta;
                    }
                    else
                    {
                        textArray[6].color = Color.white;
                    }
                }
            }
            if (GameDirector.pitcher[i].team == GameDirector.myTeam && !GameDirector.pitcher[i].isTrade && !GameDirector.pitcher[i].isForeign)
            {
                if (isMyPB)
                {
                    GameObject currentPrefab = Instantiate(TradeListPrefab, MyContentTransform);
                    int index = i;
                    currentPrefab.GetComponent<Button>().onClick.AddListener(() => OnSelectPitcher(GameDirector.pitcher[index], currentPrefab));
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    textArray[0].text = GameDirector.pitcher[i].name;
                    textArray[1].text = DataToString.AgeToString(GameDirector.pitcher[i].born);
                    textArray[2].text = GameDirector.pitcher[i].pos.ToString();

                    int OVR = GameDirector.pitcher[i].SPEED + GameDirector.pitcher[i].COMMAND + GameDirector.pitcher[i].BREAKING;
                    if (OVR <= 5)
                    {
                        textArray[3].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                    }
                    else if (OVR >= 6 && OVR <= 10)
                    {
                        textArray[3].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                    }
                    else if (OVR >= 11 && OVR <= 14)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                    }
                    else if (OVR >= 15)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★★</color>";
                    }

                    textArray[4].text = GameDirector.pitcher[i].SPEED.ToString();
                    textArray[5].text = GameDirector.pitcher[i].COMMAND.ToString();
                    textArray[6].text = GameDirector.pitcher[i].BREAKING.ToString();
                    if (GameDirector.pitcher[i].SPEED == 3)
                    {
                        textArray[4].color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].SPEED == 4)
                    {
                        textArray[4].color = Color.cyan;
                    }
                    else if (GameDirector.pitcher[i].SPEED == 5)
                    {
                        textArray[4].color = Color.magenta;
                    }
                    else
                    {
                        textArray[4].color = Color.white;
                    }
                    if (GameDirector.pitcher[i].COMMAND == 3)
                    {
                        textArray[5].color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].COMMAND == 4)
                    {
                        textArray[5].color = Color.cyan;
                    }
                    else if (GameDirector.pitcher[i].COMMAND == 5)
                    {
                        textArray[5].color = Color.magenta;
                    }
                    else
                    {
                        textArray[5].color = Color.white;
                    }
                    if (GameDirector.pitcher[i].BREAKING == 3)
                    {
                        textArray[6].color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].BREAKING == 4)
                    {
                        textArray[6].color = Color.cyan;
                    }
                    else if (GameDirector.pitcher[i].BREAKING == 5)
                    {
                        textArray[6].color = Color.magenta;
                    }
                    else
                    {
                        textArray[6].color = Color.white;
                    }
                }
            }
            
        }
        enemyPitcherCount = EPC;
        myPitcherCount = MPC;

        int EBC = 0;
        int MBC = 0;

        for (int i = 0; i < GameDirector.batterCount; i++)
        {
            if (GameDirector.batter[i].team == teamName && !GameDirector.batter[i].isTrade)
            {
                EBC++;
            }
            if (GameDirector.batter[i].team == GameDirector.myTeam && !GameDirector.batter[i].isTrade)
            {
                MBC++;
            }
            if (GameDirector.batter[i].team == teamName && !GameDirector.batter[i].isTrade && !GameDirector.batter[i].isForeign)
            {
                if (!isEnemyPB)
                {
                    GameObject currentPrefab = Instantiate(TradeListPrefab, EnemyContentTransform);
                    int index = i;
                    currentPrefab.GetComponent<Button>().onClick.AddListener(() => OnSelectBatter(GameDirector.batter[index], currentPrefab));
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    textArray[0].text = GameDirector.batter[i].name;
                    textArray[1].text = DataToString.AgeToString(GameDirector.batter[i].born);
                    textArray[2].text = DataToString.PosToString(GameDirector.batter[i].pos);

                    int OVR = GameDirector.batter[i].POWER + GameDirector.batter[i].CONTACT + GameDirector.batter[i].EYE;
                    if (OVR <= 5)
                    {
                        textArray[3].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                    }
                    else if (OVR >= 6 && OVR <= 10)
                    {
                        textArray[3].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                    }
                    else if (OVR >= 11 && OVR <= 14)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                    }
                    else if (OVR >= 15)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★★</color>";
                    }

                    textArray[4].text = GameDirector.batter[i].POWER.ToString();
                    textArray[5].text = GameDirector.batter[i].CONTACT.ToString();
                    textArray[6].text = GameDirector.batter[i].EYE.ToString();
                    if (GameDirector.batter[i].POWER == 3)
                    {
                        textArray[4].color = Color.green;
                    }
                    else if (GameDirector.batter[i].POWER == 4)
                    {
                        textArray[4].color = Color.cyan;
                    }
                    else if (GameDirector.batter[i].POWER == 5)
                    {
                        textArray[4].color = Color.magenta;
                    }
                    else
                    {
                        textArray[4].color = Color.white;
                    }
                    if (GameDirector.batter[i].CONTACT == 3)
                    {
                        textArray[5].color = Color.green;
                    }
                    else if (GameDirector.batter[i].CONTACT == 4)
                    {
                        textArray[5].color = Color.cyan;
                    }
                    else if (GameDirector.batter[i].CONTACT == 5)
                    {
                        textArray[5].color = Color.magenta;
                    }
                    else
                    {
                        textArray[5].color = Color.white;
                    }
                    if (GameDirector.batter[i].EYE == 3)
                    {
                        textArray[6].color = Color.green;
                    }
                    else if (GameDirector.batter[i].EYE == 4)
                    {
                        textArray[6].color = Color.cyan;
                    }
                    else if (GameDirector.batter[i].EYE == 5)
                    {
                        textArray[6].color = Color.magenta;
                    }
                    else
                    {
                        textArray[6].color = Color.white;
                    }
                }
            }
            if (GameDirector.batter[i].team == GameDirector.myTeam && !GameDirector.batter[i].isTrade && !GameDirector.batter[i].isForeign)
            {
                if (!isMyPB)
                {
                    GameObject currentPrefab = Instantiate(TradeListPrefab, MyContentTransform);
                    int index = i;
                    currentPrefab.GetComponent<Button>().onClick.AddListener(() => OnSelectBatter(GameDirector.batter[index], currentPrefab));
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    textArray[0].text = GameDirector.batter[i].name;
                    textArray[1].text = DataToString.AgeToString(GameDirector.batter[i].born);
                    textArray[2].text = DataToString.PosToString(GameDirector.batter[i].pos);

                    int OVR = GameDirector.batter[i].POWER + GameDirector.batter[i].CONTACT + GameDirector.batter[i].EYE;
                    if (OVR <= 5)
                    {
                        textArray[3].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                    }
                    else if (OVR >= 6 && OVR <= 10)
                    {
                        textArray[3].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                    }
                    else if (OVR >= 11 && OVR <= 14)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                    }
                    else if (OVR >= 15)
                    {
                        textArray[3].text = "<color=#FFFF00>★★★★</color>";
                    }

                    textArray[4].text = GameDirector.batter[i].POWER.ToString();
                    textArray[5].text = GameDirector.batter[i].CONTACT.ToString();
                    textArray[6].text = GameDirector.batter[i].EYE.ToString();
                    if (GameDirector.batter[i].POWER == 3)
                    {
                        textArray[4].color = Color.green;
                    }
                    else if (GameDirector.batter[i].POWER == 4)
                    {
                        textArray[4].color = Color.cyan;
                    }
                    else if (GameDirector.batter[i].POWER == 5)
                    {
                        textArray[4].color = Color.magenta;
                    }
                    else
                    {
                        textArray[4].color = Color.white;
                    }
                    if (GameDirector.batter[i].CONTACT == 3)
                    {
                        textArray[5].color = Color.green;
                    }
                    else if (GameDirector.batter[i].CONTACT == 4)
                    {
                        textArray[5].color = Color.cyan;
                    }
                    else if (GameDirector.batter[i].CONTACT == 5)
                    {
                        textArray[5].color = Color.magenta;
                    }
                    else
                    {
                        textArray[5].color = Color.white;
                    }
                    if (GameDirector.batter[i].EYE == 3)
                    {
                        textArray[6].color = Color.green;
                    }
                    else if (GameDirector.batter[i].EYE == 4)
                    {
                        textArray[6].color = Color.cyan;
                    }
                    else if (GameDirector.batter[i].EYE == 5)
                    {
                        textArray[6].color = Color.magenta;
                    }
                    else
                    {
                        textArray[6].color = Color.white;
                    }
                }
            }
        }
        myBatterCount = MBC;
        enemyBatterCount = EBC;
    }
}
