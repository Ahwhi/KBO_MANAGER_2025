using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneDirector : MonoBehaviour
{
    public static Stack<string> sceneHistory = new Stack<string>();
    public static SceneDirector Instance;
    //공통
    private Button HomeButton;
    private Button BackButton;
    //메인
    private Button GameStartButton;
    private Button ScheduleButton;
    private Button ManageButton;
    private Button RecordButton;
    private Button MarketButton;
    //일정표
    private Button PostSeasonButton;
    //팀관리
    private Button ManageBatterButton;
    private Button ManagePitcherButton;
    //기록실
    private Button TeamRankButton;
    private Button PitcherRankButton;
    private Button BatterRankButton;
    //이적시장
    private Button TradeButton;
    private Button ForeignButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sceneHistory.Push(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "GameModule")
        {
            HomeButton = GameObject.Find("ButtonHome").GetComponent<Button>();
            BackButton = GameObject.Find("ButtonBack").GetComponent<Button>();
            HomeButton.onClick.AddListener(() => OnHomeButton());
            BackButton.onClick.AddListener(() => OnBackButton());
        }
        if (scene.name == "Main")
        {
            GameStartButton = GameObject.Find("Button_GameStart").GetComponent<Button>();
            ScheduleButton = GameObject.Find("Button_Schedule").GetComponent<Button>();
            ManageButton = GameObject.Find("Button_Manage").GetComponent<Button>();
            RecordButton = GameObject.Find("Button_LeaderBoard").GetComponent<Button>();
            MarketButton = GameObject.Find("Button_Market").GetComponent<Button>();
            GameStartButton.onClick.AddListener(() => OnStartGame());
            ScheduleButton.onClick.AddListener(() => OnSchedule());
            ManageButton.onClick.AddListener(() => OnManageBatter());
            RecordButton.onClick.AddListener(() => OnTeamRank());
            MarketButton.onClick.AddListener(() => OnMarket());
            if (GameDirector.currentDate.month >= 11 || (GameDirector.currentDate.month == 10 && GameDirector.currentDate.day >= 11))
            {
                SetButton(GameStartButton, null, false);
            }
            if (GameDirector.isPostSeason)
            {
                SetButton(MarketButton, null, false);
            }
        }
        if (scene.name == "ScheduleScreen" || scene.name == "PostSeason")
        {
            ScheduleButton = GameObject.Find("Button_Schedule").GetComponent<Button>();
            PostSeasonButton = GameObject.Find("Button_PostSeason").GetComponent<Button>();
            ScheduleButton.onClick.AddListener(() => OnSchedule());
            PostSeasonButton.onClick.AddListener(() => OnPostSeason());
            if (!GameDirector.isPostSeason)
            {
                SetButton(PostSeasonButton, null, false);
            }
        }
        if (scene.name == "TeamRank" || scene.name == "PitcherRank" || scene.name == "BatterRank")
        {
            TeamRankButton = GameObject.Find("Button_TeamRank").GetComponent<Button>();
            PitcherRankButton = GameObject.Find("Button_PitcherRank").GetComponent<Button>();
            BatterRankButton = GameObject.Find("Button_BatterRank").GetComponent<Button>();
            TeamRankButton.onClick.AddListener(() => OnTeamRank());
            PitcherRankButton.onClick.AddListener(() => OnPitcherRank());
            BatterRankButton.onClick.AddListener(() => OnBatterRank());
        }
        if (scene.name == "ManageBatter" || scene.name == "ManagePitcher")
        {
            ManageBatterButton = GameObject.Find("Button_BatterManage").GetComponent<Button>();
            ManagePitcherButton = GameObject.Find("Button_PitcherManage").GetComponent<Button>();
            ManageBatterButton.onClick.AddListener(() => OnManageBatter());
            ManagePitcherButton.onClick.AddListener(() => OnManagePitcher());
        }
        if (scene.name == "TradeMarket" || scene.name == "ForeignPlayer")
        {
            TradeButton = GameObject.Find("Button_Trade").GetComponent<Button>();
            ForeignButton = GameObject.Find("Button_Foreign").GetComponent<Button>();
            TradeButton.onClick.AddListener(() => OnMarket());
            ForeignButton.onClick.AddListener(() => OnForeign());
        }
        if (scene.name != "GameModule" && sceneHistory.Count < 2)
        {
            BackButton.gameObject.SetActive(false);
        }
        else if (scene.name != "GameModule" && sceneHistory.Count >= 2)
        {
            BackButton.gameObject.SetActive(true);
        }
    }

    public static void GoToScene(string sceneName)
    {
        sceneHistory.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);
    }

    public static void SetButton(Button button, Material material, bool OnOff)
    {
        if (OnOff) {
            button.interactable = true;
            button.image.material = material;
        } else
        {
            button.interactable = false;
            button.image.material = null;
        }
    }

    public void OnHomeButton()
    {
        GoToScene("Main");
    }

    public void OnBackButton()
    {
        if (sceneHistory.Count > 1)
        {
            string previousScene = sceneHistory.Peek();
            StartCoroutine(ScenePop());
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.Log("더 이상 뒤로 갈 씬이 없습니다.");
        }
    }

    IEnumerator ScenePop()
    {
        sceneHistory.Pop();
        yield return null;
    }

    public void OnStartGame()
    {
        if (GameDirector.CheckBatterPosAvailable())
        {
            GameDirector.PlayGame();
        } else
        {
            MainScreen.isCannot = true;
        }
    }

    public void OnSchedule()
    {
        GoToScene("ScheduleScreen");
    }

    public void OnPostSeason()
    {
        GoToScene("PostSeason");
    }

    public void OnManageBatter()
    {
        GoToScene("ManageBatter");
    }

    public void OnManagePitcher()
    {
        GoToScene("ManagePitcher");
    }

    public void OnTeamRank()
    {
        GoToScene("TeamRank");
    }

    public void OnBatterRank()
    {
        GoToScene("BatterRank");
    }

    public void OnPitcherRank()
    {
        GoToScene("PitcherRank");
    }

    public void OnMarket()
    {
        GoToScene("TradeMarket");
    }

    public void OnForeign()
    {
        GoToScene("ForeignPlayer");
    }
}
