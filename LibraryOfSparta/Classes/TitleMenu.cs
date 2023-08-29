using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace LibraryOfSparta.Classes
{
    public class TitleMenu : Scene
    {
        LogoAnimation animation  = null;
        LogoAnimation animation2 = null;

        BorderCursor Bcursor;
        int Icursor;

        List<Logo> cursorAbleLogo;

        [DllImport("user32.dll")]
        public static extern int ShowCursor(bool bShow);

        public void Init()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Core.PlayPlayerBGM(Define.BGM_PATH + "/Menu.wav");

            ShowCursor(false);

            Console.Clear();
            Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);

            //타이틀 로고
            Logo LibraryOfSparta = new Logo(3, 7, "img_LibraryOfSparta");
            LibraryOfSparta.Draw();

            //커서가 사용할 로고
            Logo Play = new Logo(5, 22, "img_Play");
            Play.Draw();

            Logo Credit = new Logo(5, 30, "img_Credit");
            Credit.Draw();

            Logo Quit = new Logo(5, 38, "img_Quit");
            Quit.Draw();

            cursorAbleLogo = new List<Logo>();
            cursorAbleLogo.Add(Play);
            cursorAbleLogo.Add(Credit);
            cursorAbleLogo.Add(Quit);



            //커서
            Bcursor = new BorderCursor();
            Icursor = 0;
            Bcursor.Draw(cursorAbleLogo[Icursor]);



            //페이드인아웃 애니메이션
            
            Logo Keter = new Logo(90, 20, "img_Keter2");

            animation = new LogoAnimation(Keter);
            animation2 = new LogoAnimation(Keter);


            Console.ResetColor();

            DrawInfoText();
        }
        public void Update()
        {
            //animation
            animation.FadeLeft();

            ConsoleKeyInfo key = Core.GetKey();

            if(key == default)
            {
                return;
            }

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                    if (Icursor > 0) Bcursor.Draw(cursorAbleLogo[--Icursor]);
                    else Icursor = cursorAbleLogo.Count - 1; Bcursor.Draw(cursorAbleLogo[Icursor]);
                    break;

                case ConsoleKey.DownArrow:
                    Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                    if (Icursor < cursorAbleLogo.Count - 1) Bcursor.Draw(cursorAbleLogo[++Icursor]);
                    else Icursor = 0; Bcursor.Draw(cursorAbleLogo[Icursor]);
                    break;

                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                    switch (Bcursor.logo.imgName)
                    {
                        case "img_Play":
                            Core.LoadScene(1);
                            return;
                        case "img_Credit":
                            return;
                        case "img_Quit":
                            Program.Exit();
                            return;
                    }
                    break;
            }

            DrawInfoText();
        }

        public void DrawInfoText()
        {
            string info = "";

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            if(Icursor == 0)
            {
                info = "게임을 시작합니다";
            }
            else if(Icursor == 1)
            {
                info = "크레딧을 감상합니다";
            }
            else
            {
                info = "게임을 종료 합니다";
            }

            Console.SetCursorPosition((Define.SCREEN_X - 20) / 2, Define.SCREEN_Y - 2);
            Console.Write("                                                                          ");
            Console.SetCursorPosition((Define.SCREEN_X - 20) / 2, Define.SCREEN_Y - 2);
            Console.Write(info);

            Console.ResetColor();
        }
    }

    public class BorderCursor
    {
        public Logo logo;
        char lt = '┌';
        char tb = '─';
        char rt = '┐';
        char lr = '│';
        char lb = '└';
        char rb = '┘';
        char blank = ' ';

        public void Clear()
        {
            Console.SetCursorPosition(logo.x - 1, logo.y - 1);
            Console.Write(blank);
            Console.SetCursorPosition(logo.x + logo.width, logo.y - 1);
            Console.Write(blank);
            Console.SetCursorPosition(logo.x - 1, logo.y + logo.height);
            Console.Write(blank);
            Console.SetCursorPosition(logo.x + logo.width, logo.y + logo.height);
            Console.Write(blank);

            //Draw Side
            for (int i = logo.x; i < logo.x + logo.width; i++)
            {
                Console.SetCursorPosition(i, logo.y - 1);
                Console.Write(blank);
                Console.SetCursorPosition(i, logo.y + logo.height);
                Console.Write(blank);
            }
            for (int i = logo.y; i < logo.y + logo.height; i++)
            {
                Console.SetCursorPosition(logo.x - 1, i);
                Console.Write(blank);
                Console.SetCursorPosition(logo.x + logo.width, i);
                Console.Write(blank);
            }
        }

        public void Draw(Logo logo)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (this.logo != null) this.Clear();

            this.logo = logo;
            //Draw Edge
            Console.SetCursorPosition(logo.x - 1, logo.y - 1);
            Console.Write(lt);
            Console.SetCursorPosition(logo.x + logo.width, logo.y - 1);
            Console.Write(rt);
            Console.SetCursorPosition(logo.x - 1, logo.y + logo.height);
            Console.Write(lb);
            Console.SetCursorPosition(logo.x + logo.width, logo.y + logo.height);
            Console.Write(rb);

            //Draw Side
            for (int i = logo.x; i < logo.x + logo.width; i++)
            {
                Console.SetCursorPosition(i, logo.y - 1);
                Console.Write(tb);
                Console.SetCursorPosition(i, logo.y + logo.height);
                Console.Write(tb);
            }
            for (int i = logo.y; i < logo.y + logo.height; i++)
            {
                Console.SetCursorPosition(logo.x - 1, i);
                Console.Write(lr);
                Console.SetCursorPosition(logo.x + logo.width, i);
                Console.Write(lr);
            }
            Console.SetCursorPosition(0, 0);

            Console.ResetColor();
        }
    }

    public class Logo
    {
        public string imgName;
        string img;
        string[] lines;
        public int x, y;
        public int width, height;
        char blank = ' ';

        public Logo(int x, int y, string imgName)
        {
            this.x = x;
            this.y = y;
            this.imgName = imgName;

            img = File.ReadAllText(Define.LOCAL_GAME_PATH + "/Images/" + imgName + ".txt");
            lines = img.Split('\n');

            width = lines[0].Length;
            height = lines.Length;
        }
        public void Redefine(string imgName)
        {
            this.imgName = imgName;

            img = File.ReadAllText(Define.LOCAL_GAME_PATH + "/Images/" + imgName + ".txt");
            lines = img.Split('\n');

            width = lines[0].Length;
            height = lines.Length;
        }


        public void Draw()
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(lines[i]);
            }
        }
        public void Draw(int x, int y)
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(lines[i]);
            }
        }

        public void Clear()
        {
            for (int i = x; i < x+width; i++)
            {
                for (int j = y; j < y+height; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(blank);
                }
            }
        }
        public void Clear(int x, int y)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(blank);
                }
            }
        }

        public void Relocation(int x, int y)
        {
            //this.Clear();
            this.x = x;
            this.y = y;
            this.Draw();
        }
    }

    public class LogoAnimation
    {

        public Logo logo;
        public int frame;
        int Maxframe = 70;
        bool isEnd;
        

        public LogoAnimation(Logo logo)
        {
            this.logo = logo;
            this.frame = 0;
            this.isEnd = false;
        }

        public void Release()
        {
            isEnd = true;
        }

        public void Draw()
        {
            logo.Draw();
            frame++;
        }

        public void RightClear() 
        {
            for (int j = logo.y; j < logo.y + logo.height; j++)
            {
                Console.SetCursorPosition(logo.x + logo.width - 1 - frame, j);
                Console.Write(' ');
            }
        }
        public void LeftDraw(ConsoleColor FontColor)
        {
            RightClear();
            Console.ForegroundColor = FontColor;
            logo.Draw(logo.x - frame, logo.y);
            Console.ResetColor();
            frame++;
        }
        public void FadeLeft()
        {
            if (frame < 7)
            {
                LeftDraw(ConsoleColor.DarkGray);
            }
            else if (frame < 14)
            {
                LeftDraw(ConsoleColor.Gray);
            }
            else if (frame < 21)
            {
                LeftDraw(ConsoleColor.White);
            }
            else if (frame < 35)
            {
                LeftDraw(ConsoleColor.White);
            }
            else if (frame < 42)
            {
                LeftDraw(ConsoleColor.White);
            }
            else if (frame < 49)
            {
                LeftDraw(ConsoleColor.Gray);
            }
            else if (frame < 56)
            {
                LeftDraw(ConsoleColor.DarkGray);
            }
            else if (frame == 56)
            {
                logo.Clear(logo.x - frame, logo.y);
                frame++;
            }
            else if (frame < Maxframe)
            {
                frame++;
            }
            else if (frame == Maxframe)
            {
                isEnd = true;
                frame = 0;
            }

            Console.SetCursorPosition(0, 0);
        }
    }
}

