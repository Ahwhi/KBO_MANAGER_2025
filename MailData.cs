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
            GameDirector.GetMail("�е��� ���", "������!\n���� " + myScore.ToString() + ":" + enemyScore.ToString() + "���� " + DataToString.TeamToString(EnemyTeam) + "�� ���� �ϼ̽��ϴ�.\n�����ε� �̷��� ���� ������ �����ֽø� �����ϰڽ��ϴ�.", "��ī�� ȸ��");
        }
        public static void VeryBigDefeat(int myScore, int enemyScore, TeamName EnemyTeam)
        {
            GameDirector.GetMail("������ ����", "������!\n���� " + myScore.ToString() + ":" + enemyScore.ToString() + "���� " + DataToString.TeamToString(EnemyTeam) + "���� �й��ϼ̽��ϴ�.\n�̷� �������δ� ���� ������ ������ ���� �� �����ϴ�.\n�� �� ������ ������ ����մϴ�.", "��ī�� ȸ��");
        }
        public static void HappyBatter(string name)
        {
            GameDirector.GetMail("�ְ��� ��", "�ȳ��ϼ���. ������.\n���� ������ �ְ����ϴ�. ���� ���µ� ����, �����ε� �̷� ������� ������ ��� ������ �ͽ��ϴ�.\n�����԰� �Բ����, ����� ���� �̷� �� ���� �� �����ϴ�.", name);
        }

        public static void SadBatter(string name)
        {
            GameDirector.GetMail("���� ����", "�ȳ��ϼ���. ������.\n���� ���� �־��̿����ϴ�. ���� ���µ� ���� �ʰ�, �������ϴ� �ð��� �ʿ��� �� ������...\n���� ��Ź�帳�ϴ�.", name);
        }
    }
    public class GetStockMail
    {
        public static void M1()
        {
            GameDirector.GetMail("ȯ���մϴ�!", "�ȳ��ϼ���.\nKBO MANAGER 2025 ������ �÷��� ���ּż� �����մϴ�.\n�� ������ ���� ��Ʈ������������ ���� �Ǿ����Ƿ� ����� �̿��̳� ������� �Ұ����մϴ�.\n��ſ� ���� �Ǽ���!", "������");
        }

        public static void M2()
        {
            GameDirector.GetMail("������ ����", "�ȳ��ϼ���.\n2025 ���� �ѱ����ξ߱� �������� �� ������ �ٰ��Խ��ϴ�.\n���� ��� �����ֽñ� �ٶ��ϴ�.\n������ �����ּ���.", "������");
        }

        public static void M3()
        {
            GameDirector.GetMail("���� ����", "�ȳ��ϼ���.\n������� Ʈ���̵�� �뺴�� ���� ��� ������ �����մϴ�.\n���� ���� ������ 10��� �Դϴ�.\n������ �Ǵ��� ����մϴ�.", "����");
        }

        public static void M4()
        {
            GameDirector.GetMail("2�� ��� ����", "�ȳ��ϼ���.\n2������ ���� ������ ������ ���� ������ֽø� �����ϰڽ��ϴ�.\n������ ȭ�鿡�� �巡��&������� ��ü�� �� �ֽ��ϴ�.", "2�� ����");
        }

        public static void M5()
        {
            GameDirector.GetMail("���� �߰� ����", "�ȳ��ϼ���.\n�����غ��� ����� ���� ��ϴµ� ���� �� ������ ������ ���� �� ������.\n�߰� ���� 1����� ������ �帳�ϴ�.", "������");
        }

        public static void M6()
        {
            GameDirector.GetMail("�ý�Ÿ�� ���", "���� ������� �� �ý�Ÿ���� ��ҵǾ����ϴ�.\n�ֳ��ϸ� �ý�Ÿ������ �����ϱ�� �����ҽ��ϴ�.", "KBO����");
        }

        public static void M7()
        {
            GameDirector.GetMail("���� �޼���", "�ȳ��ϼ���, ������!\n���� ���� ���� ���Ŵϴ�. �� ���� ����� �� �� �ִ� ������ ��ȸ�Դϴ�.\n���� ����� �� �����մϴ�.", "��ī�� ȸ��");
        }

        public static void M8()
        {
            GameDirector.GetMail("���� ���� ����", "�ȳ��ϼ���.\n2025 �ѱ����ξ߱� ���Խ����� ���� �Ǿ����ϴ�.", "KBO����");
        }

        public static void M9()
        {
            GameDirector.GetMail("���� ����", "�ȳ��ϼ���.\n2025 �ѱ����ξ߱� ������ ���� ���� �Ǿ����ϴ�.", "KBO����");
        }
    }
}
