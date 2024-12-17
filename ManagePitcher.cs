using GameData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagePitcher : MonoBehaviour
{
    public Transform content;
    public GameObject ManagePitcherPrefab;
    public Color SecondLineColor;
    private Dictionary<GameObject, Pitcher> pitcherData = new Dictionary<GameObject, Pitcher>();
    TMP_Text[] textArray;
    public static bool isUpdate = false;

    void InitManagePitcher()
    {
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("ManagePitcher");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        var sortedPitcherList = new List<Pitcher>(GameDirector.pitcher);
        sortedPitcherList.Sort((pitcher1, pitcher2) => pitcher1.posInTeam.CompareTo(pitcher2.posInTeam));
        int LineCheck = 0;
        for (int i = 0; i < sortedPitcherList.Count; i++)
        {
            if (sortedPitcherList[i].team == GameDirector.myTeam)
            {
                int posInTeam = sortedPitcherList[i].posInTeam;
                if (posInTeam >= 1 && posInTeam <= 30)
                {
                    GameObject currentPrefab = Instantiate(ManagePitcherPrefab, content);
                    if (LineCheck++ % 2 == 0)
                    {
                        currentPrefab.GetComponent<Image>().color = SecondLineColor;
                    }
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    Slider HPSlider = currentPrefab.GetComponentInChildren<Slider>();
                    HPSlider.value = sortedPitcherList[i].HP;
                    if (sortedPitcherList[i].HP == 100)
                    {
                        HPSlider.fillRect.GetComponent<Image>().color = new Color(0f, 1f, 0f, 2 / 3f);
                    }
                    else if (sortedPitcherList[i].HP <= 99 & sortedPitcherList[i].HP >= 50)
                    {
                        HPSlider.fillRect.GetComponent<Image>().color = new Color(1f, 1f, 0f, 2 / 3f);
                    }
                    else
                    {
                        HPSlider.fillRect.GetComponent<Image>().color = new Color(1f, 0f, 0f, 2 / 3f);
                    }
                    UpdateTextArray(textArray, sortedPitcherList[i]);
                    DragHandler dragHandler = currentPrefab.GetComponent<DragHandler>();
                    if (dragHandler != null)
                    {
                        dragHandler.pitcherInfo = sortedPitcherList[i];
                    }
                }
            }
        }
    }

    void UpdateTextArray(TMP_Text[] textArray, Pitcher pitcher)
    {
        if (textArray != null)
        {
            textArray[0].text = pitcher.hand.ToString();
            textArray[1].text = pitcher.pos.ToString();
            if (pitcher.posInTeam >= 6 && pitcher.posInTeam <= 14)
            {
                if (pitcher.posInTeam == 12)
                {
                    textArray[2].text = "SU1";
                } else if (pitcher.posInTeam == 13)
                {
                    textArray[2].text = "SU2";
                } else if (pitcher.posInTeam == 14)
                {
                    textArray[2].text = "CP";
                } else
                {
                    textArray[2].text = "RP";
                }
                textArray[2].color = new Color(0f / 255f, 172f / 255f, 255f / 255f, 1f);//Color.green;
            }
            else if (pitcher.posInTeam >= 15)
            {
                textArray[2].text = "2±º";
                textArray[2].color = Color.gray;
            }
            else
            {
                textArray[2].text = "SP" + pitcher.posInTeam.ToString();
                textArray[2].color = Color.cyan;
            }
            textArray[3].text = pitcher.name;
            int OVR = pitcher.SPEED + pitcher.COMMAND + pitcher.BREAKING;
            if (OVR <= 5)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú</color><color=#FFFFFF>¡Ù¡Ù¡Ù</color>";

            }
            else if (OVR >= 6 && OVR <= 10)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú¡Ú</color><color=#FFFFFF>¡Ù¡Ù</color>";
            }
            else if (OVR >= 11 && OVR <= 14)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú¡Ú¡Ú</color><color=#FFFFFF>¡Ù</color>";
            }
            else if (OVR >= 15)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú¡Ú¡Ú¡Ú</color>";
            }
            

            

            textArray[5].text = pitcher.game.ToString();
            if (pitcher.inningsPitched2 == 0)
            {
                textArray[6].text = pitcher.inningsPitched1.ToString();
            } else
            {
                textArray[6].text = pitcher.inningsPitched1.ToString() + "." + pitcher.inningsPitched2.ToString();
            }

            textArray[7].text = pitcher.win.ToString();
            textArray[8].text = pitcher.lose.ToString();
            textArray[9].text = pitcher.hold.ToString();
            textArray[10].text = pitcher.save.ToString();
            textArray[11].text = pitcher.earnedRunAverage.ToString("F2");
            textArray[12].text = pitcher.strikeOut.ToString();
            textArray[13].text = pitcher.baseOnBall.ToString();
            textArray[14].text = pitcher.homerunAllowed.ToString();
            textArray[15].text = pitcher.WHIP.ToString("F2");
        }
    }

    void Start()
    {
        InitManagePitcher();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    void Update()
    {
        if (isUpdate)
        {
            InitManagePitcher();
            isUpdate = false;
        }
    }
}