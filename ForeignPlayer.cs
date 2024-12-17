using GameData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ForeignPlayer : MonoBehaviour
{
    public Transform content;
    public GameObject ForeignListPrefab, WaveListPrefab;
    public GameObject PTooltip, BTooltip, BuyPanel, WaveListPanel, CannotPanel;
    public Button PButton, BButton, WaveButton;
    public TextMeshProUGUI Nametext, Moneytext;
    public Material DefaultMaterial;
    public Canvas canvas1;
    public static ForeignPitcher TargetP;
    public static ForeignBatter TargetB;
    public static long TargetValue;
    private bool isBatterView, isBatterSelected;
    TMP_Text[] textArray;
    Image[] currentImage;

    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1f;
        isBatterView = false;
        isBatterSelected = false;
        Moneytext.text = "예산: " + GameDirector.money.ToString("N0") + "원";
        OnViewPitcher();
    }

    public void OnViewPitcher()
    {
        isBatterView = false;
        SceneDirector.SetButton(PButton, DefaultMaterial, false);
        SceneDirector.SetButton(BButton, DefaultMaterial, true);
        PTooltip.SetActive(true);
        BTooltip.SetActive(false);
        GetForigenPlayerList();
    }

    public void OnViewBatter()
    {
        isBatterView = true;
        SceneDirector.SetButton(PButton, DefaultMaterial, true);
        SceneDirector.SetButton(BButton, DefaultMaterial, false);
        PTooltip.SetActive(false);
        BTooltip.SetActive(true);
        GetForigenPlayerList();
    }

    public void OnOK()
    {
        SceneDirector.GoToScene("ForeignPlayer");
    }

    public void OnWaveP(Pitcher pitcher)
    {
        GameDirector.money -= TargetValue;
        pitcher.name = TargetP.name;
        pitcher.born = TargetP.born;
        pitcher.hand = TargetP.hand;
        pitcher.pos = TargetP.pos;
        pitcher.SPEED = TargetP.SPEED;
        pitcher.COMMAND = TargetP.COMMAND;
        pitcher.BREAKING = TargetP.BREAKING;
        pitcher.HP = 100;
        pitcher.game = 0;
        pitcher.inningsPitched1 = 0;
        pitcher.inningsPitched2 = 0;
        pitcher.win = 0;
        pitcher.lose = 0;
        pitcher.save = 0;
        pitcher.hold = 0;
        pitcher.hitAllowed = 0;
        pitcher.homerunAllowed = 0;
        pitcher.strikeOut = 0;
        pitcher.baseOnBall = 0;
        pitcher.earnedRuns = 0;
        pitcher.earnedRunAverage = 0;
        pitcher.WHIP = 0;
        GameDirector.Fbatter.Clear();
        GameDirector.Fpitcher.Clear();
        GameDirector.foreignBatterCandidateCount = 0;
        GameDirector.foreignPitcherCandidateCount = 0;
        CreateData.CreateForeignCandidatePlayer();
        TargetValue = 0;
        SceneDirector.GoToScene("ForeignPlayer");
    }

    public void OnWaveB(Batter batter)
    {
        GameDirector.money -= TargetValue;
        batter.name = TargetB.name;
        batter.born = TargetB.born;
        batter.hand = TargetB.hand;
        batter.pos = TargetB.pos;
        batter.POWER = TargetB.POWER;
        batter.CONTACT = TargetB.CONTACT;
        batter.EYE = TargetB.EYE;
        batter.game = 0;
        batter.plateAppearance = 0;
        batter.totalBase = 0;
        batter.battingAverage = 0;
        batter.atBat = 0;
        batter.hit = 0;
        batter.homerun = 0;
        batter.RBI = 0;
        batter.baseOnBall = 0;
        batter.runScored = 0;
        batter.stolenBased = 0;
        batter.OPS = 0;
        batter.SLG = 0;
        batter.OBP = 0;
        GameDirector.Fbatter.Clear();
        GameDirector.Fpitcher.Clear();
        GameDirector.foreignBatterCandidateCount = 0;
        GameDirector.foreignPitcherCandidateCount = 0;
        CreateData.CreateForeignCandidatePlayer();
        TargetValue = 0;
        SceneDirector.GoToScene("ForeignPlayer");
    }

    void SelectedWaveP(Pitcher pitcher)
    {
        SceneDirector.SetButton(WaveButton, DefaultMaterial, true);
        WaveButton.onClick.AddListener(() => OnWaveP(pitcher));
    }

    void SelectedWaveB(Batter batter)
    {
        SceneDirector.SetButton(WaveButton, DefaultMaterial, true);
        WaveButton.onClick.AddListener(() => OnWaveB(batter));
    }

    void SelectedCandidateP(ForeignPitcher pitcher, float value)
    {
        if (value * 100000000 <= GameDirector.money)
        {
            BuyPanel.SetActive(true);
            Nametext.text = pitcher.name + " 선수를 영입하기 위해 용병 투수 1명의 웨이버 공시가 필요합니다. 어떤 선수를 웨이버 공시하시겠습니까?";
            TargetP = pitcher;
            TargetValue = (long)(value * 100000000);
            GetBasicList();
        } else
        {
            CannotPanel.SetActive(true);
        }
        canvas1.GetComponent<GraphicRaycaster>().enabled = false;
    }

    void SelectedCandidateB(ForeignBatter batter, float value)
    {
        if (value * 100000000 <= GameDirector.money)
        {
            isBatterSelected = true;
            BuyPanel.SetActive(true);
            Nametext.text = batter.name + " 선수를 영입하기 위해 용병 타자 1명의 웨이버 공시가 필요합니다. 어떤 선수를 웨이버 공시하시겠습니까?";
            TargetB = batter;
            TargetValue = (long)(value * 100000000);
            GetBasicList();
        } else
        {
            CannotPanel.SetActive(true);
        }
        canvas1.GetComponent<GraphicRaycaster>().enabled = false;
    }

    void GetBasicList()
    {
        if (!isBatterSelected)
        {
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if (GameDirector.pitcher[i].team == GameDirector.myTeam && GameDirector.pitcher[i].isForeign)
                {
                    GameObject currentPrefab = Instantiate(WaveListPrefab, WaveListPanel.transform);
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    int index = i;
                    currentPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
                    currentPrefab.GetComponent<Button>().onClick.AddListener(() => SelectedWaveP(GameDirector.pitcher[index]));
                    textArray[0].text = GameDirector.pitcher[i].name;
                    textArray[1].text = GameDirector.pitcher[i].pos.ToString();
                    int OVR = GameDirector.pitcher[i].SPEED + GameDirector.pitcher[i].COMMAND + GameDirector.pitcher[i].BREAKING;
                    if (OVR <= 5)
                    {
                        textArray[2].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                    }
                    else if (OVR >= 6 && OVR <= 10)
                    {
                        textArray[2].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                    }
                    else if (OVR >= 11 && OVR <= 14)
                    {
                        textArray[2].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                    }
                    else if (OVR >= 15)
                    {
                        textArray[2].text = "<color=#FFFF00>★★★★</color>";
                    }
                }
            }
        } else
        {
            for (int i = 0; i < GameDirector.batterCount; i++)
            {
                if (GameDirector.batter[i].team == GameDirector.myTeam && GameDirector.batter[i].isForeign)
                {
                    GameObject currentPrefab = Instantiate(WaveListPrefab, WaveListPanel.transform);
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    int index = i;
                    currentPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
                    currentPrefab.GetComponent<Button>().onClick.AddListener(() => SelectedWaveB(GameDirector.batter[index]));
                    textArray[0].text = GameDirector.batter[i].name;
                    textArray[1].text = DataToString.PosToString(GameDirector.batter[i].pos);
                    int OVR = GameDirector.batter[i].POWER + GameDirector.batter[i].CONTACT + GameDirector.batter[i].EYE;
                    if (OVR <= 5)
                    {
                        textArray[2].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                    }
                    else if (OVR >= 6 && OVR <= 10)
                    {
                        textArray[2].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                    }
                    else if (OVR >= 11 && OVR <= 14)
                    {
                        textArray[2].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                    }
                    else if (OVR >= 15)
                    {
                        textArray[2].text = "<color=#FFFF00>★★★★</color>";
                    }
                }
            }
        }
    }

    void GetForigenPlayerList()
    {
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("ForeignList");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        if (isBatterView)
        {
            for (int i = 0; i < GameDirector.foreignBatterCandidateCount; i++)
            {
                GameObject currentPrefab = Instantiate(ForeignListPrefab, content);
                textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                currentImage = currentPrefab.GetComponentsInChildren<Image>();
                float OVR = GameDirector.Fbatter[i].POWER + GameDirector.Fbatter[i].CONTACT + GameDirector.Fbatter[i].EYE;
                float value = OVR + OVR / 4.0f;
                int index = i;
                currentPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
                currentPrefab.GetComponent<Button>().onClick.AddListener(() => SelectedCandidateB(GameDirector.Fbatter[index], value));
                textArray[0].text = GameDirector.Fbatter[i].name;
                textArray[1].text = DataToString.AgeToString(GameDirector.Fbatter[i].born);
                currentImage[1].sprite = PlayerNation.GetNation(GameDirector.Fbatter[i].nation);
                textArray[2].text = "MLB";
                textArray[3].text = GameDirector.Fbatter[i].hand.ToString();
                textArray[4].text = DataToString.PosToString(GameDirector.Fbatter[i].pos);
                
                if (OVR <= 5)
                {
                    textArray[5].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                }
                else if (OVR >= 6 && OVR <= 10)
                {
                    textArray[5].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                }
                else if (OVR >= 11 && OVR <= 14)
                {
                    textArray[5].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                }
                else if (OVR >= 15)
                {
                    textArray[5].text = "<color=#FFFF00>★★★★</color>";
                }

                textArray[6].text = GameDirector.Fbatter[i].POWER.ToString();
                textArray[7].text = GameDirector.Fbatter[i].CONTACT.ToString();
                textArray[8].text = GameDirector.Fbatter[i].EYE.ToString();

                if (GameDirector.Fbatter[i].POWER == 3)
                {
                    textArray[6].color = Color.green;
                }
                else if (GameDirector.Fbatter[i].POWER == 4)
                {
                    textArray[6].color = Color.cyan;
                }
                else if (GameDirector.Fbatter[i].POWER == 5)
                {
                    textArray[6].color = Color.magenta;
                }

                if (GameDirector.Fbatter[i].CONTACT == 3)
                {
                    textArray[7].color = Color.green;
                }
                else if (GameDirector.Fbatter[i].CONTACT == 4)
                {
                    textArray[7].color = Color.cyan;
                }
                else if (GameDirector.Fbatter[i].CONTACT == 5)
                {
                    textArray[7].color = Color.magenta;
                }

                if (GameDirector.Fbatter[i].EYE == 3)
                {
                    textArray[8].color = Color.green;
                }
                else if (GameDirector.Fbatter[i].EYE == 4)
                {
                    textArray[8].color = Color.cyan;
                }
                else if (GameDirector.Fbatter[i].EYE == 5)
                {
                    textArray[8].color = Color.magenta;
                }

                textArray[9].text = value.ToString("F2") + " 억";
            }
        } else
        {
            for (int i = 0; i < GameDirector.foreignPitcherCandidateCount; i++)
            {
                GameObject currentPrefab = Instantiate(ForeignListPrefab, content);
                textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                currentImage = currentPrefab.GetComponentsInChildren<Image>();
                int index = i;
                float OVR = GameDirector.Fpitcher[i].SPEED + GameDirector.Fpitcher[i].COMMAND + GameDirector.Fpitcher[i].BREAKING;
                float value = OVR + OVR / 4.0f;
                currentPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
                currentPrefab.GetComponent<Button>().onClick.AddListener(() => SelectedCandidateP(GameDirector.Fpitcher[index], value));
                textArray[0].text = GameDirector.Fpitcher[i].name;
                textArray[1].text = DataToString.AgeToString(GameDirector.Fpitcher[i].born);
                currentImage[1].sprite = PlayerNation.GetNation(GameDirector.Fpitcher[i].nation);
                textArray[2].text = "MLB";
                textArray[3].text = GameDirector.Fpitcher[i].hand.ToString();
                textArray[4].text = GameDirector.Fpitcher[i].pos.ToString();
                
                if (OVR <= 5)
                {
                    textArray[5].text = "<color=#FFFF00>★</color><color=#FFFFFF>☆☆☆</color>";

                }
                else if (OVR >= 6 && OVR <= 10)
                {
                    textArray[5].text = "<color=#FFFF00>★★</color><color=#FFFFFF>☆☆</color>";
                }
                else if (OVR >= 11 && OVR <= 14)
                {
                    textArray[5].text = "<color=#FFFF00>★★★</color><color=#FFFFFF>☆</color>";
                }
                else if (OVR >= 15)
                {
                    textArray[5].text = "<color=#FFFF00>★★★★</color>";
                }

                textArray[6].text = GameDirector.Fpitcher[i].SPEED.ToString();
                textArray[7].text = GameDirector.Fpitcher[i].COMMAND.ToString();
                textArray[8].text = GameDirector.Fpitcher[i].BREAKING.ToString();

                if (GameDirector.Fpitcher[i].SPEED == 3)
                {
                    textArray[6].color = Color.green;
                }
                else if (GameDirector.Fpitcher[i].SPEED == 4)
                {
                    textArray[6].color = Color.cyan;
                }
                else if (GameDirector.Fpitcher[i].SPEED == 5)
                {
                    textArray[6].color = Color.magenta;
                }

                if (GameDirector.Fpitcher[i].COMMAND == 3)
                {
                    textArray[7].color = Color.green;
                }
                else if (GameDirector.Fpitcher[i].COMMAND == 4)
                {
                    textArray[7].color = Color.cyan;
                }
                else if (GameDirector.Fpitcher[i].COMMAND == 5)
                {
                    textArray[7].color = Color.magenta;
                }

                if (GameDirector.Fpitcher[i].BREAKING == 3)
                {
                    textArray[8].color = Color.green;
                }
                else if (GameDirector.Fpitcher[i].BREAKING == 4)
                {
                    textArray[8].color = Color.cyan;
                }
                else if (GameDirector.Fpitcher[i].BREAKING == 5)
                {
                    textArray[8].color = Color.magenta;
                }

                textArray[9].text = value.ToString("F2") + " 억";
            }
        }
        
    }
}
