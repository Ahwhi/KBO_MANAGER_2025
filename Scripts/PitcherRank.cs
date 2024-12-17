using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PitcherRank : MonoBehaviour
{
    public int MaxShow;
    public GameObject PitcherRankPrefab, content;
    public static bool isUpdate = false;
    public Button[] sortButtons;
    public Color SecondLineColor;
    public TextMeshProUGUI RegulText;
    public TMP_Dropdown myDropdown;
    public Image captionImage;
    private int sorted = 8;
    private int TeamSorted = -1;
    Transform contentTransform;
    TMP_Text[] textArray;
    Image[] currentImage;
    List<Pitcher> sortedPitcher;

    void Start()
    {
        contentTransform = content.GetComponent<Transform>();
        for (int i = 0; i < sortButtons.Length; i++)
        {
            int sortValue = i + 2;
            sortButtons[i].onClick.AddListener(() => OnSortButtonClick(sortValue));
        }
        sortedPitcher = new List<Pitcher>(GameDirector.pitcher);
        SortPitcher();
        sortButtons[sorted - 2].interactable = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1f;

        RegulText.text = "*현재 규정 이닝: " + (GameDirector.Teams[0].win + GameDirector.Teams[0].draw + GameDirector.Teams[0].lose).ToString();

        myDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = -1; i < 10; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            if (i == -1)
            {
                option.image = TeamEmblem.GetEmblem((TeamName)999);
            }
            else
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
            SortPitcher();
        }
    }

    void SortPitcher()
    {
        Func<Pitcher, object> sortKey = sorted switch
        {
            2 => pitcher => pitcher.game,
            3 => pitcher => (pitcher.inningsPitched1*100)+(pitcher.inningsPitched2*10),
            4 => pitcher => pitcher.win,
            5 => pitcher => pitcher.lose,
            6 => pitcher => pitcher.hold,
            7 => pitcher => pitcher.save,
            8 => pitcher => pitcher.earnedRunAverage,
            9 => pitcher => pitcher.strikeOut,
            10 => pitcher => pitcher.baseOnBall,
            11 => pitcher => pitcher.homerunAllowed,
            12 => pitcher => pitcher.WHIP,
            _ => pitcher => sorted,
        };
        if (sorted != 8 && sorted != 12) // 방어율, WHIP
        {
            sortedPitcher = new List<Pitcher>(GameDirector.pitcher);
            if (sorted == 4)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.win != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            } else if (sorted == 5)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.lose != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
            else if (sorted == 6)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.hold != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
            else if (sorted == 7)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.save != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
            else if (sorted == 9)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.strikeOut != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
            else if (sorted == 10)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.baseOnBall != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
            else if (sorted == 11)
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.homerunAllowed != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
            else
            {
                sortedPitcher = GameDirector.pitcher
                    .Where(pitcher => pitcher.game != 0)
                    .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                    .OrderByDescending(sortKey)
                    .ToList();
            }
        } else
        {
            List<Pitcher> filteredPitchers = GameDirector.pitcher
                .Where(pitcher => (GameDirector.Teams[(int)pitcher.team].win + GameDirector.Teams[(int)pitcher.team].lose + GameDirector.Teams[(int)pitcher.team].draw) <= pitcher.inningsPitched1)
                .Where(pitcher => TeamSorted == -1 || pitcher.team == (TeamName)TeamSorted)
                .OrderBy(sortKey)
                .ToList();
            sortedPitcher = filteredPitchers;
        }

        // 생성된 프리팹은 제거
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("PitcherRank");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        int ShowNumber = sortedPitcher.Count;
        if (ShowNumber > MaxShow)
        {
            ShowNumber = MaxShow;
        }
        int LineCheck = 0;
        for (int i = 0; i < ShowNumber; i++)
        {
            GameObject currentPrefab = Instantiate(PitcherRankPrefab, contentTransform);
            currentImage = currentPrefab.GetComponentsInChildren<Image>();
            if (LineCheck++ % 2 == 0)
            {
                currentPrefab.GetComponent<Image>().color = SecondLineColor;
            }
            textArray = currentPrefab.GetComponentsInChildren<TMP_Text>();
            currentImage[1].sprite = TeamEmblem.GetEmblem(sortedPitcher[i].team);
            textArray[0].text = (i + 1).ToString();
            textArray[1].text = sortedPitcher[i].name;
            textArray[2].text = sortedPitcher[i].game.ToString();
            if (sortedPitcher[i].inningsPitched2 == 0)
            {
                textArray[3].text = sortedPitcher[i].inningsPitched1.ToString();
            }
            else
            {
                textArray[3].text = sortedPitcher[i].inningsPitched1.ToString() + "." + sortedPitcher[i].inningsPitched2.ToString();
            }
            textArray[4].text = sortedPitcher[i].win.ToString();
            textArray[5].text = sortedPitcher[i].lose.ToString();
            textArray[6].text = sortedPitcher[i].hold.ToString();
            textArray[7].text = sortedPitcher[i].save.ToString();
            textArray[8].text = sortedPitcher[i].earnedRunAverage.ToString("F2");
            textArray[9].text = sortedPitcher[i].strikeOut.ToString();
            textArray[10].text = sortedPitcher[i].baseOnBall.ToString();
            textArray[11].text = sortedPitcher[i].homerunAllowed.ToString();
            textArray[12].text = sortedPitcher[i].WHIP.ToString("F2");
            if (sortedPitcher[i].team == GameDirector.myTeam)
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
