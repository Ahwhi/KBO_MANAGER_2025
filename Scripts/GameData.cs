using System;
using System.Collections.Generic;
using static Unity.Burst.Intrinsics.X86;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using MailData;
using UnityEditor;

namespace GameData
{

    public enum DayOfWeek
    {
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATURDAY,
        SUNDAY
    }

    public enum TeamName
    {
        SAMSUNG,
        LOTTE,
        KIA,
        KIWOOM,
        DOOSAN,
        HANWHA,
        LG,
        SSG,
        NC,
        KT
    }

    public enum BatterPosition
    {
        DH,
        C,
        _1B,
        _2B,
        _3B,
        SS,
        LF,
        CF,
        RF
    }

    public enum PitcherPosition
    {
        SP,
        RP,
        SU,
        CP
    }

    public enum Hand
    {
        L,
        R,
        S
    }

    public enum Nation
    {
        USA,
        Cuba,
        Venezuela,
        Japan
    }

    public class Game
    {
        public Queue<Batter> PlayerOnBase;
        public Queue<Pitcher> ResponsibleRunner;
        public bool IsHomeAttack { get; set; }
        public bool IsExtend { get; set; }
        public bool IsGameOver { get; set; }
        public bool isBase1 { get; set; }
        public bool isBase2 { get; set; }
        public bool isBase3 { get; set; }
        public bool isHomeLead { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int[,] InningScore { get; set; } = new int[2, 12];
        public int[,] RHEB { get; set; } = new int[2, 4];
        public int Innings { get; set; }
        public int OutCount { get; set; }
        public int TargetPitcherIndex { get; set; }
        public int TargetBatterIndex { get; set; }
        public int AwayCurrentBatter { get; set; }
        public int HomeCurrentBatter { get; set; }
        public int AwayCurrentPitcher { get; set; }
        public int HomeCurrentPitcher { get; set; }
        public int AwayCurrentPitcherInn { get; set; }
        public int HomeCurrentPitcherInn { get; set; }
        public int AwayCurrentPitcherBallCount { get; set; }
        public int HomeCurrentPitcherBallCount { get; set; }
        public int AwayCurrentPitcherEarn { get; set; }
        public int HomeCurrentPitcherEarn { get; set; }
        public int AwayCurrentHoldPitcher { get; set; }
        public int HomeCurrentHoldPitcher { get; set; }
        public int WhoWinCondition { get; set; }
        public int WhoLoseCondition { get; set; }
        public int PlayerNum { get; set; }
        public int ComputerNum { get; set; }
        public int AbillityResult1 { get; set; }
        public int AbillityResult2 { get; set; }
        public string Message { get; set; }
    }

    public class Date
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public DayOfWeek dayOfWeek { get; set; }

        public Date(int _year, int _month, int _day, DayOfWeek _dayOfWeek)
        {
            year = _year;
            month = _month;
            day = _day;
            dayOfWeek = _dayOfWeek;
        }
    }

    public class Schedule
    {
        public Date dates { get; set; }
        public TeamName homeTeam { get; set; }
        public TeamName awayTeam { get; set; }
        public int homeScore { get; set; }
        public int awayScore { get; set; }
        public bool isEnd { get; set; }
    }

    public class PostSchedule : Schedule
    {
        public TeamName DownTeam { get; set; }
        public TeamName UpTeam { get; set; }
        public bool isPass { get; set; }
    }

    public class Team
    {
        public int teamCode { get; set; }
        public int win { get; set; }
        public int draw { get; set; }
        public int lose { get; set; }
        public int currentSP { get; set; }

        public float WinRate()
        {
            int totalGamesExcludeDraw = win + lose;
            if (totalGamesExcludeDraw == 0)
            {

            }
            return totalGamesExcludeDraw > 0 ? (float)win / totalGamesExcludeDraw : 0f;
        }
    }

    public class Pitcher
    {
        public int playerId { get; set; }
        public bool isForeign { get; set; }
        public string name { get; set; }
        public TeamName team { get; set; }
        public PitcherPosition pos { get; set; }
        public Hand hand { get; set; }
        public int born { get; set; }
        public int HP { get; set; }
        public int game { get; set; }
        public int inningsPitched1 { get; set; }
        public int inningsPitched2 { get; set; }
        public int win { get; set; }
        public int lose { get; set; }
        public int hold { get; set; }
        public int save { get; set; }
        public int strikeOut { get; set; }
        public int baseOnBall { get; set; }
        public int hitAllowed { get; set; }
        public int homerunAllowed { get; set; }
        public int earnedRuns { get; set; }
        public float earnedRunAverage { get; set; }
        public float WHIP { get; set; }
        public int posInTeam { get; set; }
        public bool isAleadyAppear { get; set; }
        public int SPEED { get; set; }
        public int COMMAND { get; set; }
        public int BREAKING { get; set; }
        public bool isTrade { get; set; }
    }

    public class Batter
    {
        public int playerId { get; set; }
        public bool isForeign { get; set; }
        public string name { get; set; }
        public TeamName team { get; set; }
        public BatterPosition pos { get; set; }
        public Hand hand { get; set; }
        public int born { get; set; }
        public int game { get; set; }
        public int plateAppearance { get; set; }
        public int atBat { get; set; }
        public int hit { get; set; }
        public int totalBase { get; set; }
        public int homerun { get; set; }
        public int RBI { get; set; }
        public int runScored { get; set; }
        public int stolenBased { get; set; }
        public int baseOnBall { get; set; }
        public float battingAverage { get; set; }
        public float SLG { get; set; }
        public float OBP { get; set; }
        public float OPS { get; set; }
        public int posInTeam { get; set; }
        public bool isAleadyAppear { get; set; }
        public string todayResult { get; set; }
        public int todayHit { get; set; }
        public int todayAB { get; set; }
        public int POWER { get; set; }
        public int CONTACT { get; set; }
        public int EYE { get; set; }
        public bool isTrade { get; set; }
    }

    public class ForeignPitcher : Pitcher
    {
        public Nation nation { get; set; }
    }

    public class ForeignBatter : Batter
    {
        public Nation nation { get; set; }
    }

    public class Mail
    {
        public int code { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Sender { get; set; }
        public Date Dates { get; set; }
        public bool isRead { get; set; }
    }

    public static class TeamColor
    {
        public static Color CKiwoomHeroes = new Color(0x57 / 255f, 0x05 / 255f, 0x14 / 255f);
        public static Color CDosanBears = new Color(0x13 / 255f, 0x12 / 255f, 0x30 / 255f);
        public static Color CLotteGiants = new Color(0x04 / 255f, 0x1E / 255f, 0x42 / 255f);
        public static Color CSamsungLions = new Color(0x07 / 255f, 0x4C / 255f, 0xA1 / 255f);
        public static Color CHanwhaEagles = new Color(0xF7 / 255f, 0x36 / 255f, 0x00 / 255f);
        public static Color CKiaTigers = new Color(0xEA / 255f, 0x00 / 255f, 0x29 / 255f);
        public static Color CLGTwins = new Color(0xC3 / 255f, 0x04 / 255f, 0x52 / 255f);
        public static Color CSSGLanders = new Color(0xCE / 255f, 0x0E / 255f, 0x2D / 255f);
        public static Color CNCDinos = new Color(0x31 / 255f, 0x52 / 255f, 0x88 / 255f);
        public static Color CKtWiz = new Color(0x00 / 255f, 0x00 / 255f, 0x00 / 255f);

        public static void SetMyTeamColor()
        {
            switch (GameDirector.myTeam)
            {
                case TeamName.SAMSUNG:
                    GameDirector.myColor = CSamsungLions;
                    break;
                case TeamName.LOTTE:
                    GameDirector.myColor = CLotteGiants;
                    break;
                case TeamName.KIWOOM:
                    GameDirector.myColor = CKiwoomHeroes;
                    break;
                case TeamName.KIA:
                    GameDirector.myColor = CKiaTigers;
                    break;
                case TeamName.DOOSAN:
                    GameDirector.myColor = CDosanBears;
                    break;
                case TeamName.LG:
                    GameDirector.myColor = CLGTwins;
                    break;
                case TeamName.HANWHA:
                    GameDirector.myColor = CHanwhaEagles;
                    break;
                case TeamName.NC:
                    GameDirector.myColor = CNCDinos;
                    break;
                case TeamName.KT:
                    GameDirector.myColor = CKtWiz;
                    break;
                case TeamName.SSG:
                    GameDirector.myColor = CSSGLanders;
                    break;
                default:
                    GameDirector.myColor = Color.white;
                    break;
            }
        }
    
        public static Color SetTeamColor(TeamName team)
        {
            Color myColor;
            switch (team)
            {
                case TeamName.SAMSUNG:
                    myColor = CSamsungLions;
                    break;
                case TeamName.LOTTE:
                    myColor = CLotteGiants;
                    break;
                case TeamName.KIWOOM:
                    myColor = CKiwoomHeroes;
                    break;
                case TeamName.KIA:
                    myColor = CKiaTigers;
                    break;
                case TeamName.DOOSAN:
                    myColor = CDosanBears;
                    break;
                case TeamName.LG:
                    myColor = CLGTwins;
                    break;
                case TeamName.HANWHA:
                    myColor = CHanwhaEagles;
                    break;
                case TeamName.NC:
                    myColor = CNCDinos;
                    break;
                case TeamName.KT:
                    myColor = CKtWiz;
                    break;
                case TeamName.SSG:
                    myColor = CSSGLanders;
                    break;
                default:
                    myColor = Color.white;
                    break;
            }
            return myColor;
        }
    }

    public static class TeamEmblem
    {
        public static Sprite GetEmblem(TeamName team)
        {
            Sprite emblem;
            if ((int)team == 999)
            {
                emblem = Resources.Load<Sprite>("Emblem/KBO");
            } else
            {
                emblem = Resources.Load<Sprite>("Emblem/" + team);
            }
            return emblem;
        }
    }

    public static class PlayerNation
    {
        public static Sprite GetNation(Nation nation)
        {
            Sprite flag;
            flag = Resources.Load<Sprite>("Nation/" + nation);
            return flag;
        }
    }

    public static class CreateData
    {
        public static void CreatePlayer()
        {
            //구속 제구 변화 // 파워 컨택 선구
            #region SAMSUNG LIONS
            SetPitcherForeign(GameDirector.pitcherCount, "레예스", TeamName.SAMSUNG, Hand.R, 1996, PitcherPosition.SP, 1, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "원태인", TeamName.SAMSUNG, Hand.R, 2000, PitcherPosition.SP, 2, 3, 4, 4);
            SetPitcherForeign(GameDirector.pitcherCount, "후라도", TeamName.SAMSUNG, Hand.R, 1996, PitcherPosition.SP, 3, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "최원태", TeamName.SAMSUNG, Hand.R, 1997, PitcherPosition.SP, 4, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "백정현", TeamName.SAMSUNG, Hand.L, 1987, PitcherPosition.SP, 5, 1, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "이승현L", TeamName.SAMSUNG, Hand.L, 2002, PitcherPosition.RP, 6, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "이승현R", TeamName.SAMSUNG, Hand.R, 1991, PitcherPosition.RP, 7, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김태훈", TeamName.SAMSUNG, Hand.R, 1992, PitcherPosition.RP, 8, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "최지광", TeamName.SAMSUNG, Hand.R, 1998, PitcherPosition.RP, 9, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "배찬승", TeamName.SAMSUNG, Hand.L, 2006, PitcherPosition.RP, 10, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "김윤수", TeamName.SAMSUNG, Hand.R, 1999, PitcherPosition.RP, 11, 4, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김재윤", TeamName.SAMSUNG, Hand.R, 1990, PitcherPosition.SU, 12, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "임창민", TeamName.SAMSUNG, Hand.R, 1985, PitcherPosition.SU, 13, 2, 4, 2);
            SetPitcher(GameDirector.pitcherCount, "오승환", TeamName.SAMSUNG, Hand.R, 1982, PitcherPosition.CP, 14, 2, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "황동재", TeamName.SAMSUNG, Hand.R, 2001, PitcherPosition.RP, 15, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "최하늘", TeamName.SAMSUNG, Hand.R, 1999, PitcherPosition.RP, 16, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "김대우", TeamName.SAMSUNG, Hand.R, 1988, PitcherPosition.RP, 17, 1, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "육선엽", TeamName.SAMSUNG, Hand.R, 2005, PitcherPosition.RP, 18, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "이호성", TeamName.SAMSUNG, Hand.R, 2004, PitcherPosition.RP, 19, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "이재희", TeamName.SAMSUNG, Hand.R, 2001, PitcherPosition.SP,  20, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "김지찬", TeamName.SAMSUNG, Hand.L, 2001, BatterPosition.CF, 101, 1, 3, 4);
            SetBatter(GameDirector.batterCount, "이성규", TeamName.SAMSUNG, Hand.R, 1993, BatterPosition.RF, 102, 3, 2, 1);
            SetBatter(GameDirector.batterCount, "구자욱", TeamName.SAMSUNG, Hand.L, 1995, BatterPosition.LF, 103, 3, 5, 4);
            SetBatterForeign(GameDirector.batterCount, "디아즈", TeamName.SAMSUNG, Hand.L, 1996, BatterPosition._1B, 104, 5, 2, 1);
            SetBatter(GameDirector.batterCount, "박병호", TeamName.SAMSUNG, Hand.R, 1986, BatterPosition.DH, 105, 4, 1, 1);
            SetBatter(GameDirector.batterCount, "강민호", TeamName.SAMSUNG, Hand.R, 1985, BatterPosition.C, 106, 3, 2, 1);
            SetBatter(GameDirector.batterCount, "김영웅", TeamName.SAMSUNG, Hand.L, 2003, BatterPosition._3B, 107, 3, 2, 2);
            SetBatter(GameDirector.batterCount, "심재훈", TeamName.SAMSUNG, Hand.R, 2006, BatterPosition._2B, 108, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "이재현", TeamName.SAMSUNG, Hand.R, 2003, BatterPosition.SS, 109, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "김헌곤", TeamName.SAMSUNG, Hand.R, 1988, BatterPosition.RF, 110, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "함수호", TeamName.SAMSUNG, Hand.L, 2006, BatterPosition.CF, 111, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "류지혁", TeamName.SAMSUNG, Hand.L, 1994, BatterPosition._2B, 112, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "이병헌", TeamName.SAMSUNG, Hand.R, 1999, BatterPosition.C, 113, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "차승준", TeamName.SAMSUNG, Hand.L, 2006, BatterPosition._3B, 114, 3, 1, 1);
            SetBatter(GameDirector.batterCount, "윤정빈", TeamName.SAMSUNG, Hand.L, 1999, BatterPosition.LF, 115, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "안주형", TeamName.SAMSUNG, Hand.R, 1993, BatterPosition._2B, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "전병우", TeamName.SAMSUNG, Hand.R, 1992, BatterPosition._3B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "양도근", TeamName.SAMSUNG, Hand.R, 2003, BatterPosition.SS, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김성윤", TeamName.SAMSUNG, Hand.L, 1999, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "공민규", TeamName.SAMSUNG, Hand.L, 1999, BatterPosition.DH, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김재성", TeamName.SAMSUNG, Hand.L, 1996, BatterPosition.C, 121, 1, 1, 1);
            #endregion
            #region LOTTE GIANTS
            SetPitcherForeign(GameDirector.pitcherCount, "터커", TeamName.LOTTE, Hand.L, 1996, PitcherPosition.SP, 1, 2, 4, 2);
            SetPitcherForeign(GameDirector.pitcherCount, "반즈", TeamName.LOTTE, Hand.L, 1995, PitcherPosition.SP, 2, 1, 3, 4);
            SetPitcher(GameDirector.pitcherCount, "박세웅", TeamName.LOTTE, Hand.R, 1995, PitcherPosition.SP, 3, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "나균안", TeamName.LOTTE, Hand.R, 1998, PitcherPosition.SP, 4, 2, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "이인복", TeamName.LOTTE, Hand.R, 1991, PitcherPosition.SP, 5, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "구승민", TeamName.LOTTE, Hand.R, 1990, PitcherPosition.RP, 6, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김상수", TeamName.LOTTE, Hand.R, 1987, PitcherPosition.RP, 7, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "최이준", TeamName.LOTTE, Hand.R, 1999, PitcherPosition.RP, 8, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "최준용", TeamName.LOTTE, Hand.R, 2001, PitcherPosition.RP, 9, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "박진", TeamName.LOTTE, Hand.R, 1999, PitcherPosition.RP, 10, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "임준섭", TeamName.LOTTE, Hand.L, 1989, PitcherPosition.RP, 11, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "전미르", TeamName.LOTTE, Hand.R, 2005, PitcherPosition.SU, 12, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "정철원", TeamName.LOTTE, Hand.R, 1999, PitcherPosition.SU, 13, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "김원중", TeamName.LOTTE, Hand.R, 1993, PitcherPosition.CP, 14, 4, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "김태현", TeamName.LOTTE, Hand.L, 2005, PitcherPosition.RP, 15, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "김진욱", TeamName.LOTTE, Hand.R, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "박명현", TeamName.LOTTE, Hand.R, 2001, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "신정락", TeamName.LOTTE, Hand.R, 1987, PitcherPosition.RP, 18, 1, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "박진형", TeamName.LOTTE, Hand.R, 1994, PitcherPosition.RP, 19, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "한현희", TeamName.LOTTE, Hand.R, 1993, PitcherPosition.RP, 20, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "윤동희", TeamName.LOTTE, Hand.R, 2003, BatterPosition.CF, 101, 2, 4, 4);
            SetBatter(GameDirector.batterCount, "황성빈", TeamName.LOTTE, Hand.L, 1997, BatterPosition.LF, 102, 1, 3, 3);
            SetBatter(GameDirector.batterCount, "손호영", TeamName.LOTTE, Hand.R, 1994, BatterPosition._3B, 103, 2, 4, 3);
            SetBatterForeign(GameDirector.batterCount, "레이예스", TeamName.LOTTE, Hand.R, 1994, BatterPosition.RF, 104, 2, 5, 4);
            SetBatter(GameDirector.batterCount, "전준우", TeamName.LOTTE, Hand.R, 1986, BatterPosition.DH, 105, 2, 2, 3);
            SetBatter(GameDirector.batterCount, "유강남", TeamName.LOTTE, Hand.R, 1992, BatterPosition.C, 106, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "나승엽", TeamName.LOTTE, Hand.R, 2002, BatterPosition._1B, 107, 3, 3, 2);
            SetBatter(GameDirector.batterCount, "고승민", TeamName.LOTTE, Hand.L, 2000, BatterPosition._2B, 108, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "박승욱", TeamName.LOTTE, Hand.L, 1992, BatterPosition.SS, 109, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "최항", TeamName.LOTTE, Hand.L, 1994, BatterPosition._2B, 110, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "정훈", TeamName.LOTTE, Hand.R, 1987, BatterPosition._1B, 111, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "김민성", TeamName.LOTTE, Hand.R, 1988, BatterPosition._2B, 112, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "손성빈", TeamName.LOTTE, Hand.R, 2002, BatterPosition.C, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이주찬", TeamName.LOTTE, Hand.R, 1998, BatterPosition.SS, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "노진혁", TeamName.LOTTE, Hand.L, 1989, BatterPosition.SS, 115, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "정보근", TeamName.LOTTE, Hand.R, 1999, BatterPosition.C, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이호준", TeamName.LOTTE, Hand.R, 2004, BatterPosition._2B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "강성우", TeamName.LOTTE, Hand.R, 2005, BatterPosition._3B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "전민재", TeamName.LOTTE, Hand.R, 1999, BatterPosition.SS, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이정훈", TeamName.LOTTE, Hand.L, 1994, BatterPosition.DH, 120, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "이인한", TeamName.LOTTE, Hand.R, 1998, BatterPosition.RF, 121, 1, 1, 1);
            #endregion
            #region KIA TIGERS
            SetPitcherForeign(GameDirector.pitcherCount, "네일", TeamName.KIA, Hand.R, 1993, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "양현종", TeamName.KIA, Hand.L, 1988, PitcherPosition.SP, 2, 2, 4, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "올러", TeamName.KIA, Hand.R, 1994, PitcherPosition.SP, 3, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "윤영철", TeamName.KIA, Hand.L, 2004, PitcherPosition.SP, 4, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "황동하", TeamName.KIA, Hand.R, 2002, PitcherPosition.SP, 5, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김사윤", TeamName.KIA, Hand.L, 1994, PitcherPosition.RP, 6, 1, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "김도현", TeamName.KIA, Hand.R, 2000, PitcherPosition.RP, 7, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "이준영", TeamName.KIA, Hand.L, 1992, PitcherPosition.RP, 8, 2, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "김태형", TeamName.KIA, Hand.R, 2006, PitcherPosition.RP, 9, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "이호민", TeamName.KIA, Hand.R, 2006, PitcherPosition.RP, 10, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "최지민", TeamName.KIA, Hand.L, 2003, PitcherPosition.RP, 11, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "전상현", TeamName.KIA, Hand.R, 1996, PitcherPosition.SU, 12, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "곽도규", TeamName.KIA, Hand.L, 2004, PitcherPosition.SU, 13, 1, 2, 4);
            SetPitcher(GameDirector.pitcherCount, "정해영", TeamName.KIA, Hand.R, 2001, PitcherPosition.CP, 14, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "이의리", TeamName.KIA, Hand.L, 2002, PitcherPosition.SP, 15, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "임기영", TeamName.KIA, Hand.R, 1993, PitcherPosition.RP, 16, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "박준표", TeamName.KIA, Hand.R, 1992, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "성영탁", TeamName.KIA, Hand.R, 2004, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "유승철", TeamName.KIA, Hand.R, 1998, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김대유", TeamName.KIA, Hand.L, 1991, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박찬호", TeamName.KIA, Hand.R, 1995, BatterPosition.SS, 101, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "나성범", TeamName.KIA, Hand.L, 1989, BatterPosition.RF, 102, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "김도영", TeamName.KIA, Hand.R, 2003, BatterPosition._3B, 103, 5, 4, 4);
            SetBatterForeign(GameDirector.batterCount, "위즈덤", TeamName.KIA, Hand.R, 1991, BatterPosition._1B, 104, 5, 1, 1);
            SetBatter(GameDirector.batterCount, "최형우", TeamName.KIA, Hand.L, 1984, BatterPosition.DH, 105, 3, 3, 3);
            SetBatter(GameDirector.batterCount, "김태군", TeamName.KIA, Hand.R, 1989, BatterPosition.C, 106, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "김선빈", TeamName.KIA, Hand.R, 1989, BatterPosition._2B, 107, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "최원준", TeamName.KIA, Hand.L, 1997, BatterPosition.CF, 108, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "이창진", TeamName.KIA, Hand.R, 1991, BatterPosition.LF, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "이우성", TeamName.KIA, Hand.R, 1994, BatterPosition._1B, 110, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "서건창", TeamName.KIA, Hand.L, 1989, BatterPosition._2B, 111, 1, 3, 2);
            SetBatter(GameDirector.batterCount, "변우혁", TeamName.KIA, Hand.R, 2000, BatterPosition._1B, 112, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "한승택", TeamName.KIA, Hand.R, 1994, BatterPosition.C, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "황대인", TeamName.KIA, Hand.R, 1996, BatterPosition._1B, 114, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "한준수", TeamName.KIA, Hand.R, 1999, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박민", TeamName.KIA, Hand.R, 2001, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "홍종표", TeamName.KIA, Hand.R, 2000, BatterPosition.DH, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "오선우", TeamName.KIA, Hand.L, 1996, BatterPosition._2B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "고종욱", TeamName.KIA, Hand.L, 1989, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "예진원", TeamName.KIA, Hand.L, 1999, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박정우", TeamName.KIA, Hand.L, 1998, BatterPosition.LF, 121, 1, 1, 1);
            #endregion
            #region KIWOOM HEROES
            SetPitcherForeign(GameDirector.pitcherCount, "로젠버그", TeamName.KIWOOM, Hand.L, 1995, PitcherPosition.SP, 1, 2, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "하영민", TeamName.KIWOOM, Hand.R, 1995, PitcherPosition.SP, 2, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "김윤하", TeamName.KIWOOM, Hand.R, 2005, PitcherPosition.SP, 3, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김선기", TeamName.KIWOOM, Hand.R, 1991, PitcherPosition.SP, 4, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "정현우", TeamName.KIWOOM, Hand.L, 2006, PitcherPosition.SP, 5, 3, 5, 2);
            SetPitcher(GameDirector.pitcherCount, "김연주", TeamName.KIWOOM, Hand.R, 2004, PitcherPosition.RP, 6, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김서준", TeamName.KIWOOM, Hand.R, 2006, PitcherPosition.RP, 7, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "전준표", TeamName.KIWOOM, Hand.R, 2005, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "박정훈", TeamName.KIWOOM, Hand.L, 2006, PitcherPosition.RP, 9, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "양지율", TeamName.KIWOOM, Hand.R, 1998, PitcherPosition.RP, 10, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "원종현", TeamName.KIWOOM, Hand.R, 1987, PitcherPosition.RP, 11, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "조영건", TeamName.KIWOOM, Hand.R, 1999, PitcherPosition.SU, 12, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "조상우", TeamName.KIWOOM, Hand.R, 1994, PitcherPosition.SU, 13, 3, 4, 2);
            SetPitcher(GameDirector.pitcherCount, "주승우", TeamName.KIWOOM, Hand.R, 2000, PitcherPosition.CP, 14, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "이종민", TeamName.KIWOOM, Hand.L, 2001, PitcherPosition.RP, 15, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김인범", TeamName.KIWOOM, Hand.R, 2000, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "문성현", TeamName.KIWOOM, Hand.R, 1991, PitcherPosition.RP, 17, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "손현기", TeamName.KIWOOM, Hand.L, 2005, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "장필준", TeamName.KIWOOM, Hand.R, 1988, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "이강준", TeamName.KIWOOM, Hand.R, 2001, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이주형", TeamName.KIWOOM, Hand.R, 2001, BatterPosition.CF, 101, 1, 4, 4);
            SetBatter(GameDirector.batterCount, "송성문", TeamName.KIWOOM, Hand.R, 1996, BatterPosition._3B, 102, 2, 5, 3);
            SetBatterForeign(GameDirector.batterCount, "푸이그", TeamName.KIWOOM, Hand.R, 1990, BatterPosition.RF, 103, 4, 3, 2);
            SetBatterForeign(GameDirector.batterCount, "카디네스", TeamName.KIWOOM, Hand.R, 1997, BatterPosition.LF, 104, 5, 2, 1);
            SetBatter(GameDirector.batterCount, "최주환", TeamName.KIWOOM, Hand.R, 1988, BatterPosition._1B, 105, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "김태진", TeamName.KIWOOM, Hand.L, 1995, BatterPosition.SS, 106, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "김건희", TeamName.KIWOOM, Hand.R, 2004, BatterPosition.C, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "이용규", TeamName.KIWOOM, Hand.R, 1985, BatterPosition.DH, 108, 1, 2, 5);
            SetBatter(GameDirector.batterCount, "고영우", TeamName.KIWOOM, Hand.R, 2001, BatterPosition._2B, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "변상권", TeamName.KIWOOM, Hand.L, 1997, BatterPosition.LF, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김웅빈", TeamName.KIWOOM, Hand.L, 1996, BatterPosition.DH, 111, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "장재영", TeamName.KIWOOM, Hand.R, 2002, BatterPosition.RF, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김동헌", TeamName.KIWOOM, Hand.R, 2004, BatterPosition.C, 113, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "김재현", TeamName.KIWOOM, Hand.R, 1993, BatterPosition.C, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "염승원", TeamName.KIWOOM, Hand.L, 2006, BatterPosition._2B, 115, 1, 3, 2);
            SetBatter(GameDirector.batterCount, "김병휘", TeamName.KIWOOM, Hand.R, 2001, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "여동욱", TeamName.KIWOOM, Hand.R, 2006, BatterPosition._3B, 117, 3, 2, 2);
            SetBatter(GameDirector.batterCount, "이원석", TeamName.KIWOOM, Hand.R, 1986, BatterPosition._1B, 118, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "김동엽", TeamName.KIWOOM, Hand.R, 1990, BatterPosition.DH, 119, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "박주홍", TeamName.KIWOOM, Hand.L, 2001, BatterPosition.LF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이형종", TeamName.KIWOOM, Hand.R, 1989, BatterPosition.CF, 121, 1, 1, 1);
            #endregion
            #region DOOSAN BEARS
            SetPitcher(GameDirector.pitcherCount, "곽빈", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcherForeign(GameDirector.pitcherCount, "어빈", TeamName.DOOSAN, Hand.L, 1994, PitcherPosition.SP, 2, 2, 5, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "해치", TeamName.DOOSAN, Hand.R, 1994, PitcherPosition.SP, 3, 3, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "최원준", TeamName.DOOSAN, Hand.R, 1994, PitcherPosition.SP, 4, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "이영하", TeamName.DOOSAN, Hand.R, 1997, PitcherPosition.SP, 5, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "김민규", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.RP, 6, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "황희천", TeamName.DOOSAN, Hand.L, 2006, PitcherPosition.RP, 7, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "최승용", TeamName.DOOSAN, Hand.L, 2001, PitcherPosition.RP, 8, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "홍건희", TeamName.DOOSAN, Hand.R, 1992, PitcherPosition.RP, 9, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "김유성", TeamName.DOOSAN, Hand.R, 2002, PitcherPosition.RP, 10, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "박치국", TeamName.DOOSAN, Hand.R, 1998, PitcherPosition.RP, 11, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "최지강", TeamName.DOOSAN, Hand.L, 2001, PitcherPosition.SU, 12, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "이병헌", TeamName.DOOSAN, Hand.L, 2003, PitcherPosition.SU, 13, 3, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "김택연", TeamName.DOOSAN, Hand.R, 2005, PitcherPosition.CP, 14, 4, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "권휘", TeamName.DOOSAN, Hand.R, 2000, PitcherPosition.RP, 15, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김도윤", TeamName.DOOSAN, Hand.R, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김정우", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김명신", TeamName.DOOSAN, Hand.R, 1993, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "박신지", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김호준", TeamName.DOOSAN, Hand.L, 1998, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "정수빈", TeamName.DOOSAN, Hand.L, 1990, BatterPosition.CF, 101, 1, 3, 3);
            SetBatterForeign(GameDirector.batterCount, "케이브", TeamName.DOOSAN, Hand.L, 1992, BatterPosition.RF, 102, 2, 4, 3);
            SetBatter(GameDirector.batterCount, "양의지", TeamName.DOOSAN, Hand.R, 1987, BatterPosition.C, 103, 2, 5, 4);
            SetBatter(GameDirector.batterCount, "김재환", TeamName.DOOSAN, Hand.L, 1988, BatterPosition.LF, 104, 4, 3, 2);
            SetBatter(GameDirector.batterCount, "강승호", TeamName.DOOSAN, Hand.R, 1994, BatterPosition._1B, 105, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "양석환", TeamName.DOOSAN, Hand.R, 1991, BatterPosition.DH, 106, 3, 2, 2);
            SetBatter(GameDirector.batterCount, "박준순", TeamName.DOOSAN, Hand.R, 2006, BatterPosition._2B, 107, 2, 4, 1);
            SetBatter(GameDirector.batterCount, "이유찬", TeamName.DOOSAN, Hand.R, 1998, BatterPosition._3B, 108, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "전민재", TeamName.DOOSAN, Hand.R, 1999, BatterPosition.SS, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "김기연", TeamName.DOOSAN, Hand.R, 1997, BatterPosition.C, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박지훈", TeamName.DOOSAN, Hand.R, 2000, BatterPosition.SS, 111, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "조수행", TeamName.DOOSAN, Hand.L, 1993, BatterPosition.LF, 112, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "임종성", TeamName.DOOSAN, Hand.R, 2005, BatterPosition._2B, 113, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "김인태", TeamName.DOOSAN, Hand.L, 1994, BatterPosition.LF, 114, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "홍성호", TeamName.DOOSAN, Hand.L, 1997, BatterPosition.RF, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김민석", TeamName.DOOSAN, Hand.R, 2004, BatterPosition.CF, 116, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "김민혁", TeamName.DOOSAN, Hand.R, 1996, BatterPosition._1B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박계범", TeamName.DOOSAN, Hand.R, 1996, BatterPosition._3B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "여동건", TeamName.DOOSAN, Hand.R, 2005, BatterPosition.SS, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "추재현", TeamName.DOOSAN, Hand.L, 1999, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박민준", TeamName.DOOSAN, Hand.R, 2002, BatterPosition.C, 121, 1, 1, 1);
            #endregion
            #region HANHWA EAGLES
            SetPitcherForeign(GameDirector.pitcherCount, "와이스", TeamName.HANWHA, Hand.R, 1996, PitcherPosition.SP, 1, 2, 4, 5);
            SetPitcher(GameDirector.pitcherCount, "류현진", TeamName.HANWHA, Hand.L, 1987, PitcherPosition.SP, 2, 2, 5, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "폰세", TeamName.HANWHA, Hand.R, 1994, PitcherPosition.SP, 3, 4, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "엄상백", TeamName.HANWHA, Hand.R, 1996, PitcherPosition.SP, 4, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "문동주", TeamName.HANWHA, Hand.R, 2003, PitcherPosition.SP, 5, 5, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김규연", TeamName.HANWHA, Hand.R, 2002, PitcherPosition.RP, 6, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "정우주", TeamName.HANWHA, Hand.R, 2006, PitcherPosition.RP, 7, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "황준서", TeamName.HANWHA, Hand.L, 2005, PitcherPosition.RP, 8, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "이민우", TeamName.HANWHA, Hand.R, 1993, PitcherPosition.RP, 9, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "권민규", TeamName.HANWHA, Hand.L, 2006, PitcherPosition.RP, 10, 1, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "김서현", TeamName.HANWHA, Hand.R, 2004, PitcherPosition.RP, 11, 4, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "한승혁", TeamName.HANWHA, Hand.R, 1993, PitcherPosition.SU, 12, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "박상원", TeamName.HANWHA, Hand.R, 1994, PitcherPosition.SU, 13, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "주현상", TeamName.HANWHA, Hand.R, 1992, PitcherPosition.CP, 14, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "윤대경", TeamName.HANWHA, Hand.R, 1994, PitcherPosition.RP, 15, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김기중", TeamName.HANWHA, Hand.L, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "장시환", TeamName.HANWHA, Hand.R, 1987, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "장민재", TeamName.HANWHA, Hand.R, 1990, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "배민서", TeamName.HANWHA, Hand.R, 1999, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "이태양", TeamName.HANWHA, Hand.R, 1990, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "황영묵", TeamName.HANWHA, Hand.L, 1999, BatterPosition._2B, 101, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "심우준", TeamName.HANWHA, Hand.R, 1995, BatterPosition.SS, 102, 1, 2, 2);
            SetBatterForeign(GameDirector.batterCount, "플로리얼", TeamName.HANWHA, Hand.L, 1997, BatterPosition.CF, 103, 3, 4, 4);
            SetBatter(GameDirector.batterCount, "노시환", TeamName.HANWHA, Hand.R, 2000, BatterPosition.DH, 104, 4, 3, 1);
            SetBatter(GameDirector.batterCount, "채은성", TeamName.HANWHA, Hand.R, 1990, BatterPosition._1B, 105, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "이진영", TeamName.HANWHA, Hand.R, 1997, BatterPosition.RF, 106, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "최재훈", TeamName.HANWHA, Hand.R, 1989, BatterPosition.C, 107, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "문현빈", TeamName.HANWHA, Hand.L, 2004, BatterPosition._3B, 108, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "권광민", TeamName.HANWHA, Hand.L, 1997, BatterPosition.LF, 109, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "한지윤", TeamName.HANWHA, Hand.R, 2006, BatterPosition.C, 110, 2, 4, 1);
            SetBatter(GameDirector.batterCount, "안치홍", TeamName.HANWHA, Hand.R, 1990, BatterPosition._1B, 111, 2, 2, 4);
            SetBatter(GameDirector.batterCount, "이도윤", TeamName.HANWHA, Hand.R, 1996, BatterPosition.SS, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이원석", TeamName.HANWHA, Hand.R, 1999, BatterPosition.CF, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김태연", TeamName.HANWHA, Hand.R, 1997, BatterPosition.RF, 114, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "이재원", TeamName.HANWHA, Hand.R, 1988, BatterPosition.C, 115, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "한경빈", TeamName.HANWHA, Hand.L, 1998, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김인환", TeamName.HANWHA, Hand.L, 1994, BatterPosition._1B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김건", TeamName.HANWHA, Hand.R, 2000, BatterPosition._2B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "유로결", TeamName.HANWHA, Hand.R, 2000, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "유민", TeamName.HANWHA, Hand.R, 2003, BatterPosition.LF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "최인호", TeamName.HANWHA, Hand.L, 2000, BatterPosition.RF, 121, 1, 1, 1);
            #endregion
            #region LG TWINS
            SetPitcherForeign(GameDirector.pitcherCount, "에르난데스", TeamName.LG, Hand.R, 1995, PitcherPosition.SP, 1, 4, 4, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "치리노스", TeamName.LG, Hand.R, 1993, PitcherPosition.SP, 2, 3, 3, 4);
            SetPitcher(GameDirector.pitcherCount, "손주영", TeamName.LG, Hand.L, 1998, PitcherPosition.SP, 3, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "임찬규", TeamName.LG, Hand.R, 1992, PitcherPosition.SP, 4, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "박시원", TeamName.LG, Hand.R, 2006, PitcherPosition.SP, 5, 2, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "김영우", TeamName.LG, Hand.R, 2005, PitcherPosition.RP, 6, 4, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "정우영", TeamName.LG, Hand.R, 1999, PitcherPosition.RP, 7, 2, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "이우찬", TeamName.LG, Hand.L, 1992, PitcherPosition.RP, 8, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "함덕주", TeamName.LG, Hand.L, 1995, PitcherPosition.RP, 9, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "김유영", TeamName.LG, Hand.L, 1994, PitcherPosition.RP, 10, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "유영찬", TeamName.LG, Hand.R, 1997, PitcherPosition.RP, 11, 3, 4, 2);
            SetPitcher(GameDirector.pitcherCount, "김강률", TeamName.LG, Hand.R, 1988, PitcherPosition.SU, 12, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "김진성", TeamName.LG, Hand.R, 1985, PitcherPosition.SU, 13, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "장현식", TeamName.LG, Hand.R, 1995, PitcherPosition.CP, 14, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "이지강", TeamName.LG, Hand.R, 1999, PitcherPosition.RP, 15, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "최채흥", TeamName.LG, Hand.L, 1995, PitcherPosition.RP, 16, 1, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "박명근", TeamName.LG, Hand.R, 2004, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "최용하", TeamName.LG, Hand.R, 2002, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "임준영", TeamName.LG, Hand.L, 2000, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김진수", TeamName.LG, Hand.R, 1998, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "홍창기", TeamName.LG, Hand.L, 1993, BatterPosition.RF, 101, 1, 5, 5);
            SetBatter(GameDirector.batterCount, "문성주", TeamName.LG, Hand.L, 1997, BatterPosition.LF, 102, 1, 2, 1);
            SetBatterForeign(GameDirector.batterCount, "오스틴", TeamName.LG, Hand.R, 1993, BatterPosition._1B, 103, 3, 4, 4);
            SetBatter(GameDirector.batterCount, "문보경", TeamName.LG, Hand.R, 2000, BatterPosition._3B, 104, 3, 3, 2);
            SetBatter(GameDirector.batterCount, "김현수", TeamName.LG, Hand.L, 1988, BatterPosition.DH, 105, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "박동원", TeamName.LG, Hand.R, 1990, BatterPosition.C, 106, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "오지환", TeamName.LG, Hand.R, 1990, BatterPosition.SS, 107, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "박해민", TeamName.LG, Hand.L, 1990, BatterPosition.CF, 108, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "신민재", TeamName.LG, Hand.L, 1996, BatterPosition._2B, 109, 1, 2, 4);
            SetBatter(GameDirector.batterCount, "박관우", TeamName.LG, Hand.L, 2006, BatterPosition.CF, 110, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "추세현", TeamName.LG, Hand.R, 2006, BatterPosition.RF, 111, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "구본혁", TeamName.LG, Hand.R, 1997, BatterPosition.SS, 112, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "김민수", TeamName.LG, Hand.R, 1998, BatterPosition._3B, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김현종", TeamName.LG, Hand.R, 2004, BatterPosition.CF, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이한림", TeamName.LG, Hand.R, 2006, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김주성", TeamName.LG, Hand.R, 1998, BatterPosition._1B, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이영빈", TeamName.LG, Hand.L, 2002, BatterPosition.SS, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "송찬의", TeamName.LG, Hand.R, 1999, BatterPosition._2B, 118, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "김성진", TeamName.LG, Hand.R, 2000, BatterPosition._3B, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "안익훈", TeamName.LG, Hand.L, 1996, BatterPosition.CF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "최원영", TeamName.LG, Hand.R, 2003, BatterPosition.DH, 121, 1, 1, 1);
            #endregion
            #region SSG LANDERS
            SetPitcher(GameDirector.pitcherCount, "김광현", TeamName.SSG, Hand.L, 1988, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcherForeign(GameDirector.pitcherCount, "화이트", TeamName.SSG, Hand.R, 1994, PitcherPosition.SP, 2, 3, 2, 2);
            SetPitcherForeign(GameDirector.pitcherCount, "앤더슨", TeamName.SSG, Hand.R, 1994, PitcherPosition.SP, 3, 4, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "박종훈", TeamName.SSG, Hand.R, 1991, PitcherPosition.SP, 4, 1, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "송영진", TeamName.SSG, Hand.R, 2004, PitcherPosition.SP, 5, 1, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "최민준", TeamName.SSG, Hand.R, 1999, PitcherPosition.RP, 6, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "전영준", TeamName.SSG, Hand.R, 2002, PitcherPosition.RP, 7, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "한두솔", TeamName.SSG, Hand.L, 1997, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "서진용", TeamName.SSG, Hand.R, 1992, PitcherPosition.RP, 9, 3, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "이로운", TeamName.SSG, Hand.R, 2004, PitcherPosition.RP, 10, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "조병현", TeamName.SSG, Hand.R, 2002, PitcherPosition.RP, 11, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "노경은", TeamName.SSG, Hand.R, 1984, PitcherPosition.SU, 12, 1, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "김민", TeamName.SSG, Hand.R, 1999, PitcherPosition.SU, 13, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "문승원", TeamName.SSG, Hand.R, 1989, PitcherPosition.CP, 14, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "백승건", TeamName.SSG, Hand.L, 2000, PitcherPosition.RP, 15, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "신헌민", TeamName.SSG, Hand.R, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김건우", TeamName.SSG, Hand.L, 2002, PitcherPosition.RP, 17, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김택형", TeamName.SSG, Hand.L, 1996, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "박시후", TeamName.SSG, Hand.L, 2001, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "장지훈", TeamName.SSG, Hand.R, 1998, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박성한", TeamName.SSG, Hand.R, 1998, BatterPosition.SS, 101, 1, 3, 3);
            SetBatter(GameDirector.batterCount, "정준재", TeamName.SSG, Hand.L, 2003, BatterPosition._2B, 102, 1, 2, 2);
            SetBatterForeign(GameDirector.batterCount, "에레디아", TeamName.SSG, Hand.R, 1991, BatterPosition.LF, 103, 4, 5, 5);
            SetBatter(GameDirector.batterCount, "최정", TeamName.SSG, Hand.R, 1987, BatterPosition._3B, 104, 5, 3, 4);
            SetBatter(GameDirector.batterCount, "한유섬", TeamName.SSG, Hand.R,1989, BatterPosition.RF, 105, 3, 2, 1);
            SetBatter(GameDirector.batterCount, "이지영", TeamName.SSG, Hand.R, 1986, BatterPosition.C, 106, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "박지환", TeamName.SSG, Hand.R, 2005, BatterPosition.DH, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "최지훈", TeamName.SSG, Hand.R, 1999, BatterPosition.CF, 108, 1, 3, 3);
            SetBatter(GameDirector.batterCount, "고명준", TeamName.SSG, Hand.R, 2002, BatterPosition._1B, 109, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "이율예", TeamName.SSG, Hand.R, 2006, BatterPosition.C, 110, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "오태곤", TeamName.SSG, Hand.R, 1991, BatterPosition.RF, 111, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김성현", TeamName.SSG, Hand.R, 1987, BatterPosition._2B, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "현원회", TeamName.SSG, Hand.R, 2001, BatterPosition.C, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "하재훈", TeamName.SSG, Hand.R, 1990, BatterPosition.CF, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "신범수", TeamName.SSG, Hand.R, 1998, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김찬형", TeamName.SSG, Hand.R, 1997, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "최준우", TeamName.SSG, Hand.R, 1999, BatterPosition._2B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "최상민", TeamName.SSG, Hand.L, 1999, BatterPosition.RF, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박정빈", TeamName.SSG, Hand.R, 2002, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이원준", TeamName.SSG, Hand.R, 2006, BatterPosition.CF, 120, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "이정범", TeamName.SSG, Hand.L, 1998, BatterPosition.LF, 121, 1, 1, 1);
            #endregion 
            #region NC DINOS
            SetPitcherForeign(GameDirector.pitcherCount, "하트", TeamName.NC, Hand.L, 1992, PitcherPosition.SP, 1, 3, 5, 5);
            SetPitcher(GameDirector.pitcherCount, "이재학", TeamName.NC, Hand.R, 1990, PitcherPosition.SP, 2, 2, 3, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "라일리", TeamName.NC, Hand.R, 1996, PitcherPosition.SP, 3, 4, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "신민혁", TeamName.NC, Hand.R, 1999, PitcherPosition.SP, 4, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "김시훈", TeamName.NC, Hand.R, 1999, PitcherPosition.SP, 5, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "최우석", TeamName.NC, Hand.R, 2005, PitcherPosition.RP, 6, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김진호", TeamName.NC, Hand.R, 1998, PitcherPosition.RP, 7, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "한재승", TeamName.NC, Hand.R, 2001, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김태훈", TeamName.NC, Hand.R, 2006, PitcherPosition.RP, 9, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "임정호", TeamName.NC, Hand.L, 2000, PitcherPosition.RP, 10, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "류진욱", TeamName.NC, Hand.R, 1996, PitcherPosition.RP, 11, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김재열", TeamName.NC, Hand.R, 1996, PitcherPosition.SU, 12, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "김영규", TeamName.NC, Hand.R, 2000, PitcherPosition.SU, 13, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "이용찬", TeamName.NC, Hand.R, 1989, PitcherPosition.CP, 14, 2, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "최성영", TeamName.NC, Hand.L, 1997, PitcherPosition.RP, 15, 3, 1, 3);
            SetPitcher(GameDirector.pitcherCount, "소이현", TeamName.NC, Hand.R, 1999, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "목지훈", TeamName.NC, Hand.R, 2004, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "이세민", TeamName.NC, Hand.R, 2006, PitcherPosition.RP, 18, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "서동욱", TeamName.NC, Hand.R, 2004, PitcherPosition.RP, 19, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "박동수", TeamName.NC, Hand.R, 1999, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박민우", TeamName.NC, Hand.L, 1993, BatterPosition._2B, 101, 1, 4, 3);
            SetBatter(GameDirector.batterCount, "손아섭", TeamName.NC, Hand.R, 1988, BatterPosition.RF, 102, 1, 4, 4);
            SetBatter(GameDirector.batterCount, "천재환", TeamName.NC, Hand.R, 1994, BatterPosition.LF, 103, 2, 2, 1);
            SetBatterForeign(GameDirector.batterCount, "데이비슨", TeamName.NC, Hand.R, 1991, BatterPosition._1B, 104, 5, 1, 1);
            SetBatter(GameDirector.batterCount, "권희동", TeamName.NC, Hand.R, 1990, BatterPosition.DH, 105, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "김휘집", TeamName.NC, Hand.R, 2002, BatterPosition.SS, 106, 2, 3, 2);
            SetBatter(GameDirector.batterCount, "유재현", TeamName.NC, Hand.R, 2005, BatterPosition._3B, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "박세혁", TeamName.NC, Hand.L, 1990, BatterPosition.C, 108, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "최정원", TeamName.NC, Hand.L, 2000, BatterPosition.CF, 109, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "김성욱", TeamName.NC, Hand.R, 1993, BatterPosition.RF, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "도태훈", TeamName.NC, Hand.L, 1993, BatterPosition._1B, 111, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "서호철", TeamName.NC, Hand.R, 1996, BatterPosition._2B, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "한재환", TeamName.NC, Hand.R, 2001, BatterPosition.DH, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김한별", TeamName.NC, Hand.R, 2001, BatterPosition.SS, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "안중열", TeamName.NC, Hand.R, 1995, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김형준", TeamName.NC, Hand.R, 1999, BatterPosition.C, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "박주찬", TeamName.NC, Hand.R, 1996, BatterPosition._1B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김주원", TeamName.NC, Hand.S, 2002, BatterPosition.SS, 118, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "오영수", TeamName.NC, Hand.L, 2000, BatterPosition._1B, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "송승환", TeamName.NC, Hand.R, 2000, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "한석현", TeamName.NC, Hand.L, 1994, BatterPosition.CF, 121, 1, 1, 2);
            #endregion
            #region KT WIZ
            SetPitcherForeign(GameDirector.pitcherCount, "쿠에바스", TeamName.KT, Hand.R, 1990, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "고영표", TeamName.KT, Hand.R, 1991, PitcherPosition.SP, 2, 3, 4, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "헤이수스", TeamName.KT, Hand.L, 1996, PitcherPosition.SP, 3, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "오원석", TeamName.KT, Hand.L, 2001, PitcherPosition.SP, 4, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "육청명", TeamName.KT, Hand.R, 2005, PitcherPosition.SP, 5, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "원상현", TeamName.KT, Hand.R, 2004, PitcherPosition.RP, 6, 1, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "박건우", TeamName.KT, Hand.R, 2006, PitcherPosition.RP, 7, 1, 5, 1);
            SetPitcher(GameDirector.pitcherCount, "성재헌", TeamName.KT, Hand.L, 1997, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "소형준", TeamName.KT, Hand.R, 2001, PitcherPosition.RP, 9, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "조이현", TeamName.KT, Hand.R, 1995, PitcherPosition.RP, 10, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "주권", TeamName.KT, Hand.R, 1995, PitcherPosition.RP, 11, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "우규민", TeamName.KT, Hand.R, 1985, PitcherPosition.SU, 12, 1, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "김동현", TeamName.KT, Hand.R, 2006, PitcherPosition.SU, 13, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "박영현", TeamName.KT, Hand.R, 2003, PitcherPosition.CP, 14, 3, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "문용익", TeamName.KT, Hand.R, 1995, PitcherPosition.RP, 15, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "최동환", TeamName.KT, Hand.R, 1989, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "이채호", TeamName.KT, Hand.R, 1998, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "손동현", TeamName.KT, Hand.R, 2001, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "김민수", TeamName.KT, Hand.R, 1992, PitcherPosition.RP, 19, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "최동환", TeamName.KT, Hand.R, 1989, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "김민혁", TeamName.KT, Hand.L, 1995, BatterPosition.LF, 101, 1, 3, 3);
            SetBatterForeign(GameDirector.batterCount, "로하스", TeamName.KT, Hand.S, 1990, BatterPosition.RF, 102, 4, 4, 4);
            SetBatter(GameDirector.batterCount, "오윤석", TeamName.KT, Hand.R, 1992, BatterPosition._2B, 103, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "강백호", TeamName.KT, Hand.L, 1999, BatterPosition.C, 104, 4, 3, 4);
            SetBatter(GameDirector.batterCount, "문상철", TeamName.KT, Hand.R, 1991, BatterPosition._1B, 105, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "황재균", TeamName.KT, Hand.R, 1987, BatterPosition.DH, 106, 3, 2, 3);
            SetBatter(GameDirector.batterCount, "배정대", TeamName.KT, Hand.R, 1995, BatterPosition.CF, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "김상수", TeamName.KT, Hand.R, 1990, BatterPosition.SS, 108, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "허경민", TeamName.KT, Hand.R, 1990, BatterPosition._3B, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "박민석", TeamName.KT, Hand.R, 2006, BatterPosition.CF, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "오재일", TeamName.KT, Hand.L, 1986, BatterPosition._1B, 111, 3, 1, 1);
            SetBatter(GameDirector.batterCount, "천성호", TeamName.KT, Hand.L, 1997, BatterPosition._2B, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "장진혁", TeamName.KT, Hand.L, 1993, BatterPosition.CF, 113, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "조대현", TeamName.KT, Hand.R, 1999, BatterPosition.C, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "장성우", TeamName.KT, Hand.R, 1990, BatterPosition.C, 115, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "장준원", TeamName.KT, Hand.R, 1995, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "이호연", TeamName.KT, Hand.L, 1995, BatterPosition._2B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "송민섭", TeamName.KT, Hand.R, 1991, BatterPosition.CF, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "안치영", TeamName.KT, Hand.L, 1998, BatterPosition.LF, 119, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "신범준", TeamName.KT, Hand.L, 2002, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "정영웅", TeamName.KT, Hand.L, 1999, BatterPosition.DH, 121, 1, 1, 1);
            #endregion


            for (int i = 0; i < GameDirector.pitcherCount; i++)
            {
                GameDirector.pitcher[i].playerId = i;
            }

            for (int i = 0; i < GameDirector.batterCount; i++)
            {
                GameDirector.batter[i].playerId = i + GameDirector.pitcherCount;
            }

        }

        public static void CreateForeignCandidatePlayer()
        {
            for (int i = 0; i < 10; i++)
            {
                int DecideNation = UnityEngine.Random.Range(0, 4);
                int DecideName = UnityEngine.Random.Range(0, 11);
                int DecideBorn = UnityEngine.Random.Range(1988, 2003);
                int DecideHand = UnityEngine.Random.Range(0, 2);
                int DecidePos = UnityEngine.Random.Range(0, 2);
                int DecideOVR1 = UnityEngine.Random.Range(1, 6);
                int DecideOVR2 = UnityEngine.Random.Range(1, 6);
                int DecideOVR3 = UnityEngine.Random.Range(1, 6);
                SetForeignCandidatePitcher(i, GetNationallyName((Nation)DecideNation, DecideName), (Nation)DecideNation, (Hand)DecideHand, DecideBorn, (PitcherPosition)DecidePos, DecideOVR1, DecideOVR2, DecideOVR3);
            }

            for (int i = 0; i < 10; i++)
            {
                int DecideNation = UnityEngine.Random.Range(0, 4);
                int DecideName = UnityEngine.Random.Range(0, 11);
                int DecideBorn = UnityEngine.Random.Range(1988, 2003);
                int DecideHand = UnityEngine.Random.Range(0, 2);
                int DecidePos = UnityEngine.Random.Range(1, 9);
                int DecideOVR1 = UnityEngine.Random.Range(1, 6);
                int DecideOVR2 = UnityEngine.Random.Range(1, 6);
                int DecideOVR3 = UnityEngine.Random.Range(1, 6);
                SetForeignCandidateBatter(i, GetNationallyName((Nation)DecideNation, DecideName), (Nation)DecideNation, (Hand)DecideHand, DecideBorn, (BatterPosition)DecidePos, DecideOVR1, DecideOVR2, DecideOVR3);
            }
        }

        public static string GetNationallyName(Nation nation, int DecideName)
        {
            string name = "존";
            switch (nation) {
                case Nation.USA: // 미국
                    if (DecideName == 0) { name = "스미스"; }
                    else if (DecideName == 1) { name = "존슨"; }
                    else if (DecideName == 2) { name = "윌리엄스"; }
                    else if (DecideName == 3) { name = "존스"; }
                    else if (DecideName == 4) { name = "브라운"; }
                    else if (DecideName == 5) { name = "데이비스"; }
                    else if (DecideName == 6) { name = "밀러"; }
                    else if (DecideName == 7) { name = "윌슨"; }
                    else if (DecideName == 8) { name = "무어"; }
                    else if (DecideName == 9) { name = "테일러"; }
                    else if (DecideName == 10) { name = "안데르손"; }
                    break;
                case Nation.Cuba: // 쿠바
                    if (DecideName == 0) { name = "곤잘레스"; }
                    else if (DecideName == 1) { name = "게레로"; }
                    else if (DecideName == 2) { name = "구즈만"; }
                    else if (DecideName == 3) { name = "구티에레스"; }
                    else if (DecideName == 4) { name = "누네즈"; }
                    else if (DecideName == 5) { name = "데라로사"; }
                    else if (DecideName == 6) { name = "델가도"; }
                    else if (DecideName == 7) { name = "도밍게즈"; }
                    else if (DecideName == 8) { name = "로렌조"; }
                    else if (DecideName == 9) { name = "로메로"; }
                    else if (DecideName == 10) { name = "라미레스"; }
                    break;
                case Nation.Venezuela: // 베네수엘라
                    if (DecideName == 0) { name = "리베라"; }
                    else if (DecideName == 1) { name = "레이디아즈"; }
                    else if (DecideName == 2) { name = "라몬"; }
                    else if (DecideName == 3) { name = "카를로스"; }
                    else if (DecideName == 4) { name = "카브레라"; }
                    else if (DecideName == 5) { name = "페레스"; }
                    else if (DecideName == 6) { name = "페티트"; }
                    else if (DecideName == 7) { name = "알투베"; }
                    else if (DecideName == 8) { name = "모레노"; }
                    else if (DecideName == 9) { name = "모랄레스"; }
                    else if (DecideName == 10) { name = "블랑코"; }
                    break;
                case Nation.Japan: // 일본
                    if (DecideName == 0) { name = "아오"; }
                    else if (DecideName == 1) { name = "하루토"; }
                    else if (DecideName == 2) { name = "아오이"; }
                    else if (DecideName == 3) { name = "아사히"; }
                    else if (DecideName == 4) { name = "렌"; }
                    else if (DecideName == 5) { name = "미나토"; }
                    else if (DecideName == 6) { name = "유이토"; }
                    else if (DecideName == 7) { name = "유우마"; }
                    else if (DecideName == 8) { name = "히나타"; }
                    else if (DecideName == 9) { name = "이츠키"; }
                    else if (DecideName == 10) { name = "유키"; }
                    break;
            }
            return name;
        }

        public static void SetPitcher(int index, string name, TeamName team, Hand hand, int age, PitcherPosition Pos, int posinteam, int _SPEED, int _COMMAND, int _BREAKING)
        {
            GameDirector.pitcher.Add(new Pitcher());
            GameDirector.pitcher[index].name = name;
            GameDirector.pitcher[index].isForeign = false;
            GameDirector.pitcher[index].team = team;
            GameDirector.pitcher[index].hand = hand;
            GameDirector.pitcher[index].born = age;
            GameDirector.pitcher[index].pos = Pos;
            GameDirector.pitcher[index].posInTeam = posinteam;
            GameDirector.pitcher[index].HP = 100;
            GameDirector.pitcher[index].SPEED = _SPEED;
            GameDirector.pitcher[index].COMMAND = _COMMAND;
            GameDirector.pitcher[index].BREAKING = _BREAKING;
            GameDirector.pitcherCount++;
            GameDirector.totalPlayerCount++;
        }

        public static void SetPitcherForeign(int index, string name, TeamName team, Hand hand, int age, PitcherPosition Pos, int posinteam, int _SPEED, int _COMMAND, int _BREAKING)
        {
            GameDirector.pitcher.Add(new Pitcher());
            GameDirector.pitcher[index].name = name;
            GameDirector.pitcher[index].isForeign = true;
            GameDirector.pitcher[index].team = team;
            GameDirector.pitcher[index].hand = hand;
            GameDirector.pitcher[index].born = age;
            GameDirector.pitcher[index].pos = Pos;
            GameDirector.pitcher[index].posInTeam = posinteam;
            GameDirector.pitcher[index].HP = 100;
            GameDirector.pitcher[index].SPEED = _SPEED;
            GameDirector.pitcher[index].COMMAND = _COMMAND;
            GameDirector.pitcher[index].BREAKING = _BREAKING;
            GameDirector.pitcherCount++;
            GameDirector.totalPlayerCount++;
        }

        public static void SetBatter(int index, string name, TeamName team, Hand hand, int age, BatterPosition Pos, int posinteam, int _POWER, int _CONTACT, int _EYE)
        {
            GameDirector.batter.Add(new Batter());
            GameDirector.batter[index].name = name;
            GameDirector.batter[index].isForeign = false;
            GameDirector.batter[index].team = team;
            GameDirector.batter[index].hand = hand;
            GameDirector.batter[index].born = age;
            GameDirector.batter[index].pos = Pos;
            GameDirector.batter[index].posInTeam = posinteam;
            GameDirector.batter[index].POWER = _POWER;
            GameDirector.batter[index].CONTACT = _CONTACT;
            GameDirector.batter[index].EYE = _EYE;
            GameDirector.batterCount++;
            GameDirector.totalPlayerCount++;
        }

        public static void SetBatterForeign(int index, string name, TeamName team, Hand hand, int age, BatterPosition Pos, int posinteam, int _POWER, int _CONTACT, int _EYE)
        {
            GameDirector.batter.Add(new Batter());
            GameDirector.batter[index].name = name;
            GameDirector.batter[index].isForeign = true;
            GameDirector.batter[index].team = team;
            GameDirector.batter[index].hand = hand;
            GameDirector.batter[index].born = age;
            GameDirector.batter[index].pos = Pos;
            GameDirector.batter[index].posInTeam = posinteam;
            GameDirector.batter[index].POWER = _POWER;
            GameDirector.batter[index].CONTACT = _CONTACT;
            GameDirector.batter[index].EYE = _EYE;
            GameDirector.batterCount++;
            GameDirector.totalPlayerCount++;
        }

        public static void SetForeignCandidatePitcher(int index, string name, Nation nation, Hand hand, int age, PitcherPosition Pos, int _SPEED, int _COMMAND, int _BREAKING)
        {
            GameDirector.Fpitcher.Add(new ForeignPitcher());
            GameDirector.Fpitcher[index].playerId = GameDirector.totalPlayerCount + index;
            GameDirector.Fpitcher[index].name = name;
            GameDirector.Fpitcher[index].isForeign = true;
            GameDirector.Fpitcher[index].nation = nation;
            GameDirector.Fpitcher[index].hand = hand;
            GameDirector.Fpitcher[index].born = age;
            GameDirector.Fpitcher[index].pos = Pos;
            GameDirector.Fpitcher[index].HP = 100;
            GameDirector.Fpitcher[index].SPEED = _SPEED;
            GameDirector.Fpitcher[index].COMMAND = _COMMAND;
            GameDirector.Fpitcher[index].BREAKING = _BREAKING;
            GameDirector.foreignPitcherCandidateCount++;
        }

        public static void SetForeignCandidateBatter(int index, string name, Nation nation, Hand hand, int age, BatterPosition Pos, int _POWER, int _CONTACT, int _EYE)
        {
            GameDirector.Fbatter.Add(new ForeignBatter());
            GameDirector.Fbatter[index].name = name;
            GameDirector.Fbatter[index].playerId = GameDirector.totalPlayerCount + GameDirector.foreignPitcherCandidateCount + index;
            GameDirector.Fbatter[index].isForeign = true;
            GameDirector.Fbatter[index].nation = nation;
            GameDirector.Fbatter[index].hand = hand;
            GameDirector.Fbatter[index].born = age;
            GameDirector.Fbatter[index].pos = Pos;
            GameDirector.Fbatter[index].POWER = _POWER;
            GameDirector.Fbatter[index].CONTACT = _CONTACT;
            GameDirector.Fbatter[index].EYE = _EYE;
            GameDirector.foreignBatterCandidateCount++;
        }

        public static void CreateDate()
        {
            GameDirector.currentDate = new Date(2025, 3, 23, DayOfWeek.SUNDAY);
        }

        public static void CreateTeam()
        {
            for (int t = 0; t < 10; t++)
            {
                GameDirector.Teams.Add(new Team());
                GameDirector.Teams[t].teamCode = t;
                GameDirector.Teams[t].win = 0;
                GameDirector.Teams[t].draw = 0;
                GameDirector.Teams[t].lose = 0;
                GameDirector.Teams[t].currentSP = 1;
            }
        }

        public static void ShuffleTeams(int[] teams)
        {
            for (int i = 0; i < 10; i++)
            {
                int j = UnityEngine.Random.Range(0, 10);  // 랜덤으로 팀의 인덱스를 선택
                int temp = teams[i];
                teams[i] = teams[j];
                teams[j] = temp;
            }
        }

        public static Date UpdateDate(Date _date)
        {
            if (_date.month == 1 || _date.month == 3 || _date.month == 5 || _date.month == 7 || _date.month == 8 || _date.month == 10 || _date.month == 12)
            {
                if (_date.day != 31)
                {
                    _date.day++;
                }
                else
                {
                    if (_date.month != 12)
                    {
                        _date.month++;
                    }
                    else
                    {
                        _date.year++;
                        _date.month = 1;
                    }
                    _date.day = 1;
                }
            }
            else if (_date.month == 2)
            {
                if (_date.day != 28)
                {
                    _date.day++;
                }
                else
                {
                    _date.month++;
                    _date.day = 1;
                }
            }
            else
            {
                if (_date.day != 30)
                {
                    _date.day++;
                }
                else
                {
                    _date.month++;
                    _date.day = 1;
                }
            }
            _date.dayOfWeek++;
            if (_date.dayOfWeek == (DayOfWeek)7)
            {
                _date.dayOfWeek = 0;
            }
            return _date;
        }

        public static void CreateSchedule()
        {
            int matchCount = 0;
            int dayOfWeekCount = 0;
            Date _Date = new Date(2025, 3, 25, DayOfWeek.TUESDAY);
            int[] teams = new int[10];
            for (int i = 0; i < 10; i++)
            {
                teams[i] = i;
            }
            for (int day = 0; day < 48; day++)
            {
                ShuffleTeams(teams);
                for (int c = 0; c < 3; c++)
                {
                    for (int i = 0; i < 10 / 2; i++)
                    {
                        int team1 = teams[i * 2];
                        int team2 = teams[i * 2 + 1];
                        GameDirector.schedule.Add(new Schedule());
                        GameDirector.schedule[matchCount].homeTeam = (TeamName)team1;
                        GameDirector.schedule[matchCount].awayTeam = (TeamName)team2;
                        GameDirector.schedule[matchCount].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
                        GameDirector.schedule[matchCount].isEnd = false;
                        matchCount++;
                    }

                    _Date = UpdateDate(_Date);
                    dayOfWeekCount++;
                    if (dayOfWeekCount == 6)
                    {
                        _Date = UpdateDate(_Date);
                        dayOfWeekCount = 0;
                    }
                }
            }
            GameDirector.totalMatchCount = matchCount;
        }
    
        public static void CreatePostSeasonSchedule(TeamName th5, TeamName th4, TeamName th3, TeamName th2, TeamName th1)
        {
            Date _Date = new Date(GameDirector.schedule[GameDirector.totalMatchCount - 1].dates.year,
                      GameDirector.schedule[GameDirector.totalMatchCount - 1].dates.month,
                      GameDirector.schedule[GameDirector.totalMatchCount - 1].dates.day,
                      GameDirector.schedule[GameDirector.totalMatchCount - 1].dates.dayOfWeek);
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            // 와일드카드
            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[0].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[0].UpTeam = th4;
            GameDirector.postSchedule[0].DownTeam = th5;
            GameDirector.postSchedule[0].homeTeam = th4;
            GameDirector.postSchedule[0].awayTeam = th5;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[1].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[1].UpTeam = th4;
            GameDirector.postSchedule[1].DownTeam = th5;
            GameDirector.postSchedule[1].homeTeam = th5;
            GameDirector.postSchedule[1].awayTeam = th4;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            // 준플레이오프
            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[2].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[2].UpTeam = th3;
            GameDirector.postSchedule[2].homeTeam = th3;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[3].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[3].UpTeam = th3;
            GameDirector.postSchedule[3].homeTeam = th3;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[4].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[4].UpTeam = th3;
            GameDirector.postSchedule[4].awayTeam = th3;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[5].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[5].UpTeam = th3;
            GameDirector.postSchedule[5].awayTeam = th3;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[6].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[6].UpTeam = th3;
            GameDirector.postSchedule[6].homeTeam = th3;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            // 플레이오프
            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[7].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[7].UpTeam = th2;
            GameDirector.postSchedule[7].homeTeam = th2;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[8].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[8].UpTeam = th2;
            GameDirector.postSchedule[8].homeTeam = th2;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[9].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[9].UpTeam = th2;
            GameDirector.postSchedule[9].awayTeam = th2;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[10].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[10].UpTeam = th2;
            GameDirector.postSchedule[10].awayTeam = th2;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[11].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[11].UpTeam = th2;
            GameDirector.postSchedule[11].homeTeam = th2;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            // 한국시리즈
            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[12].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[12].UpTeam = th1;
            GameDirector.postSchedule[12].homeTeam = th1;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[13].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[13].UpTeam = th1;
            GameDirector.postSchedule[13].homeTeam = th1;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[14].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[14].UpTeam = th1;
            GameDirector.postSchedule[14].awayTeam = th1;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[15].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[15].UpTeam = th1;
            GameDirector.postSchedule[15].awayTeam = th1;
            _Date = UpdateDate(_Date);
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[16].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[16].UpTeam = th1;
            GameDirector.postSchedule[16].homeTeam = th1;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[17].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[17].UpTeam = th1;
            GameDirector.postSchedule[17].homeTeam = th1;
            _Date = UpdateDate(_Date);

            GameDirector.postSchedule.Add(new PostSchedule());
            GameDirector.postSchedule[18].dates = new Date(_Date.year, _Date.month, _Date.day, _Date.dayOfWeek);
            GameDirector.postSchedule[18].UpTeam = th1;
            GameDirector.postSchedule[18].homeTeam = th1;
            _Date = UpdateDate(_Date);

            for (int i = 0; i < 19; i++)
            {
                GameDirector.postSchedule[i].isEnd = false;
                GameDirector.postSchedule[i].isPass = false;
            }
        }

        public static void CreateNewGame(TeamName myTeam)
        {
            GameDirector.myTeam = myTeam;
            GameDirector.money = 1000000000;
            CreateDate();
            CreateTeam();
            CreatePlayer();
            CreateForeignCandidatePlayer();
            CreateSchedule();
            GetStockMail.M1(); // 환영 메세지
            SaveLoad.SaveData();
        }
    }

    public static class DataToString
    {
        public static string AgeToString(int born)
        {
            int currentYear = GameDirector.currentDate.year;
            int age = currentYear - born + 1;
            return age.ToString();
        }

        public static string DayOfWeekToString(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.MONDAY: return "월요일";
                case DayOfWeek.TUESDAY: return "화요일";
                case DayOfWeek.WEDNESDAY: return "수요일";
                case DayOfWeek.THURSDAY: return "목요일";
                case DayOfWeek.FRIDAY: return "금요일";
                case DayOfWeek.SATURDAY: return "토요일";
                case DayOfWeek.SUNDAY: return "일요일";
                default: return "알수없음";
            }
        }

        public static string TeamToString(TeamName team)
        {
            switch (team)
            {
                case TeamName.SAMSUNG: return "삼성 라이온즈";
                case TeamName.LOTTE: return "롯데 자이언츠";
                case TeamName.KIA: return "기아 타이거즈";
                case TeamName.KIWOOM: return "키움 히어로즈";
                case TeamName.DOOSAN: return "두산 베어스";
                case TeamName.HANWHA: return "한화 이글스";
                case TeamName.LG: return "LG 트윈스";
                case TeamName.SSG: return "SSG 랜더스";
                case TeamName.NC: return "NC 다이노스";
                case TeamName.KT: return "KT 위즈";
                default: return "알 수 없음";
            }
        }

        public static string PosToString(BatterPosition pos)
        {
            switch (pos)
            {
                case BatterPosition.DH: return "DH";
                case BatterPosition.C: return "C";
                case BatterPosition._1B: return "1B";
                case BatterPosition._2B: return "2B";
                case BatterPosition._3B: return "3B";
                case BatterPosition.SS: return "SS";
                case BatterPosition.LF: return "LF";
                case BatterPosition.CF: return "CF";
                case BatterPosition.RF: return "RF";
                default: return "알 수 없음";
            }
        }
    }
}
