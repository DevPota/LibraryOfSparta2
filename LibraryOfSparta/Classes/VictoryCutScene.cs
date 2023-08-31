using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfSparta.Classes
{
    public class VictoryCutScene:Scene
    {
        int max = 3;
        int min = 0;
        int cursor_x = Define.SCREEN_X - 55;
        int cursor_y = Define.SCREEN_Y - 6;
        int cursormin_y = Define.SCREEN_Y - 10;
        int cursormax_y = Define.SCREEN_Y - 6;
        int cursorblank;

        public int Level;
        public int Floor;

        Logo Check;
        LogoAnimation CheckPaternAnimation;
        Logo Lock;
        LogoAnimation LockAnimation;
        Logo UnLock;
        LogoAnimation UnLockAnimation;

        public void DrawWalls()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("┏");
            for (int i = 1; i < Define.SCREEN_X - 3; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("━");
            }
            Console.SetCursorPosition(Define.SCREEN_X-2, 0);
            Console.Write("┓");

            for (int i = 1; i < Define.SCREEN_Y - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("┃");
                Console.SetCursorPosition(Define.SCREEN_X - 2, i);
                Console.Write("┃");
            }

            for (int i = 1; i < Define.SCREEN_X-3; i++)
            {
                Console.SetCursorPosition(i, Define.SCREEN_Y - 5);
                Console.Write("━");
            }

            Console.SetCursorPosition(0, Define.SCREEN_Y-1);
            Console.Write("┗");
            for (int i = 1; i < Define.SCREEN_X-2; i++)
            {
                Console.SetCursorPosition(i, Define.SCREEN_Y - 1);
                Console.Write("━");
            }
            Console.SetCursorPosition(Define.SCREEN_X - 2, Define.SCREEN_Y - 1);
            Console.Write("┛");
        }
        public int GetEnemyEmotionLevel()
        {
            return Level;
        }

        public void Init(int Level, int Floor)
        {
            this.Level = Level;
            this.Floor = Floor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            DrawWalls();
            Console.ResetColor();

            Check = new Logo(6, 2, "win_Check");
            CheckPaternAnimation = new LogoAnimation(Check);

            Lock = new Logo(Define.SCREEN_X / 2 - 8, Define.SCREEN_Y / 2 - 8, "win_Lock");
            LockAnimation = new LogoAnimation(Lock);

            UnLock = new Logo(Define.SCREEN_X / 2 - 8, Define.SCREEN_Y / 2 - 10, "win_UnLock");
            UnLockAnimation = new LogoAnimation(UnLock);

            Console.SetCursorPosition(Define.SCREEN_X / 2 - 14, Define.SCREEN_Y - 12);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("아무 키를 눌러 카드를 획득");
            Console.ResetColor();
        }
        bool trigger = false;
        public void Update()
        {
            ConsoleKeyInfo key = Core.GetKey();

            if(key != default && trigger == false)
            {
                Console.SetCursorPosition(Define.SCREEN_X / 2 - 14, Define.SCREEN_Y - 12);
                Console.Write("                                        ");
                trigger = true;
                Core.PlaySFX(Define.SFX_PATH + "/Gatcha.wav");
            }

            if(trigger == true)
            {
                if (CheckPaternAnimation.isEnd == false)
                {
                    CheckPaternAnimation.check(13);
                    LockAnimation.Draw(ConsoleColor.Yellow);
                }
                else
                {
                    LockAnimation.Clear();
                    CheckPaternAnimation.blink();
                    UnLockAnimation.unlock();
                    if (UnLockAnimation.isEnd == true)
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/Dead.wav");
                        Core.LoadScene(10);
                        return;
                    }
                }
            }
        }
    }
}
