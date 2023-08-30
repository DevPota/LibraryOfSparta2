using System;
using LibraryOfSparta.Common;
using LibraryOfSparta.Classes;
using LibraryOfSparta.Managers;

namespace LibraryOfSparta
{
    public class Result : Scene
    {
        Draw d = new Draw();

        int max = 3;
        int min = 0;
        int cursor_x    = Define.SCREEN_X - 55;
        int cursor_y    = Define.SCREEN_Y - 6;
        int cursormin_y = Define.SCREEN_Y - 10;
        int cursormax_y = Define.SCREEN_Y - 6;
        int cursorblank;

        List<int> rewardList = new List<int>();

        public void DrawWalls()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("┏");
            for (int i = 1; i < Define.SCREEN_X; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("━");
            }
            Console.SetCursorPosition(Define.SCREEN_X, 0);
            Console.Write("┓");

            for (int i = 1; i < Define.SCREEN_Y; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("┃");
                Console.SetCursorPosition(Define.SCREEN_X, i);
                Console.Write("┃");
            }

            for (int i = 1; i < Define.SCREEN_X; i++)
            {
                Console.SetCursorPosition(i, Define.SCREEN_Y - 5);
                Console.Write("━");
            }

            Console.SetCursorPosition(0, Define.SCREEN_Y);
            Console.Write("┗");
            for (int i = 1; i < Define.SCREEN_X; i++)
            {
                Console.SetCursorPosition(i, Define.SCREEN_Y);
                Console.Write("━");
            }
            Console.SetCursorPosition(Define.SCREEN_X, Define.SCREEN_Y);
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
                    GetRewardCard(n);
                    d.DrawGet(98, 13);
                    break;
                case 1:
                    GetRewardCard(n);
                    d.DrawGet(98, 22);
                    break;
                case 2:
                    GetRewardCard(n);
                    d.DrawGet(98, 31);
                    break;
                case 4:
                    Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                    Core.LoadScene(1);
                    return;
            }
        }

        public void Init(int battleScore, int floorIndex)
        {
            Core.PlayPlayerBGM(Define.BGM_PATH + "/Result.wav");
            Core.RenderSystemUI();

            for (int i = 1; i < Define.SCREEN_X - 2; i++)
            {
                Console.SetCursorPosition(i, Define.SCREEN_Y - 5);
                Console.Write("━");
            }

            Random rand = new Random();

            d.DrawReward(Define.SCREEN_X - 126, Define.SCREEN_Y - 36);
            d.DrawBonus(Define.SCREEN_X - 125, Define.SCREEN_Y - 27);
            d.DrawBonus(Define.SCREEN_X - 125, Define.SCREEN_Y - 18);
            d.DrawLock(Define.SCREEN_X - 75, Define.SCREEN_Y - 37);
            d.DrawLock(Define.SCREEN_X - 75, Define.SCREEN_Y - 28);
            d.DrawLock(Define.SCREEN_X - 75, Define.SCREEN_Y - 19);

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
                cursormin_y = Define.SCREEN_Y - 6;
                cursorblank = 0;
                Cursor(cursor_x, cursor_y);
                d.DrawDefeat(Define.SCREEN_X - 90, Define.SCREEN_Y - 47);
                Console.SetCursorPosition(Define.SCREEN_X - 90, Define.SCREEN_Y - 10);
                Console.Write("전투에서 패배하여 보상을 획득할 수 없습니다.");
                Console.SetCursorPosition(Define.SCREEN_X - 75, Define.SCREEN_Y - 6);
                Console.Write("입구로 돌아간다");
            }
            else
            {
                string battleData = Core.GetData(Define.BATTLE_DATA_PATH);
                string cardData   = Core.GetData(Define.CARD_DATA_PATH);

                string[] rewardDataLines = battleData.Split('\n');
                string[] cardDataLines   = cardData.Split('\n');
                rewardList = GetRewardList(rewardDataLines[floorIndex].Split(',')[5], max);

                cursor_y = Define.SCREEN_Y - 10;
                cursormin_y = Define.SCREEN_Y - 10;
                cursorblank = 4;
                Cursor(cursor_x, cursor_y);
                Console.SetCursorPosition(Define.SCREEN_X - 85, Define.SCREEN_Y - 38);
                Console.Write("보상을 획득하거나 포기할 수 있습니다.");
                Console.SetCursorPosition(Define.SCREEN_X - 75, Define.SCREEN_Y - 6);
                Console.Write("입구로 돌아간다");

                d.DrawVictory(Define.SCREEN_X - 90, 3);
                d.Erase(Define.SCREEN_X - 75, 13, 20, 7);
                int random_number1 = rand.Next(0, 9);
                Console.SetCursorPosition(Define.SCREEN_X - 83, 16);
                Console.Write($"{GetCardName(cardDataLines, rewardList[0])} - {GetCardInfo(cardDataLines, rewardList[0])}");

                Console.SetCursorPosition(Define.SCREEN_X - 75, 40);
                Console.Write("카드 획득");
                for (int i = 1; i < max; i++)
                {
                    if (i == 1)
                    {
                        cursorblank = 3;
                        d.Erase(Define.SCREEN_X - 75, 22, 20, 7);
                        int random_number2 = rand.Next(0, 9);
                        Console.SetCursorPosition(Define.SCREEN_X - 83, 25);
                        Console.Write($"{GetCardName(cardDataLines, rewardList[i])} - {GetCardInfo(cardDataLines, rewardList[0])}");

                        Console.SetCursorPosition(Define.SCREEN_X - 75, 41);
                        Console.Write("보너스 카드 획득");
                    }
                    else
                    {
                        cursorblank = 2;
                        d.Erase(Define.SCREEN_X - 75, 31, 20, 7);
                        int random_number3 = rand.Next(0, 9);
                        Console.SetCursorPosition(Define.SCREEN_X - 83, 34);
                        Console.Write($"{GetCardName(cardDataLines, rewardList[i])} - {GetCardInfo(cardDataLines, rewardList[i])}");

                        Console.SetCursorPosition(Define.SCREEN_X - 75, 42);
                        Console.Write("보너스 카드 획득");
                    }
                }
            }
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
                        Core.PlaySFX(Define.SFX_PATH + "/Click.wav");
                        MoveCursor("UP");
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        Core.PlaySFX(Define.SFX_PATH + "/Click.wav");
                        MoveCursor("DOWN");
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Select(cursor_y - 40);
                        break;
                }
            }
        }

        public List<int> GetRewardList(string rewardLine, int max)
        {
            List<int> rewardList = new List<int>();

            string[] indices = rewardLine.Split('_');

            Random rand = new Random();

            for (int i = 0; i < max; i++)
            {
                int randomValue = rand.Next(0, indices.Length);
                rewardList.Add(int.Parse(indices[randomValue]));
            }

            return rewardList;
        }

        public string GetCardName(string[] cardDataLine, int index)
        {
            return cardDataLine[index].Split(',')[0];
        }

        public string GetCardInfo(string[] cardDataLine, int index)
        {
            return cardDataLine[index].Split(',')[6];
        }

        public void GetRewardCard(int n)
        {
            switch (n)
            {
                case 0:
                    if(AddNewCardToInventory(rewardList[n], n) == true)
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("                                       ");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("카드를 획득했습니다.");
                    }
                    else
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("                                       ");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("인벤토리가 꽉 찼습니다");
                    }
                    break;
                case 1:
                    if (AddNewCardToInventory(rewardList[n], n) == true)
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("                                       ");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("보너스 카드를 획득했습니다.");
                    }
                    else
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("                                       ");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("인벤토리가 꽉 찼습니다");
                    }
                    break;
                case 2:
                    if (AddNewCardToInventory(rewardList[n], n) == true)
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("                                       ");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("보너스 카드를 획득했습니다.");
                    }
                    else
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("                                       ");
                        Console.SetCursorPosition(Define.SCREEN_X - 128, Define.SCREEN_Y - 3);
                        Console.Write("인벤토리가 꽉 찼습니다");
                    }
                    
                    break;
            }
        }

        public bool AddNewCardToInventory(int cardIndex, int removeAtIndex)
        {
            if (rewardList[removeAtIndex] == -1)
            {
                return true;
            }
            else if(Core.SaveData.Inventory.Count >= 100)
            {
                return false;
            }
            else
            {
                Core.SaveData.Inventory.Add(cardIndex);
                rewardList[removeAtIndex] = -1;

                Core.Save();
                return true;
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
