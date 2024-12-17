using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatterRank : MonoBehaviour
{
    public int MaxShow;
    public GameObject BatterRankPrefab, content;
    public static bool isUpdate = false;
    public Button[] sortButtons;
    public Color SecondLineColor;
    public TextMeshProUGUI RegulText;
    public TMP_Dropdown myDropdown;
    public Image captionImage;
    private int sorted = 6;
    private int TeamSorted = -1;
    Transform contentTransform;
    TMP_Text[] textArray;
    Image[] currentImage;
    List<Batter> sortedBatter;
    

    void Start()
    {
        contentTransform = content.GetComponent<Transform>();
        for (int i = 0; i < sortButtons.Length; i++)
        {
            int sortValue = i + 2;
            sortButtons[i].onClick.AddListener(() => OnSortButtonClick(sortValue));
        }
        sortedBatter = new List<Batter>(GameDirector.batter);
        SortBatter();
        sortButtons[sorted - 2].interactable = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1f;

        RegulText.text = "*현재 규정 타석: " + ((GameDirector.Teams[0].win + GameDirector.Teams[0].draw + GameDirector.Teams[0].lose)*3).ToString();

        myDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = -1; i< 10; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            if (i == -1)
            {
                option.image = TeamEmblem.GetEmblem((TeamName)999);
            } else
            {
                option.image = TeamEmblem.GetEmblem((TeamName)i);
            }
            options.Add(option);
        }
        myDropdown.AddOptions(options);
        captionImage.sprite = TeamEmblem.GetEmblem((TeamName)999);
        myDropdown.onValueChanged.AddListener(UpdateSelection);
    }

    void UpdateSelection(int index)
    {
        TMP_Dropdown.OptionData selectedOption = myDropdown.options[index];
        captionImage.sprite = selectedOption.image;
        TeamSorted = index - 1;
        isUpdate = true;
    }

    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;
            SortBatter();
        }
    }

    void SortBatter()
    {
        Func<Batter, object> sortKey = sorted switch
        {
            2 => batter => batter.game,
            3 => batter => batter.plateAppearance,
            4 => batter => batter.atBat,
            5 => batter => batter.hit,
            6 => batter => batter.battingAverage,
            7 => batter => batter.homerun,
            8 => batter => batter.RBI,
            9 => batter => batter.baseOnBall,
            10 => batter => batter.OBP,
            11 => batter => batter.SLG,
            12 => batter => batter.OPS,
            _ => batter => sorted,
        };

        if (sorted != 6 && sorted != 10 && sorted != 11 && sorted != 12)
        {
            sortedBatter = new List<Batter>(GameDirector.batter);
            if (sorted == 5)
            {
                sortedBatter = GameDirector.batter
                .Where(batter => batter.hit != 0)
                .Where(batter => TeamSorted == -1 || batter.team == (TeamName)TeamSorted)
                .OrderByDescending(sortKey)
                .ToList();
            } else if (sorted == 7)
            {
                sortedBatter = GameDirector.batter
                .Where(batter => batter.homerun != 0)
                .Where(batter => TeamSorted == -1 || batter.team == (TeamName)TeamSorted)
                .OrderByDescending(sortKey)
                .ToList();

            }
            else if (sorted == 8)
            {
                sortedBatter = GameDirector.batter
                .Where(batter => batter.RBI != 0)
                .Where(batter => TeamSorted == -1 || batter.team == (TeamName)TeamSorted)
                .OrderByDescending(sortKey)
                .ToList();
            }
            else if (sorted == 9)
            {
                sortedBatter = GameDirector.batter
                .Where(batter => batter.baseOnBall != 0)
                .Where(batter => TeamSorted == -1 || batter.team == (TeamName)TeamSorted)
                .OrderByDescending(sortKey)
                .ToList();
            }
            else
            {
                sortedBatter = GameDirector.batter
                .Where(batter => batter.game != 0)
                .Where(batter => TeamSorted == -1 || batter.team == (TeamName)TeamSorted)
                .OrderByDescending(sortKey)
                .ToList();

            }
        } else
        {
            List<Batter> filteredBatters = GameDirector.batter
                .Where(batter => (GameDirector.Teams[(int)batter.team].win + GameDirector.Teams[(int)batter.team].lose + GameDirector.Teams[(int)batter.team].draw)*3 <= batter.plateAppearance)
                .Where(batter => TeamSorted == -1 || batter.team == (TeamName)TeamSorted)
                .OrderByDescending(sortKey)
                .ToList();
            sortedBatter = filteredBatters;
        }

        // 생성된 프리팹은 제거
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("BatterRank");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        int ShowNumber = sortedBatter.Count;
        if (ShowNumber > MaxShow)
        {
            ShowNumber = MaxShow;
        }
        int LineCheck = 0;
        for (int i = 0; i < ShowNumber; i++)
        {
            GameObject currentPrefab = Instantiate(BatterRankPrefab, contentTransform);
            currentImage = currentPrefab.GetComponentsInChildren<Image>();
            if (LineCheck++ % 2 == 0)
            {
                currentPrefab.GetComponent<Image>().color = SecondLineColor;
            }
            textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
            currentImage[1].sprite = TeamEmblem.GetEmblem(sortedBatter[i].team);
            textArray[0].text = (i+1).ToString();
            textArray[1].text = sortedBatter[i].name;
            textArray[2].text = sortedBatter[i].game.ToString();
            textArray[3].text = sortedBatter[i].plateAppearance.ToString();
            textArray[4].text = sortedBatter[i].atBat.ToString();
            textArray[5].text = sortedBatter[i].hit.ToString();
            textArray[6].text = sortedBatter[i].battingAverage.ToString("F3");
            textArray[7].text = sortedBatter[i].homerun.ToString();
            textArray[8].text = sortedBatter[i].RBI.ToString();
            textArray[9].text = sortedBatter[i].baseOnBall.ToString();
            textArray[10].text = sortedBatter[i].OBP.ToString("F3");
            textArray[11].text = sortedBatter[i].SLG.ToString("F3");
            textArray[12].text = sortedBatter[i].OPS.ToString("F3");
            if (sortedBatter[i].team == GameDirector.myTeam)
            {
                for (int u = 0; u < 13; u++)
                {
                    textArray[u].color = Color.green;
                }
            }
            else
            {
                for (int u = 0; u < 13; u++)
                {
                    textArray[u].color = Color.white;
                }
            }
        }
    }

    public void OnSortButtonClick(int sortOption)
    {
        sortButtons[sorted - 2].interactable = true;
        sorted = sortOption;
        sortButtons[sorted - 2].interactable = false;
        isUpdate = true;
    }
}