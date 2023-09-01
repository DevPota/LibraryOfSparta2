using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfSparta.Classes
{
    public class BattleIntro:Scene
    {
        public int cursorIndex;

        List<LogoAnimation> AnimationList = null;

        LogoAnimation animation0 = null;
        LogoAnimation animation1 = null;
        LogoAnimation animation2 = null;
        LogoAnimation animation3 = null;
        LogoAnimation animation4 = null;
        LogoAnimation animation5 = null;
        LogoAnimation animation6 = null;
        LogoAnimation animation7 = null;
        LogoAnimation animation8 = null;
        LogoAnimation animation9 = null;

        int Acursor;

        int skipstack;


        public BattleIntro(int cursorIndex)
        {
            this.cursorIndex = cursorIndex;
        }

        public void Init()
        {
            AnimationList= new List<LogoAnimation>();
            Acursor = 0;

            skipstack = 0;

            Logo Malkuth = new Logo(40, 20, "title_Malkuth");
            Logo Yesod = new Logo(40, 20, "title_Yesod");
            Logo Hod = new Logo(40, 20, "title_Hod");
            Logo Netzach = new Logo(40, 20, "title_Netzach");
            Logo Tiphereth = new Logo(40, 20, "title_Tiphereth");
            Logo Gebura = new Logo(40, 20, "title_Gebura");
            Logo Chesed = new Logo(40, 20, "title_Chesed");
            Logo Binah = new Logo(40, 20, "title_Binah");
            Logo Hokma = new Logo(40, 20, "title_Hokma");
            Logo Keter = new Logo(40, 20, "title_Keter");

            animation0 = new LogoAnimation(Malkuth);
            animation1 = new LogoAnimation(Yesod);
            animation2 = new LogoAnimation(Hod);
            animation3 = new LogoAnimation(Netzach);
            animation4 = new LogoAnimation(Tiphereth);
            animation5 = new LogoAnimation(Gebura);
            animation6 = new LogoAnimation(Chesed);
            animation7 = new LogoAnimation(Binah);
            animation8 = new LogoAnimation(Hokma);
            animation9 = new LogoAnimation(Keter);

            AnimationList = new List<LogoAnimation>();
            AnimationList.Add(animation0);
            AnimationList.Add(animation1);
            AnimationList.Add(animation2);
            AnimationList.Add(animation3);
            AnimationList.Add(animation4);
            AnimationList.Add(animation5);
            AnimationList.Add(animation6);
            AnimationList.Add(animation7);
            AnimationList.Add(animation8);
            AnimationList.Add(animation9);

            for(int i = 0; i < AnimationList.Count; i++)
            {
                AnimationList[i].Relocation(Define.SCREEN_X / 2 - 25
                                          , Define.SCREEN_Y / 2 - 15);
            }


            
        }

        public void Update()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(cursorIndex);
            if (AnimationList[cursorIndex].isEnd == false)
            {
                switch (Acursor)
                {
                    case 0:
                        AnimationList[cursorIndex].SmokeDirectDown();
                        break;
                    case 1:
                        AnimationList[cursorIndex].blinkRed();
                        break;
                }
            }
            else
            {
                Acursor++;
                AnimationList[cursorIndex].isEnd = false;
                if (Acursor >= 2)
                {
                    Console.Clear();
                    Core.LoadScene(3);
                    return;
                }
            }

            ConsoleKeyInfo key = Core.GetKey();

            if (key == default)
            {
                return;
            }

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    if (skipstack < 1)
                    {
                        Console.SetCursorPosition(Define.SCREEN_X - 20, Define.SCREEN_Y - 4);
                        Console.Write("■ [SPACE] SKIP ■");
                        skipstack++;
                    }
                    else
                    {
                        Core.LoadScene(3);
                        return;
                    }
                    break;
            }

        }
    }
}
