using LibraryOfSparta.Common;
using LibraryOfSparta.Classes;
using System.Timers;
using System.Threading;
using System.Runtime.InteropServices;

namespace LibraryOfSparta.Managers
{
    public static class GameManager
    {
        static Player player { get; set; } = null;
        static Enemy  enemy  { get; set; } = null;

        static Battle battle { get; set; } = null;

        static string[] floorData          = null;

        static int playerEmotion = 0;
        static int enemyEmotion  = 0;
        static int playerToken   = 0;
        static int enemyToken    = 0;

        static int playerCost       = 0;
        static int playerCostFilled = 0;

        static int playerHands      = 0;
        static int playerDrawFilled = 0;

        static int enemyFilled = 0;

        static bool isPlayerWinning = true;

        public static void InitBattle(int battleIndex)
        {
            playerEmotion = 0;
            enemyEmotion = 0;
            playerToken = 0;
            enemyToken = 0;

            playerCost = 0;
            playerCostFilled = 0;

            playerHands = 0;
            playerDrawFilled = 0;

            string   battleData = Core.GetData(Define.BATTLE_DATA_PATH);
            string[] lines      = battleData.Split('\n');

            floorData = lines[battleIndex].Split(',');

            player = new Player();
            enemy  = new Enemy(int.Parse(floorData[4]), int.Parse(floorData[4]), floorData[3]);

            battle = new Battle();
            battle.Init();

            // Draw enemy
            battle.RenderEnemyName(floorData[0], floorData[1]);
            battle.RenderEnemyHPBar(enemy.Hp, enemy.MaxHp);
            battle.RenderEnemyATBBar();
            battle.DrawEnemy(Define.IMAGE_PATH + "/Img_" + floorData[2] + ".txt");

            // Draw Card Queue
            battle.RenderCardQueue();
            battle.UpdateCardQueue();

            // battle status
            battle.RenderPlayerHPBar(player.Hp, player.MaxHp);
            battle.RenderPlayerStatus(player);
            battle.RenderPlayerBuffStatus();
            UpdateEmotion();

            battle.RenderBattleDialog("장윤서 매니저", "TIL 작성 하셔야죠?");
        }

        public static void UpdateEmotion()
        {
            int[] rules = {0, 0, 1, 1, 1, 2};

            battle.RenderEmotionLevel(playerEmotion, enemyEmotion, playerToken, enemyToken);

            Core.PlayPlayerBGM(Define.BGM_PATH + "/" + floorData[2] + "_" + rules[playerEmotion] + ".wav");
            Core.PlayEnemyBGM(Define.BGM_PATH + "/" + "Enemy_" + rules[enemyEmotion] + ".wav");

            if(playerEmotion >= enemyEmotion)
            {
                if(isPlayerWinning == false)
                {
                    isPlayerWinning = true;
                    Core.PlaySFX(Define.SFX_PATH + "/FingerTip.wav");
                }
                Core.PauseEnemyBGM();
                Core.ResumePlayerBGM();
            }
            else
            {
                if(isPlayerWinning == true)
                {
                    isPlayerWinning = false;
                    Core.PlaySFX(Define.SFX_PATH + "/FingerTip.wav");
                }

                Core.PausePlayerBGM();
                Core.ResumeEnemyBGM();
            }
        }

        public static void UpdatePlayerCost()
        {
            if(playerCost >= 10)
            {
                return;
            }

            playerCostFilled += player.Spd / 5;

            if(playerCostFilled >= 31)
            {
                playerCostFilled = 0;
                playerCost++;

                if (playerCost >= 10)
                {
                    playerCost = 10;
                }
            }

            battle.RenderPlayerCostATBBar(playerCost, playerCostFilled);
        }

        public static void UpdatePlayerDraw()
        {
            playerDrawFilled += player.Fcs / 10;

            if (playerDrawFilled >= 31)
            {
                playerDrawFilled = 0;
                

                if (playerHands >= 4)
                {
                    
                }
            }

            battle.RenderPlayerDrawATBBar(playerDrawFilled);
        }

        public static void UpdateEnemyATB()
        {
            playerDrawFilled += player.Fcs / 10;

            if (playerDrawFilled >= 31)
            {
                playerDrawFilled = 0;


                if (playerHands >= 4)
                {
                    
                }
            }

            battle.RenderPlayerDrawATBBar(playerDrawFilled);
        }

        public static void PlayerInput()
        {
            ConsoleKeyInfo key = Core.GetKey();

            if (key.Key == ConsoleKey.A)
            {
                Test(true);
            }
            else if (key.Key == ConsoleKey.D)
            {
                Test(false);
            }

            Core.ReleaseKey();
        }

        public static void Test(bool asd)
        {
            if(asd == true)
            {
                if(playerEmotion == 5)
                {
                    UpdateEmotion();
                    return;
                }

                playerToken++;

                if(playerToken == 5)
                {
                    playerToken = 0;
                    playerEmotion++;
                }
            }
            else
            {
                if(enemyEmotion == 5)
                {
                    UpdateEmotion();
                    return;
                }

                enemyToken++;

                if(enemyToken == 5)
                {
                    enemyToken = 0;
                    enemyEmotion++;
                }
            }

            UpdateEmotion();
        }

        public static void Update()
        {
            UpdatePlayerCost();
            UpdatePlayerDraw();
            UpdateEnemyATB();
            PlayerInput();
        }
    }
}