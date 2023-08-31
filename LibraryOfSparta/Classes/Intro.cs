using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfSparta.Classes
{
    public class Intro:Scene
    {
        Logo TeamLogo;
        LogoAnimation TeamLogoAnimation;
        Logo Sparta;
        LogoAnimation SpartaAnimation;

        List<LogoAnimation> LogoAnimationList;
        int Acursor;

        int skipstack;

        public void Init()
        {
            Console.CursorVisible = false;

            Sparta = new Logo(Define.SCREEN_X / 2 - 18, Define.SCREEN_Y / 2 - 16, "intro_Spartan");
            SpartaAnimation = new LogoAnimation(Sparta);
            TeamLogo = new Logo(Define.SCREEN_X / 2 - 25, Define.SCREEN_Y / 2 - 15, "intro_Team");
            TeamLogoAnimation = new LogoAnimation(TeamLogo);

            LogoAnimationList = new List<LogoAnimation>();
            LogoAnimationList.Add(SpartaAnimation);
            LogoAnimationList.Add(TeamLogoAnimation);
            Acursor = 0;
            
            Core.PlaySFX(Define.SFX_PATH + "/Sparta.wav");

            skipstack = 0;

        }

        public void Update() 
        {
            //애니메이션
            if (LogoAnimationList[Acursor].isEnd == false)
            {
                LogoAnimationList[Acursor].Fade();
            }
            else
            {
                LogoAnimationList[Acursor].isEnd = false;
                Acursor++;
                if (Acursor == LogoAnimationList.Count)
                {
                    Core.LoadScene(0); return;
                }
            }

            //스킵기능
            ConsoleKeyInfo key = Core.GetKey();
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
                        Core.LoadScene(0);
                        return;
                    }
                    break;
            }

        }
    }
}
