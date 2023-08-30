using LibraryOfSparta.Classes;
using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace LibraryOfSparta
{
    public class Entrance : Scene
    {
        Random random = new Random();
        public enum EntranceMenu
        {
            MENU_SELECT,
            FLOOR_SELECT
        }

        public int          CursorIndex { get ; set; }

        public EntranceMenu CurrentMenu { get; set; } = EntranceMenu.MENU_SELECT;

        string[] floorData = null;

        public void Init()
        {
            string battleData = Core.GetData(Define.BATTLE_DATA_PATH);
            floorData = battleData.Split('\n');

            DrawPanel();

            //㉾
            DrawStar();
            DrawBackGround();
            DrawTower();

            DrawMenu();

            DrawCursor();
            DrawFloorInfo(0);
        }

        public void DrawCursor()
        {
            if(CurrentMenu == EntranceMenu.MENU_SELECT)
            {
                int x = 88;
                int y = 35;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                char cursor   = '▶';
                string blank  = "  ";

                switch(CursorIndex)
                {
                    case 0:
                        Console.SetCursorPosition(x, y+2);
                        Console.Write(blank);
                        Console.SetCursorPosition(x, y);
                        Console.Write(cursor);
                        break;
                    case 1:
                        Console.SetCursorPosition(x, y);
                        Console.Write(blank);
                        Console.SetCursorPosition(x, y+4);
                        Console.Write(blank);
                        Console.SetCursorPosition(x, y+2);
                        Console.Write(cursor);
                        break;
                    case 2:
                        Console.SetCursorPosition(x, y+2);
                        Console.Write(blank);
                        Console.SetCursorPosition(x, y+4);
                        Console.Write(cursor);
                        break;
                }
            }
            else if (CurrentMenu == EntranceMenu.FLOOR_SELECT)
            {
                int x = 70;
                int y = 41;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                char cursor = '◀';
                string blank = "  ";

                if(CursorIndex != 0)
                {
                    Console.SetCursorPosition(x, (y - ((CursorIndex - 1) * 3)));
                    Console.Write(blank);
                }
                Console.SetCursorPosition(x, (y - ((CursorIndex + 1) * 3)));
                Console.Write(blank);
                Console.SetCursorPosition(x, (y - (CursorIndex * 3)));
                Console.Write(cursor);

                ReleaseFloorInfo();
                DrawFloorInfo(CursorIndex);
            }

            Console.SetCursorPosition(Define.SCREEN_X -2, Define.SCREEN_Y - 2);
            Console.ResetColor();
        }

        public void DrawBackGround()//㉾
        {
            for (int i = 0; i < 28; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(35, Define.SCREEN_Y - (8 + i));
                Console.Write("███████████████");

                if(i < 18)
                {
                    Console.SetCursorPosition(6, Define.SCREEN_Y - (8 + i));
                    Console.Write("████████████");
                }

            }


            for (int i = 0; i < 15; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(37, Define.SCREEN_Y - (8 + i));
                Console.Write("███████");

                Console.SetCursorPosition(22, Define.SCREEN_Y - (8 + i));
                Console.Write("████████");
                if(i < 10 )
                {
                    Console.SetCursorPosition(10, Define.SCREEN_Y - (8 + i));
                    Console.Write("███████████");
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(32, Define.SCREEN_Y - (8 + i));
                Console.Write("████");

                Console.SetCursorPosition(7, Define.SCREEN_Y - (8 + i));
                Console.Write("█████");

                if(i < 3)
                {
                    Console.SetCursorPosition(17, Define.SCREEN_Y - (8 + i));
                    Console.Write("███");
                }
            }
        }

        public void DrawStar()
        {
            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(5, 74);
                x = x % 2 == 1 ? x : x + 1; 

                int y = random.Next(8, 45 );
                y = x % 2 == 1 ? y : y + 1;

                if(x > 47)
                {
                    y = random.Next(4, 13);
                    y = x % 2 == 1 ? y : y + 1;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(x, y);
                Console.Write("⠐");
            }
        }

        public void DrawTower()//㉾
        {
            for (int i = 0; i < 27; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(45, Define.SCREEN_Y - (10 + i));
                Console.Write("█▒▒▒▒▒▒█████▒▒▒▒▒▒█");
                if (i % 3 == 0 && i != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(45, Define.SCREEN_Y - (10 + i));
                    Console.Write("≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘");
                    int j = i / 3 - 1;
                }

                if (i == 13 || i == 14)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.SetCursorPosition(45, Define.SCREEN_Y - (10 + i));
                    Console.Write("█▒▒▒▒▒▒█████▒▒▒▒▒▒█");
                }

                if (i == 25 || i == 26)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.SetCursorPosition(45, Define.SCREEN_Y - (10 + i));
                    Console.Write("█▒▒▒▒▒▒█████▒▒▒▒▒▒█");
                }
                if(i == 26)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(45, Define.SCREEN_Y - 37);
                    Console.Write("≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘≘");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(47, Define.SCREEN_Y - 40);
                    Console.Write("███████████████");
                    Console.SetCursorPosition(45, Define.SCREEN_Y - 39);
                    Console.Write("███████     ███████");
                    Console.SetCursorPosition(45, Define.SCREEN_Y - 38);
                    Console.Write("███████▃▃▃▃▃███████");
                }
            }
            //기반
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(45, Define.SCREEN_Y - 10);
            Console.Write("███████▃▃▃▃▃███████");

            Console.SetCursorPosition(43, Define.SCREEN_Y - 9);
            Console.Write("████▒▒▒▒▒▐   ▌▒▒▒▒▒████");
            Console.SetCursorPosition(41, Define.SCREEN_Y - 8);
            Console.Write("▗▟████▒▒▒▒▒▐   ▌▒▒▒▒▒████▙▖");

            //바닥
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(5, Define.SCREEN_Y - 7);
            Console.Write(new string('█', Define.SCREEN_X - 59)); //5~71 = 66

        }

        public void DrawMenu()
        {
            int x = 93;
            int y = 32;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(x+1, y);
            Console.WriteLine("⌺ 행동 선택 ⌺");
            Console.SetCursorPosition(x, y+3);
            Console.WriteLine("▟   층 선택");
            Console.SetCursorPosition(x, y+5);
            Console.WriteLine("❏   덱 셋팅");
            Console.SetCursorPosition(x, y+7);
            Console.WriteLine("⮌   메인 메뉴로");
            Console.ResetColor();
        }

        public void DrawPanel()//㉾
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, 2);
            Console.Write("┌" + new string('─', Define.SCREEN_X - 5) + "┐");

            for (int i = 3; i < Define.SCREEN_Y - 5; i++)
            {
                Console.SetCursorPosition(2, i);
                Console.Write("│" + new string(' ', Define.SCREEN_X - 5) + "│");
            }

            Console.SetCursorPosition(2, Define.SCREEN_Y - 5);
            Console.Write("└" + new string('─', Define.SCREEN_X - 5) + "┘");

            Console.SetCursorPosition(4, 3);
            Console.Write("┌" + new string('─', Define.SCREEN_X - 60) + "┐");

            for (int i = 4; i < Define.SCREEN_Y - 6; i++)
            {
                Console.SetCursorPosition(4, i);
                Console.Write("│" + new string(' ', Define.SCREEN_X - 60) + "│");
            }

            Console.SetCursorPosition(4, Define.SCREEN_Y - 6);
            Console.Write("└" + new string('─', Define.SCREEN_X - 60) + "┘");

            Console.SetCursorPosition(80, 3);
            Console.Write("┌" + new string('─', Define.SCREEN_X - 90) + "┐");

            for (int i = 4; i < Define.SCREEN_Y - 20; i++)
            {
                Console.SetCursorPosition(80, i);
                Console.Write("│" + new string(' ', Define.SCREEN_X - 90) + "│");
            }

            Console.SetCursorPosition(80, Define.SCREEN_Y - 20);
            Console.Write("└" + new string('─', Define.SCREEN_X - 90) + "┘");

            Console.SetCursorPosition(80, 31);

            Console.ResetColor();
        }
        public void DrawSelectText()//㉾
        {

            Console.SetCursorPosition(6, Define.SCREEN_Y - 46);
            Console.Write("╔═╗┌─┐ ┬  ┌─┐┌─┐┌┬┐  ╔══╗┬  ┌─┐┌─┐ ┬─ ┐  ");
            Console.SetCursorPosition(6, Define.SCREEN_Y - 45);
            Console.Write("╚═╗├┤  │  ├┤ │   │   ╠═╣ │  │ ││ │ ├ ┬┘  ");
            Console.SetCursorPosition(6, Define.SCREEN_Y - 44);
            Console.Write("╚═╝└─┘ ┴─┘└─┘└─┘ ┴   ╚   ┴─┘└─┘└─┘ ┴ └─  ");
        }
        public void RemoveSelectText()//㉾
        {

            Console.SetCursorPosition(6, Define.SCREEN_Y - 46);
            Console.Write("                                       ");
            Console.SetCursorPosition(6, Define.SCREEN_Y - 45);
            Console.Write("                                       ");
            Console.SetCursorPosition(6, Define.SCREEN_Y - 44);
            Console.Write("                                         ");
        }

        public void Update()
        {
            ConsoleKeyInfo key = Core.GetKey();

            if(key == default)
            {
                return;
            }
            else
            {
                switch(key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow :
                        Core.PlaySFX(Define.SFX_PATH + "/Click.wav");
                        if (CurrentMenu == EntranceMenu.MENU_SELECT && CursorIndex == 0)
                        {
                            return;
                        }
                        else if(CurrentMenu == EntranceMenu.MENU_SELECT)
                        {
                            CursorIndex--;
                        }

                        if (CurrentMenu == EntranceMenu.FLOOR_SELECT && CursorIndex+1 >= Core.SaveData.CurrentFloor)
                        {
                            return;
                        }
                        else if(CurrentMenu == EntranceMenu.FLOOR_SELECT)
                        {
                            CursorIndex++;
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        Core.PlaySFX(Define.SFX_PATH + "/Click.wav");
                        if (CurrentMenu == EntranceMenu.MENU_SELECT && CursorIndex == 2)
                        {
                            return;
                        }
                        else if(CurrentMenu == EntranceMenu.MENU_SELECT)
                        {
                            CursorIndex++;
                        }
                        if(CurrentMenu == EntranceMenu.FLOOR_SELECT && CursorIndex == 0)
                        {
                            return;
                        }
                        else if(CurrentMenu == EntranceMenu.FLOOR_SELECT)
                        {
                            CursorIndex--;
                        }
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        if (CurrentMenu == EntranceMenu.MENU_SELECT)
                        {
                            Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                            switch (CursorIndex)
                            {
                                case 0 :
                                    CurrentMenu = EntranceMenu.FLOOR_SELECT;
                                    CursorIndex = 0;
                                    Console.SetCursorPosition(88, 35);
                                    Console.Write("  ");
                                    Console.SetCursorPosition(84, 26);
                                    Console.Write("ENTER를 눌러 시작");
                                    Console.SetCursorPosition(84, 28);
                                    Console.Write("ESC를 눌러 뒤로가기");
                                    DrawSelectText(); //㉾
                                    break;
                                case 1:
                                    Core.LoadScene(2);
                                    return;
                                case 2:
                                    Core.LoadScene(0);
                                    return;
                            }
                        }
                        else if(CurrentMenu == EntranceMenu.FLOOR_SELECT)
                        {
                            if(Core.SaveData.Deck.Count != 10)
                            {
                                Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                                return;
                            }
                            else
                            {
                                Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                                Core.LoadScene(3);
                                return;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        Core.PlaySFX(Define.SFX_PATH + "/Card_Cancel.wav");
                        if (CurrentMenu == EntranceMenu.FLOOR_SELECT)
                        {
                            CurrentMenu = EntranceMenu.MENU_SELECT;
                            Console.SetCursorPosition(84, 26);
                            Console.Write("                     ");
                            Console.SetCursorPosition(84, 28);
                            Console.Write("                     ");
                            Console.SetCursorPosition(70, (41 - (CursorIndex * 3)));
                            Console.Write("  ");
                            RemoveSelectText();//㉾

                            CursorIndex = 0;
                        }
                        break;
                }

                DrawCursor();
            }

        }

        public void DrawFloorInfo(int index)
        {
            int x = 84;
            int y = 5;

            string blank = "                           ";

            string[] data = floorData[index+1].Split(',');

            Console.SetCursorPosition(x, y);
            Console.Write(blank);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(x, y);
            Console.Write(data[0]);
            Console.ResetColor();
            Console.SetCursorPosition(x, y+2);
            Console.Write(blank);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(x, y+2);
            Console.Write("담당자 : {0}",data[1]);
            Console.ResetColor();
            y += 4;

            int cut = 20;

            Console.SetCursorPosition(x, y);
            for (int i = 0; i < data[6].Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(data[6][i]);
                Console.ResetColor();
                if((i+1) % cut == 0)
                {
                    y++;
                    Console.SetCursorPosition(x, y);
                }
            }
        }

        public void ReleaseFloorInfo()
        {
            int x = 84;
            int y = 5;

            string blank = "                                    ";

            Console.SetCursorPosition(x, y);
            Console.Write(blank);
            Console.SetCursorPosition(x, y + 2);
            Console.Write(blank);

            y += 4;

            for(int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(blank);
            }
        }

        public void Release()
        {
        }
    }
}