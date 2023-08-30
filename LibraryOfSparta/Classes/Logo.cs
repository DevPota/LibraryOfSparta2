using LibraryOfSparta.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfSparta.Classes
{
    /* 
     * 이미지 클래스
     * public string imgName;       이미지 파일명
     * string img;                  이미지 내용
     * string[] lines;              이미지 내용을 Split('\n')
     * public int x, y;             이미지 왼쪽 위 모서리 위치
     * public int width, height;    이미지 가로길이 세로길이
     * 
     * void Redefine(string imgName)이미지 파일 재설정
     * void Draw()                  이미지 그리기
     * void Clear()                 이미지 지우기
     * void Clear(x,y)              x,y위치 이미지 지우기
     */
    public class Logo
    {
        public string imgName;
        string img;
        string[] lines;
        public int x, y;
        public int width, height;

        public Logo(int x, int y, string imgName)
        {
            this.x = x;
            this.y = y;
            this.imgName = imgName;

            img = File.ReadAllText(Define.LOCAL_GAME_PATH + "/Images/Logo/" + imgName + ".txt");
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
            string b = "";
            string c = " ";

            for (int i = x; i < x + width; i++)
            {
                b += c;
            }
            for (int j = y; j < y + height; j++)
            {
                Console.SetCursorPosition(x, j);
                Console.Write(b);
            }
        }
        public void Clear(int x, int y)
        {
            string b = "";
            string c = " ";

            for (int i = x; i < x + width; i++)
            {
                b += c;
            }
            for (int j = y; j < y + height; j++)
            {
                Console.SetCursorPosition(x, j);
                Console.Write(b);
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
        public int Maxframe;
        public bool isEnd;


        public LogoAnimation(Logo logo)
        {
            this.logo = logo;
            this.frame = 0;
            this.isEnd = false;
        }



        //급하게 만든것 쓰지말것
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
            if (frame < 5)
            {
                LeftDraw(ConsoleColor.DarkGray);
            }
            else if (frame < 10)
            {
                LeftDraw(ConsoleColor.Gray);
            }
            else if (frame < 15)
            {
                LeftDraw(ConsoleColor.White);
            }
            else if (frame < 20)
            {
                LeftDraw(ConsoleColor.White);
            }
            else if (frame < 25)
            {
                LeftDraw(ConsoleColor.White);
            }
            else if (frame < 30)
            {
                LeftDraw(ConsoleColor.Gray);
            }
            else if (frame < 35)
            {
                LeftDraw(ConsoleColor.DarkGray);
            }
            else if (frame == 35)
            {
                logo.Clear(logo.x - frame, logo.y); frame++;
            }
            else if (frame == 40)
            {
                //logo.Clear(logo.x - frame, logo.y);
                //frame++;
                isEnd = true;
                frame = 0;
            }
            else frame++;

            Console.SetCursorPosition(0, 0);
        }

        public void LeftClear()
        {
            for (int j = logo.y; j < logo.y + logo.height; j++)
            {
                Console.SetCursorPosition(logo.x + frame, j);
                Console.Write(' ');
            }
        }
        public void RightDraw(ConsoleColor FontColor)
        {
            RightClear();
            Console.ForegroundColor = FontColor;
            logo.Draw(logo.x + frame, logo.y);
            Console.ResetColor();
            frame++;
        }
        public void FadeRight()
        {
            if (frame < 5)
            {
                RightDraw(ConsoleColor.DarkGray);
            }
            else if (frame < 10)
            {
                RightDraw(ConsoleColor.Gray);
            }
            else if (frame < 15)
            {
                RightDraw(ConsoleColor.White);
            }
            else if (frame < 20)
            {
                RightDraw(ConsoleColor.White);
            }
            else if (frame < 25)
            {
                RightDraw(ConsoleColor.White);
            }
            else if (frame < 30)
            {
                RightDraw(ConsoleColor.Gray);
            }
            else if (frame < 35)
            {
                RightDraw(ConsoleColor.DarkGray);
            }
            else if (frame == 35)
            {
                logo.Clear(logo.x + frame, logo.y); frame++;
            }
            else if (frame == 40)
            {
                //logo.Clear(logo.x - frame, logo.y);
                //frame++;
                isEnd = true;
                frame = 0;
            }
            else frame++;

            Console.SetCursorPosition(0, 0);
        }

        public void CreditImgRight()
        {
            if (frame < 5) MoveLeft(1, ConsoleColor.DarkGray);
            else if (frame < 10) MoveLeft(1, ConsoleColor.Gray);
            else if (frame < 40) Draw(ConsoleColor.White);
            else if (frame < 45) MoveRight(1, ConsoleColor.Gray);
            else if (frame < 50) MoveRight(1, ConsoleColor.DarkGray);
            else if(frame == 50)
            {
                Clear();
                frame = 0;
                isEnd = true;
            }
        }
        public void CreditImgLeft()
        {
            if (frame < 5) MoveRight(1, ConsoleColor.DarkGray);
            else if (frame < 10) MoveRight(1, ConsoleColor.Gray);
            else if (frame < 40) Draw(ConsoleColor.White);
            else if (frame < 45) MoveLeft(1, ConsoleColor.Gray);
            else if (frame < 50) MoveLeft(1, ConsoleColor.DarkGray);
            else if (frame == 50)
            {
                Clear();
                frame = 0;
                isEnd = true;
            }
        }


        //애니메이션 끝
        public void Release()
        {
            isEnd = true;
        }
        
        //이미지 그림
        public void Draw()
        {
            logo.Draw();
            frame++;
        }
        //해당 컬러로 이미지 그림
        public void Draw(ConsoleColor Fontcolor)
        {
            Console.ForegroundColor = Fontcolor;
            logo.Draw();
            Console.ResetColor();
            frame++;
        }

        //이미지 지움
        public void Clear()
        {
            logo.Clear();
            frame++;
        }

        //해당 위치로 이동시킨다
        public void SetPosition(int x, int y, ConsoleColor Fontcolor)
        {
            logo.Clear();
            Console.ForegroundColor = Fontcolor;
            logo.Relocation(x, y);
            Console.ResetColor();
            frame++;
        }

        //해당 방향으로 distance만큼 옮기고 Draw()
        public void MoveLeft(int distance, ConsoleColor Fontcolor)
        {
            for (int j = logo.y; j < logo.y + logo.height; j++)
            {
                Console.SetCursorPosition(logo.x + logo.width-2, j);
                Console.Write(' ');
            }

            Console.ForegroundColor = Fontcolor;
            logo.x -= distance;
            logo.Draw(logo.x, logo.y);
            Console.ResetColor();
            frame++;
        }
        public void MoveRight(int distance, ConsoleColor Fontcolor)
        {
            for (int j = logo.y; j < logo.y + logo.height; j++)
            {
                Console.SetCursorPosition(logo.x, j);
                Console.Write(' ');
            }

            Console.ForegroundColor = Fontcolor;
            logo.x += distance;
            logo.Draw(logo.x, logo.y);
            Console.ResetColor();
            frame++;
        }
        public void MoveUp(int distance, ConsoleColor Fontcolor)
        {
            for (int j = logo.x; j < logo.x + logo.width; j++)
            {
                Console.SetCursorPosition(j, logo.y + logo.height - 1);
                Console.Write(' ');
            }

            Console.ForegroundColor = Fontcolor;
            logo.y -= distance;
            logo.Draw(logo.x, logo.y);
            Console.ResetColor();
            frame++;
        }
        public void MoveDown(int distance, ConsoleColor Fontcolor)
        {
            for (int j = logo.x; j < logo.x + logo.width; j++)
            {
                Console.SetCursorPosition(j, logo.y);
                Console.Write(' ');
            }

            Console.ForegroundColor = Fontcolor;
            logo.y += distance;
            logo.Draw(logo.x, logo.y);
            Console.ResetColor();
            frame++;
        }

        //색깔 바꾸기
        public void ChangeColor(ConsoleColor Fontcolor)
        {
            Console.ForegroundColor = Fontcolor;
            logo.Draw();
            Console.ResetColor();
            frame++;
        }

    }

    public class LogoAnimator
    {
        

    }
}
