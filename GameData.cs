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
            //���� ���� ��ȭ // �Ŀ� ���� ����
            #region SAMSUNG LIONS
            SetPitcherForeign(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 1996, PitcherPosition.SP, 1, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 2000, PitcherPosition.SP, 2, 3, 4, 4);
            SetPitcherForeign(GameDirector.pitcherCount, "�Ķ�", TeamName.SAMSUNG, Hand.R, 1996, PitcherPosition.SP, 3, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "�ֿ���", TeamName.SAMSUNG, Hand.R, 1997, PitcherPosition.SP, 4, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.L, 1987, PitcherPosition.SP, 5, 1, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "�̽���L", TeamName.SAMSUNG, Hand.L, 2002, PitcherPosition.RP, 6, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�̽���R", TeamName.SAMSUNG, Hand.R, 1991, PitcherPosition.RP, 7, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 1992, PitcherPosition.RP, 8, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 1998, PitcherPosition.RP, 9, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.L, 2006, PitcherPosition.RP, 10, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 1999, PitcherPosition.RP, 11, 4, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 1990, PitcherPosition.SU, 12, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "��â��", TeamName.SAMSUNG, Hand.R, 1985, PitcherPosition.SU, 13, 2, 4, 2);
            SetPitcher(GameDirector.pitcherCount, "����ȯ", TeamName.SAMSUNG, Hand.R, 1982, PitcherPosition.CP, 14, 2, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "Ȳ����", TeamName.SAMSUNG, Hand.R, 2001, PitcherPosition.RP, 15, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "���ϴ�", TeamName.SAMSUNG, Hand.R, 1999, PitcherPosition.RP, 16, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "����", TeamName.SAMSUNG, Hand.R, 1988, PitcherPosition.RP, 17, 1, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 2005, PitcherPosition.RP, 18, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "��ȣ��", TeamName.SAMSUNG, Hand.R, 2004, PitcherPosition.RP, 19, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SAMSUNG, Hand.R, 2001, PitcherPosition.SP,  20, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.L, 2001, BatterPosition.CF, 101, 1, 3, 4);
            SetBatter(GameDirector.batterCount, "�̼���", TeamName.SAMSUNG, Hand.R, 1993, BatterPosition.RF, 102, 3, 2, 1);
            SetBatter(GameDirector.batterCount, "���ڿ�", TeamName.SAMSUNG, Hand.L, 1995, BatterPosition.LF, 103, 3, 5, 4);
            SetBatterForeign(GameDirector.batterCount, "�����", TeamName.SAMSUNG, Hand.L, 1996, BatterPosition._1B, 104, 5, 2, 1);
            SetBatter(GameDirector.batterCount, "�ں�ȣ", TeamName.SAMSUNG, Hand.R, 1986, BatterPosition.DH, 105, 4, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȣ", TeamName.SAMSUNG, Hand.R, 1985, BatterPosition.C, 106, 3, 2, 1);
            SetBatter(GameDirector.batterCount, "�迵��", TeamName.SAMSUNG, Hand.L, 2003, BatterPosition._3B, 107, 3, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.R, 2006, BatterPosition._2B, 108, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.R, 2003, BatterPosition.SS, 109, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "�����", TeamName.SAMSUNG, Hand.R, 1988, BatterPosition.RF, 110, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "�Լ�ȣ", TeamName.SAMSUNG, Hand.L, 2006, BatterPosition.CF, 111, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.L, 1994, BatterPosition._2B, 112, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�̺���", TeamName.SAMSUNG, Hand.R, 1999, BatterPosition.C, 113, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.L, 2006, BatterPosition._3B, 114, 3, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.L, 1999, BatterPosition.LF, 115, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.R, 1993, BatterPosition._2B, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SAMSUNG, Hand.R, 1992, BatterPosition._3B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�絵��", TeamName.SAMSUNG, Hand.R, 2003, BatterPosition.SS, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�輺��", TeamName.SAMSUNG, Hand.L, 1999, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���α�", TeamName.SAMSUNG, Hand.L, 1999, BatterPosition.DH, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���缺", TeamName.SAMSUNG, Hand.L, 1996, BatterPosition.C, 121, 1, 1, 1);
            #endregion
            #region LOTTE GIANTS
            SetPitcherForeign(GameDirector.pitcherCount, "��Ŀ", TeamName.LOTTE, Hand.L, 1996, PitcherPosition.SP, 1, 2, 4, 2);
            SetPitcherForeign(GameDirector.pitcherCount, "����", TeamName.LOTTE, Hand.L, 1995, PitcherPosition.SP, 2, 1, 3, 4);
            SetPitcher(GameDirector.pitcherCount, "�ڼ���", TeamName.LOTTE, Hand.R, 1995, PitcherPosition.SP, 3, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "���վ�", TeamName.LOTTE, Hand.R, 1998, PitcherPosition.SP, 4, 2, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "���κ�", TeamName.LOTTE, Hand.R, 1991, PitcherPosition.SP, 5, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "���¹�", TeamName.LOTTE, Hand.R, 1990, PitcherPosition.RP, 6, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "����", TeamName.LOTTE, Hand.R, 1987, PitcherPosition.RP, 7, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LOTTE, Hand.R, 1999, PitcherPosition.RP, 8, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "���ؿ�", TeamName.LOTTE, Hand.R, 2001, PitcherPosition.RP, 9, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "����", TeamName.LOTTE, Hand.R, 1999, PitcherPosition.RP, 10, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "���ؼ�", TeamName.LOTTE, Hand.L, 1989, PitcherPosition.RP, 11, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "���̸�", TeamName.LOTTE, Hand.R, 2005, PitcherPosition.SU, 12, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "��ö��", TeamName.LOTTE, Hand.R, 1999, PitcherPosition.SU, 13, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.LOTTE, Hand.R, 1993, PitcherPosition.CP, 14, 4, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LOTTE, Hand.L, 2005, PitcherPosition.RP, 15, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LOTTE, Hand.R, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڸ���", TeamName.LOTTE, Hand.R, 2001, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LOTTE, Hand.R, 1987, PitcherPosition.RP, 18, 1, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LOTTE, Hand.R, 1994, PitcherPosition.RP, 19, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LOTTE, Hand.R, 1993, PitcherPosition.RP, 20, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 2003, BatterPosition.CF, 101, 2, 4, 4);
            SetBatter(GameDirector.batterCount, "Ȳ����", TeamName.LOTTE, Hand.L, 1997, BatterPosition.LF, 102, 1, 3, 3);
            SetBatter(GameDirector.batterCount, "��ȣ��", TeamName.LOTTE, Hand.R, 1994, BatterPosition._3B, 103, 2, 4, 3);
            SetBatterForeign(GameDirector.batterCount, "���̿���", TeamName.LOTTE, Hand.R, 1994, BatterPosition.RF, 104, 2, 5, 4);
            SetBatter(GameDirector.batterCount, "���ؿ�", TeamName.LOTTE, Hand.R, 1986, BatterPosition.DH, 105, 2, 2, 3);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 1992, BatterPosition.C, 106, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "���¿�", TeamName.LOTTE, Hand.R, 2002, BatterPosition._1B, 107, 3, 3, 2);
            SetBatter(GameDirector.batterCount, "��¹�", TeamName.LOTTE, Hand.L, 2000, BatterPosition._2B, 108, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�ڽ¿�", TeamName.LOTTE, Hand.L, 1992, BatterPosition.SS, 109, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "����", TeamName.LOTTE, Hand.L, 1994, BatterPosition._2B, 110, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "����", TeamName.LOTTE, Hand.R, 1987, BatterPosition._1B, 111, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "��μ�", TeamName.LOTTE, Hand.R, 1988, BatterPosition._2B, 112, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�ռ���", TeamName.LOTTE, Hand.R, 2002, BatterPosition.C, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 1998, BatterPosition.SS, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.L, 1989, BatterPosition.SS, 115, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 1999, BatterPosition.C, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "��ȣ��", TeamName.LOTTE, Hand.R, 2004, BatterPosition._2B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 2005, BatterPosition._3B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 1999, BatterPosition.SS, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.L, 1994, BatterPosition.DH, 120, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.LOTTE, Hand.R, 1998, BatterPosition.RF, 121, 1, 1, 1);
            #endregion
            #region KIA TIGERS
            SetPitcherForeign(GameDirector.pitcherCount, "����", TeamName.KIA, Hand.R, 1993, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIA, Hand.L, 1988, PitcherPosition.SP, 2, 2, 4, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "�÷�", TeamName.KIA, Hand.R, 1994, PitcherPosition.SP, 3, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "����ö", TeamName.KIA, Hand.L, 2004, PitcherPosition.SP, 4, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "Ȳ����", TeamName.KIA, Hand.R, 2002, PitcherPosition.SP, 5, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.KIA, Hand.L, 1994, PitcherPosition.RP, 6, 1, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "�赵��", TeamName.KIA, Hand.R, 2000, PitcherPosition.RP, 7, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "���ؿ�", TeamName.KIA, Hand.L, 1992, PitcherPosition.RP, 8, 2, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIA, Hand.R, 2006, PitcherPosition.RP, 9, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "��ȣ��", TeamName.KIA, Hand.R, 2006, PitcherPosition.RP, 10, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIA, Hand.L, 2003, PitcherPosition.RP, 11, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIA, Hand.R, 1996, PitcherPosition.SU, 12, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIA, Hand.L, 2004, PitcherPosition.SU, 13, 1, 2, 4);
            SetPitcher(GameDirector.pitcherCount, "���ؿ�", TeamName.KIA, Hand.R, 2001, PitcherPosition.CP, 14, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "���Ǹ�", TeamName.KIA, Hand.L, 2002, PitcherPosition.SP, 15, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ӱ⿵", TeamName.KIA, Hand.R, 1993, PitcherPosition.RP, 16, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "����ǥ", TeamName.KIA, Hand.R, 1992, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "����Ź", TeamName.KIA, Hand.R, 2004, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "����ö", TeamName.KIA, Hand.R, 1998, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.KIA, Hand.L, 1991, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȣ", TeamName.KIA, Hand.R, 1995, BatterPosition.SS, 101, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.L, 1989, BatterPosition.RF, 102, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "�赵��", TeamName.KIA, Hand.R, 2003, BatterPosition._3B, 103, 5, 4, 4);
            SetBatterForeign(GameDirector.batterCount, "�����", TeamName.KIA, Hand.R, 1991, BatterPosition._1B, 104, 5, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.L, 1984, BatterPosition.DH, 105, 3, 3, 3);
            SetBatter(GameDirector.batterCount, "���±�", TeamName.KIA, Hand.R, 1989, BatterPosition.C, 106, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "�輱��", TeamName.KIA, Hand.R, 1989, BatterPosition._2B, 107, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "�ֿ���", TeamName.KIA, Hand.L, 1997, BatterPosition.CF, 108, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "��â��", TeamName.KIA, Hand.R, 1991, BatterPosition.LF, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�̿켺", TeamName.KIA, Hand.R, 1994, BatterPosition._1B, 110, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "����â", TeamName.KIA, Hand.L, 1989, BatterPosition._2B, 111, 1, 3, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.R, 2000, BatterPosition._1B, 112, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "�ѽ���", TeamName.KIA, Hand.R, 1994, BatterPosition.C, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "Ȳ����", TeamName.KIA, Hand.R, 1996, BatterPosition._1B, 114, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "���ؼ�", TeamName.KIA, Hand.R, 1999, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ڹ�", TeamName.KIA, Hand.R, 2001, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "ȫ��ǥ", TeamName.KIA, Hand.R, 2000, BatterPosition.DH, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.L, 1996, BatterPosition._2B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.L, 1989, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.L, 1999, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIA, Hand.L, 1998, BatterPosition.LF, 121, 1, 1, 1);
            #endregion
            #region KIWOOM HEROES
            SetPitcherForeign(GameDirector.pitcherCount, "��������", TeamName.KIWOOM, Hand.L, 1995, PitcherPosition.SP, 1, 2, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "�Ͽ���", TeamName.KIWOOM, Hand.R, 1995, PitcherPosition.SP, 2, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.R, 2005, PitcherPosition.SP, 3, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�輱��", TeamName.KIWOOM, Hand.R, 1991, PitcherPosition.SP, 4, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.L, 2006, PitcherPosition.SP, 5, 3, 5, 2);
            SetPitcher(GameDirector.pitcherCount, "�迬��", TeamName.KIWOOM, Hand.R, 2004, PitcherPosition.RP, 6, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�輭��", TeamName.KIWOOM, Hand.R, 2006, PitcherPosition.RP, 7, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "����ǥ", TeamName.KIWOOM, Hand.R, 2005, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.L, 2006, PitcherPosition.RP, 9, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.R, 1998, PitcherPosition.RP, 10, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.R, 1987, PitcherPosition.RP, 11, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.R, 1999, PitcherPosition.SU, 12, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.KIWOOM, Hand.R, 1994, PitcherPosition.SU, 13, 3, 4, 2);
            SetPitcher(GameDirector.pitcherCount, "�ֽ¿�", TeamName.KIWOOM, Hand.R, 2000, PitcherPosition.CP, 14, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.L, 2001, PitcherPosition.RP, 15, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "���ι�", TeamName.KIWOOM, Hand.R, 2000, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.R, 1991, PitcherPosition.RP, 17, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.L, 2005, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KIWOOM, Hand.R, 1988, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�̰���", TeamName.KIWOOM, Hand.R, 2001, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIWOOM, Hand.R, 2001, BatterPosition.CF, 101, 1, 4, 4);
            SetBatter(GameDirector.batterCount, "�ۼ���", TeamName.KIWOOM, Hand.R, 1996, BatterPosition._3B, 102, 2, 5, 3);
            SetBatterForeign(GameDirector.batterCount, "Ǫ�̱�", TeamName.KIWOOM, Hand.R, 1990, BatterPosition.RF, 103, 4, 3, 2);
            SetBatterForeign(GameDirector.batterCount, "ī��׽�", TeamName.KIWOOM, Hand.R, 1997, BatterPosition.LF, 104, 5, 2, 1);
            SetBatter(GameDirector.batterCount, "����ȯ", TeamName.KIWOOM, Hand.R, 1988, BatterPosition._1B, 105, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIWOOM, Hand.L, 1995, BatterPosition.SS, 106, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "�����", TeamName.KIWOOM, Hand.R, 2004, BatterPosition.C, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�̿��", TeamName.KIWOOM, Hand.R, 1985, BatterPosition.DH, 108, 1, 2, 5);
            SetBatter(GameDirector.batterCount, "����", TeamName.KIWOOM, Hand.R, 2001, BatterPosition._2B, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�����", TeamName.KIWOOM, Hand.L, 1997, BatterPosition.LF, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�����", TeamName.KIWOOM, Hand.L, 1996, BatterPosition.DH, 111, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "���翵", TeamName.KIWOOM, Hand.R, 2002, BatterPosition.RF, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�赿��", TeamName.KIWOOM, Hand.R, 2004, BatterPosition.C, 113, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIWOOM, Hand.R, 1993, BatterPosition.C, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���¿�", TeamName.KIWOOM, Hand.L, 2006, BatterPosition._2B, 115, 1, 3, 2);
            SetBatter(GameDirector.batterCount, "�躴��", TeamName.KIWOOM, Hand.R, 2001, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIWOOM, Hand.R, 2006, BatterPosition._3B, 117, 3, 2, 2);
            SetBatter(GameDirector.batterCount, "�̿���", TeamName.KIWOOM, Hand.R, 1986, BatterPosition._1B, 118, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�赿��", TeamName.KIWOOM, Hand.R, 1990, BatterPosition.DH, 119, 2, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȫ", TeamName.KIWOOM, Hand.L, 2001, BatterPosition.LF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KIWOOM, Hand.R, 1989, BatterPosition.CF, 121, 1, 1, 1);
            #endregion
            #region DOOSAN BEARS
            SetPitcher(GameDirector.pitcherCount, "����", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcherForeign(GameDirector.pitcherCount, "���", TeamName.DOOSAN, Hand.L, 1994, PitcherPosition.SP, 2, 2, 5, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "��ġ", TeamName.DOOSAN, Hand.R, 1994, PitcherPosition.SP, 3, 3, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "�ֿ���", TeamName.DOOSAN, Hand.R, 1994, PitcherPosition.SP, 4, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�̿���", TeamName.DOOSAN, Hand.R, 1997, PitcherPosition.SP, 5, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "��α�", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.RP, 6, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "Ȳ��õ", TeamName.DOOSAN, Hand.L, 2006, PitcherPosition.RP, 7, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�ֽ¿�", TeamName.DOOSAN, Hand.L, 2001, PitcherPosition.RP, 8, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "ȫ����", TeamName.DOOSAN, Hand.R, 1992, PitcherPosition.RP, 9, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.DOOSAN, Hand.R, 2002, PitcherPosition.RP, 10, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "��ġ��", TeamName.DOOSAN, Hand.R, 1998, PitcherPosition.RP, 11, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.DOOSAN, Hand.L, 2001, PitcherPosition.SU, 12, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�̺���", TeamName.DOOSAN, Hand.L, 2003, PitcherPosition.SU, 13, 3, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "���ÿ�", TeamName.DOOSAN, Hand.R, 2005, PitcherPosition.CP, 14, 4, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "����", TeamName.DOOSAN, Hand.R, 2000, PitcherPosition.RP, 15, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�赵��", TeamName.DOOSAN, Hand.R, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "����", TeamName.DOOSAN, Hand.R, 1993, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڽ���", TeamName.DOOSAN, Hand.R, 1999, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "��ȣ��", TeamName.DOOSAN, Hand.L, 1998, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.L, 1990, BatterPosition.CF, 101, 1, 3, 3);
            SetBatterForeign(GameDirector.batterCount, "���̺�", TeamName.DOOSAN, Hand.L, 1992, BatterPosition.RF, 102, 2, 4, 3);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.R, 1987, BatterPosition.C, 103, 2, 5, 4);
            SetBatter(GameDirector.batterCount, "����ȯ", TeamName.DOOSAN, Hand.L, 1988, BatterPosition.LF, 104, 4, 3, 2);
            SetBatter(GameDirector.batterCount, "����ȣ", TeamName.DOOSAN, Hand.R, 1994, BatterPosition._1B, 105, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "�缮ȯ", TeamName.DOOSAN, Hand.R, 1991, BatterPosition.DH, 106, 3, 2, 2);
            SetBatter(GameDirector.batterCount, "���ؼ�", TeamName.DOOSAN, Hand.R, 2006, BatterPosition._2B, 107, 2, 4, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.R, 1998, BatterPosition._3B, 108, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.R, 1999, BatterPosition.SS, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "��⿬", TeamName.DOOSAN, Hand.R, 1997, BatterPosition.C, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.R, 2000, BatterPosition.SS, 111, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.L, 1993, BatterPosition.LF, 112, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.R, 2005, BatterPosition._2B, 113, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.L, 1994, BatterPosition.LF, 114, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "ȫ��ȣ", TeamName.DOOSAN, Hand.L, 1997, BatterPosition.RF, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "��μ�", TeamName.DOOSAN, Hand.R, 2004, BatterPosition.CF, 116, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "�����", TeamName.DOOSAN, Hand.R, 1996, BatterPosition._1B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ڰ��", TeamName.DOOSAN, Hand.R, 1996, BatterPosition._3B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.R, 2005, BatterPosition.SS, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.DOOSAN, Hand.L, 1999, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ڹ���", TeamName.DOOSAN, Hand.R, 2002, BatterPosition.C, 121, 1, 1, 1);
            #endregion
            #region HANHWA EAGLES
            SetPitcherForeign(GameDirector.pitcherCount, "���̽�", TeamName.HANWHA, Hand.R, 1996, PitcherPosition.SP, 1, 2, 4, 5);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.HANWHA, Hand.L, 1987, PitcherPosition.SP, 2, 2, 5, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "����", TeamName.HANWHA, Hand.R, 1994, PitcherPosition.SP, 3, 4, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.HANWHA, Hand.R, 1996, PitcherPosition.SP, 4, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.HANWHA, Hand.R, 2003, PitcherPosition.SP, 5, 5, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "��Կ�", TeamName.HANWHA, Hand.R, 2002, PitcherPosition.RP, 6, 2, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.HANWHA, Hand.R, 2006, PitcherPosition.RP, 7, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "Ȳ�ؼ�", TeamName.HANWHA, Hand.L, 2005, PitcherPosition.RP, 8, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�̹ο�", TeamName.HANWHA, Hand.R, 1993, PitcherPosition.RP, 9, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ǹα�", TeamName.HANWHA, Hand.L, 2006, PitcherPosition.RP, 10, 1, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "�輭��", TeamName.HANWHA, Hand.R, 2004, PitcherPosition.RP, 11, 4, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ѽ���", TeamName.HANWHA, Hand.R, 1993, PitcherPosition.SU, 12, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڻ��", TeamName.HANWHA, Hand.R, 1994, PitcherPosition.SU, 13, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.HANWHA, Hand.R, 1992, PitcherPosition.CP, 14, 3, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.HANWHA, Hand.R, 1994, PitcherPosition.RP, 15, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.HANWHA, Hand.L, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "���ȯ", TeamName.HANWHA, Hand.R, 1987, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.HANWHA, Hand.R, 1990, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "��μ�", TeamName.HANWHA, Hand.R, 1999, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "���¾�", TeamName.HANWHA, Hand.R, 1990, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "Ȳ����", TeamName.HANWHA, Hand.L, 1999, BatterPosition._2B, 101, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�ɿ���", TeamName.HANWHA, Hand.R, 1995, BatterPosition.SS, 102, 1, 2, 2);
            SetBatterForeign(GameDirector.batterCount, "�÷θ���", TeamName.HANWHA, Hand.L, 1997, BatterPosition.CF, 103, 3, 4, 4);
            SetBatter(GameDirector.batterCount, "���ȯ", TeamName.HANWHA, Hand.R, 2000, BatterPosition.DH, 104, 4, 3, 1);
            SetBatter(GameDirector.batterCount, "ä����", TeamName.HANWHA, Hand.R, 1990, BatterPosition._1B, 105, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "������", TeamName.HANWHA, Hand.R, 1997, BatterPosition.RF, 106, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.HANWHA, Hand.R, 1989, BatterPosition.C, 107, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.HANWHA, Hand.L, 2004, BatterPosition._3B, 108, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "�Ǳ���", TeamName.HANWHA, Hand.L, 1997, BatterPosition.LF, 109, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.HANWHA, Hand.R, 2006, BatterPosition.C, 110, 2, 4, 1);
            SetBatter(GameDirector.batterCount, "��ġȫ", TeamName.HANWHA, Hand.R, 1990, BatterPosition._1B, 111, 2, 2, 4);
            SetBatter(GameDirector.batterCount, "�̵���", TeamName.HANWHA, Hand.R, 1996, BatterPosition.SS, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�̿���", TeamName.HANWHA, Hand.R, 1999, BatterPosition.CF, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���¿�", TeamName.HANWHA, Hand.R, 1997, BatterPosition.RF, 114, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "�����", TeamName.HANWHA, Hand.R, 1988, BatterPosition.C, 115, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�Ѱ��", TeamName.HANWHA, Hand.L, 1998, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȯ", TeamName.HANWHA, Hand.L, 1994, BatterPosition._1B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���", TeamName.HANWHA, Hand.R, 2000, BatterPosition._2B, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���ΰ�", TeamName.HANWHA, Hand.R, 2000, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "����", TeamName.HANWHA, Hand.R, 2003, BatterPosition.LF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȣ", TeamName.HANWHA, Hand.L, 2000, BatterPosition.RF, 121, 1, 1, 1);
            #endregion
            #region LG TWINS
            SetPitcherForeign(GameDirector.pitcherCount, "����������", TeamName.LG, Hand.R, 1995, PitcherPosition.SP, 1, 4, 4, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "ġ���뽺", TeamName.LG, Hand.R, 1993, PitcherPosition.SP, 2, 3, 3, 4);
            SetPitcher(GameDirector.pitcherCount, "���ֿ�", TeamName.LG, Hand.L, 1998, PitcherPosition.SP, 3, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.R, 1992, PitcherPosition.SP, 4, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "�ڽÿ�", TeamName.LG, Hand.R, 2006, PitcherPosition.SP, 5, 2, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "�迵��", TeamName.LG, Hand.R, 2005, PitcherPosition.RP, 6, 4, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "���쿵", TeamName.LG, Hand.R, 1999, PitcherPosition.RP, 7, 2, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "�̿���", TeamName.LG, Hand.L, 1992, PitcherPosition.RP, 8, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�Դ���", TeamName.LG, Hand.L, 1995, PitcherPosition.RP, 9, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.L, 1994, PitcherPosition.RP, 10, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.R, 1997, PitcherPosition.RP, 11, 3, 4, 2);
            SetPitcher(GameDirector.pitcherCount, "�谭��", TeamName.LG, Hand.R, 1988, PitcherPosition.SU, 12, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.R, 1985, PitcherPosition.SU, 13, 2, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.R, 1995, PitcherPosition.CP, 14, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.R, 1999, PitcherPosition.RP, 15, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "��ä��", TeamName.LG, Hand.L, 1995, PitcherPosition.RP, 16, 1, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "�ڸ��", TeamName.LG, Hand.R, 2004, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ֿ���", TeamName.LG, Hand.R, 2002, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "���ؿ�", TeamName.LG, Hand.L, 2000, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.LG, Hand.R, 1998, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "ȫâ��", TeamName.LG, Hand.L, 1993, BatterPosition.RF, 101, 1, 5, 5);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.L, 1997, BatterPosition.LF, 102, 1, 2, 1);
            SetBatterForeign(GameDirector.batterCount, "����ƾ", TeamName.LG, Hand.R, 1993, BatterPosition._1B, 103, 3, 4, 4);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.R, 2000, BatterPosition._3B, 104, 3, 3, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.L, 1988, BatterPosition.DH, 105, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "�ڵ���", TeamName.LG, Hand.R, 1990, BatterPosition.C, 106, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "����ȯ", TeamName.LG, Hand.R, 1990, BatterPosition.SS, 107, 2, 3, 3);
            SetBatter(GameDirector.batterCount, "���ع�", TeamName.LG, Hand.L, 1990, BatterPosition.CF, 108, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "�Ź���", TeamName.LG, Hand.L, 1996, BatterPosition._2B, 109, 1, 2, 4);
            SetBatter(GameDirector.batterCount, "�ڰ���", TeamName.LG, Hand.L, 2006, BatterPosition.CF, 110, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "�߼���", TeamName.LG, Hand.R, 2006, BatterPosition.RF, 111, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.R, 1997, BatterPosition.SS, 112, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "��μ�", TeamName.LG, Hand.R, 1998, BatterPosition._3B, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.R, 2004, BatterPosition.CF, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���Ѹ�", TeamName.LG, Hand.R, 2006, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���ּ�", TeamName.LG, Hand.R, 1998, BatterPosition._1B, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�̿���", TeamName.LG, Hand.L, 2002, BatterPosition.SS, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.R, 1999, BatterPosition._2B, 118, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�輺��", TeamName.LG, Hand.R, 2000, BatterPosition._3B, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.LG, Hand.L, 1996, BatterPosition.CF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ֿ���", TeamName.LG, Hand.R, 2003, BatterPosition.DH, 121, 1, 1, 1);
            #endregion
            #region SSG LANDERS
            SetPitcher(GameDirector.pitcherCount, "�豤��", TeamName.SSG, Hand.L, 1988, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcherForeign(GameDirector.pitcherCount, "ȭ��Ʈ", TeamName.SSG, Hand.R, 1994, PitcherPosition.SP, 2, 3, 2, 2);
            SetPitcherForeign(GameDirector.pitcherCount, "�ش���", TeamName.SSG, Hand.R, 1994, PitcherPosition.SP, 3, 4, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SSG, Hand.R, 1991, PitcherPosition.SP, 4, 1, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "�ۿ���", TeamName.SSG, Hand.R, 2004, PitcherPosition.SP, 5, 1, 1, 2);
            SetPitcher(GameDirector.pitcherCount, "�ֹ���", TeamName.SSG, Hand.R, 1999, PitcherPosition.RP, 6, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SSG, Hand.R, 2002, PitcherPosition.RP, 7, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ѵμ�", TeamName.SSG, Hand.L, 1997, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SSG, Hand.R, 1992, PitcherPosition.RP, 9, 3, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "�̷ο�", TeamName.SSG, Hand.R, 2004, PitcherPosition.RP, 10, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SSG, Hand.R, 2002, PitcherPosition.RP, 11, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.SSG, Hand.R, 1984, PitcherPosition.SU, 12, 1, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "���", TeamName.SSG, Hand.R, 1999, PitcherPosition.SU, 13, 3, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "���¿�", TeamName.SSG, Hand.R, 1989, PitcherPosition.CP, 14, 2, 3, 2);
            SetPitcher(GameDirector.pitcherCount, "��°�", TeamName.SSG, Hand.L, 2000, PitcherPosition.RP, 15, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.SSG, Hand.R, 2002, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "��ǿ�", TeamName.SSG, Hand.L, 2002, PitcherPosition.RP, 17, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SSG, Hand.L, 1996, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڽ���", TeamName.SSG, Hand.L, 2001, PitcherPosition.RP, 19, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.SSG, Hand.R, 1998, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ڼ���", TeamName.SSG, Hand.R, 1998, BatterPosition.SS, 101, 1, 3, 3);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.L, 2003, BatterPosition._2B, 102, 1, 2, 2);
            SetBatterForeign(GameDirector.batterCount, "�������", TeamName.SSG, Hand.R, 1991, BatterPosition.LF, 103, 4, 5, 5);
            SetBatter(GameDirector.batterCount, "����", TeamName.SSG, Hand.R, 1987, BatterPosition._3B, 104, 5, 3, 4);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R,1989, BatterPosition.RF, 105, 3, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R, 1986, BatterPosition.C, 106, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "����ȯ", TeamName.SSG, Hand.R, 2005, BatterPosition.DH, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R, 1999, BatterPosition.CF, 108, 1, 3, 3);
            SetBatter(GameDirector.batterCount, "�����", TeamName.SSG, Hand.R, 2002, BatterPosition._1B, 109, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R, 2006, BatterPosition.C, 110, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "���°�", TeamName.SSG, Hand.R, 1991, BatterPosition.RF, 111, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�輺��", TeamName.SSG, Hand.R, 1987, BatterPosition._2B, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȸ", TeamName.SSG, Hand.R, 2001, BatterPosition.C, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R, 1990, BatterPosition.CF, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�Ź���", TeamName.SSG, Hand.R, 1998, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R, 1997, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���ؿ�", TeamName.SSG, Hand.R, 1999, BatterPosition._2B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ֻ��", TeamName.SSG, Hand.L, 1999, BatterPosition.RF, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.R, 2002, BatterPosition.CF, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�̿���", TeamName.SSG, Hand.R, 2006, BatterPosition.CF, 120, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.SSG, Hand.L, 1998, BatterPosition.LF, 121, 1, 1, 1);
            #endregion 
            #region NC DINOS
            SetPitcherForeign(GameDirector.pitcherCount, "��Ʈ", TeamName.NC, Hand.L, 1992, PitcherPosition.SP, 1, 3, 5, 5);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.NC, Hand.R, 1990, PitcherPosition.SP, 2, 2, 3, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "���ϸ�", TeamName.NC, Hand.R, 1996, PitcherPosition.SP, 3, 4, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�Ź���", TeamName.NC, Hand.R, 1999, PitcherPosition.SP, 4, 1, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.NC, Hand.R, 1999, PitcherPosition.SP, 5, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ֿ켮", TeamName.NC, Hand.R, 2005, PitcherPosition.RP, 6, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "����ȣ", TeamName.NC, Hand.R, 1998, PitcherPosition.RP, 7, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�����", TeamName.NC, Hand.R, 2001, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.NC, Hand.R, 2006, PitcherPosition.RP, 9, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "����ȣ", TeamName.NC, Hand.L, 2000, PitcherPosition.RP, 10, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.NC, Hand.R, 1996, PitcherPosition.RP, 11, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "���翭", TeamName.NC, Hand.R, 1996, PitcherPosition.SU, 12, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�迵��", TeamName.NC, Hand.R, 2000, PitcherPosition.SU, 13, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "�̿���", TeamName.NC, Hand.R, 1989, PitcherPosition.CP, 14, 2, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "�ּ���", TeamName.NC, Hand.L, 1997, PitcherPosition.RP, 15, 3, 1, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.NC, Hand.R, 1999, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.NC, Hand.R, 2004, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�̼���", TeamName.NC, Hand.R, 2006, PitcherPosition.RP, 18, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.NC, Hand.R, 2004, PitcherPosition.RP, 19, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڵ���", TeamName.NC, Hand.R, 1999, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�ڹο�", TeamName.NC, Hand.L, 1993, BatterPosition._2B, 101, 1, 4, 3);
            SetBatter(GameDirector.batterCount, "�վƼ�", TeamName.NC, Hand.R, 1988, BatterPosition.RF, 102, 1, 4, 4);
            SetBatter(GameDirector.batterCount, "õ��ȯ", TeamName.NC, Hand.R, 1994, BatterPosition.LF, 103, 2, 2, 1);
            SetBatterForeign(GameDirector.batterCount, "���̺�", TeamName.NC, Hand.R, 1991, BatterPosition._1B, 104, 5, 1, 1);
            SetBatter(GameDirector.batterCount, "����", TeamName.NC, Hand.R, 1990, BatterPosition.DH, 105, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.R, 2002, BatterPosition.SS, 106, 2, 3, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.R, 2005, BatterPosition._3B, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�ڼ���", TeamName.NC, Hand.L, 1990, BatterPosition.C, 108, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.L, 2000, BatterPosition.CF, 109, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "�輺��", TeamName.NC, Hand.R, 1993, BatterPosition.RF, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.L, 1993, BatterPosition._1B, 111, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "��ȣö", TeamName.NC, Hand.R, 1996, BatterPosition._2B, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "����ȯ", TeamName.NC, Hand.R, 2001, BatterPosition.DH, 113, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���Ѻ�", TeamName.NC, Hand.R, 2001, BatterPosition.SS, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���߿�", TeamName.NC, Hand.R, 1995, BatterPosition.C, 115, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.R, 1999, BatterPosition.C, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.R, 1996, BatterPosition._1B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "���ֿ�", TeamName.NC, Hand.S, 2002, BatterPosition.SS, 118, 1, 2, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.NC, Hand.L, 2000, BatterPosition._1B, 119, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�۽�ȯ", TeamName.NC, Hand.R, 2000, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�Ѽ���", TeamName.NC, Hand.L, 1994, BatterPosition.CF, 121, 1, 1, 2);
            #endregion
            #region KT WIZ
            SetPitcherForeign(GameDirector.pitcherCount, "���ٽ�", TeamName.KT, Hand.R, 1990, PitcherPosition.SP, 1, 3, 4, 4);
            SetPitcher(GameDirector.pitcherCount, "��ǥ", TeamName.KT, Hand.R, 1991, PitcherPosition.SP, 2, 3, 4, 3);
            SetPitcherForeign(GameDirector.pitcherCount, "���̼���", TeamName.KT, Hand.L, 1996, PitcherPosition.SP, 3, 3, 3, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KT, Hand.L, 2001, PitcherPosition.SP, 4, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "��û��", TeamName.KT, Hand.R, 2005, PitcherPosition.SP, 5, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KT, Hand.R, 2004, PitcherPosition.RP, 6, 1, 3, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڰǿ�", TeamName.KT, Hand.R, 2006, PitcherPosition.RP, 7, 1, 5, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KT, Hand.L, 1997, PitcherPosition.RP, 8, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KT, Hand.R, 2001, PitcherPosition.RP, 9, 2, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KT, Hand.R, 1995, PitcherPosition.RP, 10, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ֱ�", TeamName.KT, Hand.R, 1995, PitcherPosition.RP, 11, 2, 2, 2);
            SetPitcher(GameDirector.pitcherCount, "��Թ�", TeamName.KT, Hand.R, 1985, PitcherPosition.SU, 12, 1, 2, 3);
            SetPitcher(GameDirector.pitcherCount, "�赿��", TeamName.KT, Hand.R, 2006, PitcherPosition.SU, 13, 3, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ڿ���", TeamName.KT, Hand.R, 2003, PitcherPosition.CP, 14, 3, 4, 3);
            SetPitcher(GameDirector.pitcherCount, "������", TeamName.KT, Hand.R, 1995, PitcherPosition.RP, 15, 3, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�ֵ�ȯ", TeamName.KT, Hand.R, 1989, PitcherPosition.RP, 16, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "��äȣ", TeamName.KT, Hand.R, 1998, PitcherPosition.RP, 17, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "�յ���", TeamName.KT, Hand.R, 2001, PitcherPosition.RP, 18, 1, 1, 1);
            SetPitcher(GameDirector.pitcherCount, "��μ�", TeamName.KT, Hand.R, 1992, PitcherPosition.RP, 19, 1, 2, 1);
            SetPitcher(GameDirector.pitcherCount, "�ֵ�ȯ", TeamName.KT, Hand.R, 1989, PitcherPosition.RP, 20, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�����", TeamName.KT, Hand.L, 1995, BatterPosition.LF, 101, 1, 3, 3);
            SetBatterForeign(GameDirector.batterCount, "���Ͻ�", TeamName.KT, Hand.S, 1990, BatterPosition.RF, 102, 4, 4, 4);
            SetBatter(GameDirector.batterCount, "������", TeamName.KT, Hand.R, 1992, BatterPosition._2B, 103, 2, 2, 2);
            SetBatter(GameDirector.batterCount, "����ȣ", TeamName.KT, Hand.L, 1999, BatterPosition.C, 104, 4, 3, 4);
            SetBatter(GameDirector.batterCount, "����ö", TeamName.KT, Hand.R, 1991, BatterPosition._1B, 105, 2, 2, 1);
            SetBatter(GameDirector.batterCount, "Ȳ���", TeamName.KT, Hand.R, 1987, BatterPosition.DH, 106, 3, 2, 3);
            SetBatter(GameDirector.batterCount, "������", TeamName.KT, Hand.R, 1995, BatterPosition.CF, 107, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "����", TeamName.KT, Hand.R, 1990, BatterPosition.SS, 108, 1, 2, 3);
            SetBatter(GameDirector.batterCount, "����", TeamName.KT, Hand.R, 1990, BatterPosition._3B, 109, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "�ڹμ�", TeamName.KT, Hand.R, 2006, BatterPosition.CF, 110, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KT, Hand.L, 1986, BatterPosition._1B, 111, 3, 1, 1);
            SetBatter(GameDirector.batterCount, "õ��ȣ", TeamName.KT, Hand.L, 1997, BatterPosition._2B, 112, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KT, Hand.L, 1993, BatterPosition.CF, 113, 1, 2, 2);
            SetBatter(GameDirector.batterCount, "������", TeamName.KT, Hand.R, 1999, BatterPosition.C, 114, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�强��", TeamName.KT, Hand.R, 1990, BatterPosition.C, 115, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "���ؿ�", TeamName.KT, Hand.R, 1995, BatterPosition.SS, 116, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "��ȣ��", TeamName.KT, Hand.L, 1995, BatterPosition._2B, 117, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "�۹μ�", TeamName.KT, Hand.R, 1991, BatterPosition.CF, 118, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "��ġ��", TeamName.KT, Hand.L, 1998, BatterPosition.LF, 119, 1, 1, 2);
            SetBatter(GameDirector.batterCount, "�Ź���", TeamName.KT, Hand.L, 2002, BatterPosition.RF, 120, 1, 1, 1);
            SetBatter(GameDirector.batterCount, "������", TeamName.KT, Hand.L, 1999, BatterPosition.DH, 121, 1, 1, 1);
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
            string name = "��";
            switch (nation) {
                case Nation.USA: // �̱�
                    if (DecideName == 0) { name = "���̽�"; }
                    else if (DecideName == 1) { name = "����"; }
                    else if (DecideName == 2) { name = "��������"; }
                    else if (DecideName == 3) { name = "����"; }
                    else if (DecideName == 4) { name = "����"; }
                    else if (DecideName == 5) { name = "���̺�"; }
                    else if (DecideName == 6) { name = "�з�"; }
                    else if (DecideName == 7) { name = "����"; }
                    else if (DecideName == 8) { name = "����"; }
                    else if (DecideName == 9) { name = "���Ϸ�"; }
                    else if (DecideName == 10) { name = "�ȵ�����"; }
                    break;
                case Nation.Cuba: // ���
                    if (DecideName == 0) { name = "���߷���"; }
                    else if (DecideName == 1) { name = "�Է���"; }
                    else if (DecideName == 2) { name = "���"; }
                    else if (DecideName == 3) { name = "��Ƽ������"; }
                    else if (DecideName == 4) { name = "������"; }
                    else if (DecideName == 5) { name = "����λ�"; }
                    else if (DecideName == 6) { name = "������"; }
                    else if (DecideName == 7) { name = "���ְ���"; }
                    else if (DecideName == 8) { name = "�η���"; }
                    else if (DecideName == 9) { name = "�θ޷�"; }
                    else if (DecideName == 10) { name = "��̷���"; }
                    break;
                case Nation.Venezuela: // ���׼�����
                    if (DecideName == 0) { name = "������"; }
                    else if (DecideName == 1) { name = "���̵����"; }
                    else if (DecideName == 2) { name = "���"; }
                    else if (DecideName == 3) { name = "ī���ν�"; }
                    else if (DecideName == 4) { name = "ī�극��"; }
                    else if (DecideName == 5) { name = "�䷹��"; }
                    else if (DecideName == 6) { name = "��ƼƮ"; }
                    else if (DecideName == 7) { name = "������"; }
                    else if (DecideName == 8) { name = "�𷹳�"; }
                    else if (DecideName == 9) { name = "�������"; }
                    else if (DecideName == 10) { name = "�����"; }
                    break;
                case Nation.Japan: // �Ϻ�
                    if (DecideName == 0) { name = "�ƿ�"; }
                    else if (DecideName == 1) { name = "�Ϸ���"; }
                    else if (DecideName == 2) { name = "�ƿ���"; }
                    else if (DecideName == 3) { name = "�ƻ���"; }
                    else if (DecideName == 4) { name = "��"; }
                    else if (DecideName == 5) { name = "�̳���"; }
                    else if (DecideName == 6) { name = "������"; }
                    else if (DecideName == 7) { name = "���츶"; }
                    else if (DecideName == 8) { name = "����Ÿ"; }
                    else if (DecideName == 9) { name = "����Ű"; }
                    else if (DecideName == 10) { name = "��Ű"; }
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
                int j = UnityEngine.Random.Range(0, 10);  // �������� ���� �ε����� ����
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

            // ���ϵ�ī��
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

            // ���÷��̿���
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

            // �÷��̿���
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

            // �ѱ��ø���
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
            GetStockMail.M1(); // ȯ�� �޼���
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
                case DayOfWeek.MONDAY: return "������";
                case DayOfWeek.TUESDAY: return "ȭ����";
                case DayOfWeek.WEDNESDAY: return "������";
                case DayOfWeek.THURSDAY: return "�����";
                case DayOfWeek.FRIDAY: return "�ݿ���";
                case DayOfWeek.SATURDAY: return "�����";
                case DayOfWeek.SUNDAY: return "�Ͽ���";
                default: return "�˼�����";
            }
        }

        public static string TeamToString(TeamName team)
        {
            switch (team)
            {
                case TeamName.SAMSUNG: return "�Ｚ ���̿���";
                case TeamName.LOTTE: return "�Ե� ���̾���";
                case TeamName.KIA: return "��� Ÿ�̰���";
                case TeamName.KIWOOM: return "Ű�� �������";
                case TeamName.DOOSAN: return "�λ� ���";
                case TeamName.HANWHA: return "��ȭ �̱۽�";
                case TeamName.LG: return "LG Ʈ����";
                case TeamName.SSG: return "SSG ������";
                case TeamName.NC: return "NC ���̳뽺";
                case TeamName.KT: return "KT ����";
                default: return "�� �� ����";
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
                default: return "�� �� ����";
            }
        }
    }
}
