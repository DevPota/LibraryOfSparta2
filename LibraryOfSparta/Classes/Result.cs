using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D = LibraryOfSparta.Common.Define;
using System.Windows.Markup;
using LibraryOfSparta.Common;
using System.Xml.Linq;

namespace LibraryOfSparta
{
    public class Result
    {
        public void DrawWalls()
        {
            Console.SetWindowSize(Define.SCREEN_X + 3, Define.SCREEN_Y + 3);

            Console.SetCursorPosition(0, 0);
            Console.Write("┏");
            for (int i = 1; i < D.SCREEN_X; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("━");
            }
            Console.SetCursorPosition(D.SCREEN_X, 0);
            Console.Write("┓");

            for (int i = 1; i < D.SCREEN_Y; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("┃");
                Console.SetCursorPosition(D.SCREEN_X, i);
                Console.Write("┃");
            }

            for (int i = 1; i < D.SCREEN_X; i++)
            {
                Console.SetCursorPosition(i, D.SCREEN_Y - 5);
                Console.Write("━");
            }

            Console.SetCursorPosition(0, D.SCREEN_Y);
            Console.Write("┗");
            for (int i = 1; i < D.SCREEN_X; i++)
            {
                Console.SetCursorPosition(i, D.SCREEN_Y);
                Console.Write("━");
            }
            Console.SetCursorPosition(D.SCREEN_X, D.SCREEN_Y);
            Console.Write("┛");
        }

        public void ResultScene()
        {
            Console.SetCursorPosition(40, 3);
            Console.Write(@" _   _  _____  _____  _____  _____ ______ __   __");
            Console.SetCursorPosition(40, 4);
            Console.Write(@"| | | ||_   _|/  __ \|_   _||  _  || ___ \\ \ / /");
            Console.SetCursorPosition(40, 5);
            Console.Write(@"| | | |  | |  | /  \/  | |  | | | || |_/ / \ V / ");
            Console.SetCursorPosition(40, 6);
            Console.Write(@"| | | |  | |  | |      | |  | | | ||    /   \ /  ");
            Console.SetCursorPosition(40, 7);
            Console.Write(@"\ \_/ / _| |_ | \__/\  | |  \ \_/ /| |\ \   | |");
            Console.SetCursorPosition(40, 8);
            Console.Write(@" \___/  \___/  \____/  \_/   \___/ \_| \_|  \_/");

            Console.SetCursorPosition(49, 11);
            Console.Write("새로운 카드를 획득하였습니다.");

            Console.SetCursorPosition(36, 14);
            Console.Write(@" _________________________________");
            for (int i = 15; i < 37; i++)
            {
                Console.SetCursorPosition(36, i);
                Console.Write(@"|                                 |");
            }
            Console.SetCursorPosition(36, 37);
            Console.Write(@"|_________________________________|");

            Console.SetCursorPosition(80, 16);
            Console.Write("보유중인 카드");
            /*
            for (int i = 17,int j = 17; i < 보유중인카드갯수; i++)
            {
                if (// 보유목록에 안뜬 카드라면)
                {
                    Console.SetCursorPosition(80, j);
                    목록에 카드 이름 추가
                    j++;
                }
                else // 보유목록에 이미 있는 카드라면
                {
                    그 카드 갯수 1증가
                }
            }
            */

            Console.SetCursorPosition(47, 22);
            Console.Write($"완벽한 타격");       // 변수
            Console.SetCursorPosition(42, 28);
            Console.Write($"적에게 피해를 20 줍니다.");       // 변수

            Console.SetCursorPosition(59, 41);
            Console.Write($"1. 획득");

            Console.SetCursorPosition(2, 46);
            Console.Write(">>");

            string input = Console.ReadLine();
            int parseInput = int.Parse(input);

            switch (parseInput)
            {
                case 1:
                    AddCardClass.AddCardReply();
                    AddCardClass.AddCard();
                    break;
            }

            Console.SetCursorPosition(55, 41);
            Console.Write($"1. 던전 선택창으로");
            Console.SetCursorPosition(2, 48);
            Console.Write(">>");

            string input2 = Console.ReadLine();
            int parseInput2 = int.Parse(input);

            switch (parseInput2)
            {
                case 1:
                    // 던전 선택창으로
                    break;
            }
        }
    }
    public class Card
    {
        public string NAME { get; set; }
        public string POWER { get; set; }
        public string TYPE { get; set; }
        public string COST { get; set; }
        public string DIALOG { get; set; }
        // public override string ToString() => NAME;
    }

    public class AddCardClass
    {
        public static void AddCardReply()
        {
            Console.SetCursorPosition(1, 47);
            Console.Write("카드를 획득했습니다.");
        }
        public static void AddCard()
        {
            using(var reward = new StreamReader("CardBase.csv"))
            {
                List<Card> CardList = new List<Card>();

                int count = 0;

                Console.SetCursorPosition(5, 2);

                while (!reward.EndOfStream)
                {
                    string line = reward.ReadLine();
                    string[] values = line.Split(',');
                    CardList.Add(new Card() { NAME = $"{values[0]}", POWER = $"{values[1]}", TYPE = $"{values[2]}", COST = $"{values[3]}", DIALOG = $"{values[4]}" });

                    Console.WriteLine(values);
                    Console.WriteLine(CardList);
                    count++;
                }
            }
        }
    }
}


