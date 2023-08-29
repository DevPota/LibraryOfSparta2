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
using System.Reflection.Metadata.Ecma335;
using LibraryOfSparta.Classes;

namespace LibraryOfSparta
{
    public class Result
    {
        MyCard c = new MyCard();
        RewardCard r = new RewardCard();
        GetReward g = new GetReward();
        Battle b = new Battle();

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

            c.MakeInventory();
            c.ShowInventory();

            // 던전 선택창에서 스테이지 값 하나 받아와서 변경

            r.MakeReward();
            r.GiveReward(3);
            // r.GiveReward(b.RenderEmotionLevel());    나중엔 이거사용 Battle.cs RenderEmotionLevel void -> int, return eELevel

            /*
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(30, 15 + i*2);
                Console.Write($"{r.GiveReward(i).NAME} - {r.GiveReward(i).DIALOG}");       // 0 대신 스테이지 변수값 지금은 모든 카드 나타내기
            }
            */

            Console.SetCursorPosition(59, 41);
            Console.Write("1. 획득");

            Console.SetCursorPosition(2, 46);
            Console.Write(">>");

            while(true)
            {
                string input = Console.ReadLine();
                int parseInput = int.Parse(input);

                if (parseInput == 1)
                {
                    g.GetRewardCard();
                    break;
                }
                else
                {
                    Console.SetCursorPosition(2, 46);
                    Console.Write("                                               ");
                    Console.SetCursorPosition(2, 46);
                    Console.Write("잘못된 입력입니다. >>");
                    continue;
                }
            }

            Console.SetCursorPosition(55, 41);
            Console.Write($"1. 던전 선택창으로");
            Console.SetCursorPosition(2, 48);
            Console.Write(">>");

            while (true)
            {
                string input2 = Console.ReadLine();
                int parseInput2 = int.Parse(input2);

                if (parseInput2 == 1)
                {
                    // 던전 선택창으로
                    break;
                }
                else
                {
                    Console.SetCursorPosition(2, 48);
                    Console.Write("                                               ");
                    Console.SetCursorPosition(2, 48);
                    Console.Write("잘못된 입력입니다. >>");
                    continue;
                }
            }
        }
    }
    public class Card
    {
        public string NAME { get; set; }
        public int POWER { get; set; }
        public int TYPE { get; set; }
        public int COST { get; set; }
        public string DIALOG { get; set; }
    }
    public class Reward // 층별 보상 카드
    {
        public List<Card> RewardCard = new();
        public void AddCard(Card card)
        {
            RewardCard.Add(card);
        }
    }
    
    public class Inventory // 인벤토리
    {
        public List<Card> InventoryCard = new();
        public void AddCard(Card card)
        {
            InventoryCard.Add(card);
        }
    }
    public class MyCard // 보유 카드 목록
    {
        public static Inventory Inventories = new();
        public void MakeInventory()
        {
            for (int i = 0; i < 5; i++)
            {
                Inventories.AddCard(new Card() { NAME = "타격", POWER = 6, TYPE = 0, COST = 1, DIALOG = "피해를 6 줍니다." });
            }
            for (int i = 0; i < 4; i++)
            {
                Inventories.AddCard(new Card() { NAME = "수비", POWER = 5, TYPE = 1, COST = 1, DIALOG = "방어도를 5 얻습니다." });
            }
            for (int i = 0; i < 1; i++)
            {
                Inventories.AddCard(new Card() { NAME = "회피", POWER = 8, TYPE = 2, COST = 1, DIALOG = "8 이하의 피해를 한번 회피합니다." });
            } 
        }
        public void ShowInventory()
        {
            int strike = 1; int defend = 1; int evasion = 1;    // 타격 수비 회피 개수..더 좋은방법없을까

            Console.SetCursorPosition(110, 16);
            Console.Write("보유중인 카드");

            foreach (Card card in Inventories.InventoryCard)
            {
                switch (card.NAME)
                {
                    case "타격":
                            Console.SetCursorPosition(110, 18);          // 하드코딩
                            Console.Write($"{card.NAME} X {strike}");
                            strike++;
                        break;
                    case "수비":
                            Console.SetCursorPosition(110, 19);
                            Console.Write($"{card.NAME} X {defend}");
                            defend++;
                        break;
                    case "회피":
                            Console.SetCursorPosition(110, 20);
                            Console.Write($"{card.NAME} X {evasion}");
                            evasion++;
                        break;
                }
            }
        }
    }
    public class RewardCard // 스테이지별 보상 카드 목록
    {
        Battle b = new Battle();
        public static Reward Rewards = new();
        public void MakeReward()
        {
            Rewards.AddCard(new Card() { NAME = "무모한 타격", POWER = 6, TYPE = 0, COST = 0, DIALOG = "피해를 4 줍니다." });
            Rewards.AddCard(new Card() { NAME = "가벼운 회피", POWER = 4, TYPE = 2, COST = 0, DIALOG = "5 이하의 피해를 한번 회피합니다." });
            Rewards.AddCard(new Card() { NAME = "직감", POWER = 3, TYPE = 1, COST = 0, DIALOG = "방어도를 3 얻습니다." });
            Rewards.AddCard(new Card() { NAME = "철의 파동", POWER = 5, TYPE = 3, COST = 1, DIALOG = "피해를 5 주고 방어도를 5 얻습니다." });
            Rewards.AddCard(new Card() { NAME = "돌진", POWER = 10, TYPE = 3, COST = 2, DIALOG = "피해를 10 주고 방어도를 10 얻습니다." });
            Rewards.AddCard(new Card() { NAME = "대학살", POWER = 16, TYPE = 0, COST = 2, DIALOG = "피해를 16 줍니다." });
            Rewards.AddCard(new Card() { NAME = "유령 갑옷", POWER = 10, TYPE = 1, COST = 1, DIALOG = "방어도를 10 얻습니다." });
            Rewards.AddCard(new Card() { NAME = "구르기", POWER = 13, TYPE = 2, COST = 2, DIALOG = "13 이하의 피해를 한번 회피합니다." });
            Rewards.AddCard(new Card() { NAME = "화염 방패", POWER = 16, TYPE = 1, COST = 2, DIALOG = "방어도를 16 얻습니다." });
            Rewards.AddCard(new Card() { NAME = "몽둥이질", POWER = 28, TYPE = 0, COST = 3, DIALOG = "피해를 28 줍니다." });
        }
        public Card GiveReward(int eELevel)                 // return 이 아니라 카드1개 2개 3개 띄우는 식으로 해야함
        {
            Random rand = new Random();
            int random_number = rand.Next(10);

            if (eELevel < 2)
            {
                return Rewards.RewardCard[random_number];
            }
            else if (eELevel >= 2 && eELevel < 4)
            {
                return Rewards.RewardCard[random_number];
                return Rewards.RewardCard[random_number];
            }
            else
            {
                return Rewards.RewardCard[random_number];
                return Rewards.RewardCard[random_number];
                return Rewards.RewardCard[random_number];
            }
        }
    }
    public class GetReward
    {
        public void GetRewardCard()
        {
            Console.SetCursorPosition(2, 47);
            Console.Write("카드를 획득했습니다.");

        }
    }
}


