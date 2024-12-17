using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

namespace MailData
{
    public class  GetValueMail
    {
        public static void VeryBigWin(int myScore, int enemyScore, TeamName EnemyTeam)
        {
            GameDirector.GetMail("압도적 대승", "감독님!\n무려 " + myScore.ToString() + ":" + enemyScore.ToString() + "으로 " + DataToString.TeamToString(EnemyTeam) + "을 격파 하셨습니다.\n앞으로도 이렇게 좋은 경기력을 보여주시면 감사하겠습니다.", "팬카페 회장");
        }
        public static void VeryBigDefeat(int myScore, int enemyScore, TeamName EnemyTeam)
        {
            GameDirector.GetMail("비참한 대패", "감독님!\n무려 " + myScore.ToString() + ":" + enemyScore.ToString() + "으로 " + DataToString.TeamToString(EnemyTeam) + "한테 패배하셨습니다.\n이런 경기력으로는 좋은 시즌을 보내기 힘들 것 같습니다.\n좀 더 발전된 경기력을 고대합니다.", "팬카페 회장");
        }
        public static void HappyBatter(string name)
        {
            GameDirector.GetMail("최고의 폼", "안녕하세요. 감독님.\n오늘 게임은 최고였습니다. 요즘 경기력도 좋고, 앞으로도 이런 모습으로 시즌을 계속 보내고 싶습니다.\n감독님과 함께라면, 우승의 꿈을 이룰 수 있을 것 같습니다.", name);
        }

        public static void SadBatter(string name)
        {
            GameDirector.GetMail("성적 문제", "안녕하세요. 감독님.\n오늘 경기는 최악이였습니다. 요즘 경기력도 좋지 않고, 재정비하는 시간이 필요할 것 같은데...\n조언 부탁드립니다.", name);
        }
    }
    public class GetStockMail
    {
        public static void M1()
        {
            GameDirector.GetMail("환영합니다!", "안녕하세요.\nKBO MANAGER 2025 게임을 플레이 해주셔서 감사합니다.\n본 게임은 개인 포트폴리오용으로 제작 되었으므로 상업적 이용이나 재배포가 불가능합니다.\n즐거운 게임 되세요!", "제작자");
        }

        public static void M2()
        {
            GameDirector.GetMail("개막전 응원", "안녕하세요.\n2025 시즌 한국프로야구 개막전이 코 앞으로 다가왔습니다.\n팀을 우승 시켜주시길 바랍니다.\n성과를 보여주세요.", "구단주");
        }

        public static void M3()
        {
            GameDirector.GetMail("권한 위임", "안녕하세요.\n당신한테 트레이드와 용병에 관한 모든 권한을 위임합니다.\n올해 이적 예산은 10억원 입니다.\n현명한 판단을 기대합니다.", "단장");
        }

        public static void M4()
        {
            GameDirector.GetMail("2군 기용 독려", "안녕하세요.\n2군에도 좋은 선수가 많으니 종종 기용해주시면 감사하겠습니다.\n팀관리 화면에서 드래그&드롭으로 교체할 수 있습니다.", "2군 감독");
        }

        public static void M5()
        {
            GameDirector.GetMail("예산 추가 지급", "안녕하세요.\n생각해보니 당신이 팀을 운영하는데 돈이 더 있으면 성적에 좋을 것 같군요.\n추가 예산 1억원을 지급해 드립니다.", "구단주");
        }

        public static void M6()
        {
            GameDirector.GetMail("올스타전 취소", "금일 열리기로 한 올스타전은 취소되었습니다.\n왜냐하면 올스타전까지 구현하기는 귀찮았습니다.", "KBO총장");
        }

        public static void M7()
        {
            GameDirector.GetMail("응원 메세지", "안녕하세요, 감독님!\n저희 딸이 많이 아픕니다. 이 팀의 우승을 볼 수 있는 마지막 기회입니다.\n올해 우승이 꼭 절실합니다.", "팬카페 회장");
        }

        public static void M8()
        {
            GameDirector.GetMail("정규 시즌 종료", "안녕하세요.\n2025 한국프로야구 정규시즌이 종료 되었습니다.", "KBO총장");
        }

        public static void M9()
        {
            GameDirector.GetMail("시즌 종료", "안녕하세요.\n2025 한국프로야구 시즌이 최종 종료 되었습니다.", "KBO총장");
        }
    }
}
