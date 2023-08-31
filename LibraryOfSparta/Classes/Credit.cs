using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfSparta.Classes
{


    public class Credit : Scene
    {
        char borderin = '■';
        char borderout = '〓';
        Logo CREDIT = new Logo(Define.SCREEN_X / 2 - 23, 1, "img_LargeCredit");

        //팀원 초상화
        Logo teammate1 = null;
        LogoAnimation teammate1_anim = null;
        Logo teammate2 = null;
        LogoAnimation teammate2_anim = null;
        Logo teammate3 = null;
        LogoAnimation teammate3_anim = null;
        Logo teammate4 = null;
        LogoAnimation teammate4_anim = null;
        Logo teammate5 = null;
        LogoAnimation teammate5_anim = null;
        int portrait_cursor;
        //팀원 역할
        Logo role_1 = null;
        LogoAnimation role_1_anim = null;
        Logo role_2 = null;
        LogoAnimation role_2_anim = null;
        Logo role_3 = null;
        LogoAnimation role_3_anim = null;
        Logo role_4 = null;
        LogoAnimation role_4_anim = null;
        Logo role_5 = null;
        LogoAnimation role_5_anim = null;
        int role_cursor;

        List<LogoAnimation> portrait;
        List<LogoAnimation> role;


        int skipstack;

        public void Init()
        {
            //커서 안보이기
            Console.CursorVisible = false;

            //화면 테두리와 CREDIT 그리기
            for (int i = 0; i < Define.SCREEN_X - 1; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write(borderout);
                Console.SetCursorPosition(i, Define.SCREEN_Y - 2);
                Console.Write(borderout);
            }
            for (int i = 0; i < Define.SCREEN_X - 1; i++)
            {
                Console.SetCursorPosition(i, 3);
                Console.Write(borderin);
                Console.SetCursorPosition(i, Define.SCREEN_Y - 3);
                Console.Write(borderin);
            }
            CREDIT.Draw();


            //애니메이션 List
            portrait = new List<LogoAnimation>();
            portrait_cursor = 0;
            role = new List<LogoAnimation>();
            role_cursor = 0;

            //팀원 초상화
            teammate1 = new Logo(85, 12, "img_HaeChanKim");
            teammate1_anim = new LogoAnimation(teammate1);
            teammate2 = new Logo(5, 12, "img_HongJunLee");
            teammate2_anim = new LogoAnimation(teammate2);
            teammate3 = new Logo(85, 15, "img_MinGyuWoo");
            teammate3_anim = new LogoAnimation(teammate3);
            teammate4 = new Logo(5, 12, "img_GyungHyunLee");
            teammate4_anim = new LogoAnimation(teammate4);
            teammate5 = new Logo(85, 15, "img_SaeJinKim");
            teammate5_anim = new LogoAnimation(teammate5);

            portrait.Add(teammate1_anim);
            portrait.Add(teammate2_anim);
            portrait.Add(teammate3_anim);
            portrait.Add(teammate4_anim);
            portrait.Add(teammate5_anim);

            //팀원 역할
            role_1 = new Logo(1, 10, "img_HaeChanKim_Role");
            role_1_anim = new LogoAnimation(role_1);
            role_2 = new Logo(80, 10, "img_HongJunLee_Role");
            role_2_anim = new LogoAnimation(role_2);
            role_3 = new Logo(1, 10, "img_MinGyuWoo_Role");
            role_3_anim = new LogoAnimation(role_3);
            role_4 = new Logo(80, 10, "img_GyungHyunLee_Role");
            role_4_anim = new LogoAnimation(role_4);
            role_5 = new Logo(5, 10, "img_SaeJinKim_Role");
            role_5_anim = new LogoAnimation(role_5);

            role.Add(role_1_anim);
            role.Add(role_2_anim);
            role.Add(role_3_anim);
            role.Add(role_4_anim);
            role.Add(role_5_anim);


            skipstack = 0;
        }

        public void Update()
        {
            //애니메이션
            if (portrait[portrait_cursor].isEnd)
            {
                portrait[portrait_cursor].isEnd = false;
                //portrait_cursor = (portrait_cursor < portrait.Count)? portrait_cursor + 1 : 0;
                portrait_cursor = portrait_cursor + 1;
                
            }
            //애니메이션 다 끝나면 타이틀 화면으로
            if (!(portrait_cursor < portrait.Count))
            {
                Console.Clear();
                portrait_cursor = 0;
                Core.LoadScene(0);
                return;
            }

            if (portrait_cursor % 2 == 0) portrait[portrait_cursor].CreditImgRight();
            else portrait[portrait_cursor].CreditImgLeft();

            if (role[role_cursor].isEnd)
            {
                role[role_cursor].isEnd = false;
                role_cursor = (role_cursor < role.Count - 1) ? role_cursor + 1 : 0;
            }
            if (portrait_cursor % 2 == 0) role[role_cursor].CreditImgLeft();
            else role[role_cursor].CreditImgRight();

            




            
            ConsoleKeyInfo key = Core.GetKey();

            if (key == default)
            {
                return;
            }

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
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
