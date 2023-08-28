using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D = LibraryOfSparta.Common.Define;
using System.Windows.Markup;

namespace LibraryOfSparta
{
    public class Result
    {
        public void DrawWalls()
        {
            Console.SetWindowSize(D.SCREEN_X + 3, D.SCREEN_Y + 3);

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

            Console.SetCursorPosition(45, 14);
            Console.Write(@" __________________________________");
            for (int i = 15; i < 37; i++)
            {
                Console.SetCursorPosition(45, i);
                Console.Write(@"|                                  |");
            }
            Console.SetCursorPosition(45, 37);
            Console.Write(@"|__________________________________|");

            Console.SetCursorPosition(57, 22);
            Console.Write($"완벽한 타격");       // 변수
            Console.SetCursorPosition(52, 28);
            Console.Write($"적에게 피해를 20 줍니다.");       // 변수

            Console.SetCursorPosition(51, 41);
            Console.Write($"1. 획득");
            Console.SetCursorPosition(65, 41);
            Console.Write($"2. 넘기기");

            Console.SetCursorPosition(2, 46);
            Console.Write(">>");

            string input = Console.ReadLine();
            int parseInput = int.Parse(input);

            switch (parseInput)
            {
                case 1:
                    AddCard();
                    break;
                case 2:
                    Console.SetCursorPosition(2, 47);
                    Console.Write("카드를 넘깁니다.");
                    break;
            }
        }
        public void AddCard()
        {
            Console.SetCursorPosition(1, 47);
            Console.Write("카드를 획득했습니다.");
        }
    }
    public class Card
    {
        public string NAME { get; set; }
        public string POWER { get; set; }
        public string TYPE { get; set; }
        public string COST { get; set; }
        public string DIALOG { get; set; }

        public Card(string name, string power, string type, string cost, string dialog)
        {
            NAME = name;
            POWER = power;
            TYPE = type;
            COST = cost;
            DIALOG = dialog;
        }
        public void getCard()
        {
            StreamReader reward = new StreamReader("CardBase.csv");

            List<Card> cards = new List<Card>();

            int count = 0;

            Console.SetCursorPosition(5, 2);

            while (!reward.EndOfStream)
            {
                string line = reward.ReadLine();
                string[] values = line.Split(',');
                cards.Add(new Card(values[0], values[1], values[2], values[3], values[4]));
                count++;
            }

            Random rand = new Random();
            int random_number = rand.Next(count);

            Console.WriteLine($"{cards[random_number]}");
        }
    }
}


