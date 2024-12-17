using GameData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageBatter : MonoBehaviour
{
    public Transform content;
    public GameObject ManageBatterPrefab;
    public Color SecondLineColor;
    private Dictionary<GameObject, Batter> batterData = new Dictionary<GameObject, Batter>();
    TMP_Text[] textArray;
    public static bool isUpdate = false;

    void InitManageBatter()
    {
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("ManageBatter");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        var sortedBatterList = new List<Batter>(GameDirector.batter);
        sortedBatterList.Sort((batter1, batter2) => batter1.posInTeam.CompareTo(batter2.posInTeam));
        int LineCheck = 0;
        for (int i = 0; i < sortedBatterList.Count; i++)
        {
            if (sortedBatterList[i].team == GameDirector.myTeam)
            {
                int posInTeam = sortedBatterList[i].posInTeam;
                if (posInTeam >= 101 && posInTeam <= 130)
                {
                    GameObject currentPrefab = Instantiate(ManageBatterPrefab, content);
                    if (LineCheck++ % 2 == 0)
                    {
                        currentPrefab.GetComponent<Image>().color = SecondLineColor;
                    }
                    textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
                    UpdateTextArray(textArray, sortedBatterList[i]);
                    DragHandler dragHandler = currentPrefab.GetComponent<DragHandler>();
                    if (dragHandler != null)
                    {
                        dragHandler.batterInfo = sortedBatterList[i];
                    }
                }
            }
        }
    }

    void UpdateTextArray(TMP_Text[] textArray, Batter batter)
    {
        if (textArray != null)
        {
            textArray[0].text = batter.hand.ToString();
            textArray[1].text = DataToString.PosToString(batter.pos);
            if (batter.posInTeam >= 110 && batter.posInTeam <= 115)
            {
                textArray[2].text = "SUB";
                textArray[2].color = new Color(0f / 255f, 172f / 255f, 255f / 255f, 1f);//Color.green;
            }
            else if (batter.posInTeam >= 116)
            {
                textArray[2].text = "2±º";
                textArray[2].color = Color.gray;
            }
            else
            {
                textArray[2].text = (batter.posInTeam - 100).ToString();
                textArray[2].color = Color.cyan;
            }
            textArray[3].text = batter.name;
            int OVR = batter.POWER + batter.CONTACT + batter.EYE;
            if (OVR <= 5)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú</color><color=#FFFFFF>¡Ù¡Ù¡Ù</color>";

            } else if (OVR >= 6 && OVR <= 10)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú¡Ú</color><color=#FFFFFF>¡Ù¡Ù</color>";
            } else if (OVR >= 11 && OVR <= 14)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú¡Ú¡Ú</color><color=#FFFFFF>¡Ù</color>";
            } else if (OVR >= 15)
            {
                textArray[4].text = "<color=#FFFF00>¡Ú¡Ú¡Ú¡Ú</color>";
            }
            textArray[5].text = batter.game.ToString();
            textArray[6].text = batter.plateAppearance.ToString();
            textArray[7].text = batter.atBat.ToString();
            textArray[8].text = batter.hit.ToString();
            textArray[9].text = batter.homerun.ToString();
            textArray[10].text = batter.RBI.ToString();
            textArray[11].text = batter.runScored.ToString();
            textArray[12].text = batter.baseOnBall.ToString();
            textArray[13].text = batter.battingAverage.ToString("F3");
            textArray[14].text = batter.OBP.ToString("F3");
            textArray[15].text = batter.SLG.ToString("F3");
            textArray[16].text = batter.OPS.ToString("F3");
        }
    }

    void Start()
    {
        InitManageBatter();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    void Update()
    {
        if (isUpdate)
        {
            InitManageBatter();
            isUpdate = false;
        }
    }
}
