using System.IO;
using System.Threading.Tasks;
using LibraryOfSparta.Common;

namespace LibraryOfSparta.Classes
{
    public class Battle
    {
        public void Init()
        {
            Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);

            char lt = '┌';
            char tb = '─';
            char rt = '┐';
            char lr = '│';
            char lb = '└';
            char rb = '┘';


            // Enemy
            int leftTopX = 1;
            int leftTopY = 1;
            int leftTopEndX = 80;
            int leftTopEndY = 30;

            // enemy Border
            Console.SetCursorPosition(leftTopX, leftTopY);

            Console.Write(lt);
            for (int i = 1; i < leftTopEndX - 1; i++)
            {
                Console.Write(tb);
            }
            Console.Write(rt);

            // enemy border left right
            for (int i = leftTopY + 1; i < leftTopEndY - 1; i++)
            {
                Console.SetCursorPosition(leftTopX, i);
                Console.Write(lr);
                Console.SetCursorPosition(leftTopEndX, i);
                Console.Write(lr);
            }

            // enemy border bottom
            Console.SetCursorPosition(leftTopX, leftTopEndY - 1);

            Console.Write(lb);
            for (int i = 1; i < leftTopEndX - 1; i++)
            {
                Console.Write(tb);
            }
            Console.Write(rb);

            // Draw enemy
            RenderEnemyName();
            RenderEnemyHPBar();
            RenderEnemyATBBar();
            DrawEnemy();

            // Card Queue
            int rightTopX = 82;
            int rightTopY = 3;
            int rightTopEndX = 128;
            int rightTopEndY = 20;

            for (int i = rightTopY; i < rightTopEndY; i++)
            {
                Console.SetCursorPosition(rightTopX, i);
                Console.Write('|');

                Console.SetCursorPosition(rightTopX + 20, i);
                Console.Write('|');
            }

            // Card Queue Indexing
            List<string> hands = new List<string>();
            hands.Add("1 코스트\n가벼운 공격");
            hands.Add("2 코스트\n집중 공격");
            hands.Add("1 코스트\n가벼운 공격");
            hands.Add("1 코스트\n회피");

            int handStart = rightTopEndY - 1;

            string cardFrame = new string('─', 16);

            for (int i = 0; i < hands.Count; i++)
            {
                string[] temp = hands[i].Split('\n');

                Console.SetCursorPosition(rightTopX + 2, handStart);
                Console.Write(cardFrame);
                Console.SetCursorPosition(rightTopX + 2, handStart - 1);
                Console.Write(temp[1]);
                Console.SetCursorPosition(rightTopX + 2, handStart - 2);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(temp[0]);
                Console.ResetColor();
                Console.SetCursorPosition(rightTopX + 2, handStart - 3);
                Console.Write(cardFrame);
                Console.SetCursorPosition(rightTopX + 22, handStart - 1);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[{0}]", 1 + i);
                Console.ResetColor();

                handStart -= 4;
            }

            // Cost and player Cost ATB
            char side = '|';
            char bar = '█';

            Console.SetCursorPosition(rightTopX, rightTopEndY + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("보유 코스트 : {0}/{1}", 4, 10);

            Console.SetCursorPosition(rightTopX, rightTopEndY + 3);
            Console.Write(" ≫ 코스트");
            Console.SetCursorPosition(rightTopX, rightTopEndY + 4);
            Console.ResetColor();
            Console.Write(side);
            Console.ForegroundColor = ConsoleColor.Yellow;

            int atbCostFill = 14;
            int atbCostEmpty = 11;
            Console.SetCursorPosition(rightTopX + 1, rightTopEndY + 4);
            for (int i = 0; i < atbCostFill; i++)
            {
                Console.Write(bar);

            }
            Console.ResetColor();
            Console.SetCursorPosition(rightTopX + atbCostFill, rightTopEndY + 4);
            for (int i = 0; i < atbCostEmpty; i++)
            {
                Console.Write(' ');

            }
            Console.Write(side);

            // player Draw ATB

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.SetCursorPosition(rightTopX, rightTopEndY + 6);
            Console.Write(" 心 카드 드로우");
            Console.SetCursorPosition(rightTopX, rightTopEndY + 7);
            Console.ResetColor();
            Console.Write(side);
            Console.ForegroundColor = ConsoleColor.Cyan;

            int atbDrawFill = 7;
            int atbDrawEmpty = 18;
            Console.SetCursorPosition(rightTopX + 1, rightTopEndY + 7);
            for (int i = 0; i < atbDrawFill; i++)
            {
                Console.Write(bar);

            }
            Console.ResetColor();
            Console.SetCursorPosition(rightTopX + atbDrawFill, rightTopEndY + 7);
            for (int i = 0; i < atbDrawEmpty; i++)
            {
                Console.Write(' ');

            }
            Console.Write(side);

            // player status
            int leftBottomX = 3;
            int leftBottomY = 31;
            int leftBottomEndX = 80;
            int leftBottomEndY = 46;

            // Draw dialog
            int dialogX = leftBottomX;
            int dialogY = leftBottomY - 1;

            Console.SetCursorPosition(dialogX, dialogY);
            Console.Write("{0}의 {1}!", "당신", "가벼운 공격");
            Console.SetCursorPosition(dialogX, dialogY + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}는 {1} 대미지를 받았다!", "박종민 매니저", "10");

            // Draw HPBar
            int hpBarX = leftBottomX;
            int hpBarY = leftBottomY + 2;
            int currentHP = 24;
            int maxHPBar = 50;

            Console.SetCursorPosition(hpBarX, hpBarY);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("체력");
            Console.ResetColor();


            Console.SetCursorPosition(hpBarX + 4, hpBarY);
            Console.Write(side);

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < currentHP; i++)
            {
                Console.Write(bar);
            }
            Console.ResetColor();
            for (int i = 0; i < maxHPBar - currentHP; i++)
            {
                Console.Write(' ');
            }
            Console.Write(side);

            Console.SetCursorPosition(hpBarX + 58, hpBarY);
            Console.Write("{0}/{1}", currentHP, maxHPBar);

            // Status
            int statusX = leftBottomX + 1;
            int statusY = leftBottomY + 5;

            int str = 10;
            int def = 0;
            int spd = 8;
            int focus = 4;

            int emotionLevel = 1;
            int atkbuffs = 0;
            int defBuffs = 5;
            int spdBuffs = 0;
            int focusBuffs = 0;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(statusX, statusY);
            Console.Write("❤ 스테이터스");
            Console.ResetColor();
            Console.SetCursorPosition(statusX, statusY + 2);
            Console.Write("✊ 힘   : {0} + {1} ({2})", str, emotionLevel + atkbuffs, str + emotionLevel + atkbuffs);
            Console.SetCursorPosition(statusX, statusY + 3);
            Console.Write("⛨ 방어 : {0} + {1} ({2})", def, emotionLevel + defBuffs, def + emotionLevel + defBuffs);
            Console.SetCursorPosition(statusX, statusY + 4);
            Console.Write("≫ 속도 : {0} + {1} ({2})", spd, emotionLevel + spdBuffs, spdBuffs + emotionLevel + spdBuffs);
            Console.SetCursorPosition(statusX, statusY + 5);
            Console.Write("心 집중 : {0} + {1} ({2})", focus, emotionLevel + focusBuffs, focusBuffs + emotionLevel + focusBuffs);

            // Buff and Debuff
            int buffX = leftBottomX + 30;
            int buffY = leftBottomY + 5;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(buffX, buffY);
            Console.Write("버프");
            List<string> buffList = new List<string>();
            buffList.Add("회피");
            buffList.Add("가벼운 방어");

            buffY += 2;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = 0; i < buffList.Count; i++)
            {
                Console.SetCursorPosition(buffX, buffY + i);
                Console.Write(buffList[i]);
            }
            Console.ResetColor();

            int debuffX = leftBottomX + 50;
            int debuffY = leftBottomY + 5;
            List<string> debuffList = new List<string>();
            debuffList.Add("과제 2배");
            debuffList.Add("과제 4배");
            debuffList.Add("과제 2배");
            debuffList.Add("과제 2배");
            debuffList.Add("마비");
            debuffList.Add("과제 2배");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(debuffX, debuffY);
            Console.Write("디버프");

            debuffY += 2;
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < debuffList.Count; i++)
            {
                Console.SetCursorPosition(debuffX, debuffY + i);
                Console.Write(debuffList[i]);
            }
            Console.ResetColor();

            // emotion level
            int rightBottomX = 82;
            int rightBottomY = 31;

            int enemyEmotionLevel = 2;

            int playerToken = 4;
            int enemyToken = 1;

            string token = "██████ ";

            Console.SetCursorPosition(rightBottomX + 8, rightBottomY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("⚔ 상대가 유리한 상황 ⚔");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(rightBottomX, rightBottomY + 2);
            Console.Write("당신의 감정 레벨 : {0}", emotionLevel);
            Console.SetCursorPosition(rightBottomX, rightBottomY + 4);
            Console.ResetColor();

            Console.Write('|');
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < playerToken; i++)
            {
                Console.Write(token);
            }
            Console.ResetColor();
            Console.SetCursorPosition(rightBottomX + 35, rightBottomY + 4);
            Console.Write('|');

            // ----------------------------------------------

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(rightBottomX, rightBottomY + 6);
            Console.Write("상대의 감정 레벨 : {0}", enemyEmotionLevel);
            Console.SetCursorPosition(rightBottomX, rightBottomY + 8);
            Console.ResetColor();

            Console.Write('|');
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < enemyToken; i++)
            {
                Console.Write(token);
            }
            Console.ResetColor();
            Console.SetCursorPosition(rightBottomX + 35, rightBottomY + 8);
            Console.Write('|');

            Console.SetCursorPosition(rightBottomX, rightBottomY + 12);
            Console.Write("A. 메뉴로 돌아가기");
        }

        public void RenderEnemyName()
        {
            int pivotX = 34;
            int pivotY = 2;
            int pivotMaxX = 80;
            char tb = '─';

            Console.SetCursorPosition(3, pivotY);
            Console.Write("총류의 층 10F");
            Console.SetCursorPosition(pivotX, pivotY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("박종민 매니저");
            Console.ResetColor();
            Console.SetCursorPosition(pivotX, pivotY + 1);
            for (int i = 2; i < pivotMaxX; i++)
            {
                Console.SetCursorPosition(i, pivotY + 1);
                Console.Write(tb);
            }
        }

        public void RenderEnemyHPBar()
        {
            int pivotX = 3;
            int pivotY = 4;
            int pivotMaxX = 50;
            char side = '|';
            char bar = '█';


            Console.SetCursorPosition(pivotX, pivotY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("체력 : 800/1000");
            Console.ResetColor();

            Console.SetCursorPosition(pivotX, pivotY + 1);
            Console.Write(side);
            Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0; i < pivotMaxX - 10; i++)
            {
                Console.Write(bar);
            }
            Console.ResetColor();
            for (int i = 0; i < 10; i++)
            {
                Console.Write(' ');
            }
            Console.ResetColor();
            Console.Write(side);
        }

        public void RenderEnemyATBBar(int enemyATB = 30)
        {
            int pivotX = 3;
            int pivotY = 6;
            int pivotMaxX = 50;
            char side = '|';
            char bar  = '█';


            Console.SetCursorPosition(pivotX, pivotY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("안녕하세요?");
            Console.ResetColor();

            Console.SetCursorPosition(pivotX, pivotY + 1);
            Console.Write(side);
            Console.ForegroundColor = ConsoleColor.Yellow;

            pivotX++;

            for (int i = 0; i < enemyATB; i++)
            {
                Console.SetCursorPosition(pivotX + i, pivotY + 1);
                Console.Write(bar);
            }
            Console.ResetColor();
            for (int i = enemyATB; i < pivotMaxX; i++)
            {
                Console.SetCursorPosition(pivotX + i, pivotY + 1);
                //Console.SetCursorPosition(pivotX + i, pivotY + 1);
                Console.Write(' ');
            }
            Console.ResetColor();
            Console.Write(side);
        }

        public void DrawEnemy()
        {
            int pivotX = 3;
            int pivotY = 8;
            int pivotMaxX = 77;
            int pivotMaxY = 29;

            string img = File.ReadAllText(Define.LOCAL_GAME_PATH + "/Images/Img_Keter.txt");

            string[] lines = img.Split('\n');

            Console.SetCursorPosition(pivotX, pivotY);

            for (int i = 0; i < pivotMaxY - pivotY; i++)
            {
                Console.SetCursorPosition(pivotX, pivotY + i);
                Console.Write(lines[i]);
            }
        }

        public void Update()
        {

        }
    }
}
