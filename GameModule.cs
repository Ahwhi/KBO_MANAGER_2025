using GameData;
using MailData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModule : MonoBehaviour
{
    private Color HomeColor, AwayColor;
    private int GameMode = 0; //0=����, 1=�ڵ�����, 2=2���, 3=3���
    private bool isChange = false;
    private bool isChangeExecuted = false;
    private bool isPrefer = false;
    private bool isProcess = false;
    private bool isLeave = false;
    private bool isPitChange = false;
    public int StandardNum1;
    public int StandardNum2;
    public Color CHighLight;
    public Color CBasic;
    public Color CBase;
    public Image LeftOut, RightOut, Base1, Base2, Base3, AwayBG, HomeBG;
    public TextMeshProUGUI Message, GameInfoT;
    public TextMeshProUGUI HomeTeamScoreT, AwayTeamScoreT, CurrentInning, CurrentDay, HomeTeamNameB, AwayTeamNameB, InningAttack, HomeTeamNameC, AwayTeamNameC, HomeTeamScore, AwayTeamScore;
    public GameObject HLine1, HLine2, HLine3, HLine4, HLine5, HLine6, HLine7, HLine8, HLine9, ALine1, ALine2, ALine3, ALine4, ALine5, ALine6, ALine7, ALine8, ALine9;
    public TextMeshProUGUI HomeBResult, AwayBResult, HomeBResultNum, AwayBResultNum;
    public TextMeshProUGUI PitName, PitGame, PitInning, PitBallCount, PitW, PitL, PitH, PitS, PitERA;
    public TextMeshProUGUI HomeInningS1, HomeInningS2, HomeInningS3, HomeInningS4, HomeInningS5, HomeInningS6, HomeInningS7, HomeInningS8, HomeInningS9, HomeInningS10, HomeInningS11, HomeInningS12;
    public TextMeshProUGUI HomeRHEB1, HomeRHEB2, HomeRHEB3, HomeRHEB4;
    public TextMeshProUGUI AwayInningS1, AwayInningS2, AwayInningS3, AwayInningS4, AwayInningS5, AwayInningS6, AwayInningS7, AwayInningS8, AwayInningS9, AwayInningS10, AwayInningS11, AwayInningS12;
    public TextMeshProUGUI AwayRHEB1, AwayRHEB2, AwayRHEB3, AwayRHEB4;
    public TextMeshProUGUI ResultInformation, WinnerP, LoserP, SaveP, LoadMessage;
    public GameObject P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, B0, B1, B2, B3, B4, B5, B6;
    public GameObject ProcessPanel, GameOverPanel, HomeBatterTopPanel, AwayBatterTopPanel, ResultPanel, PitcherPanel, ChangePitPanel, ChangeBatPanel, ModePanel, SCPanel, LoadCanvas, ALLENDPanel;
    public Slider loadSlider, PitHPSlider;
    public TMP_FontAsset boldFont;
    public TMP_FontAsset regularFont;
    public Button PlayButton;
    public Toggle toggle1, toggle2, toggle3, toggle4;
    public static Game game;

    void Start()
    {
        game = new Game();
        if (!GameDirector.isSimulation)
        {
            GameDirector.remainGames = 5;
            
        } else
        {
            loadSlider.maxValue = 5;
            loadSlider.value = 5 - GameDirector.remainGames;
        }
        game.PlayerOnBase = new Queue<Batter>();
        game.ResponsibleRunner = new Queue<Pitcher>();
        game.Innings = 1;
        game.HomeCurrentBatter = 101;
        game.AwayCurrentBatter = 101;
        game.WhoWinCondition = 1000;
        game.WhoLoseCondition = 1000;
        game.HomeCurrentHoldPitcher = 1000;
        game.AwayCurrentHoldPitcher = 1000;
        if (!GameDirector.isPostSeason)
        {
            game.HomeCurrentPitcher = GameDirector.Teams[(int)GameDirector.schedule[GameDirector.currentGame].homeTeam].currentSP;
            game.AwayCurrentPitcher = GameDirector.Teams[(int)GameDirector.schedule[GameDirector.currentGame].awayTeam].currentSP;
        } else
        {
            game.HomeCurrentPitcher = GameDirector.Teams[(int)GameDirector.postSchedule[GameDirector.currentGame].homeTeam].currentSP;
            game.AwayCurrentPitcher = GameDirector.Teams[(int)GameDirector.postSchedule[GameDirector.currentGame].awayTeam].currentSP;
        }
        StartCoroutine(GameMainModule(GameDirector.currentGame, game));
        GameOverPanel.SetActive(false);
        ResultPanel.SetActive(false);
        ChangePitPanel.SetActive(false);
        ChangeBatPanel.SetActive(false);
        toggle1.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, 0));
        toggle2.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, 1));
        toggle3.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, 2));
        toggle4.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, 3));
        ModePanel.SetActive(false);

        // ���� ������ ��� ���� ������
        if (!GameDirector.isPostSeason)
        {
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if (GameDirector.pitcher[i].team == GameDirector.schedule[GameDirector.currentGame].homeTeam && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher)
                {
                    GameDirector.pitcher[i].game++;
                }
                if (GameDirector.pitcher[i].team == GameDirector.schedule[GameDirector.currentGame].awayTeam && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher)
                {
                    GameDirector.pitcher[i].game++;
                }
            }
            // ���� Ÿ�ڵ��� ��� ���� ������
            for (int n = 0; n < GameDirector.batterCount; n++)
            {
                if (GameDirector.batter[n].team == GameDirector.schedule[GameDirector.currentGame].homeTeam && GameDirector.batter[n].posInTeam >= 101 && GameDirector.batter[n].posInTeam <= 109)
                {
                    GameDirector.batter[n].game++;
                }
                if (GameDirector.batter[n].team == GameDirector.schedule[GameDirector.currentGame].awayTeam && GameDirector.batter[n].posInTeam >= 101 && GameDirector.batter[n].posInTeam <= 109)
                {
                    GameDirector.batter[n].game++;
                }
            }
        }
    }

    void Update()
    {
        // ��� ȭ��
        if (isPrefer)
        {
            ModePanel.SetActive(true);
        }
        else
        {
            ModePanel.SetActive(false);
        }

        // �ڵ� ����϶� �����÷��� True
        if (GameMode >=1 && GameMode <=3 && !game.IsGameOver)
        {
            isProcess = true;
        } else if (GameMode == 0 && !game.IsGameOver) // ���� ����϶� �����̽��ε� ����
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isProcess = true;
            }
        }

        // ���� �����϶� ��üâ ����
        if (!GameDirector.isPostSeason)
        {
            if ((game.IsHomeAttack && GameDirector.schedule[GameDirector.currentGame].homeTeam == GameDirector.myTeam) ||
            !game.IsHomeAttack && GameDirector.schedule[GameDirector.currentGame].awayTeam == GameDirector.myTeam)
            {
                if (isChange)
                {
                    ChangeBatPanel.SetActive(true);
                }
                else
                {
                    ChangeBatPanel.SetActive(false);
                }
                if (isChange && !isChangeExecuted)
                {
                    ChangeBatter(GameDirector.currentGame, game);
                    isChangeExecuted = true;
                }
            }
            else // ���� �����϶� ��üâ ����
            {
                if (isChange)
                {
                    ChangePitPanel.SetActive(true);
                }
                else
                {
                    ChangePitPanel.SetActive(false);
                }
                if (isChange && !isChangeExecuted)
                {
                    ChangePitcher(GameDirector.currentGame, game);
                    isChangeExecuted = true;
                }
            }
        } else
        {
            if ((game.IsHomeAttack && GameDirector.postSchedule[GameDirector.currentGame].homeTeam == GameDirector.myTeam) ||
            !game.IsHomeAttack && GameDirector.postSchedule[GameDirector.currentGame].awayTeam == GameDirector.myTeam)
            {
                if (isChange)
                {
                    ChangeBatPanel.SetActive(true);
                }
                else
                {
                    ChangeBatPanel.SetActive(false);
                }
                if (isChange && !isChangeExecuted)
                {
                    ChangeBatter(GameDirector.currentGame, game);
                    isChangeExecuted = true;
                }
            }
            else // ���� �����϶� ��üâ ����
            {
                if (isChange)
                {
                    ChangePitPanel.SetActive(true);
                }
                else
                {
                    ChangePitPanel.SetActive(false);
                }
                if (isChange && !isChangeExecuted)
                {
                    ChangePitcher(GameDirector.currentGame, game);
                    isChangeExecuted = true;
                }
            }
        }
    }

    IEnumerator GameMainModule(int t, Game Game)
    {
        while (true)
        {
            // �̴� ����
            if (Game.Innings % 2 == 1)
            {
                Game.IsHomeAttack = false; // 1, 3, 5... ��
            }
            else
            {
                Game.IsHomeAttack = true; // 2, 4, 6... ��
            }

            // ��ǻ�� ������ü AI
            if (!GameDirector.isPostSeason)
            {
                if (GameDirector.isSimulation ||
                (!GameDirector.isSimulation && game.IsHomeAttack && GameDirector.schedule[t].homeTeam == GameDirector.myTeam) || // ��ǻ�Ͱ� �������̰� ����� AI ����
                (!GameDirector.isSimulation && !game.IsHomeAttack && GameDirector.schedule[t].awayTeam == GameDirector.myTeam)) // ��ǻ�Ͱ� Ȩ���̰� ����� AI ����
                {
                    if (!game.IsGameOver)
                    {
                        ComputerAI(t, Game);
                    }
                }
            } else
            {
                if (GameDirector.isSimulation ||
                (!GameDirector.isSimulation && game.IsHomeAttack && GameDirector.postSchedule[t].homeTeam == GameDirector.myTeam) || // ��ǻ�Ͱ� �������̰� ����� AI ����
                (!GameDirector.isSimulation && !game.IsHomeAttack && GameDirector.postSchedule[t].awayTeam == GameDirector.myTeam)) // ��ǻ�Ͱ� Ȩ���̰� ����� AI ����
                {
                    if (!game.IsGameOver)
                    {
                        ComputerAI(t, Game);
                    }
                }
            }

            // UI ������Ʈ (�ùķ��̼��� �ε� ȭ�� ���)
            GameModuleUI(GameDirector.currentGame, game);
            if (GameDirector.isSimulation)
            {
                LoadCanvas.SetActive(true);
            }

            // ���� �ɷ�ġ �ݿ��� ��� ���� ����
            Game.AbillityResult1 = StandardNum1;
            Game.AbillityResult1 += GameDirector.pitcher[Game.TargetPitcherIndex].COMMAND;
            Game.AbillityResult1 += GameDirector.pitcher[Game.TargetPitcherIndex].HP / 60;
            Game.AbillityResult1 -= GameDirector.batter[Game.TargetBatterIndex].CONTACT;
            Game.AbillityResult2 = StandardNum2;
            Game.AbillityResult2 += (GameDirector.pitcher[Game.TargetPitcherIndex].SPEED * 4);
            Game.AbillityResult2 -= (GameDirector.batter[Game.TargetBatterIndex].POWER * 16);
            Game.PlayerNum = UnityEngine.Random.Range(0, Game.AbillityResult1 + 1);
            Game.ComputerNum = UnityEngine.Random.Range(0, Game.AbillityResult1 + 1);
            int diff = Mathf.Abs(Game.PlayerNum - Game.ComputerNum);

            if (GameDirector.isSimulation && !game.IsGameOver)
            {
                yield return new WaitForSeconds(0.001f);
            } else
            {
                if (GameMode == 0)
                {
                    // ��ư ���������� �� �� ����
                    isProcess = false;
                    yield return new WaitUntil(() => Processing());
                }
                else if (GameMode == 1)
                {
                    yield return new WaitForSeconds(2f);
                }
                else if (GameMode == 2)
                {
                    yield return new WaitForSeconds(1f);
                }
                else if (GameMode == 3)
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }

            // ���� ���� ����
            //Debug.Log("["+GameDirector.batter[Game.TargetBatterIndex].name + "] (AbillityResult1: " + game.AbillityResult1.ToString() + ")(AbillityResult2: " + game.AbillityResult2.ToString() + ")");
            if (diff > 7)
            {
                diff = Game.AbillityResult1 - diff;
            }
            if (Game.PlayerNum == Game.ComputerNum)
            {
                // �⺻ 90, +����*4, -�Ŀ�*16 �ϰ��
                // ����5 VS Ÿ��1: 90 + 20 - 16 = 94 / Ȩ�� 6% Ȯ��
                // ����3 VS Ÿ��2: 90 + 12 - 32 = 70 / Ȩ�� 30% Ȯ��
                // ����2 VS Ÿ��3: 90 + 8 - 48 = 50 / Ȩ�� 50% Ȯ��
                // ����1 VS Ÿ��5: 90 + 4 - 80 = 14 / Ȩ�� 86% Ȯ��
                int rd = UnityEngine.Random.Range(1, 101);
                //Debug.Log(rd.ToString());
                if (rd > Game.AbillityResult2)
                {
                    GamePlay_HomeRun(t, Game); // 1~100 > 39 Ÿ�ڰ� ���Ѱ�.
                }
                else
                {
                    GamePlay_BigHit(t, Game);
                }
            }
            else if (diff == 1)
            {
                GamePlay_Hit(t, Game);
            }
            else if (diff == 2)
            {
                int AbillityResult3 = (GameDirector.pitcher[Game.TargetPitcherIndex].COMMAND * 14) - (GameDirector.batter[game.TargetBatterIndex].EYE * 4);
                if (AbillityResult3 < 15)
                {
                    AbillityResult3 = 15; // �������� ���� ������ �������� ���� Ÿ�ڿ��� ������ ������ ������� �ʰ���. 85%Ȯ���� ����
                }
                if (UnityEngine.Random.Range(1, 101) > AbillityResult3)
                {
                    // (16,4)�϶� ���� 1 vs ���� 5 = -4, Ȯ�� 100% ����. ���� 3 vs ���� 3 = 36 Ȯ�� 64% ����, ���� 5 vs ���� 1 = 76, 24% Ȯ�� ����
                    // (14,4)�϶� ���� 1 vs ���� 5 = -6, Ȯ�� 100% ����, ���� 3 vs ���� 3 = 30 Ȯ�� 70% ����, ���� 5 vs ���� 1 = 66, 33% Ȯ�� ����
                    GamePlay_BaseOnBalls(t, Game);
                } else
                {
                    int whatPlay = UnityEngine.Random.Range(-1, 9) + GameDirector.pitcher[game.TargetPitcherIndex].BREAKING;
                    if (whatPlay >= 5)
                    {
                        GamePlay_StrikeOut(t, Game);
                    } else
                    {
                        if (Game.PlayerNum > Game.ComputerNum)
                        {
                            GamePlay_GroundBall(t, Game);
                        }
                        else
                        {
                            GamePlay_FlyBall(t, Game);
                        }
                    }
                }
            }
            else if (diff == 3)
            {
                GamePlay_StrikeOut(t, Game);
            }
            else if (diff == 4)
            {
                if (UnityEngine.Random.Range(0, 20) < GameDirector.pitcher[Game.TargetPitcherIndex].SPEED) // ����1 => ����5% 2=>10%... 5=>25%
                {
                    GamePlay_StrikeOut(t, Game);
                } else
                {
                    if (Game.PlayerNum > Game.ComputerNum)
                    {
                        GamePlay_GroundBall(t, Game);
                    }
                    else
                    {
                        GamePlay_FlyBall(t, Game);
                    }
                }
            }
            else
            {
                if (Game.PlayerNum > Game.ComputerNum)
                {
                    GamePlay_GroundBall(t, Game);
                }
                else
                {
                    GamePlay_FlyBall(t, Game);
                }
            }
            int AbillityResult4 = GameDirector.batter[Game.TargetBatterIndex].EYE - GameDirector.pitcher[Game.TargetPitcherIndex].COMMAND;
            // ������ 3 - ������ 1 = 2
            int NumBallCounts = UnityEngine.Random.Range(1, 7 + AbillityResult4); // �ϰ�쿣 �⺻(1~6) -> (1~8)
            if (Game.IsHomeAttack)
            {
                Game.AwayCurrentPitcherBallCount += NumBallCounts;
            } else
            {
                Game.HomeCurrentPitcherBallCount += NumBallCounts;
            }
            GameDirector.pitcher[Game.TargetPitcherIndex].HP -= (NumBallCounts / 2) + 1;
            if (GameDirector.pitcher[Game.TargetPitcherIndex].HP <= 0)
            {
                GameDirector.pitcher[Game.TargetPitcherIndex].HP = 0;
            }

            GameConditionsCheck(Game);

            // ���� ����
            // 9ȸ��(18), 10ȸ��(20), 11ȸ��(22)�� ���� �÷��׸� ����
            if (Game.Innings == 18 || Game.Innings == 20 || Game.Innings == 22)
            {
                Game.IsExtend = false;
            }
            else if (Game.Innings == 19 || Game.Innings == 21 || Game.Innings == 23)
            {
                // 10ȸ��(19), 11ȸ��(21), 12ȸ��(23)�� �������� �̱�� �ִٸ� 
                // �̹� Ȩ���� �̱� �� ������ �̴��̹Ƿ� �� �ݴ��� ��쿡�� ���� �÷��׸� Ų��
                if (Game.HomeScore >= Game.AwayScore)
                {
                    Game.IsExtend = true;
                }
            }

            if (Game.Innings == 18 && Game.HomeScore > Game.AwayScore)
            { // 9ȸ�� Ȩ�� ���� �ռ��� �¸� (������)
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 20 && Game.HomeScore > Game.AwayScore)
            { // 10ȸ�� Ȩ�� ���� �ռ��� �¸� (������)
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 22 && Game.HomeScore > Game.AwayScore)
            { // 11ȸ�� Ȩ�� ���� �ռ��� �¸� (������)
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 24 && Game.HomeScore > Game.AwayScore)
            { // 12ȸ�� Ȩ�� ���� �ռ��� �¸� (������)
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 19 && Game.HomeScore < Game.AwayScore && Game.IsExtend == false)
            { // 10ȸ�� ���۽� Ȯ��
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 21 && Game.HomeScore < Game.AwayScore && Game.IsExtend == false)
            { // 11ȸ�� ���۽� Ȯ��
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 23 && Game.HomeScore < Game.AwayScore && Game.IsExtend == false)
            { // 12ȸ�� ���۽� Ȯ��
                StartCoroutine(GameEnd(t, Game));
            }
            if (Game.Innings == 25)
            { //13ȸ�� ���۽� Ȯ�� (���º�)
                StartCoroutine(GameEnd(t, Game));
            }
        }
    }

    public void GameModuleUI(int t, Game Game)
    {
        // ž
        HomeTeamScoreT.text = game.HomeScore.ToString();
        AwayTeamScoreT.text = game.AwayScore.ToString();
        if (!GameDirector.isPostSeason)
        {
            GameInfoT.text = "���Ʈ���̽�";
            if (GameDirector.currentDate.month == 3 && GameDirector.currentDate.day == 25)
            {
                GameInfoT.text += "(������)";
            }
        } else
        {
            if (GameDirector.currentGame == 0 || GameDirector.currentGame == 1)
            {
                GameInfoT.text = "���ϵ�ī�� " + (GameDirector.currentGame + 1).ToString() + "����";
            } else if (GameDirector.currentGame >= 2 && GameDirector.currentGame <= 6) {
                    GameInfoT.text = "���÷��̿��� " + (GameDirector.currentGame - 1).ToString() + "����";
            } else if (GameDirector.currentGame >= 7 && GameDirector.currentGame <= 11) {
                GameInfoT.text = "�÷��̿��� " + (GameDirector.currentGame - 6).ToString() + "����";
            } else if (GameDirector.currentGame >= 12 && GameDirector.currentGame <= 18)
            {
                GameInfoT.text = "�ѱ��ø��� " + (GameDirector.currentGame - 11).ToString() + "����";
            }
        }
        
        CurrentDay.text = GameDirector.currentDate.year.ToString() + "�� " + GameDirector.currentDate.month.ToString() + "�� " + GameDirector.currentDate.day.ToString() + "�� " + DataToString.DayOfWeekToString(GameDirector.currentDate.dayOfWeek);

        // �� ����
        if (!GameDirector.isPostSeason)
        {
            HomeColor = TeamColor.SetTeamColor(GameDirector.schedule[t].homeTeam);
            AwayColor = TeamColor.SetTeamColor(GameDirector.schedule[t].awayTeam);
        } else
        {
            HomeColor = TeamColor.SetTeamColor(GameDirector.postSchedule[t].homeTeam);
            AwayColor = TeamColor.SetTeamColor(GameDirector.postSchedule[t].awayTeam);
        }
        
        HomeBG.color = HomeColor;
        HomeTeamNameC.color = HomeColor;
        HomeBatterTopPanel.GetComponent<Image>().color = HomeColor;
        AwayBG.color = AwayColor;
        AwayTeamNameC.color = AwayColor;
        AwayBatterTopPanel.GetComponent<Image>().color = AwayColor;

        // �� �ΰ�
        Image HomeEmblemPanel = GameObject.Find("HomeEmblemPanel").GetComponent<Image>();
        Image AwayEmblemPanel = GameObject.Find("AwayEmblemPanel").GetComponent<Image>();
        if (!GameDirector.isPostSeason)
        {
            HomeEmblemPanel.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].homeTeam);
            AwayEmblemPanel.sprite = TeamEmblem.GetEmblem(GameDirector.schedule[t].awayTeam);
        } else
        {
            HomeEmblemPanel.sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[t].homeTeam);
            AwayEmblemPanel.sprite = TeamEmblem.GetEmblem(GameDirector.postSchedule[t].awayTeam);
        }

        // ������
        if (!GameDirector.isPostSeason)
        {
            HomeTeamNameB.text = DataToString.TeamToString(GameDirector.schedule[t].homeTeam);
            AwayTeamNameB.text = DataToString.TeamToString(GameDirector.schedule[t].awayTeam);
        } else
        {
            HomeTeamNameB.text = DataToString.TeamToString(GameDirector.postSchedule[t].homeTeam);
            AwayTeamNameB.text = DataToString.TeamToString(GameDirector.postSchedule[t].awayTeam);
        }
        var HomeInningScore = new[] { HomeInningS1, HomeInningS2, HomeInningS3, HomeInningS4, HomeInningS5, HomeInningS6, HomeInningS7, HomeInningS8, HomeInningS9, HomeInningS10, HomeInningS11, HomeInningS12 };
        var AwayInningScore = new[] { AwayInningS1, AwayInningS2, AwayInningS3, AwayInningS4, AwayInningS5, AwayInningS6, AwayInningS7, AwayInningS8, AwayInningS9, AwayInningS10, AwayInningS11, AwayInningS12 };
        var HomeRHEB = new[] { HomeRHEB1, HomeRHEB2, HomeRHEB3, HomeRHEB4 };
        var AwayRHEB = new[] { AwayRHEB1, AwayRHEB2, AwayRHEB3, AwayRHEB4 };
        for (int i = 0; i < 12; i++)
        {
            AwayInningScore[i].text = "-";
            HomeInningScore[i].text = "-";
        }
        for (int i = 1; i <= Game.Innings; i++)
        {
            if (i % 2 == 1) { // ��
                AwayInningScore[i / 2].text = game.InningScore[0, i / 2].ToString();
                if (i == Game.Innings)
                {
                    AwayInningScore[i / 2].color = Color.green;
                } else
                {
                    AwayInningScore[i / 2].color = Color.white;
                }
            } else { // ��
                HomeInningScore[(i / 2) - 1].text = game.InningScore[1, (i / 2) - 1].ToString();
                if (i == Game.Innings)
                {
                    HomeInningScore[(i / 2) - 1].color = Color.green;
                }
                else
                {
                    HomeInningScore[(i / 2) - 1].color = Color.white;
                }
            }  
        }
        for (int i = 0; i < 4; i++)
        {
            AwayRHEB[i].text = game.RHEB[0, i].ToString();
            HomeRHEB[i].text = game.RHEB[1, i].ToString();
        }

        // ���ھ��
        if (!GameDirector.isPostSeason)
        {
            HomeTeamNameC.text = DataToString.TeamToString(GameDirector.schedule[t].homeTeam);
            AwayTeamNameC.text = DataToString.TeamToString(GameDirector.schedule[t].awayTeam);
        } else
        {
            HomeTeamNameC.text = DataToString.TeamToString(GameDirector.postSchedule[t].homeTeam);
            AwayTeamNameC.text = DataToString.TeamToString(GameDirector.postSchedule[t].awayTeam);
        }
        HomeTeamScore.text = game.HomeScore.ToString();
        AwayTeamScore.text = game.AwayScore.ToString();

        // �޽���
        Message.text = game.Message;

        // �ƿ�
        if (Game.OutCount == 0) {
            LeftOut.color = Color.gray;
            RightOut.color = Color.gray;
        } else if (Game.OutCount == 1) {
            LeftOut.color = Color.red;
            RightOut.color = Color.gray;
        } else if (Game.OutCount == 2) {
            LeftOut.color = Color.red;
            RightOut.color = Color.red;
        }

        // ������ü
        if (isPitChange)
        {
            isPitChange = false;
            SCPanel.SetActive(true);
        } else
        {
            SCPanel.SetActive(false);
        }

        // ���̽�
        if (Game.isBase1 == true) {
            Base1.color = CBase;
        } else {
            Base1.color = Color.gray;
        }
        if (Game.isBase2 == true) {
            Base2.color = CBase;
        } else {
            Base2.color = Color.gray;
        }
        if (Game.isBase3 == true) {
            Base3.color = CBase;
        } else {
            Base3.color = Color.gray;
        }

        // ����
        if (!Game.IsGameOver)
        {
            if (Game.IsHomeAttack)
            {
                if (!GameDirector.isPostSeason)
                {
                    InningAttack.text = (Game.Innings / 2) + "ȸ�� " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " ����";
                } else
                {
                    InningAttack.text = (Game.Innings / 2) + "ȸ�� " + DataToString.TeamToString(GameDirector.postSchedule[t].homeTeam) + " ����";
                }
                CurrentInning.text = (Game.Innings / 2) + "ȸ��";
                PitName.color = AwayColor;
            }
            else
            {
                if (!GameDirector.isPostSeason)
                {
                    InningAttack.text = ((Game.Innings / 2) + 1) + "ȸ�� " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " ����";
                } else
                {
                    InningAttack.text = ((Game.Innings / 2) + 1) + "ȸ�� " + DataToString.TeamToString(GameDirector.postSchedule[t].awayTeam) + " ����";
                }
                CurrentInning.text = ((Game.Innings / 2) + 1) + "ȸ��";
                PitName.color = HomeColor;
            }
        } else
        {
            InningAttack.text = "��� ����"; // ���߿� ����� �����ָ� �ɵ�?
        }
        
        if (!GameDirector.isPostSeason)
        {
            // ���� ����
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if ((Game.IsHomeAttack == true && GameDirector.pitcher[i].team == GameDirector.schedule[t].awayTeam && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher) ||
                    (Game.IsHomeAttack == false && GameDirector.pitcher[i].team == GameDirector.schedule[t].homeTeam && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher))
                {
                    Game.TargetPitcherIndex = i;
                    PitName.text = GameDirector.pitcher[i].name;
                    PitGame.text = GameDirector.pitcher[i].game.ToString();
                    PitInning.text = GameDirector.pitcher[i].inningsPitched1.ToString() + "." + GameDirector.pitcher[i].inningsPitched2.ToString();
                    PitHPSlider.value = GameDirector.pitcher[i].HP;
                    if (GameDirector.pitcher[i].HP >= 60)
                    {
                        PitHPSlider.fillRect.GetComponent<Image>().color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].HP < 60 && GameDirector.pitcher[i].HP >= 30)
                    {
                        PitHPSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                    }
                    else
                    {
                        PitHPSlider.fillRect.GetComponent<Image>().color = Color.red;
                    }
                    if (Game.IsHomeAttack)
                    {
                        PitBallCount.text = Game.AwayCurrentPitcherBallCount.ToString();
                    }
                    else
                    {
                        PitBallCount.text = Game.HomeCurrentPitcherBallCount.ToString();
                    }
                    PitW.text = GameDirector.pitcher[i].win.ToString();
                    PitL.text = GameDirector.pitcher[i].lose.ToString();
                    PitS.text = GameDirector.pitcher[i].save.ToString();
                    PitH.text = GameDirector.pitcher[i].hold.ToString();
                    PitERA.text = GameDirector.pitcher[i].earnedRunAverage.ToString("F2");
                }
            }
        } else
        {
            // ���� ����
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if ((Game.IsHomeAttack == true && GameDirector.pitcher[i].team == GameDirector.postSchedule[t].awayTeam && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher) ||
                    (Game.IsHomeAttack == false && GameDirector.pitcher[i].team == GameDirector.postSchedule[t].homeTeam && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher))
                {
                    Game.TargetPitcherIndex = i;
                    PitName.text = GameDirector.pitcher[i].name;
                    PitGame.text = GameDirector.pitcher[i].game.ToString();
                    PitInning.text = GameDirector.pitcher[i].inningsPitched1.ToString() + "." + GameDirector.pitcher[i].inningsPitched2.ToString();
                    PitHPSlider.value = GameDirector.pitcher[i].HP;
                    if (GameDirector.pitcher[i].HP >= 60)
                    {
                        PitHPSlider.fillRect.GetComponent<Image>().color = Color.green;
                    }
                    else if (GameDirector.pitcher[i].HP < 60 && GameDirector.pitcher[i].HP >= 30)
                    {
                        PitHPSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                    }
                    else
                    {
                        PitHPSlider.fillRect.GetComponent<Image>().color = Color.red;
                    }
                    if (Game.IsHomeAttack)
                    {
                        PitBallCount.text = Game.AwayCurrentPitcherBallCount.ToString();
                    }
                    else
                    {
                        PitBallCount.text = Game.HomeCurrentPitcherBallCount.ToString();
                    }
                    PitW.text = GameDirector.pitcher[i].win.ToString();
                    PitL.text = GameDirector.pitcher[i].lose.ToString();
                    PitS.text = GameDirector.pitcher[i].save.ToString();
                    PitH.text = GameDirector.pitcher[i].hold.ToString();
                    PitERA.text = GameDirector.pitcher[i].earnedRunAverage.ToString("F2");
                }
            }
        }

        if (!GameDirector.isPostSeason)
        {
            // Ȩ��
            for (int n = 0; n < GameDirector.batterCount; n++)
            {
                if (GameDirector.batter[n].team == GameDirector.schedule[t].homeTeam)
                {
                    if (Game.HomeCurrentBatter == GameDirector.batter[n].posInTeam && Game.IsHomeAttack)
                    {
                        Game.TargetBatterIndex = n;
                        HomeBResult.text = GameDirector.batter[Game.TargetBatterIndex].todayResult;
                        HomeBResultNum.text = GameDirector.batter[n].name + " ���� ��� (" + GameDirector.batter[Game.TargetBatterIndex].todayHit + "/" + GameDirector.batter[Game.TargetBatterIndex].todayAB + ")";
                        AwayBResult.text = "";
                        AwayBResultNum.text = "";
                    }
                    int batterPosition = GameDirector.batter[n].posInTeam - 101;
                    if (batterPosition >= 0 && batterPosition < 9)
                    {
                        GameObject currentLine = GetLineHomeBatterObject(batterPosition);
                        TMP_Text[] textArray;
                        textArray = currentLine.GetComponentsInChildren<TMP_Text>();
                        var targetBatter = GameDirector.batter[n];
                        textArray[1].text = targetBatter.name;
                        textArray[2].text = DataToString.PosToString(targetBatter.pos);
                        textArray[3].text = targetBatter.battingAverage.ToString("F3");
                        textArray[4].text = targetBatter.homerun.ToString();
                        textArray[5].text = targetBatter.RBI.ToString();
                        Color highlightColor = (Game.HomeCurrentBatter == targetBatter.posInTeam && Game.IsHomeAttack) ? CHighLight : CBasic;
                        TMP_FontAsset fontHome = (Game.HomeCurrentBatter == targetBatter.posInTeam && game.IsHomeAttack) ? boldFont : regularFont;
                        currentLine.GetComponent<Image>().color = highlightColor;

                    }
                }
            }

            // ������
            for (int n = 0; n < GameDirector.batterCount; n++)
            {
                if (GameDirector.batter[n].team == GameDirector.schedule[t].awayTeam)
                {
                    if (Game.AwayCurrentBatter == GameDirector.batter[n].posInTeam && !Game.IsHomeAttack)
                    {
                        Game.TargetBatterIndex = n;
                        AwayBResult.text = GameDirector.batter[Game.TargetBatterIndex].todayResult;
                        AwayBResultNum.text = GameDirector.batter[n].name + " ���� ��� (" + GameDirector.batter[Game.TargetBatterIndex].todayHit + "/" + GameDirector.batter[Game.TargetBatterIndex].todayAB + ")";
                        HomeBResult.text = "";
                        HomeBResultNum.text = "";
                    }
                    int batterPosition = GameDirector.batter[n].posInTeam - 101;
                    if (batterPosition >= 0 && batterPosition < 9)
                    {
                        GameObject currentLine = GetLineAwayBatterObject(batterPosition);
                        TMP_Text[] textArray;
                        textArray = currentLine.GetComponentsInChildren<TMP_Text>();
                        var targetBatter = GameDirector.batter[n];
                        textArray[1].text = targetBatter.name;
                        textArray[2].text = DataToString.PosToString(targetBatter.pos);
                        textArray[3].text = targetBatter.battingAverage.ToString("F3");
                        textArray[4].text = targetBatter.homerun.ToString();
                        textArray[5].text = targetBatter.RBI.ToString();
                        Color highlightColor = (Game.AwayCurrentBatter == targetBatter.posInTeam && !game.IsHomeAttack) ? CHighLight : CBasic;
                        TMP_FontAsset fontAway = (Game.AwayCurrentBatter == targetBatter.posInTeam && !game.IsHomeAttack) ? boldFont : regularFont;
                        currentLine.GetComponent<Image>().color = highlightColor;

                    }
                }
            }
        } else
        {
            // Ȩ��
            for (int n = 0; n < GameDirector.batterCount; n++)
            {
                if (GameDirector.batter[n].team == GameDirector.postSchedule[t].homeTeam)
                {
                    if (Game.HomeCurrentBatter == GameDirector.batter[n].posInTeam && Game.IsHomeAttack)
                    {
                        Game.TargetBatterIndex = n;
                        HomeBResult.text = GameDirector.batter[Game.TargetBatterIndex].todayResult;
                        HomeBResultNum.text = GameDirector.batter[n].name + " ���� ��� (" + GameDirector.batter[Game.TargetBatterIndex].todayHit + "/" + GameDirector.batter[Game.TargetBatterIndex].todayAB + ")";
                        AwayBResult.text = "";
                        AwayBResultNum.text = "";
                    }
                    int batterPosition = GameDirector.batter[n].posInTeam - 101;
                    if (batterPosition >= 0 && batterPosition < 9)
                    {
                        GameObject currentLine = GetLineHomeBatterObject(batterPosition);
                        TMP_Text[] textArray;
                        textArray = currentLine.GetComponentsInChildren<TMP_Text>();
                        var targetBatter = GameDirector.batter[n];
                        textArray[1].text = targetBatter.name;
                        textArray[2].text = DataToString.PosToString(targetBatter.pos);
                        textArray[3].text = targetBatter.battingAverage.ToString("F3");
                        textArray[4].text = targetBatter.homerun.ToString();
                        textArray[5].text = targetBatter.RBI.ToString();
                        Color highlightColor = (Game.HomeCurrentBatter == targetBatter.posInTeam && Game.IsHomeAttack) ? CHighLight : CBasic;
                        TMP_FontAsset fontHome = (Game.HomeCurrentBatter == targetBatter.posInTeam && game.IsHomeAttack) ? boldFont : regularFont;
                        currentLine.GetComponent<Image>().color = highlightColor;

                    }
                }
            }

            // ������
            for (int n = 0; n < GameDirector.batterCount; n++)
            {
                if (GameDirector.batter[n].team == GameDirector.postSchedule[t].awayTeam)
                {
                    if (Game.AwayCurrentBatter == GameDirector.batter[n].posInTeam && !Game.IsHomeAttack)
                    {
                        Game.TargetBatterIndex = n;
                        AwayBResult.text = GameDirector.batter[Game.TargetBatterIndex].todayResult;
                        AwayBResultNum.text = GameDirector.batter[n].name + " ���� ��� (" + GameDirector.batter[Game.TargetBatterIndex].todayHit + "/" + GameDirector.batter[Game.TargetBatterIndex].todayAB + ")";
                        HomeBResult.text = "";
                        HomeBResultNum.text = "";
                    }
                    int batterPosition = GameDirector.batter[n].posInTeam - 101;
                    if (batterPosition >= 0 && batterPosition < 9)
                    {
                        GameObject currentLine = GetLineAwayBatterObject(batterPosition);
                        TMP_Text[] textArray;
                        textArray = currentLine.GetComponentsInChildren<TMP_Text>();
                        var targetBatter = GameDirector.batter[n];
                        textArray[1].text = targetBatter.name;
                        textArray[2].text = DataToString.PosToString(targetBatter.pos);
                        textArray[3].text = targetBatter.battingAverage.ToString("F3");
                        textArray[4].text = targetBatter.homerun.ToString();
                        textArray[5].text = targetBatter.RBI.ToString();
                        Color highlightColor = (Game.AwayCurrentBatter == targetBatter.posInTeam && !game.IsHomeAttack) ? CHighLight : CBasic;
                        TMP_FontAsset fontAway = (Game.AwayCurrentBatter == targetBatter.posInTeam && !game.IsHomeAttack) ? boldFont : regularFont;
                        currentLine.GetComponent<Image>().color = highlightColor;

                    }
                }
            }
        }
    }

    IEnumerator GameEnd(int t, Game game)
    {
        // UI �� �÷��� ����
        GameDirector.remainGames--;
        GameMode = 0;
        game.IsGameOver = true;
        game.Message = "��Ⱑ ���� �Ǿ����ϴ�.";
        ProcessPanel.SetActive(false);
        //GameOverCanvas.SetActive(true);
        ResultPanel.SetActive(true);
        PitcherPanel.SetActive(false);

        // ��� ������ ����
        if (game.Innings == 18 && game.HomeScore > game.AwayScore && game.InningScore[1, 8] == 0)
        {
            game.Innings--; // ������ ��Ÿ�� ���ָ� �ȵǴµ� 9ȸ�ʿ� �������� ���־���
        }
        if (game.Innings == 19 || game.Innings == 21 || game.Innings == 23 || game.Innings == 25)
        {
            game.Innings--; // 10ȸ��, 11ȸ��, 12ȸ��, 13ȸ�ʿ� �����ٴ°� �� ���̴׿� ����
        }

        // �����쿡 ���� ����
        if (!GameDirector.isPostSeason)
        {
            GameDirector.schedule[t].homeScore = game.HomeScore;
            GameDirector.schedule[t].awayScore = game.AwayScore;
        } else
        {
            GameDirector.postSchedule[t].homeScore = game.HomeScore;
            GameDirector.postSchedule[t].awayScore = game.AwayScore;
        }

        // ��� ��� ���
        if (game.HomeScore > game.AwayScore)
        {
            if (!GameDirector.isPostSeason)
            {
                ResultInformation.text = game.HomeScore + ":" + game.AwayScore + " " + DataToString.TeamToString(GameDirector.schedule[t].homeTeam) + " �¸�";
            } else
            {
                ResultInformation.text = game.HomeScore + ":" + game.AwayScore + " " + DataToString.TeamToString(GameDirector.postSchedule[t].homeTeam) + " �¸�";
            }
            
        }
        else if (game.HomeScore < game.AwayScore)
        {
            if (!GameDirector.isPostSeason)
            {
                ResultInformation.text = game.HomeScore + ":" + game.AwayScore + " " + DataToString.TeamToString(GameDirector.schedule[t].awayTeam) + " �¸�";
            } else
            {
                ResultInformation.text = game.HomeScore + ":" + game.AwayScore + " " + DataToString.TeamToString(GameDirector.postSchedule[t].awayTeam) + " �¸�";
            }
        }
        else if (game.HomeScore == game.AwayScore)
        {
            ResultInformation.text = game.HomeScore + ":" + game.AwayScore + " ���º�";
        }

        // ��� ����
        if (!GameDirector.isPostSeason && !GameDirector.isSimulation)
        {
            if (GameDirector.schedule[t].homeTeam == GameDirector.myTeam && game.HomeScore - game.AwayScore >= 10)
            {
                GetValueMail.VeryBigWin(game.HomeScore, game.AwayScore, GameDirector.schedule[t].awayTeam);
            }
            else if (GameDirector.schedule[t].awayTeam == GameDirector.myTeam && game.AwayScore - game.HomeScore >= 10)
            {
                GetValueMail.VeryBigWin(game.AwayScore, game.HomeScore, GameDirector.schedule[t].homeTeam);
            }

            if (GameDirector.schedule[t].homeTeam == GameDirector.myTeam && game.AwayScore - game.HomeScore >= 10)
            {
                GetValueMail.VeryBigDefeat(game.HomeScore, game.AwayScore, GameDirector.schedule[t].awayTeam);
            }
            else if (GameDirector.schedule[t].awayTeam == GameDirector.myTeam && game.HomeScore - game.AwayScore >= 10)
            {
                GetValueMail.VeryBigDefeat(game.AwayScore, game.HomeScore, GameDirector.schedule[t].homeTeam);
            }
        }

        bool isSaveP = false;
        if (!GameDirector.isPostSeason)
        {
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if (GameDirector.pitcher[i].team == GameDirector.schedule[t].homeTeam)
                {
                    if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoWinCondition)
                    {
                        WinnerP.text = "�¸� ����: " + GameDirector.pitcher[i].name;
                        if (game.HomeCurrentPitcherInn == 27 && game.HomeCurrentPitcherEarn == 0)
                        {
                            WinnerP.text += " (�Ϻ�)";
                        }
                        else if (game.HomeCurrentPitcherInn == 27 && game.HomeCurrentPitcherEarn != 0)
                        {
                            WinnerP.text += " (����)";
                        }
                        GameDirector.pitcher[i].win++;
                    }
                    else if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoLoseCondition)
                    {
                        LoserP.text = "���� ����: " + GameDirector.pitcher[i].name;
                        GameDirector.pitcher[i].lose++;
                    }

                    if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.HomeCurrentHoldPitcher && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher && GameDirector.pitcher[i].posInTeam != game.WhoWinCondition)
                    {
                        SaveP.text = "���̺� ����: " + GameDirector.pitcher[i].name;
                        GameDirector.pitcher[i].save++;
                        isSaveP = true;
                    }
                }
                else if (GameDirector.pitcher[i].team == GameDirector.schedule[t].awayTeam)
                {
                    if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoLoseCondition)
                    {
                        LoserP.text = "���� ����: " + GameDirector.pitcher[i].name;
                        GameDirector.pitcher[i].lose++;
                    }
                    else if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoWinCondition)
                    {
                        WinnerP.text = "�¸� ����: " + GameDirector.pitcher[i].name;
                        if (game.AwayCurrentPitcherInn == 27 && game.AwayCurrentPitcherEarn == 0)
                        {
                            WinnerP.text += " (�Ϻ�)";
                        }
                        else if (game.AwayCurrentPitcherInn == 27 && game.AwayCurrentPitcherEarn != 0)
                        {
                            WinnerP.text += " (����)";
                        }
                        GameDirector.pitcher[i].win++;
                    }
                    if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.AwayCurrentHoldPitcher && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher && GameDirector.pitcher[i].posInTeam != game.WhoWinCondition)
                    {
                        SaveP.text = "���̺� ����: " + GameDirector.pitcher[i].name;
                        GameDirector.pitcher[i].save++;
                        isSaveP = true;
                    }
                }
            }
        } else
        {
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if (GameDirector.pitcher[i].team == GameDirector.postSchedule[t].homeTeam)
                {
                    if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoWinCondition)
                    {
                        WinnerP.text = "�¸� ����: " + GameDirector.pitcher[i].name;
                        if (game.HomeCurrentPitcherInn == 27 && game.HomeCurrentPitcherEarn == 0)
                        {
                            WinnerP.text += " (�Ϻ�)";
                        }
                        else if (game.HomeCurrentPitcherInn == 27 && game.HomeCurrentPitcherEarn != 0)
                        {
                            WinnerP.text += " (����)";
                        }
                        //GameDirector.pitcher[i].win++;
                    }
                    else if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoLoseCondition)
                    {
                        LoserP.text = "���� ����: " + GameDirector.pitcher[i].name;
                        //GameDirector.pitcher[i].lose++;
                    }

                    if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.HomeCurrentHoldPitcher && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher && GameDirector.pitcher[i].posInTeam != game.WhoWinCondition)
                    {
                        SaveP.text = "���̺� ����: " + GameDirector.pitcher[i].name;
                        //GameDirector.pitcher[i].save++;
                        isSaveP = true;
                    }
                }
                else if (GameDirector.pitcher[i].team == GameDirector.postSchedule[t].awayTeam)
                {
                    if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoLoseCondition)
                    {
                        LoserP.text = "���� ����: " + GameDirector.pitcher[i].name;
                        //GameDirector.pitcher[i].lose++;
                    }
                    else if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.WhoWinCondition)
                    {
                        WinnerP.text = "�¸� ����: " + GameDirector.pitcher[i].name;
                        if (game.AwayCurrentPitcherInn == 27 && game.AwayCurrentPitcherEarn == 0)
                        {
                            WinnerP.text += " (�Ϻ�)";
                        }
                        else if (game.AwayCurrentPitcherInn == 27 && game.AwayCurrentPitcherEarn != 0)
                        {
                            WinnerP.text += " (����)";
                        }
                        //GameDirector.pitcher[i].win++;
                    }
                    if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.AwayCurrentHoldPitcher && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher && GameDirector.pitcher[i].posInTeam != game.WhoWinCondition)
                    {
                        SaveP.text = "���̺� ����: " + GameDirector.pitcher[i].name;
                        //GameDirector.pitcher[i].save++;
                        isSaveP = true;
                    }
                }
            }
        }
        
        // ���� ��� ����
        if (game.HomeScore == game.AwayScore)
        {
            Destroy(WinnerP.gameObject);
            Destroy(LoserP.gameObject);
        }

        // ���̺� ������ ��� ����
        if (!isSaveP)
        {
            Destroy(SaveP.gameObject);
        }

        // ��� ��� ������Ʈ �� �������� ����
        if (!GameDirector.isPostSeason)
        {
            if (game.HomeScore > game.AwayScore)
            {
                GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].win++;
                GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].lose++;
            }
            else if (game.HomeScore < game.AwayScore)
            {
                GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].lose++;
                GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].win++;
            }
            else if (game.HomeScore == game.AwayScore)
            {
                GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].draw++;
                GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].draw++;
            }

            GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].currentSP++;
            GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].currentSP++;
            if (GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].currentSP == 6)
            {
                GameDirector.Teams[(int)GameDirector.schedule[t].homeTeam].currentSP = 1;
            }
            if (GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].currentSP == 6)
            {
                GameDirector.Teams[(int)GameDirector.schedule[t].awayTeam].currentSP = 1;
            }
        } else
        {
            GameDirector.Teams[(int)GameDirector.postSchedule[t].homeTeam].currentSP++;
            GameDirector.Teams[(int)GameDirector.postSchedule[t].awayTeam].currentSP++;
            if (GameDirector.Teams[(int)GameDirector.postSchedule[t].homeTeam].currentSP == 6)
            {
                GameDirector.Teams[(int)GameDirector.postSchedule[t].homeTeam].currentSP = 1;
            }
            if (GameDirector.Teams[(int)GameDirector.postSchedule[t].awayTeam].currentSP == 6)
            {
                GameDirector.Teams[(int)GameDirector.postSchedule[t].awayTeam].currentSP = 1;
            }
        }
        

        // Ÿ���� ���� ��� �ʱ�ȭ
        for (int i = 0; i<GameDirector.batterCount; i++)
        {
            if (!GameDirector.isPostSeason && GameDirector.batter[i].team == GameDirector.myTeam && !GameDirector.isSimulation)
            {
                if (GameDirector.batter[i].game >= 5 && GameDirector.batter[i].battingAverage < 0.1f && GameDirector.batter[i].todayHit == 0)
                {
                    GetValueMail.SadBatter(GameDirector.batter[i].name);
                } else if (GameDirector.batter[i].battingAverage > 0.3f && GameDirector.batter[i].todayHit >= 4)
                {
                    GetValueMail.HappyBatter(GameDirector.batter[i].name);
                }
            }
            GameDirector.batter[i].todayAB = 0;
            GameDirector.batter[i].todayHit = 0;
            GameDirector.batter[i].todayResult = "";
            GameDirector.batter[i].isAleadyAppear = false;
            
        }

        // ������ ���� ��� �ʱ�ȭ
        for (int i = 0; i < GameDirector.pitcherCount; i++)
        {
            GameDirector.pitcher[i].isAleadyAppear = false;
            float k = GameDirector.pitcher[i].inningsPitched2 * (float)1 / 3;
            GameDirector.pitcher[i].earnedRunAverage = (GameDirector.pitcher[i].earnedRuns * 9) / (float)(GameDirector.pitcher[i].inningsPitched1 + k);
            GameDirector.pitcher[i].WHIP = (float)(GameDirector.pitcher[i].hitAllowed + GameDirector.pitcher[i].baseOnBall) / (float)(GameDirector.pitcher[i].inningsPitched1 + k);
        }

        // �ش� ������ ���� ���� ó��
        if (!GameDirector.isPostSeason)
        {
            GameDirector.schedule[t].isEnd = true;
        } else
        {
            GameDirector.postSchedule[t].isEnd = true;
        }
        
        if (!GameDirector.isPostSeason)
        {
            // �� ������ �������� ��ư ������ ���� ���
            if (!GameDirector.isSimulation)
            {
                GameOverPanel.SetActive(true);
                yield return new WaitUntil(() => Leaving());
                isLeave = false;
            }

            // �� ������ �������� ���� ���� ������ ���ӵ� ����
            if ((GameDirector.schedule[t].homeTeam == GameDirector.myTeam || GameDirector.schedule[t].awayTeam == GameDirector.myTeam) && !GameDirector.isSimulation)
            {
                GameDirector.isSimulation = true;
                GameDirector.tempMyTeam = GameDirector.myTeam;
            }

            // ���� ��⸦ ��� ����� �ϸ鼭 ����
            for (int i = 0; i < GameDirector.totalMatchCount; i++)
            {
                if (GameDirector.schedule[i].dates.year == GameDirector.schedule[t].dates.year &&
                    GameDirector.schedule[i].dates.month == GameDirector.schedule[t].dates.month &&
                    GameDirector.schedule[i].dates.day == GameDirector.schedule[t].dates.day)
                {
                    if (GameDirector.schedule[i].isEnd == false)
                    {
                        GameDirector.myTeam = GameDirector.schedule[i].homeTeam;
                        GameDirector.PlayGame();
                    }
                }
            }

            // ��� ��Ⱑ �������� ����� ��������
            if (GameDirector.isSimulation)
            {
                loadSlider.value = 5 - GameDirector.remainGames;
                if (GameDirector.remainGames == 0)
                {
                    LoadMessage.text = "��� ��Ⱑ ���� �Ǿ����ϴ�.";
                    ALLENDPanel.SetActive(true);
                }
                yield return new WaitUntil(() => Leaving());
                isLeave = false;
            }
        } else // ����Ʈ ���� �ϰ��
        {
            if (t < 2) // ���ϵ�ī��
            {
                if (t==0)
                {
                    if (game.HomeScore > game.AwayScore)
                    {
                        GameDirector.postSchedule[1].isPass = true;
                        GameDirector.postSchedule[2].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[2].awayTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[3].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[3].awayTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[4].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[4].homeTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[5].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[5].homeTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[6].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[6].awayTeam = GameDirector.postSchedule[t].homeTeam;
                    }
                } else if (t==1)
                {
                    if (game.HomeScore > game.AwayScore)
                    {
                        GameDirector.postSchedule[2].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[2].awayTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[3].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[3].awayTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[4].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[4].homeTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[5].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[5].homeTeam = GameDirector.postSchedule[t].homeTeam;

                        GameDirector.postSchedule[6].DownTeam = GameDirector.postSchedule[t].homeTeam;
                        GameDirector.postSchedule[6].awayTeam = GameDirector.postSchedule[t].homeTeam;
                    } else
                    {
                        GameDirector.postSchedule[2].DownTeam = GameDirector.postSchedule[t].awayTeam;
                        GameDirector.postSchedule[2].awayTeam = GameDirector.postSchedule[t].awayTeam;

                        GameDirector.postSchedule[3].DownTeam = GameDirector.postSchedule[t].awayTeam;
                        GameDirector.postSchedule[3].awayTeam = GameDirector.postSchedule[t].awayTeam;

                        GameDirector.postSchedule[4].DownTeam = GameDirector.postSchedule[t].awayTeam;
                        GameDirector.postSchedule[4].homeTeam = GameDirector.postSchedule[t].awayTeam;

                        GameDirector.postSchedule[5].DownTeam = GameDirector.postSchedule[t].awayTeam;
                        GameDirector.postSchedule[5].homeTeam = GameDirector.postSchedule[t].awayTeam;

                        GameDirector.postSchedule[6].DownTeam = GameDirector.postSchedule[t].awayTeam;
                        GameDirector.postSchedule[6].awayTeam = GameDirector.postSchedule[t].awayTeam;
                    }
                }
            } else if (t >= 2 && t <= 6) // ���÷��̿���
            {
                if (game.HomeScore > game.AwayScore)
                {
                    if (t == 2 || t == 3 || t == 6)
                    {
                        GameDirector.UpTeamWin++;
                    } else
                    {
                        GameDirector.DownTeamWin++;
                    }
                } else
                {
                    if (t == 2 || t == 3 || t == 6)
                    {
                        GameDirector.DownTeamWin++;
                    }
                    else
                    {
                        GameDirector.UpTeamWin++;
                    }
                }
                if (GameDirector.UpTeamWin >= 3) // 3����
                {
                    GameDirector.UpTeamWin = 0;
                    GameDirector.DownTeamWin = 0;
                    if (t == 4)
                    {
                        GameDirector.postSchedule[5].isPass = true;
                        GameDirector.postSchedule[6].isPass = true;
                    } else if (t == 5)
                    {
                        GameDirector.postSchedule[6].isPass = true;
                    }
                    GameDirector.postSchedule[7].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[7].awayTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[8].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[8].awayTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[9].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[9].homeTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[10].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[10].homeTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[11].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[11].awayTeam = GameDirector.postSchedule[t].UpTeam;
                } else if (GameDirector.DownTeamWin >= 3) 
                {
                    GameDirector.UpTeamWin = 0;
                    GameDirector.DownTeamWin = 0;
                    if (t == 4)
                    {
                        GameDirector.postSchedule[5].isPass = true;
                        GameDirector.postSchedule[6].isPass = true;
                    }
                    else if (t == 5)
                    {
                        GameDirector.postSchedule[6].isPass = true;
                    }
                    GameDirector.postSchedule[7].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[7].awayTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[8].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[8].awayTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[9].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[9].homeTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[10].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[10].homeTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[11].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[11].awayTeam = GameDirector.postSchedule[t].DownTeam;
                }
            }
            else if (t >= 7 && t <= 11) // �÷��̿���
            {
                if (game.HomeScore > game.AwayScore)
                {
                    if (t == 7 || t == 8 || t == 11)
                    {
                        GameDirector.UpTeamWin++;
                    }
                    else
                    {
                        GameDirector.DownTeamWin++;
                    }
                }
                else
                {
                    if (t == 7 || t == 8 || t == 11)
                    {
                        GameDirector.DownTeamWin++;
                    }
                    else
                    {
                        GameDirector.UpTeamWin++;
                    }
                }
                if (GameDirector.UpTeamWin >= 3) // 3����
                {
                    GameDirector.UpTeamWin = 0;
                    GameDirector.DownTeamWin = 0;
                    if (t == 9)
                    {
                        GameDirector.postSchedule[10].isPass = true;
                        GameDirector.postSchedule[11].isPass = true;
                    }
                    else if (t == 10)
                    {
                        GameDirector.postSchedule[11].isPass = true;
                    }
                    GameDirector.postSchedule[12].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[12].awayTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[13].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[13].awayTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[14].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[14].homeTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[15].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[15].homeTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[16].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[16].awayTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[17].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[17].awayTeam = GameDirector.postSchedule[t].UpTeam;

                    GameDirector.postSchedule[18].DownTeam = GameDirector.postSchedule[t].UpTeam;
                    GameDirector.postSchedule[18].awayTeam = GameDirector.postSchedule[t].UpTeam;
                }
                else if (GameDirector.DownTeamWin >= 3)
                {
                    GameDirector.UpTeamWin = 0;
                    GameDirector.DownTeamWin = 0;
                    if (t == 9)
                    {
                        GameDirector.postSchedule[10].isPass = true;
                        GameDirector.postSchedule[11].isPass = true;
                    }
                    else if (t == 10)
                    {
                        GameDirector.postSchedule[11].isPass = true;
                    }
                    GameDirector.postSchedule[12].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[12].awayTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[13].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[13].awayTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[14].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[14].homeTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[15].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[15].homeTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[16].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[16].awayTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[17].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[17].awayTeam = GameDirector.postSchedule[t].DownTeam;

                    GameDirector.postSchedule[18].DownTeam = GameDirector.postSchedule[t].DownTeam;
                    GameDirector.postSchedule[18].awayTeam = GameDirector.postSchedule[t].DownTeam;
                }
            }
            else if (t >= 12 && t <= 18) // �ѱ��ø���
            {
                if (game.HomeScore > game.AwayScore)
                {
                    if (t == 12 || t == 13 || t == 16 || t == 17 || t == 18)
                    {
                        GameDirector.UpTeamWin++;
                    }
                    else
                    {
                        GameDirector.DownTeamWin++;
                    }
                }
                else
                {
                    if (t == 12 || t == 13 || t == 16 || t == 17 || t == 18)
                    {
                        GameDirector.DownTeamWin++;
                    }
                    else
                    {
                        GameDirector.UpTeamWin++;
                    }
                }
                if (GameDirector.UpTeamWin >= 4) // 4����
                {
                    GameDirector.UpTeamWin = 0;
                    GameDirector.DownTeamWin = 0;
                    if (t == 15)
                    {
                        GameDirector.postSchedule[16].isPass = true;
                        GameDirector.postSchedule[17].isPass = true;
                        GameDirector.postSchedule[18].isPass = true;
                    }
                    else if (t == 16)
                    {
                        GameDirector.postSchedule[17].isPass = true;
                        GameDirector.postSchedule[18].isPass = true;
                    }
                    else if (t == 17)
                    {
                        GameDirector.postSchedule[18].isPass = true;
                    }
                    GameDirector.KingTeam = GameDirector.postSchedule[t].UpTeam;
                }
                else if (GameDirector.DownTeamWin >= 4)
                {
                    GameDirector.UpTeamWin = 0;
                    GameDirector.DownTeamWin = 0;
                    if (t == 15)
                    {
                        GameDirector.postSchedule[16].isPass = true;
                        GameDirector.postSchedule[17].isPass = true;
                        GameDirector.postSchedule[18].isPass = true;
                    }
                    else if (t == 16)
                    {
                        GameDirector.postSchedule[17].isPass = true;
                        GameDirector.postSchedule[18].isPass = true;
                    }
                    else if (t == 17)
                    {
                        GameDirector.postSchedule[18].isPass = true;
                    }
                    GameDirector.KingTeam = GameDirector.postSchedule[t].DownTeam;
                }
            }

            // �� ������ �������� ��ư ������ ���� ���
            if (!GameDirector.isSimulation)
            {
                GameOverPanel.SetActive(true);
                yield return new WaitUntil(() => Leaving());
                isLeave = false;
            }
        }

        // ��¥ ������Ʈ
        GameDirector.currentDate = CreateData.UpdateDate(GameDirector.currentDate);
        GameDirector.isSimulation = false;
        GameDirector.myTeam = GameDirector.tempMyTeam;

        // ��� ���� ü�� ȸ��
        for (int i = 0; i < GameDirector.pitcherCount; i++)
        {
            GameDirector.pitcher[i].HP += 15;
            if (GameDirector.pitcher[i].HP >= 100)
            {
                GameDirector.pitcher[i].HP = 100;
            }
        }

        // ���� �� ���� ������ ������
        SaveLoad.SaveData();
        SceneManager.LoadScene("Main");
    }

    void GameConditionsCheck(Game Game)
    {
        // �¸� ���� ���� ���� ����
        if (Game.HomeScore > Game.AwayScore)
        {
            // ���� �߰��� �� if�� (�׽�Ʈ �ؾ���)
            //if (!Game.isHomeLead && Game.WhoLoseCondition != 1000)
            //{
            //    Game.WhoLoseCondition = 1000;
            //}
            //if (!Game.isHomeLead)
            //{
            //    Game.isHomeLead = true;
            //}
            // Ȩ ������ 5ȸ��(9)�� �������� ���� �¸� ���
            if (Game.Innings >= 10 && Game.WhoWinCondition == 1000)
            {
                Game.WhoWinCondition = Game.HomeCurrentPitcher;
            }
            if (Game.WhoLoseCondition == 1000)
            {
                Game.WhoLoseCondition = Game.AwayCurrentPitcher;
            }
        }
        else if (Game.HomeScore < Game.AwayScore)
        {
            // ���� �߰��� �� if�� (�׽�Ʈ �ؾ���)
            //if (Game.isHomeLead && Game.WhoLoseCondition != 1000)
            //{
            //    Game.WhoLoseCondition = 1000;
            //}
            //if (Game.isHomeLead)
            //{
            //    Game.isHomeLead = false;
            //}
            // ���� ������ 5ȸ��(10)�� �������� ���� �¸� ���
            if (Game.Innings >= 11 && Game.WhoWinCondition == 1000)
            {
                Game.WhoWinCondition = Game.AwayCurrentPitcher;
            }
            if (Game.WhoLoseCondition == 1000)
            {
                Game.WhoLoseCondition = Game.HomeCurrentPitcher;
            }
        }
        else if ((Game.HomeScore == Game.AwayScore))
        {
            // �ʱ� ���� Ȥ�� ������ �ʱ�ȭ
            Game.WhoWinCondition = 1000;
            Game.WhoLoseCondition = 1000;
        }

        // Ȧ�� ���� ����
        if ((Game.HomeScore - 3 <= Game.AwayScore) &&
            (Game.HomeScore > Game.AwayScore) &&
            Game.Innings >= 11 &&
            Game.HomeCurrentPitcherInn >= 1 &&
            Game.HomeCurrentPitcher != Game.WhoWinCondition &&
            Game.HomeCurrentPitcher >= 6)
        {
            // Ȩ ����
            Game.HomeCurrentHoldPitcher = Game.HomeCurrentPitcher;
        }
        else if ((Game.HomeScore >= Game.AwayScore - 3) &&
            (Game.HomeScore < Game.AwayScore) &&
            Game.Innings >= 12 &&
            Game.AwayCurrentPitcherInn >= 1 &&
            Game.AwayCurrentPitcher != Game.WhoWinCondition &&
            Game.AwayCurrentPitcher >= 6)
        {
            // ���� ����
            Game.AwayCurrentHoldPitcher = Game.AwayCurrentPitcher;
        }
        else if (Game.HomeScore <= Game.AwayScore && Game.HomeCurrentHoldPitcher != 1000)
        {
            // ���� ����Ű�� Ȧ�� �ʱ�ȭ
            Game.HomeCurrentHoldPitcher = 1000;
        }
        else if (Game.HomeScore >= Game.AwayScore && Game.AwayCurrentHoldPitcher != 1000)
        {
            // ���� ����Ű�� Ȧ�� �ʱ�ȭ
            Game.AwayCurrentHoldPitcher = 1000;
        }
    }

    void ComputerAI(int t, Game game)
    {
        if (!game.IsHomeAttack)
        {
            EvaluatePitcherChange(game.HomeCurrentPitcher, game.HomeCurrentPitcherInn, game.HomeCurrentPitcherEarn, t, game);
        }
        else
        {
            EvaluatePitcherChange(game.AwayCurrentPitcher, game.AwayCurrentPitcherInn, game.AwayCurrentPitcherEarn, t, game);
        }
    }

    void EvaluatePitcherChange(int currentPitcher, int pitcherInn, int pitcherEarn, int t, Game game)
    {
        int changePossibility = -1;
        if (currentPitcher <= 5) // ���� ����
        {
            if (pitcherInn < 15 && pitcherEarn >= 5 && pitcherEarn <= 6) // 4.2�̴� ���� 5~6����
            {
                changePossibility = UnityEngine.Random.Range(0, 4); // �� �� ��ü ���ɼ� 40% 
            }
            else if (pitcherInn <= 15 && pitcherEarn >= 7) // 5�̴� ���� 7���� �̻�
            {
                changePossibility = UnityEngine.Random.Range(0, 9); // �� �� ��ü ���ɼ� 100% 
            }
            else if (pitcherInn < 18 && pitcherInn >= 15 && pitcherEarn >= 2) // 5~5.2�̴� 2���� �̻�
            {
                changePossibility = UnityEngine.Random.Range(0, 1); // �� �� ��ü ���ɼ� 10%
            }
            else if (pitcherInn < 21 && pitcherInn >= 18 && pitcherEarn >= 2) // 6~6.2�̴� 2���� �̻�
            {
                changePossibility = UnityEngine.Random.Range(0, 2); // �� �� ��ü ���ɼ� 20%
            }
            else if (pitcherInn >= 21 && pitcherEarn <= 1) // 7�̴� �̻� 1���� �̻� ����
            {
                changePossibility = UnityEngine.Random.Range(0, 2); // �� �� ��ü ���ɼ� 20%
            }
            else if (pitcherInn >= 21 && pitcherEarn > 1) // 7�̴� �̻� 2���� �̻� �̻�
            {
                changePossibility = UnityEngine.Random.Range(0, 5); // �� �� ��ü ���ɼ� 50%
            }
        }
        else // ���� ����
        {
            if (pitcherInn >= 3 && pitcherEarn == 0) // 1�̴� �̻� 0����
            {
                changePossibility = UnityEngine.Random.Range(0, 2); // �� �� ��ü ���ɼ� 20%
            }
            else if (pitcherInn >= 3 && pitcherEarn >= 1) // 1�̴� �̻� 1���� �̻�
            {
                changePossibility = UnityEngine.Random.Range(0, 4); // �� �� ��ü ���ɼ� 40%
            }
            else if (pitcherInn < 3 && pitcherEarn >= 1) // 1�̴� ���� 1���� �̻�
            {
                changePossibility = UnityEngine.Random.Range(0, 4); // �� �� ��ü ���ɼ� 40%
            }
        }
        if (UnityEngine.Random.Range(0, 10) <= changePossibility)
        {
            isPitChange = true;
            ComputerAIChange(t, game);
        }
    }

    void ComputerAIChange(int t, Game game)
    {
        List<Pitcher> WaitHomePitcher = new List<Pitcher>();
        List<Pitcher> WaitAwayPitcher = new List<Pitcher>();
        if (!game.IsHomeAttack) // ������ �����ϰ�� Ȩ�� ������ü
        {
            for (int k = 0; k < GameDirector.pitcherCount; k++)
            {
                if (!GameDirector.isPostSeason)
                {
                    if (GameDirector.schedule[t].homeTeam == GameDirector.pitcher[k].team && !GameDirector.pitcher[k].isAleadyAppear && GameDirector.pitcher[k].posInTeam >= 6 && GameDirector.pitcher[k].posInTeam <= 14)
                    {
                        WaitHomePitcher.Add(GameDirector.pitcher[k]);
                    }
                    if (game.HomeScore > game.AwayScore && GameDirector.schedule[t].homeTeam == GameDirector.pitcher[k].team && game.HomeCurrentHoldPitcher == GameDirector.pitcher[k].posInTeam)
                    {
                        GameDirector.pitcher[k].hold++;
                    }
                } else
                {
                    if (GameDirector.postSchedule[t].homeTeam == GameDirector.pitcher[k].team && !GameDirector.pitcher[k].isAleadyAppear && GameDirector.pitcher[k].posInTeam >= 6 && GameDirector.pitcher[k].posInTeam <= 14)
                    {
                        WaitHomePitcher.Add(GameDirector.pitcher[k]);
                    }
                    if (game.HomeScore > game.AwayScore && GameDirector.postSchedule[t].homeTeam == GameDirector.pitcher[k].team && game.HomeCurrentHoldPitcher == GameDirector.pitcher[k].posInTeam)
                    {
                        //GameDirector.pitcher[k].hold++;
                    }
                }
                
            }
            int WhoChange = UnityEngine.Random.Range(0, WaitHomePitcher.Count);
            if (game.HomeScore - 3 <= game.AwayScore && game.HomeScore > game.AwayScore && game.Innings >= 17) // ������ ���� AI
            {
                for (int z = 0; z < WaitHomePitcher.Count; z++)
                {
                    if (WaitHomePitcher[z].posInTeam == 14)
                    {
                        WhoChange = z;
                        break;
                    }
                }
            }
            game.HomeCurrentPitcherInn = 0;
            game.HomeCurrentPitcherEarn = 0;
            game.HomeCurrentPitcherBallCount = 0;
            game.HomeCurrentPitcher = WaitHomePitcher[WhoChange].posInTeam;
            if (!GameDirector.isPostSeason)
            {
                WaitHomePitcher[WhoChange].game++;
            }
            WaitHomePitcher[WhoChange].isAleadyAppear = true;
        }
        else // Ȩ�� �����ϰ�� ������ ������ü
        {
            for (int k = 0; k < GameDirector.pitcherCount; k++)
            {
                if (!GameDirector.isPostSeason)
                {
                    if (GameDirector.schedule[t].awayTeam == GameDirector.pitcher[k].team && !GameDirector.pitcher[k].isAleadyAppear && GameDirector.pitcher[k].posInTeam >= 6 && GameDirector.pitcher[k].posInTeam <= 14)
                    {
                        WaitAwayPitcher.Add(GameDirector.pitcher[k]);
                    }
                    if (game.HomeScore < game.AwayScore && GameDirector.schedule[t].awayTeam == GameDirector.pitcher[k].team && game.AwayCurrentHoldPitcher == GameDirector.pitcher[k].posInTeam)
                    {
                        GameDirector.pitcher[k].hold++;
                    }
                } else
                {
                    if (GameDirector.postSchedule[t].awayTeam == GameDirector.pitcher[k].team && !GameDirector.pitcher[k].isAleadyAppear && GameDirector.pitcher[k].posInTeam >= 6 && GameDirector.pitcher[k].posInTeam <= 14)
                    {
                        WaitAwayPitcher.Add(GameDirector.pitcher[k]);
                    }
                    if (game.HomeScore < game.AwayScore && GameDirector.postSchedule[t].awayTeam == GameDirector.pitcher[k].team && game.AwayCurrentHoldPitcher == GameDirector.pitcher[k].posInTeam)
                    {
                        //GameDirector.pitcher[k].hold++;
                    }
                }
                
            }
            int WhoChange = UnityEngine.Random.Range(0, WaitAwayPitcher.Count);
            if (game.AwayScore - 3 <= game.HomeScore && game.HomeScore < game.AwayScore && game.Innings >= 18) // ������ ���� AI
            {
                for (int z = 0; z < WaitAwayPitcher.Count; z++)
                {
                    if (WaitAwayPitcher[z].posInTeam == 14)
                    {
                        WhoChange = z;
                        break;
                    }
                }
            }
            game.AwayCurrentPitcherInn = 0;
            game.AwayCurrentPitcherEarn = 0;
            game.AwayCurrentPitcherBallCount = 0;
            game.AwayCurrentPitcher = WaitAwayPitcher[WhoChange].posInTeam;
            if (!GameDirector.isPostSeason)
            {
                WaitAwayPitcher[WhoChange].game++;
            }
            WaitAwayPitcher[WhoChange].isAleadyAppear = true;
        }
    }

    void GamePlay_Out(int t, Game game, int num)
    {
        if (!GameDirector.isPostSeason)
        {
            if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.schedule[t].homeTeam)
            {
                game.HomeCurrentPitcherInn += num;
            }
            else if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.schedule[t].awayTeam)
            {
                game.AwayCurrentPitcherInn += num;
            }
            GameDirector.batter[game.TargetBatterIndex].todayAB++;
        } else
        {
            if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.postSchedule[t].homeTeam)
            {
                game.HomeCurrentPitcherInn += num;
            }
            else if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.postSchedule[t].awayTeam)
            {
                game.AwayCurrentPitcherInn += num;
            }
            GameDirector.batter[game.TargetBatterIndex].todayAB++;
        }
    }

    void GamePlay_Earn(int t, Game game, int num)
    {
        if (!GameDirector.isPostSeason)
        {
            if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.schedule[t].homeTeam)
            {
                game.HomeCurrentPitcherEarn += num;
            }
            else if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.schedule[t].awayTeam)
            {
                game.AwayCurrentPitcherEarn += num;
            }
            for (int i = 0; i < num; i++)
            {
                if (game.PlayerOnBase.Count > 0)
                {
                    Batter batter = game.PlayerOnBase.Dequeue();
                    batter.runScored++;
                }
            }
            for (int i = 0; i < num; i++)
            {
                if (game.ResponsibleRunner.Count > 0)
                {
                    Pitcher pitcher = game.ResponsibleRunner.Dequeue();
                    pitcher.earnedRuns++;
                }
                else
                {
                    Debug.Log("å�� ���� ���� �Դϴ�.");
                }
            }
        } else
        {
            if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.postSchedule[t].homeTeam)
            {
                game.HomeCurrentPitcherEarn += num;
            }
            else if (GameDirector.pitcher[game.TargetPitcherIndex].team == GameDirector.postSchedule[t].awayTeam)
            {
                game.AwayCurrentPitcherEarn += num;
            }
            for (int i = 0; i < num; i++)
            {
                if (game.PlayerOnBase.Count > 0)
                {
                    Batter batter = game.PlayerOnBase.Dequeue();
                    //batter.runScored++;
                }
            }
            for (int i = 0; i < num; i++)
            {
                if (game.ResponsibleRunner.Count > 0)
                {
                    Pitcher pitcher = game.ResponsibleRunner.Dequeue();
                    //pitcher.earnedRuns++;
                }
                else
                {
                    Debug.Log("å�� ���� ���� �Դϴ�.");
                }
            }
        }
        
    }

    void GamePlay_BaseOnBalls(int t, Game game)
    {
        if (game.IsHomeAttack == true)
        {
            game.RHEB[1, 3]++;
        }
        else
        {
            game.RHEB[0, 3]++;
        }
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].baseOnBall++;
            GameDirector.pitcher[game.TargetPitcherIndex].baseOnBall++;
        }
        if (game.isBase1 == false && game.isBase2 == false && game.isBase3 == false)
        {
            game.isBase1 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == true && game.isBase2 == false && game.isBase3 == false)
        {
            game.isBase2 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == false && game.isBase2 == true && game.isBase3 == false)
        {
            game.isBase1 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == false && game.isBase2 == false && game.isBase3 == true)
        {
            game.isBase1 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == true && game.isBase2 == true && game.isBase3 == false)
        {
            game.isBase3 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == true && game.isBase2 == false && game.isBase3 == true)
        {
            game.isBase2 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == false && game.isBase2 == true && game.isBase3 == true)
        {
            game.isBase1 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        }
        else if (game.isBase1 == true && game.isBase2 == true && game.isBase3 == true)
        {
            if (game.IsHomeAttack == true)
            {
                game.HomeScore++;
                game.InningScore[1, (game.Innings / 2) - 1]++;
                game.RHEB[1, 0]++;
            }
            else
            {
                game.AwayScore++;
                game.InningScore[0, game.Innings / 2]++;
                game.RHEB[0, 0]++;
            }
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� �о�� ����";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "����(1Ÿ��) ";
            if (!GameDirector.isPostSeason)
            {
                GameDirector.batter[game.TargetBatterIndex].RBI++;
            }
            GamePlay_Earn(t, game, 1);
            UpdatePitcherStats(game);
        }
        game.PlayerOnBase.Enqueue(GameDirector.batter[game.TargetBatterIndex]);
        game.ResponsibleRunner.Enqueue(GameDirector.pitcher[game.TargetPitcherIndex]);
        UpdateBatterNumber(game);
    }

    void GamePlay_Hit(int t, Game game)
    {
        GameDirector.batter[game.TargetBatterIndex].todayHit++;
        GameDirector.batter[game.TargetBatterIndex].todayAB++;
        // ��Ÿ
        if (game.IsHomeAttack == true)
        {
            game.RHEB[1, 1]++;
        }
        else
        {
            game.RHEB[0, 1]++;
        }
        if (game.isBase1 == false && game.isBase2 == false && game.isBase3 == false)
        {
            game.isBase1 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ��Ÿ";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ ";
        }
        else if (game.isBase1 == true && game.isBase2 == false && game.isBase3 == false)
        {
            game.isBase2 = true;
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ��Ÿ";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ ";
        }
        else if (game.isBase1 == false && game.isBase2 == true && game.isBase3 == false)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            {
                game.isBase1 = true;
                game.isBase2 = false;
                game.isBase3 = true;
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ ";
            }
            else
            {
                game.isBase1 = true;
                game.isBase2 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, game.Innings / 2]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, (game.Innings / 2) + 1]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ����Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == false && game.isBase2 == false && game.isBase3 == true)
        {
            game.isBase1 = true;
            game.isBase3 = false;
            if (game.IsHomeAttack == true)
            {
                game.HomeScore++;
                game.InningScore[1, (game.Innings / 2) - 1]++;
                game.RHEB[1, 0]++;
            }
            else
            {
                game.AwayScore++;
                game.InningScore[0, game.Innings / 2]++;
                game.RHEB[0, 0]++;
            }
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ����Ÿ";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(1Ÿ��) ";
            if (!GameDirector.isPostSeason)
            {
                GameDirector.batter[game.TargetBatterIndex].RBI++;
            }
            GamePlay_Earn(t, game, 1);
            UpdatePitcherStats(game);
        }
        else if (game.isBase1 == true && game.isBase2 == true && game.isBase3 == false)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            {
                game.isBase3 = true;
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ ";
            }
            else
            {
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ����Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == true && game.isBase2 == false && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            {
                game.isBase2 = true;
                game.isBase3 = false;
            }
            if (game.IsHomeAttack == true)
            {
                game.HomeScore++;
                game.InningScore[1, (game.Innings / 2) - 1]++;
                game.RHEB[1, 0]++;
            }
            else
            {
                game.AwayScore++;
                game.InningScore[0, game.Innings / 2]++;
                game.RHEB[0, 0]++;
            }
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ����Ÿ";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(1Ÿ��) ";
            if (!GameDirector.isPostSeason)
            {
                GameDirector.batter[game.TargetBatterIndex].RBI++;
            }
            GamePlay_Earn(t, game, 1);
            UpdatePitcherStats(game);
        }
        else if (game.isBase1 == false && game.isBase2 == true && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            { // 2Ÿ��
                game.isBase1 = true;
                game.isBase2 = false;
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ����Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
            else
            { // 1Ÿ��
                game.isBase1 = true;
                game.isBase2 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ����Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == true && game.isBase2 == true && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            {
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ����Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
            else
            {
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ����Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        game.PlayerOnBase.Enqueue(GameDirector.batter[game.TargetBatterIndex]);
        game.ResponsibleRunner.Enqueue(GameDirector.pitcher[game.TargetPitcherIndex]);
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].atBat++;
            GameDirector.batter[game.TargetBatterIndex].hit++;
            GameDirector.batter[game.TargetBatterIndex].totalBase++;
            GameDirector.pitcher[game.TargetPitcherIndex].hitAllowed++;
        }
        UpdateBatterStats(game);
        UpdatePitcherStats(game);
        UpdateBatterNumber(game);
    }

    void GamePlay_BigHit(int t, Game game)
    {
        GameDirector.batter[game.TargetBatterIndex].todayHit++;
        GameDirector.batter[game.TargetBatterIndex].todayAB++;
        // ��Ÿ
        if (game.IsHomeAttack == true)
        {
            game.RHEB[1, 1]++;
        }
        else
        {
            game.RHEB[0, 1]++;
        }
        if (game.isBase1 == false && game.isBase2 == false && game.isBase3 == false)
        {
            if (UnityEngine.Random.Range(1, 101) > 20)
            {
                game.isBase2 = true;
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
            }
            else
            {
                game.isBase3 = true;
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
            }
        }
        else if (game.isBase1 == true && game.isBase2 == false && game.isBase3 == false)
        {
            if (UnityEngine.Random.Range(1, 101) > 40)
            {
                game.isBase1 = false;
                game.isBase2 = true;
                game.isBase3 = true;
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
            }
            else if (UnityEngine.Random.Range(1, 101) < 80)
            {
                game.isBase1 = false;
                game.isBase2 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
            else
            {
                game.isBase1 = false;
                game.isBase3 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == false && game.isBase2 == true && game.isBase3 == false)
        {
            if (UnityEngine.Random.Range(1, 101) > 90)
            {
                game.isBase3 = true;
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
            }
            else if (UnityEngine.Random.Range(1, 101) > 20 && UnityEngine.Random.Range(1, 101) <= 90)
            {
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
            else
            {
                game.isBase2 = false;
                game.isBase3 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == false && game.isBase2 == false && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 20)
            {
                game.isBase2 = true;
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
            else
            {
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == true && game.isBase2 == true && game.isBase3 == false)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            {
                game.isBase1 = false;
                game.isBase2 = true;
                game.isBase3 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
            else if (UnityEngine.Random.Range(1, 101) > 20 && UnityEngine.Random.Range(1, 101) <= 50)
            {
                game.isBase1 = false;
                game.isBase2 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
            else
            {
                game.isBase1 = false;
                game.isBase2 = false;
                game.isBase3 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == true && game.isBase2 == false && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 50)
            {
                game.isBase1 = false;
                game.isBase2 = true;
                game.isBase3 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
            else if (UnityEngine.Random.Range(1, 101) > 20 && UnityEngine.Random.Range(1, 101) <= 50)
            {
                game.isBase1 = false;
                game.isBase2 = true;
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
            else
            {
                game.isBase1 = false;
                game.isBase2 = false;
                game.isBase3 = true;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == false && game.isBase2 == true && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 30)
            {
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
            else if (UnityEngine.Random.Range(1, 101) > 20 && UnityEngine.Random.Range(1, 101) <= 30)
            {
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore++;
                    game.InningScore[1, (game.Innings / 2) - 1]++;
                    game.RHEB[1, 0]++;
                }
                else
                {
                    game.AwayScore++;
                    game.InningScore[0, game.Innings / 2]++;
                    game.RHEB[0, 0]++;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(1Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI++;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 1);
                UpdatePitcherStats(game);
            }
            else
            {
                game.isBase2 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
        }
        else if (game.isBase1 == true && game.isBase2 == true && game.isBase3 == true)
        {
            if (UnityEngine.Random.Range(1, 101) > 40)
            {
                game.isBase1 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 2;
                    game.InningScore[1, (game.Innings / 2) - 1] += 2;
                    game.RHEB[1, 0] += 2;
                }
                else
                {
                    game.AwayScore += 2;
                    game.InningScore[0, game.Innings / 2] += 2;
                    game.RHEB[0, 0] += 2;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 2Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(2Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 2;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 2);
                UpdatePitcherStats(game);
            }
            else if (UnityEngine.Random.Range(1, 101) > 20 && UnityEngine.Random.Range(1, 101) <= 40)
            {
                game.isBase1 = false;
                game.isBase3 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 3;
                    game.InningScore[1, (game.Innings / 2) - 1] += 3;
                    game.RHEB[1, 0] += 3;
                }
                else
                {
                    game.AwayScore += 3;
                    game.InningScore[0, game.Innings / 2] += 3;
                    game.RHEB[0, 0] += 3;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� �Ͼ��� 3Ÿ�� ���� 2��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ÿ(3Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 3;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 2;
                }
                GamePlay_Earn(t, game, 3);
                UpdatePitcherStats(game);
            }
            else
            {
                game.isBase1 = false;
                game.isBase2 = false;
                if (game.IsHomeAttack == true)
                {
                    game.HomeScore += 3;
                    game.InningScore[1, (game.Innings / 2) - 1] += 3;
                    game.RHEB[1, 0] += 3;
                }
                else
                {
                    game.AwayScore += 3;
                    game.InningScore[0, game.Innings / 2] += 3;
                    game.RHEB[0, 0] += 3;
                }
                game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� �Ͼ��� 3Ÿ�� ���� 3��Ÿ";
                GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ÿ(3Ÿ��) ";
                if (!GameDirector.isPostSeason)
                {
                    GameDirector.batter[game.TargetBatterIndex].RBI += 3;
                    GameDirector.batter[game.TargetBatterIndex].totalBase += 3;
                }
                GamePlay_Earn(t, game, 3);
                UpdatePitcherStats(game);
            }
        }
        game.PlayerOnBase.Enqueue(GameDirector.batter[game.TargetBatterIndex]);
        game.ResponsibleRunner.Enqueue(GameDirector.pitcher[game.TargetPitcherIndex]);
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].atBat++;
            GameDirector.batter[game.TargetBatterIndex].hit++;
            GameDirector.pitcher[game.TargetPitcherIndex].hitAllowed++;
        }
        UpdateBatterStats(game);
        UpdatePitcherStats(game);
        UpdateBatterNumber(game);
    }

    void GamePlay_HomeRun(int t, Game game)
    {
        GameDirector.batter[game.TargetBatterIndex].todayHit++;
        GameDirector.batter[game.TargetBatterIndex].todayAB++;
        game.PlayerOnBase.Enqueue(GameDirector.batter[game.TargetBatterIndex]);
        game.ResponsibleRunner.Enqueue(GameDirector.pitcher[game.TargetPitcherIndex]);
        int TotalBase = (game.isBase1 ? 1 : 0) + (game.isBase2 ? 1 : 0) + (game.isBase3 ? 1 : 0) + 1;
        if (game.IsHomeAttack == true)
        {
            game.RHEB[1, 1] += TotalBase;
        }
        else
        {
            game.RHEB[0, 1] += TotalBase;
        }

        if (TotalBase == 1)
        {
            GamePlay_Earn(t, game, 1);
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� �ַ� Ȩ�� !";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "1��Ȩ�� ";
        }
        else if (TotalBase == 2)
        {
            GamePlay_Earn(t, game, 2);
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ���� Ȩ�� !!";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "2��Ȩ�� ";
        }
        else if (TotalBase == 3)
        {
            GamePlay_Earn(t, game, 3);
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ������ Ȩ�� !!!";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "3��Ȩ�� ";
        }
        else if (TotalBase == 4)
        {
            GamePlay_Earn(t, game, 4);
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ���� Ȩ�� !!!!";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "����Ȩ�� ";
        }
        game.isBase1 = false;
        game.isBase2 = false;
        game.isBase3 = false;
        if (game.IsHomeAttack == true)
        {
            game.HomeScore += TotalBase;
            game.InningScore[1, (game.Innings / 2) - 1] += TotalBase;
            game.RHEB[1, 0] += TotalBase;
        }
        else
        {
            game.AwayScore += TotalBase;
            game.InningScore[0, game.Innings / 2] += TotalBase;
            game.RHEB[0, 0] += TotalBase;
        }
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].RBI += TotalBase;
            GameDirector.pitcher[game.TargetPitcherIndex].homerunAllowed++;
            GameDirector.pitcher[game.TargetPitcherIndex].hitAllowed++;
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].atBat++;
            GameDirector.batter[game.TargetBatterIndex].hit++;
            GameDirector.batter[game.TargetBatterIndex].homerun++;
            GameDirector.batter[game.TargetBatterIndex].totalBase += 4;
        }
        UpdatePitcherStats(game);
        UpdateBatterStats(game);
        UpdateBatterNumber(game);
    }

    void GamePlay_FlyBall(int t, Game game)
    {
        GamePlay_Out(t, game, 1);
        game.OutCount++;
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].atBat++;
            GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2++;
            if (GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 == 3)
            {
                GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 = 0;
                GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched1++;
            }
        }
        if (game.OutCount != 3 && game.isBase3 == true && UnityEngine.Random.Range(1, 101) > 50)
        {
            game.isBase3 = false;
            if (game.IsHomeAttack == true)
            {
                game.HomeScore++;
                game.InningScore[1, (game.Innings / 2) - 1]++;
                game.RHEB[1, 0]++;
            }
            else
            {
                game.AwayScore++;
                game.InningScore[0, game.Innings / 2]++;
                game.RHEB[0, 0]++;
            }
            if (!GameDirector.isPostSeason)
            {
                GameDirector.batter[game.TargetBatterIndex].RBI++;
            }
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� 1Ÿ�� ��� �ö��� �ƿ�";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "���(1Ÿ��) ";
            GamePlay_Earn(t, game, 1);
        }
        else
        {
            game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� �ö��� �ƿ�";
            GameDirector.batter[game.TargetBatterIndex].todayResult += "��� ";
        }
        UpdatePitcherStats(game);
        UpdateBatterStats(game);
        UpdateBatterNumber(game);
        if (game.OutCount == 3)
        {
            game.PlayerOnBase.Clear();
            game.ResponsibleRunner.Clear();
            game.OutCount = 0;
            game.isBase1 = false;
            game.isBase2 = false;
            game.isBase3 = false;
            game.Innings++;
        }
    }

    void GamePlay_GroundBall(int t, Game game)
    {
        GamePlay_Out(t, game, 1);
        game.OutCount++;
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].atBat++;
            GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2++;
            if (GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 == 3)
            {
                GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 = 0;
                GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched1++;
            }
        }
        game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ���� �ƿ�";
        GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        UpdatePitcherStats(game);
        UpdateBatterStats(game);
        UpdateBatterNumber(game);
        if (game.OutCount == 3)
        {
            game.PlayerOnBase.Clear();
            game.ResponsibleRunner.Clear();
            game.OutCount = 0;
            game.isBase1 = false;
            game.isBase2 = false;
            game.isBase3 = false;
            game.Innings++;
        }
    }

    void GamePlay_StrikeOut(int t, Game game)
    {
        GamePlay_Out(t, game, 1);
        game.OutCount++;
        if (!GameDirector.isPostSeason)
        {
            GameDirector.batter[game.TargetBatterIndex].plateAppearance++;
            GameDirector.batter[game.TargetBatterIndex].atBat++;
            GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2++;
            GameDirector.pitcher[game.TargetPitcherIndex].strikeOut++;
            if (GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 == 3)
            {
                GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 = 0;
                GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched1++;
            }
        }
        game.Message = GameDirector.batter[game.TargetBatterIndex].name + "�� ���� �ƿ�";
        GameDirector.batter[game.TargetBatterIndex].todayResult += "���� ";
        UpdatePitcherStats(game);
        UpdateBatterStats(game);
        UpdateBatterNumber(game);
        if (game.OutCount == 3)
        {
            game.PlayerOnBase.Clear();
            game.ResponsibleRunner.Clear();
            game.OutCount = 0;
            game.isBase1 = false;
            game.isBase2 = false;
            game.isBase3 = false;
            game.Innings++;
        }
    }

    void UpdatePitcherStats(Game game)
    {
        float k = GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched2 * (float)1 / 3;
        GameDirector.pitcher[game.TargetPitcherIndex].earnedRunAverage = (GameDirector.pitcher[game.TargetPitcherIndex].earnedRuns * 9) / (float)(GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched1 + k);
        GameDirector.pitcher[game.TargetPitcherIndex].WHIP = (float)(GameDirector.pitcher[game.TargetPitcherIndex].hitAllowed + GameDirector.pitcher[game.TargetPitcherIndex].baseOnBall) / (float)(GameDirector.pitcher[game.TargetPitcherIndex].inningsPitched1 + k);
    }

    void UpdateBatterStats(Game game)
    {
        GameDirector.batter[game.TargetBatterIndex].battingAverage = (float)GameDirector.batter[game.TargetBatterIndex].hit / (float)GameDirector.batter[game.TargetBatterIndex].atBat;
        GameDirector.batter[game.TargetBatterIndex].SLG = (float)GameDirector.batter[game.TargetBatterIndex].totalBase / (float)GameDirector.batter[game.TargetBatterIndex].atBat;
        GameDirector.batter[game.TargetBatterIndex].OBP = (float)(GameDirector.batter[game.TargetBatterIndex].hit + GameDirector.batter[game.TargetBatterIndex].baseOnBall) / (float)GameDirector.batter[game.TargetBatterIndex].plateAppearance;
        GameDirector.batter[game.TargetBatterIndex].OPS = GameDirector.batter[game.TargetBatterIndex].SLG + GameDirector.batter[game.TargetBatterIndex].OBP;
    }

    void UpdateBatterNumber(Game game)
    {
        if (game.IsHomeAttack == true)
        {
            game.HomeCurrentBatter++;
            if (game.HomeCurrentBatter >= 110)
            {
                game.HomeCurrentBatter = 101;
            }
        }
        else
        {
            game.AwayCurrentBatter++;
            if (game.AwayCurrentBatter >= 110)
            {
                game.AwayCurrentBatter = 101;
            }
        }
    }

    GameObject GetLineHomeBatterObject(int index)
    {
        switch (index)
        {
            case 0: return HLine1;
            case 1: return HLine2;
            case 2: return HLine3;
            case 3: return HLine4;
            case 4: return HLine5;
            case 5: return HLine6;
            case 6: return HLine7;
            case 7: return HLine8;
            case 8: return HLine9;
            default: return null;
        }
    }

    GameObject GetLineAwayBatterObject(int index)
    {
        switch (index)
        {
            case 0: return ALine1;
            case 1: return ALine2;
            case 2: return ALine3;
            case 3: return ALine4;
            case 4: return ALine5;
            case 5: return ALine6;
            case 6: return ALine7;
            case 7: return ALine8;
            case 8: return ALine9;
            default: return null;
        }
    }

    void ChangePitcher(int t, Game Game)
    {
        TMP_Text[] textArray;
        if (!GameDirector.isPostSeason)
        {
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if (GameDirector.schedule[t].homeTeam == GameDirector.myTeam && GameDirector.pitcher[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.pitcher[i].posInTeam;
                    if (Game.HomeCurrentPitcher == GameDirector.pitcher[i].posInTeam)
                    {
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                    }
                    else
                    {
                        if (posInTeam >= 6 && posInTeam <= 14 && !GameDirector.pitcher[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForPitcher(posInTeam - 5);
                            textArray = GetTextArrayForPitcher(posInTeam - 5);
                            UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnPitcherClicked(obj, currentIndex));
                        }
                    }
                }
                else if (GameDirector.schedule[t].awayTeam == GameDirector.myTeam && GameDirector.pitcher[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.pitcher[i].posInTeam;
                    if (Game.AwayCurrentPitcher == GameDirector.pitcher[i].posInTeam)
                    {
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                    }
                    else
                    {
                        if (posInTeam >= 6 && posInTeam <= 14 && !GameDirector.pitcher[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForPitcher(posInTeam - 5);
                            textArray = GetTextArrayForPitcher(posInTeam - 5);
                            UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnPitcherClicked(obj, currentIndex));
                        }
                    }
                }
            }
        } else
        {
            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                if (GameDirector.postSchedule[t].homeTeam == GameDirector.myTeam && GameDirector.pitcher[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.pitcher[i].posInTeam;
                    if (Game.HomeCurrentPitcher == GameDirector.pitcher[i].posInTeam)
                    {
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                    }
                    else
                    {
                        if (posInTeam >= 6 && posInTeam <= 14 && !GameDirector.pitcher[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForPitcher(posInTeam - 5);
                            textArray = GetTextArrayForPitcher(posInTeam - 5);
                            UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnPitcherClicked(obj, currentIndex));
                        }
                    }
                }
                else if (GameDirector.postSchedule[t].awayTeam == GameDirector.myTeam && GameDirector.pitcher[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.pitcher[i].posInTeam;
                    if (Game.AwayCurrentPitcher == GameDirector.pitcher[i].posInTeam)
                    {
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                    }
                    else
                    {
                        if (posInTeam >= 6 && posInTeam <= 14 && !GameDirector.pitcher[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForPitcher(posInTeam - 5);
                            textArray = GetTextArrayForPitcher(posInTeam - 5);
                            UpdateTextArrayPit(textArray, GameDirector.pitcher[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnPitcherClicked(obj, currentIndex));
                        }
                    }
                }
            }
        }
        
    }

    void OnPitcherClicked(GameObject panel, int selectedPitIndex)
    {
        if (!GameDirector.isPostSeason)
        {
            GameDirector.pitcher[selectedPitIndex].game++;
            if (GameDirector.myTeam == GameDirector.schedule[GameDirector.currentGame].homeTeam)
            {
                for (int i = 0; i < GameDirector.pitcherCount; i++)
                {
                    if (GameDirector.pitcher[i].team == GameDirector.myTeam && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher)
                    {
                        TMP_Text[] textArray;
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[selectedPitIndex]);
                        Destroy(panel.gameObject);
                        GameDirector.pitcher[i].isAleadyAppear = true;
                        if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.HomeCurrentHoldPitcher)
                        {
                            GameDirector.pitcher[i].hold++;
                        }
                        game.HomeCurrentPitcherInn = 0;
                        game.HomeCurrentPitcherEarn = 0;
                        game.HomeCurrentPitcherBallCount = 0;
                        game.HomeCurrentPitcher = GameDirector.pitcher[selectedPitIndex].posInTeam;
                        isPitChange = true;
                        break;
                    }
                }

            }
            else if (GameDirector.myTeam == GameDirector.schedule[GameDirector.currentGame].awayTeam)
            {
                for (int i = 0; i < GameDirector.pitcherCount; i++)
                {
                    if (GameDirector.pitcher[i].team == GameDirector.myTeam && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher)
                    {
                        TMP_Text[] textArray;
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[selectedPitIndex]);
                        Destroy(panel.gameObject);
                        GameDirector.pitcher[i].isAleadyAppear = true;
                        if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.AwayCurrentHoldPitcher)
                        {
                            GameDirector.pitcher[i].hold++;
                        }
                        game.AwayCurrentPitcherInn = 0;
                        game.AwayCurrentPitcherEarn = 0;
                        game.AwayCurrentPitcherBallCount = 0;
                        game.AwayCurrentPitcher = GameDirector.pitcher[selectedPitIndex].posInTeam;
                        isPitChange = true;
                        break;
                    }
                }

            }
        } else
        {
            if (GameDirector.myTeam == GameDirector.postSchedule[GameDirector.currentGame].homeTeam)
            {
                for (int i = 0; i < GameDirector.pitcherCount; i++)
                {
                    if (GameDirector.pitcher[i].team == GameDirector.myTeam && GameDirector.pitcher[i].posInTeam == game.HomeCurrentPitcher)
                    {
                        TMP_Text[] textArray;
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[selectedPitIndex]);
                        Destroy(panel.gameObject);
                        GameDirector.pitcher[i].isAleadyAppear = true;
                        if (game.HomeScore > game.AwayScore && GameDirector.pitcher[i].posInTeam == game.HomeCurrentHoldPitcher)
                        {
                            //GameDirector.pitcher[i].hold++;
                        }
                        game.HomeCurrentPitcherInn = 0;
                        game.HomeCurrentPitcherEarn = 0;
                        game.HomeCurrentPitcherBallCount = 0;
                        game.HomeCurrentPitcher = GameDirector.pitcher[selectedPitIndex].posInTeam;
                        isPitChange = true;
                        break;
                    }
                }

            }
            else if (GameDirector.myTeam == GameDirector.postSchedule[GameDirector.currentGame].awayTeam)
            {
                for (int i = 0; i < GameDirector.pitcherCount; i++)
                {
                    if (GameDirector.pitcher[i].team == GameDirector.myTeam && GameDirector.pitcher[i].posInTeam == game.AwayCurrentPitcher)
                    {
                        TMP_Text[] textArray;
                        textArray = P0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayPit(textArray, GameDirector.pitcher[selectedPitIndex]);
                        Destroy(panel.gameObject);
                        GameDirector.pitcher[i].isAleadyAppear = true;
                        if (game.HomeScore < game.AwayScore && GameDirector.pitcher[i].posInTeam == game.AwayCurrentHoldPitcher)
                        {
                            //GameDirector.pitcher[i].hold++;
                        }
                        game.AwayCurrentPitcherInn = 0;
                        game.AwayCurrentPitcherEarn = 0;
                        game.AwayCurrentPitcherBallCount = 0;
                        game.AwayCurrentPitcher = GameDirector.pitcher[selectedPitIndex].posInTeam;
                        isPitChange = true;
                        break;
                    }
                }

            }
        }
        
        GameModuleUI(GameDirector.currentGame, game);
    }

    GameObject GetGameObjectForPitcher(int posInTeam)
    {
        switch (posInTeam)
        {
            case 1: return P1;
            case 2: return P2;
            case 3: return P3;
            case 4: return P4;
            case 5: return P5;
            case 6: return P6;
            case 7: return P7;
            case 8: return P8;
            case 9: return P9;
            default: return null;
        }
    }

    TMP_Text[] GetTextArrayForPitcher(int posInTeam)
    {
        switch (posInTeam)
        {
            case 1: return P1.GetComponentsInChildren<TMP_Text>();
            case 2: return P2.GetComponentsInChildren<TMP_Text>();
            case 3: return P3.GetComponentsInChildren<TMP_Text>();
            case 4: return P4.GetComponentsInChildren<TMP_Text>();
            case 5: return P5.GetComponentsInChildren<TMP_Text>();
            case 6: return P6.GetComponentsInChildren<TMP_Text>();
            case 7: return P7.GetComponentsInChildren<TMP_Text>();
            case 8: return P8.GetComponentsInChildren<TMP_Text>();
            case 9: return P9.GetComponentsInChildren<TMP_Text>();
            default: return null;
        }
    }

    void UpdateTextArrayPit(TMP_Text[] textArray, Pitcher pitcher)
    {
        if (textArray != null)
        {
            textArray[0].text = pitcher.name;
            textArray[1].text = pitcher.win.ToString();
            textArray[2].text = pitcher.lose.ToString();
            textArray[3].text = pitcher.hold.ToString();
            textArray[4].text = pitcher.save.ToString();
            textArray[5].text = pitcher.earnedRunAverage.ToString("F2");
        }
    }

    void ChangeBatter(int t, Game Game)
    {
        TMP_Text[] textArray;
        if (!GameDirector.isPostSeason)
        {
            for (int i = 0; i < GameDirector.batterCount; i++)
            {
                if (GameDirector.schedule[t].homeTeam == GameDirector.myTeam && GameDirector.batter[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.batter[i].posInTeam;
                    if (Game.HomeCurrentBatter == GameDirector.batter[i].posInTeam)
                    {
                        textArray = B0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                    }
                    else
                    {
                        if (posInTeam >= 110 && posInTeam <= 115 && !GameDirector.batter[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForBatter(posInTeam);
                            textArray = GetTextArrayForBatter(posInTeam);
                            UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnBatterClicked(obj, currentIndex));
                        }
                    }
                }
                else if (GameDirector.schedule[t].awayTeam == GameDirector.myTeam && GameDirector.batter[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.batter[i].posInTeam;
                    if (Game.AwayCurrentBatter == GameDirector.batter[i].posInTeam)
                    {
                        textArray = B0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                    }
                    else
                    {
                        if (posInTeam >= 110 && posInTeam <= 115 && !GameDirector.batter[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForBatter(posInTeam);
                            textArray = GetTextArrayForBatter(posInTeam);
                            UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnBatterClicked(obj, currentIndex));
                        }
                    }
                }
            }
        } else
        {
            for (int i = 0; i < GameDirector.batterCount; i++)
            {
                if (GameDirector.postSchedule[t].homeTeam == GameDirector.myTeam && GameDirector.batter[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.batter[i].posInTeam;
                    if (Game.HomeCurrentBatter == GameDirector.batter[i].posInTeam)
                    {
                        textArray = B0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                    }
                    else
                    {
                        if (posInTeam >= 110 && posInTeam <= 115 && !GameDirector.batter[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForBatter(posInTeam);
                            textArray = GetTextArrayForBatter(posInTeam);
                            UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnBatterClicked(obj, currentIndex));
                        }
                    }
                }
                else if (GameDirector.postSchedule[t].awayTeam == GameDirector.myTeam && GameDirector.batter[i].team == GameDirector.myTeam)
                {
                    int posInTeam = GameDirector.batter[i].posInTeam;
                    if (Game.AwayCurrentBatter == GameDirector.batter[i].posInTeam)
                    {
                        textArray = B0.GetComponentsInChildren<TMP_Text>();
                        UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                    }
                    else
                    {
                        if (posInTeam >= 110 && posInTeam <= 115 && !GameDirector.batter[i].isAleadyAppear)
                        {
                            GameObject obj = GetGameObjectForBatter(posInTeam);
                            textArray = GetTextArrayForBatter(posInTeam);
                            UpdateTextArrayBat(textArray, GameDirector.batter[i]);
                            int currentIndex = i;
                            obj.GetComponent<Button>().onClick.RemoveAllListeners();
                            obj.GetComponent<Button>().onClick.AddListener(() => OnBatterClicked(obj, currentIndex));
                        }
                    }
                }
            }
        }
        
    }

    void OnBatterClicked(GameObject panel, int selectedBatIndex)
    {
        int currentBatIndex = 0;
        if (!GameDirector.isPostSeason)
        {
            if (GameDirector.myTeam == GameDirector.schedule[GameDirector.currentGame].homeTeam)
            {
                for (int i = 0; i < GameDirector.batterCount; i++)
                {
                    if (GameDirector.batter[i].team == GameDirector.myTeam && GameDirector.batter[i].posInTeam == game.HomeCurrentBatter)
                    {
                        if (GameDirector.batter[i].pos != GameDirector.batter[selectedBatIndex].pos)
                        {
                            break;
                        }
                        else
                        {
                            Destroy(panel.gameObject);
                            GameDirector.batter[i].isAleadyAppear = true;
                            currentBatIndex = i;
                            int temp = GameDirector.batter[selectedBatIndex].posInTeam;
                            GameDirector.batter[selectedBatIndex].posInTeam = GameDirector.batter[currentBatIndex].posInTeam; // ������ �ٸ��� �������� ���� �ٲ���� ��
                            GameDirector.batter[currentBatIndex].posInTeam = temp;
                            GameDirector.batter[selectedBatIndex].game++;
                            break;
                        }
                    }
                }
            }
            else if (GameDirector.myTeam == GameDirector.schedule[GameDirector.currentGame].awayTeam)
            {
                for (int i = 0; i < GameDirector.batterCount; i++)
                {
                    if (GameDirector.batter[i].team == GameDirector.myTeam && GameDirector.batter[i].posInTeam == game.AwayCurrentBatter)
                    {
                        if (GameDirector.batter[i].pos != GameDirector.batter[selectedBatIndex].pos)
                        {
                            break;
                        }
                        else
                        {
                            TMP_Text[] textArray;
                            textArray = B0.GetComponentsInChildren<TMP_Text>();
                            UpdateTextArrayBat(textArray, GameDirector.batter[selectedBatIndex]);

                            Destroy(panel.gameObject);
                            GameDirector.batter[i].isAleadyAppear = true;
                            currentBatIndex = i;
                            int temp = GameDirector.batter[selectedBatIndex].posInTeam;
                            GameDirector.batter[selectedBatIndex].posInTeam = GameDirector.batter[currentBatIndex].posInTeam; // ������ �ٸ��� �������� ���� �ٲ���� ��
                            GameDirector.batter[currentBatIndex].posInTeam = temp;
                            GameDirector.batter[selectedBatIndex].game++;
                            break;
                        }
                    }
                }
            }
        } else
        {
            if (GameDirector.myTeam == GameDirector.postSchedule[GameDirector.currentGame].homeTeam)
            {
                for (int i = 0; i < GameDirector.batterCount; i++)
                {
                    if (GameDirector.batter[i].team == GameDirector.myTeam && GameDirector.batter[i].posInTeam == game.HomeCurrentBatter)
                    {
                        if (GameDirector.batter[i].pos != GameDirector.batter[selectedBatIndex].pos)
                        {
                            break;
                        }
                        else
                        {
                            Destroy(panel.gameObject);
                            GameDirector.batter[i].isAleadyAppear = true;
                            currentBatIndex = i;
                            int temp = GameDirector.batter[selectedBatIndex].posInTeam;
                            GameDirector.batter[selectedBatIndex].posInTeam = GameDirector.batter[currentBatIndex].posInTeam; // ������ �ٸ��� �������� ���� �ٲ���� ��
                            GameDirector.batter[currentBatIndex].posInTeam = temp;
                            //GameDirector.batter[selectedBatIndex].game++;
                            break;
                        }
                    }
                }
            }
            else if (GameDirector.myTeam == GameDirector.postSchedule[GameDirector.currentGame].awayTeam)
            {
                for (int i = 0; i < GameDirector.batterCount; i++)
                {
                    if (GameDirector.batter[i].team == GameDirector.myTeam && GameDirector.batter[i].posInTeam == game.AwayCurrentBatter)
                    {
                        if (GameDirector.batter[i].pos != GameDirector.batter[selectedBatIndex].pos)
                        {
                            break;
                        }
                        else
                        {
                            TMP_Text[] textArray;
                            textArray = B0.GetComponentsInChildren<TMP_Text>();
                            UpdateTextArrayBat(textArray, GameDirector.batter[selectedBatIndex]);

                            Destroy(panel.gameObject);
                            GameDirector.batter[i].isAleadyAppear = true;
                            currentBatIndex = i;
                            int temp = GameDirector.batter[selectedBatIndex].posInTeam;
                            GameDirector.batter[selectedBatIndex].posInTeam = GameDirector.batter[currentBatIndex].posInTeam; // ������ �ٸ��� �������� ���� �ٲ���� ��
                            GameDirector.batter[currentBatIndex].posInTeam = temp;
                            //GameDirector.batter[selectedBatIndex].game++;
                            break;
                        }
                    }
                }
            }
        }
        
        GameModuleUI(GameDirector.currentGame, game);
    }

    GameObject GetGameObjectForBatter(int posInTeam)
    {
        switch (posInTeam)
        {
            case 110: return B1;
            case 111: return B2;
            case 112: return B3;
            case 113: return B4;
            case 114: return B5;
            case 115: return B6;
            default: return null;
        }
    }

    TMP_Text[] GetTextArrayForBatter(int posInTeam)
    {
        switch (posInTeam)
        {
            case 110: return B1.GetComponentsInChildren<TMP_Text>();
            case 111: return B2.GetComponentsInChildren<TMP_Text>();
            case 112: return B3.GetComponentsInChildren<TMP_Text>();
            case 113: return B4.GetComponentsInChildren<TMP_Text>();
            case 114: return B5.GetComponentsInChildren<TMP_Text>();
            case 115: return B6.GetComponentsInChildren<TMP_Text>();
            default: return null;
        }
    }

    void UpdateTextArrayBat(TMP_Text[] textArray, Batter batter)
    {
        if (textArray != null)
        {
            textArray[0].text = batter.name;
            textArray[1].text = DataToString.PosToString(batter.pos);
            textArray[2].text = batter.hit.ToString();
            textArray[3].text = batter.homerun.ToString();
            textArray[4].text = batter.RBI.ToString();
            textArray[5].text = batter.battingAverage.ToString("F3");
        }
    }

    void OnToggleChanged(bool isOn, int mode)
    {
        if (isOn)
        {
            GameMode = mode;
            isPrefer = false;
            ModePanel.SetActive(false);
            // ��忡 ���� ���� ��ư
            switch (GameMode)
            {
                case 0:
                    PlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
                    PlayButton.interactable = true;
                    break;
                case 1:
                    PlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "1���";
                    PlayButton.interactable = false;
                    break;
                case 2:
                    PlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "2���";
                    PlayButton.interactable = false;
                    break;
                case 3:
                    PlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "3���";
                    PlayButton.interactable = false;
                    break;
                default:
                    PlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
                    PlayButton.interactable = true;
                    break;
            }
        }
    }

    public void OnPreferenceButton()
    {
        if (!isPrefer)
        {
            isPrefer = true;
        }
        else
        {
            isPrefer = false;
        }
    }

    public void OnChangeButton()
    {
        if (!isChange)
        {
            isChange = true;
        }
        else
        {
            isChange = false;
            isChangeExecuted = false;
        }
    }

    public void OnSelfPlay()
    {
        isProcess = true;
    }

    public void OnLeaveGame()
    {
        isLeave = true;
    }

    private bool Processing()
    {
        return isProcess;
    }

    private bool Leaving()
    {
        return isLeave;
    }
}
