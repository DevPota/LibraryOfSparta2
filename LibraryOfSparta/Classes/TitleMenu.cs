using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace LibraryOfSparta.Classes
{
    public class TitleMenu : Scene
    {
        int AnimationCursor;
        List<LogoAnimation> AnimationList = null;
        LogoAnimation animation  = null;
        LogoAnimation animation2 = null;
        LogoAnimation animation3 = null;
        LogoAnimation animation4 = null;
        LogoAnimation animation5 = null;
        LogoAnimation animation6 = null;
        LogoAnimation animation7 = null;
        LogoAnimation animation8 = null;
        LogoAnimation animation9 = null;


        BorderCursor Bcursor;
        int Icursor;

        List<Logo> cursorAbleLogo;

        public void Init()
        {
            Console.CursorVisible = false;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Core.PlayPlayerBGM(Define.BGM_PATH + "/Menu.wav");

            Console.Clear();
            Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);

            //타이틀 로고
            Logo LibraryOfSparta = new Logo(2, 6, "img_LibraryOfSparta");
            LibraryOfSparta.Draw();

            //커서가 사용할 로고
            Logo Play = new Logo(5, 20, "img_Play");
            Play.Draw();

            Logo Credit = new Logo(5, 28, "img_Credit");
            Credit.Draw();

            Logo Quit = new Logo(5, 36, "img_Quit");
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
            AnimationCursor = 0;
            Logo Keter = new Logo(60, 20, "title_Keter2");
            Logo Malkuth = new Logo(25, 20, "title_Malkuth");
            Logo Binah = new Logo(60, 20, "title_Binah");
            Logo Chesed = new Logo(25, 20, "title_Chesed");
            Logo Gebura = new Logo(60, 20, "title_Gebura");
            Logo Hod = new Logo(25, 20, "title_Hod");
            Logo Hokma = new Logo(60, 20, "title_Hokma");
            Logo Netzach = new Logo(25, 20, "title_Netzach");
            Logo Yesod = new Logo(60, 20, "title_Yesod");

            animation = new LogoAnimation(Keter);
            animation2 = new LogoAnimation(Malkuth);
            animation3 = new LogoAnimation(Binah);
            animation4 = new LogoAnimation(Chesed);
            animation5 = new LogoAnimation(Gebura);
            animation6 = new LogoAnimation(Hod);
            animation7 = new LogoAnimation(Hokma);
            animation8 = new LogoAnimation(Netzach);
            animation9 = new LogoAnimation(Yesod);

            AnimationList = new List<LogoAnimation>(); 
            AnimationList.Add(animation);
            AnimationList.Add(animation2);
            AnimationList.Add(animation3);
            AnimationList.Add(animation4);
            AnimationList.Add(animation5);
            AnimationList.Add(animation6);
            AnimationList.Add(animation7);
            AnimationList.Add(animation8);
            AnimationList.Add(animation9);

            Console.ResetColor();

            DrawInfoText();
        }

        public void Update()
        {
            //animation
            if (AnimationList[AnimationCursor].isEnd)
            {
                AnimationList[AnimationCursor].isEnd = false;
                AnimationCursor = (AnimationCursor < AnimationList.Count-1) ? AnimationCursor+1 : 0;
            }
            if (AnimationCursor % 2 == 0) AnimationList[AnimationCursor].FadeLeft();
            else AnimationList[AnimationCursor].FadeRight();


            ConsoleKeyInfo key = Core.GetKey();

            if(key == default)
            {
                return;
            }

            switch (key.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                    if (Icursor > 0) Bcursor.Draw(cursorAbleLogo[--Icursor]);
                    else Icursor = cursorAbleLogo.Count - 1; Bcursor.Draw(cursorAbleLogo[Icursor]);
                    break;

                case ConsoleKey.S:
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
                            Core.LoadScene(5);
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

            string keyinfo =  "||   방향키, WASD : 이동   ||";
            string keyinfo2 = "|| 엔터, 스페이스바 : 확인 ||";
            Console.SetCursorPosition(1, Define.SCREEN_Y - 3);
            Console.WriteLine(keyinfo);
            Console.SetCursorPosition(1, Define.SCREEN_Y - 2);
            Console.WriteLine(keyinfo2);


            string devinfo = "팀 내배캠 부수기";
            Console.SetCursorPosition(Define.SCREEN_X - devinfo.Length - 10, Define.SCREEN_Y - 2);
            Console.WriteLine(devinfo);


        }
    }


    public class BorderCursor
    {
        public Logo logo;
        char lt = '╔';
        char tb = '═';
        char rt = '╦';
        char lr = '║';
        char lb = '╚';
        char rb = '╩';
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

    
}

