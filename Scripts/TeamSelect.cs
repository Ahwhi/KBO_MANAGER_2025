using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamSelect : MonoBehaviour
{
    public Button[] teamButtons;

    void Start()
    {
        for (int i = 0; i < teamButtons.Length; i++)
        {
            TeamName teamName = (TeamName)i;
            teamButtons[i].onClick.AddListener(() => OnSelectTeam(teamName));
        }
    }

    public void OnSelectTeam(TeamName teamName)
    {
        CreateData.CreateNewGame(teamName);
        TeamColor.SetMyTeamColor();
        SceneManager.LoadScene("Main");
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("Title");
    }
}
