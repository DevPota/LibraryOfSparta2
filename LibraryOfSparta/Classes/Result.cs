﻿using System;
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
using LibraryOfSparta.Managers;
using System.ComponentModel.DataAnnotations;

namespace LibraryOfSparta
{
    public class Result : Scene
    {
        MyCard c = new MyCard();
        RewardCard r = new RewardCard();
        GetReward g = new GetReward();
        BattleUI b = new BattleUI();
        Draw d = new Draw();
        Reward Rewards = new Reward();

        int max = 3;
        int min = 0;
        int cursor_x = D.SCREEN_X - 55;
        int cursor_y = D.SCREEN_Y - 6;
        int cursormin_y = D.SCREEN_Y - 10;
        int cursormax_y = D.SCREEN_Y - 6;
        int cursorblank;

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
        public void Cursor(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.SetCursorPosition(x, y);
            Console.WriteLine("◀");

            Console.ResetColor();
        }

        public void EraseCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("  ");
        }
        public void MoveCursor(string vector)
        {
            switch ($"{vector}")
            {
                case "UP":
                    if (cursor_y == cursormax_y)       
                    {
                        EraseCursor(cursor_x, cursor_y);
                        cursor_y -= cursorblank;
                        Cursor(cursor_x, cursor_y);
                    }
                    else if (cursor_y <= cursormax_y && cursor_y > cursormin_y)  
                    {
                        EraseCursor(cursor_x, cursor_y);
                        cursor_y--;
                        Cursor(cursor_x, cursor_y);
                    }
                    break;
                case "DOWN":
                    if (cursor_y == cursormax_y - cursorblank)          
                    {
                        EraseCursor(cursor_x, cursor_y);
                        cursor_y += cursorblank;
                        Cursor(cursor_x, cursor_y);
                    }
                    else if (cursor_y < cursormax_y && cursor_y >= cursormin_y)    
                    {
                        EraseCursor(cursor_x, cursor_y);
                        cursor_y++;
                        Cursor(cursor_x, cursor_y);
                    }
                    break;
            }
        }
        public void Select(int n)
        {
            switch (n)
            {
                case 0:
                    g.GetRewardCard(n);
                    d.DrawGet(98, 13);
                    break;
                case 1:
                    g.GetRewardCard(n);
                    d.DrawGet(98, 22);
                    break;
                case 2:
                    g.GetRewardCard(n);
                    d.DrawGet(98, 31);
                    break;
                case 4:
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("                                       ");
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("던전 선택창으로 이동합니다.");
                    break;
            }
        }

        public void Init(int battleScore = 3)
        {
            Core.PlayPlayerBGM(D.BGM_PATH + "/Result.wav");
            Core.RenderSystemUI();

            // Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 1; i < D.SCREEN_X - 2; i++)
            {
                Console.SetCursorPosition(i, D.SCREEN_Y - 5);
                Console.Write("━");
            }
            // Console.ResetColor();

            Random rand = new Random();

            // c.MakeInventory();
            // c.ShowInventory();

            // 던전 선택창에서 스테이지 값 하나 받아와서 변경

            r.MakeReward(Rewards);
            // r.GiveReward(5);

            d.DrawReward(D.SCREEN_X - 126, D.SCREEN_Y - 36);
            d.DrawBonus(D.SCREEN_X - 125, D.SCREEN_Y - 27);
            d.DrawBonus(D.SCREEN_X - 125, D.SCREEN_Y - 18);
            d.DrawLock(D.SCREEN_X - 75, D.SCREEN_Y - 37);
            d.DrawLock(D.SCREEN_X - 75, D.SCREEN_Y - 28);
            d.DrawLock(D.SCREEN_X - 75, D.SCREEN_Y - 19);

            if (battleScore == -1)
            {
                max = 0;
            }
            else if (battleScore < 2)
            {
                max = 1;
            }
            else if (battleScore >= 2 && battleScore < 4)
            {
                max = 2;
            }
            else
            {
                max = 3;
            }

            if (max == 0)
            {
                cursormin_y = D.SCREEN_Y - 6;
                cursorblank = 0;
                Cursor(cursor_x, cursor_y);
                d.DrawDefeat(D.SCREEN_X - 90, D.SCREEN_Y - 47);
                Console.SetCursorPosition(D.SCREEN_X - 90, D.SCREEN_Y - 10);
                Console.Write("전투에서 패배하여 보상을 획득할 수 없습니다.");
                Console.SetCursorPosition(D.SCREEN_X - 75, D.SCREEN_Y - 6);
                Console.Write("0. 던전 선택");
            }
            else
            {
                cursor_y = D.SCREEN_Y - 10;
                cursormin_y = D.SCREEN_Y - 10;
                cursorblank = 4;
                Cursor(cursor_x, cursor_y);
                Console.SetCursorPosition(D.SCREEN_X - 85, D.SCREEN_Y - 38);
                Console.Write("보상을 획득하거나 포기할 수 있습니다.");
                Console.SetCursorPosition(D.SCREEN_X - 75, D.SCREEN_Y - 6);
                Console.Write("0. 던전 선택");

                d.DrawVictory(D.SCREEN_X - 90, 3);
                d.Erase(D.SCREEN_X - 75, 13, 20, 7);
                int random_number1 = rand.Next(0, 9);
                Console.SetCursorPosition(D.SCREEN_X - 83, 16);
                Console.Write($"{Rewards.RewardCard[random_number1].NAME} - {Rewards.RewardCard[random_number1].DIALOG}");

                Console.SetCursorPosition(D.SCREEN_X - 75, 40);
                Console.Write("1. 카드 획득");
                for (int i = min + 1; i < max; i++)
                {
                    if (i == 1)
                    {
                        cursorblank = 3;
                        d.Erase(D.SCREEN_X - 75, 22, 20, 7);
                        int random_number2 = rand.Next(0, 9);
                        Console.SetCursorPosition(D.SCREEN_X - 83, 25);
                        Console.Write($"{Rewards.RewardCard[random_number2].NAME} - {Rewards.RewardCard[random_number2].DIALOG}");

                        Console.SetCursorPosition(D.SCREEN_X - 75, 41);
                        Console.Write("2. 보너스 카드 획득");
                    }
                    else
                    {
                        cursorblank = 2;
                        d.Erase(D.SCREEN_X - 75, 31, 20, 7);
                        int random_number3 = rand.Next(0, 9);
                        Console.SetCursorPosition(D.SCREEN_X - 83, 34);
                        Console.Write($"{Rewards.RewardCard[random_number3].NAME} - {Rewards.RewardCard[random_number3].DIALOG}");

                        Console.SetCursorPosition(D.SCREEN_X - 75, 42);
                        Console.Write("3. 보너스 카드 획득");
                    }
                }
            }

            // r.GiveReward(b.RenderEmotionLevel());    나중엔 이거사용 BattleUI.cs RenderEmotionLevel void -> int, return eELevel

            // int input = CheckValidInput(0, 3, b.RenderEmotionLevel());
            //int input = CheckValidInput(0, 3, 5); // 0부터 3까지 유효한 숫자, eELevel에 따라 허용되는 숫자가 달라짐

            //switch (input)
            //{
            //    case 0:
            //        // 던전 선택창으로
            //        break;
            //    case 1:
            //        d.DrawGet(99, 13);
            //        // 카드추가
            //        break;
            //    case 2:
            //        d.DrawGet(99, 22);
            //        // 카드추가
            //        break;
            //    case 3:
            //        d.DrawGet(99, 31);
            //        // 카드추가
            //        break;
            //}
        }
        
        public void Update()
        {
            Console.SetCursorPosition(2, Define.SCREEN_Y - 4);
            ConsoleKeyInfo key = Core.GetKey();

            if(key == default)
            {
                return;
            }
            else
            {
                switch(key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        MoveCursor("UP");
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        MoveCursor("DOWN");
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Select(cursor_y - 40);
                        break;
                }
            }
        }
        //public int CheckValidInput(int min, int max, int eELevel)   // 0부터 3까지 유효한 숫자, eELevel에 따라 허용되는 숫자가 달라짐
        //{
        //    while (true)
        //    {
        //        Console.SetCursorPosition(4, 46);
        //        Console.Write("     ");
        //        Console.SetCursorPosition(2, 46);
        //        Console.Write(">> ");
        //        string input = Console.ReadLine();

        //        bool parseSuccess = int.TryParse(input, out var ret);
        //        if (parseSuccess)
        //        {
        //            if (eELevel < 2)
        //            {
        //                if (ret >= min && ret <= max - 2)
        //                    return ret;
        //            }
        //            else if (eELevel >= 2 && eELevel < 4)
        //            {
        //                if (ret >= min && ret <= max - 1)
        //                    return ret;
        //            }
        //            else
        //            {
        //                if (ret >= min && ret <= max)
        //                    return ret;
        //            }
        //        }
        //        Console.SetCursorPosition(3, 47);
        //        Console.Write("잘못된 입력입니다.");
        //    }
        //}
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
        //public List<Card> InventoryCard = new();
        //public void AddCard(Card card)
        //{
        //    InventoryCard.Add(card);
        //}
        public List<int> InventoryCard = new();
    }
    public class MyCard // 보유 카드 목록
    {
        public static Inventory Inventories = new();
        public void MakeInventory()
        {
            for (int i = 0; i < 5; i++)
            {
                Inventories.InventoryCard.Add(1);
            }
            for (int i = 0; i < 4; i++)
            {
                Inventories.InventoryCard.Add(2);
            }
            for (int i = 0; i < 1; i++)
            {
                Inventories.InventoryCard.Add(3);
            }
        }
        //public static Inventory Inventories = new();
        //public void MakeInventory()
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Inventories.AddCard(new Card() { NAME = "타격", POWER = 6, TYPE = 0, COST = 1, DIALOG = "피해를 6 줍니다." });
        //    }
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Inventories.AddCard(new Card() { NAME = "수비", POWER = 5, TYPE = 1, COST = 1, DIALOG = "방어도를 5 얻습니다." });
        //    }
        //    for (int i = 0; i < 1; i++)
        //    {
        //        Inventories.AddCard(new Card() { NAME = "회피", POWER = 8, TYPE = 2, COST = 1, DIALOG = "8 이하의 피해를 한번 회피합니다." });
        //    }
        //}
        //public void ShowInventory()
        //{
        //    int strike = 1; int defend = 1; int evasion = 1;    // 타격 수비 회피 개수..더 좋은방법없을까

        //    Console.SetCursorPosition(115, 2);
        //    Console.Write("보유중인 카드");

        //    foreach (Card card in Inventories.InventoryCard)
        //    {
        //        switch (card.NAME)
        //        {
        //            case "타격":
        //                Console.SetCursorPosition(115, 4);          // 하드코딩
        //                Console.Write($"{card.NAME} * {strike}");
        //                strike++;
        //                break;
        //            case "수비":
        //                Console.SetCursorPosition(115, 5);
        //                Console.Write($"{card.NAME} * {defend}");
        //                defend++;
        //                break;
        //            case "회피":
        //                Console.SetCursorPosition(115, 6);
        //                Console.Write($"{card.NAME} * {evasion}");
        //                evasion++;
        //                break;
        //        }
        //    }
        //}
    }
    public class RewardCard // 스테이지별 보상 카드 목록
    {
        public static Reward Rewards = new();
        Battle b = new Battle();
        public void MakeReward(Reward Rewards)
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
        //    public void GiveReward(int i)
        //    {
        //        Draw d = new Draw();
        //        Random rand = new Random();
        //        int random_number1 = rand.Next(0, 9);
        //        int random_number2 = rand.Next(0, 9);
        //        int random_number3 = rand.Next(0, 9);

        //        if (i < 2)
        //        {
        //            Console.SetCursorPosition(44, 41);
        //            Console.Write("0. 던전 선택창으로  1. 카드 획득");
        //            d.DrawReward(4, 14);
        //            Console.SetCursorPosition(47, 16);
        //            Console.Write($"{Rewards.RewardCard[random_number1].NAME} - {Rewards.RewardCard[random_number1].DIALOG}");
        //            d.DrawBonus(5, 23);
        //            d.DrawBonus(5, 32);
        //            d.DrawLock(55, 22);
        //            d.DrawLock(55, 31);
        //            // d.DrawCard(91, 22);
        //            // d.DrawCard(91, 31);
        //        }
        //        else if (i >= 2 && i < 4)
        //        {
        //            Console.SetCursorPosition(34, 41);
        //            Console.Write("0. 던전 선택창으로    1. 카드 획득    2. 보너스 카드 획득");
        //            d.DrawReward(4, 14);
        //            Console.SetCursorPosition(47, 16);
        //            Console.Write($"{Rewards.RewardCard[random_number1].NAME} - {Rewards.RewardCard[random_number1].DIALOG}");
        //            d.DrawBonus(5, 23);
        //            Console.SetCursorPosition(47, 25);
        //            Console.Write($"{Rewards.RewardCard[random_number2].NAME} - {Rewards.RewardCard[random_number2].DIALOG}");
        //            d.DrawBonus(5, 32);
        //            d.DrawLock(55, 31);
        //            // d.DrawCard(91, 31);
        //        }
        //        else
        //        {
        //            Console.SetCursorPosition(26, 41);
        //            Console.Write("0. 던전 선택창으로    1. 카드 획득    2. 보너스 카드 획득    3. 보너스 카드 획득");
        //            d.DrawReward(4, 14);
        //            Console.SetCursorPosition(47, 16);
        //            Console.Write($"{Rewards.RewardCard[random_number1].NAME} - {Rewards.RewardCard[random_number1].DIALOG}");
        //            d.DrawBonus(5, 23);
        //            Console.SetCursorPosition(47, 25);
        //            Console.Write($"{Rewards.RewardCard[random_number2].NAME} - {Rewards.RewardCard[random_number2].DIALOG}");
        //            d.DrawBonus(5, 32);
        //            Console.SetCursorPosition(47, 34);
        //            Console.Write($"{Rewards.RewardCard[random_number3].NAME} - {Rewards.RewardCard[random_number3].DIALOG}");
        //        }
        //    }
    }
    public class GetReward
    {
        public void GetRewardCard(int n)
        {
            switch (n)
            {
                case 0:
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("                                       ");
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("카드를 획득했습니다.");
                    break;
                case 1:
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("                                       ");
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("보너스 카드를 획득했습니다.");
                    break;
                case 2:
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("                                       ");
                    Console.SetCursorPosition(D.SCREEN_X - 128, D.SCREEN_Y - 3);
                    Console.Write("보너스 카드를 획득했습니다.");
                    break;
            }
        }
    }
    public class Draw
    {
        public void DrawVictory(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(x, y);
            Console.Write(@" _   _  _____  _____  _____  _____ ______ __   __");
            Console.SetCursorPosition(x, y + 1);
            Console.Write(@"| | | ||_   _|/  __ \|_   _||  _  || ___ \\ \ / /");
            Console.SetCursorPosition(x, y + 2);
            Console.Write(@"| | | |  | |  | /  \/  | |  | | | || |_/ / \ V / ");
            Console.SetCursorPosition(x, y + 3);
            Console.Write(@"| | | |  | |  | |      | |  | | | ||    /   \ /  ");
            Console.SetCursorPosition(x, y + 4);
            Console.Write(@"\ \_/ / _| |_ | \__/\  | |  \ \_/ /| |\ \   | |");
            Console.SetCursorPosition(x, y + 5);
            Console.Write(@" \___/  \___/  \____/  \_/   \___/ \_| \_|  \_/");

            Console.SetCursorPosition(x - 23, y);
            Console.Write(@"    _    ");
            Console.SetCursorPosition(x - 23, y + 1);
            Console.Write(@" /\| |/\ ");
            Console.SetCursorPosition(x - 23, y + 2);
            Console.Write(@" \ ` ' / ");
            Console.SetCursorPosition(x - 23, y + 3);
            Console.Write(@"|_     _|");
            Console.SetCursorPosition(x - 23, y + 4);
            Console.Write(@" / , . \ ");
            Console.SetCursorPosition(x - 23, y + 5);
            Console.Write(@" \/|_|\/ ");

            Console.SetCursorPosition(x + 64, y);
            Console.Write(@"    _    ");
            Console.SetCursorPosition(x + 64, y + 1);
            Console.Write(@" /\| |/\ ");
            Console.SetCursorPosition(x + 64, y + 2);
            Console.Write(@" \ ` ' / ");
            Console.SetCursorPosition(x + 64, y + 3);
            Console.Write(@"|_     _|");
            Console.SetCursorPosition(x + 64, y + 4);
            Console.Write(@" / , . \ ");
            Console.SetCursorPosition(x + 64, y + 5);
            Console.Write(@" \/|_|\/ ");

            Console.ResetColor();
        }
        public void DrawDefeat(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.SetCursorPosition(x, y);
            Console.Write(@"______  _____ ______  _____   ___   _____       ");
            Console.SetCursorPosition(x, y + 1);
            Console.Write(@"|  _  \|  ___||  ___||  ___| / _ \ |_   _|      ");
            Console.SetCursorPosition(x, y + 2);
            Console.Write(@"| | | || |__  | |_   | |__  / /_\ \  | |        ");
            Console.SetCursorPosition(x, y + 3);
            Console.Write(@"| | | ||  __| |  _|  |  __| |  _  |  | |        ");
            Console.SetCursorPosition(x, y + 4);
            Console.Write(@"| |/ / | |___ | |    | |___ | | | |  | |   _  _ ");
            Console.SetCursorPosition(x, y + 5);
            Console.Write(@"|___/  \____/ \_|    \____/ \_| |_/  \_/  (_)(_)");

            Console.ResetColor();
        }
            public void DrawCard(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("┏");
            for (int i = 1; i < 9; i++)
            {
                Console.SetCursorPosition(x + i, y);
                Console.Write("━");
            }
            Console.SetCursorPosition(x + 9, y);
            Console.Write("┓");

            for (int i = 1; i < 6; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("┃        ┃");
            }
            Console.SetCursorPosition(x, y + 3);
            Console.Write("┃    ?   ┃");

            Console.SetCursorPosition(x, y + 6);
            Console.Write("┗");
            for (int i = 1; i < 9; i++)
            {
                Console.SetCursorPosition(x + i, y + 6);
                Console.Write("━");
            }
            Console.SetCursorPosition(x + 9, y + 6);
            Console.Write("┛");
        }

        public void DrawLock(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(x, y);
            Console.Write("    ██████    ");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("  ██      ██  ");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("  ██      ██  ");
            Console.SetCursorPosition(x, y + 3);
            Console.Write("██████████████");
            Console.SetCursorPosition(x, y + 4);
            Console.Write("██████  ██████");
            Console.SetCursorPosition(x, y + 5);
            Console.Write("██████  ██████");
            Console.SetCursorPosition(x, y + 6);
            Console.Write("██████████████");

            Console.ResetColor();
        }
        public void DrawReward(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.SetCursorPosition(x, y);
            Console.Write(" ____  ____ __    __ ___ ____ ____  ");
            Console.SetCursorPosition(x, y + 1);
            Console.Write(@" || \\||   ||    ||// \\|| \\|| \\ ");
            Console.SetCursorPosition(x, y + 2);
            Console.Write(@" ||_//||== \\ /\ //||=||||_//||  ))");
            Console.SetCursorPosition(x, y + 3);
            Console.Write(@" || \\||___ \V/\V/ || |||| \\||_// ");

            Console.ResetColor();
        }
        public void DrawBonus(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.SetCursorPosition(x, y);
            Console.Write(" ____    ___   __  __ __ __  __ ");
            Console.SetCursorPosition(x, y + 1);
            Console.Write(@" || ))  // \\  ||\ || || || (( \");
            Console.SetCursorPosition(x, y + 2);
            Console.Write(@" ||=)  ((   )) ||\\|| || ||  \\ ");
            Console.SetCursorPosition(x, y + 3);
            Console.Write(@" ||_))  \\_//  || \|| \\_// \_))");

            Console.ResetColor();
        }

        public void DrawGet(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.SetCursorPosition(x, y);
            Console.Write(" _____   _____  _____   _ ");
            Console.SetCursorPosition(x, y + 1);
            Console.Write(@"|  __ \\|  ___||_   _| | |");
            Console.SetCursorPosition(x, y + 2);
            Console.Write(@"| |  \\/| |__    | |   | |");
            Console.SetCursorPosition(x, y + 3);
            Console.Write(@"| | __  |  __|   | |   | |");
            Console.SetCursorPosition(x, y + 4);
            Console.Write(@"| |_\ \ | |___   | |   |_|");
            Console.SetCursorPosition(x, y + 5);
            Console.Write(@" \____/ \____/   \_/   (_)");

            Console.ResetColor();
        }
        public void Erase(int x, int y, int a, int b)       // a, b 지우개 가로, 세로 크기 Erase(55, 13, 20, 7)
        {
            for (int i = 0; i < b; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    Console.SetCursorPosition(x + j, y + i);
                    Console.Write(" ");
                }
            }
        }
    }
}
