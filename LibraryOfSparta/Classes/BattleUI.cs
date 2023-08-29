using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using LibraryOfSparta.Common;

namespace LibraryOfSparta.Classes
{
    public enum BattleSitulation
    {
        NONE,
        ATK,
        BUFF,
        VICTORY,
        DEFEAT
    }

    public class BattleUI
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
        }

        public void RenderEnemyName(string location = "총류의 층 9F", string enemyName = "박종민 매니저")
        {
            int pivotX = 34;
            int pivotY = 2;

            Console.SetCursorPosition(3, pivotY);
            Console.Write(location);
            Console.SetCursorPosition(pivotX, pivotY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(enemyName);
            Console.ResetColor();
            Console.SetCursorPosition(2, pivotY + 1);
            string line = "──────────────────────────────────────────────────────────────────────────────";

            Console.Write(line);
        }

        public void RenderEnemyHPBar(int enemyHP = 1000, int enemyMaxHP = 1000)
        {
            int pivotX    = 3;
            int pivotY    = 4;
            int pivotMaxX = 50;
            char side     = '|';
            char bar      = '█';

            Console.SetCursorPosition(pivotX, pivotY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("체력 : {0}/{1}", enemyHP, enemyMaxHP);

            int percentage  = (int)(pivotMaxX * ((double)enemyHP / enemyMaxHP));

            Console.SetCursorPosition(pivotX, pivotY + 1);
            Console.ResetColor();
            Console.Write(side);
            for (int i = 0; i < percentage; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(i + 4, pivotY + 1);
                Console.Write(bar);
            }

            for (int i = percentage; i < pivotMaxX; i++)
            {
                Console.SetCursorPosition(i+4, pivotY + 1);
                Console.Write(' ');
            }
            Console.SetCursorPosition(pivotMaxX+4, pivotY + 1);
            Console.ResetColor();
            Console.Write(side);
        }

        public void RenderEnemyATBBar(string enemySkillName = "안녕하세요?", int enemyATB = 30, int enemyMaxATB = 50)
        {
            int pivotX = 3;
            int pivotY = 6;
            int pivotMaxX = 50;
            char side = '|';
            char bar = '█';

            Console.SetCursorPosition(pivotX, pivotY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(enemySkillName);
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
                Console.Write(' ');
            }
            Console.ResetColor();
            Console.Write(side);
        }

        public void DrawEnemy(string path)
        {
            int x = 3;
            int y = 8;
            int endY = 29;

            string img = File.ReadAllText(path);

            string[] lines = img.Split('\n');

            Console.SetCursorPosition(x, y);

            for (int i = 0; i < endY - y; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(lines[i]);
            }
        }

        public void RenderCardQueue()
        {
            int x    = 82;
            int y    = 3;
            int endX = 128;
            int endY = 20;

            for (int i = y; i < endY; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write('|');

                Console.SetCursorPosition(x + 20, i);
                Console.Write('|');
            }
        }

        public void UpdateCardQueue()
        {
            int x = 82;
            int endY = 20;

            // Card Queue Indexing
            List<string> hands = new List<string>();
            hands.Add("1 코스트\n가벼운 공격");
            hands.Add("2 코스트\n집중 공격");
            hands.Add("1 코스트\n가벼운 공격");
            hands.Add("1 코스트\n회피");

            int handStart = endY - 1;

            string cardFrame = new string('─', 16);

            for (int i = 0; i < hands.Count; i++)
            {
                string[] temp = hands[i].Split('\n');

                Console.SetCursorPosition(x + 2, handStart);
                Console.Write(cardFrame);
                Console.SetCursorPosition(x + 2, handStart - 1);
                Console.Write(temp[1]);
                Console.SetCursorPosition(x + 2, handStart - 2);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(temp[0]);
                Console.ResetColor();
                Console.SetCursorPosition(x + 2, handStart - 3);
                Console.Write(cardFrame);
                Console.SetCursorPosition(x + 22, handStart - 1);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[{0}]", 1 + i);
                Console.ResetColor();

                handStart -= 4;
            }
        }

        public void RenderPlayerCostATBBar(int playerCost = 1, int playerCostFill = 20)
        {
            int x = 84;
            int y = 19;
            int barLength = 31;

            char side = '|';
            char bar  = '█';

            Console.SetCursorPosition(x, y + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("보유 코스트 : {0}/{1}", playerCost, 10);

            Console.SetCursorPosition(x, y + 3);
            Console.Write(" ≫ 코스트");
            Console.SetCursorPosition(x, y + 4);
            Console.ResetColor();
            Console.Write(side);
            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 1; i < playerCostFill; i++)
            {
                Console.SetCursorPosition((x+1) + i, y + 4);
                Console.Write(bar);

            }
            Console.ResetColor();
            for (int i = playerCostFill; i < barLength; i++)
            {
                Console.SetCursorPosition((x+1) + i, y + 4);
                Console.Write(' ');

            }
            Console.Write(side);
        }

        public void RenderPlayerDrawATBBar(int atbDrawFill = 7)
        {
            int x = 84;
            int y = 24;
            int barLength = 31;

            char side = '|';
            char bar  = '█';

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.SetCursorPosition(x, y);
            Console.Write(" 心 카드 드로우");
            Console.SetCursorPosition(x, y+1);
            Console.ResetColor();
            Console.Write(side);
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            for (int i = 1; i < atbDrawFill; i++)
            {
                Console.SetCursorPosition((x+1) + i, y + 1);
                Console.Write(bar);

            }
            Console.ResetColor();
            for (int i = atbDrawFill; i < barLength; i++)
            {
                Console.SetCursorPosition((x+1) + i, y + 1);
                Console.Write(' ');

            }
            Console.Write(side);
        }

        public void RenderPlayerHPBar(int playerHP = 48, int playerMaxHP = 100)
        {
            // Draw HPBar
            int x = 3;
            int y = 33;
            int barLength = 51;
            int barFiled  = (int)(barLength * ((double) playerHP / playerMaxHP));

            char side = '|';
            char bar  = '█';

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("체력");
            Console.ResetColor();

            Console.SetCursorPosition(x + 4, y);
            Console.Write(side);

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 1; i < barFiled+1; i++)
            {
                Console.SetCursorPosition((x + 4) + i, y);
                Console.Write(bar);
            }
            Console.ResetColor();
            for (int i = barFiled; i < barLength; i++)
            {
                Console.SetCursorPosition((x + 4) + i, y);
                Console.Write(' ');
            }
            Console.Write(side);

            Console.SetCursorPosition(x + barLength+6, y);
            Console.Write("{0}/{1}", playerHP, playerMaxHP);
        }

        public void RenderBattleDialog(string caster = "당신", string target = "박종민 매니저", string skillName = "가벼운 공격", int power = 10, BattleSitulation situlation = BattleSitulation.NONE)
        {
            int x = 3;
            int y = 30;

            
            Console.SetCursorPosition(x, y);
            

            switch (situlation)
            {
                case BattleSitulation.NONE:
                    Console.Write("{0}", caster);
                    Console.SetCursorPosition(x, y + 1);
                    Console.Write("{0}", target);
                    break;
                case BattleSitulation.ATK:
                    Console.Write("{0}의 {1}!", caster, skillName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(x, y + 1);
                    Console.Write("{0}는 {1} 대미지를 받았다!", target, power);
                    break;
                case BattleSitulation.BUFF:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("{0}의 {1}!", caster, skillName);
                    break;
                default:
                    break;
            }

            
        }

        public void RenderPlayerStatus(Player player)
        {
            /* temp */
            int x = 4;
            int y = 36;

            int emotionLevel = 1;
            int atkbuffs = 0;
            int defBuffs = 5;
            int spdBuffs = 0;
            int focusBuffs = 0;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(x, y);
            Console.Write("❤ 스테이터스");
            Console.ResetColor();
            Console.SetCursorPosition(x, y + 2);
            Console.Write("✊ 힘   : {0} + {1} ({2})", player.Str, emotionLevel + atkbuffs, player.Str + emotionLevel + atkbuffs);
            Console.SetCursorPosition(x, y + 3);
            Console.Write("⛨ 방어 : {0} + {1} ({2})", player.Def, emotionLevel + defBuffs, player.Def + emotionLevel + defBuffs);
            Console.SetCursorPosition(x, y + 4);
            Console.Write("≫ 속도 : {0} + {1} ({2})", player.Spd, emotionLevel + spdBuffs, player.Spd + emotionLevel + spdBuffs);
            Console.SetCursorPosition(x, y + 5);
            Console.Write("心 집중 : {0} + {1} ({2})", player.Fcs, emotionLevel + focusBuffs, player.Fcs + emotionLevel + focusBuffs);
        }

        public void RenderPlayerBuffStatus()
        {
            int buffX = 34;
            int buffY = 36;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(buffX, buffY);
            Console.Write("버프");
            List<string> buffList = new List<string>();
            buffList.Add("회피");
            buffList.Add("가벼운 방어");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = 0; i < buffList.Count; i++)
            {
                Console.SetCursorPosition(buffX, (buffY + 2) + i);
                Console.Write(buffList[i]);
            }
            Console.ResetColor();

            int debuffX = 50;
            int debuffY = buffY;
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

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < debuffList.Count; i++)
            {
                Console.SetCursorPosition(debuffX, (debuffY + 2) + i);
                Console.Write(debuffList[i]);
            }
            Console.ResetColor();
        }

        public void RenderEmotionLevel(int pELevel = 1, int eELevel = 2, int pToken = 4, int eToken = 1)
        {
            int x = 82;
            int y = 31;
            int barLength = 5;

            string token = "██████ ";
            string empty = "       ";

            Console.SetCursorPosition(x + 8, y);

            if(pELevel >= eELevel)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("⚔ 당신이 유리한 상황 ⚔");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("⚔ 상대가 유리한 상황 ⚔");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(x, y + 2);
            Console.Write("당신의 감정 레벨 : {0}", pELevel);
            
            Console.ResetColor();

            Console.SetCursorPosition(x, y + 4);
            Console.Write('|');
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < pToken; i++)
            {
                Console.SetCursorPosition((x+1) + (i * token.Length), y + 4);
                Console.Write(token);
            }
            Console.ResetColor();
            for(int i = pToken; i < barLength; i++)
            {
                Console.SetCursorPosition((x+1) + (i * token.Length), y + 4);
                Console.Write(empty);
            }
            Console.Write('|');

            // ----------------------------------------------

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(x, y + 6);
            Console.Write("상대의 감정 레벨 : {0}", eELevel);
            Console.ResetColor();

            Console.SetCursorPosition(x, y + 8);
            Console.Write('|');
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < eToken; i++)
            {
                Console.SetCursorPosition((x+1) + (i * token.Length), y + 8);
                Console.Write(token);
            }
            Console.ResetColor();
            for (int i = eToken; i < barLength; i++)
            {
                Console.SetCursorPosition((x+1) + (i * token.Length), y + 8);
                Console.Write(empty);
            }
            Console.Write('|');

            Console.SetCursorPosition(x, y + 12);
            Console.Write("A. 입구로 돌아가기");
        }
    }
}
