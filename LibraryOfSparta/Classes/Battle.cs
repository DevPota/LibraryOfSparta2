using LibraryOfSparta.Common;
using System.Timers;
using System.Threading;
using System.Runtime.InteropServices;
using LibraryOfSparta.Managers;
using System.Xml.Linq;

namespace LibraryOfSparta.Classes
{
    public class Battle : Scene
    {
        Player   player { get; set; } = null;
        Enemy    enemy { get; set; } = null;
        BattleUI battleUI { get; set; } = null;

        List<int> playerHands = new List<int>();
        List<int> deck;

        string[] floorData = null;

        int playerEmotion = 0;
        int enemyEmotion = 0;
        int playerToken = 0;
        int enemyToken = 0;
        int playerCost = 0;
        int playerCostFilled = 0;
        int playerDrawFilled = 0;
        int enemyFilled = 0;

        bool isPlayerWinning = true;

        public void Initbattle(int battleIndex)
        {
            playerEmotion = 0;
            enemyEmotion = 0;
            playerToken = 0;
            enemyToken = 0;

            playerCost = 0;
            playerCostFilled = 0;

            playerDrawFilled = 0;

            string battleUIData = Core.GetData(Define.BATTLE_DATA_PATH);
            string[] lines = battleUIData.Split('\n');

            floorData = lines[battleIndex+1].Split(',');

            battleUI = new BattleUI();
            battleUI.Init();

            enemy  = new Enemy(int.Parse(floorData[4]), int.Parse(floorData[4]), floorData[3], battleUI.RenderEnemyHPBar);
            player = new Player(battleUI.RenderPlayerHPBar);

            // Draw enemy
            battleUI.RenderEnemyName(floorData[0], floorData[1]);
            battleUI.RenderEnemyHPBar(enemy.Hp, enemy.MaxHp);
            battleUI.RenderEnemyATBBar();
            battleUI.DrawEnemy(Define.IMAGE_PATH + "/Img_" + floorData[2] + ".txt");

            battleUI.RenderCardQueue();
            InitDeck();
            AddCardToHand();
            AddCardToHand();
            AddCardToHand();

            // battleUI status
            battleUI.RenderPlayerHPBar(player.Hp, player.MaxHp);
            battleUI.RenderPlayerStatus(player);
            battleUI.RenderPlayerBuffStatus();
            UpdateEmotion();

            enemy.SetPattern();

            switch(battleIndex)
            {
                case 0:
                battleUI.RenderBattleDialog("장윤서 매니저", "TIL 작성 하셔야죠?");
                    break;
                case 1:
                battleUI.RenderBattleDialog("강인 튜터", "...");
                    break;
            }
        }

        public void UpdateEmotion()
        {
            int[] rules = { 0, 0, 1, 1, 1, 2 };

            battleUI.RenderEmotionLevel(playerEmotion, enemyEmotion, playerToken, enemyToken);

            Core.PlayPlayerBGM(Define.BGM_PATH + "/" + floorData[2] + "_" + rules[playerEmotion] + ".wav");
            Core.PlayEnemyBGM(Define.BGM_PATH + "/" + "Enemy_" + rules[enemyEmotion] + ".wav");

            if (playerEmotion >= enemyEmotion)
            {
                if (isPlayerWinning == false)
                {
                    isPlayerWinning = true;
                    Core.PlaySFX(Define.SFX_PATH + "/FingerTip.wav");
                }
                Core.PauseEnemyBGM();
                Core.ResumePlayerBGM();
            }
            else
            {
                if (isPlayerWinning == true)
                {
                    isPlayerWinning = false;
                    Core.PlaySFX(Define.SFX_PATH + "/FingerTip.wav");
                }

                Core.PausePlayerBGM();
                Core.ResumeEnemyBGM();
            }
        }

        public void UpdatePlayerCost()
        {
            if (playerCost >= 10)
            {
                return;
            }

            playerCostFilled += player.Spd / 5;

            if (playerCostFilled >= 31)
            {
                playerCostFilled = 0;
                playerCost++;

                if (playerCost >= 10)
                {
                    playerCost = 10;
                }
            }

            battleUI.RenderPlayerCostATBBar(playerCost, playerCostFilled);
        }

        public void UpdatePlayerDraw()
        {
            playerDrawFilled += player.Fcs / 10;

            if (playerDrawFilled >= 31)
            {
                playerDrawFilled = 0;

                if (playerHands.Count >= 4)
                {
                    RemoveCardFromHand(0);
                    AddCardToHand();
                }
                else
                {
                    AddCardToHand();
                }
            }

            battleUI.RenderPlayerDrawATBBar(playerDrawFilled);
        }

        public void AddCardToHand()
        {
            for(int i = 0; i < deck.Count; i++)
            {
                if (deck[i] != -1)
                {
                    playerHands.Add(deck[i]);
                    deck[i] = -1;
                    battleUI.UpdateCardQueue(playerHands);
                    return;
                }
            }

            InitDeck();

            playerHands.Add(deck[0]);
            deck[0] = -1;

            battleUI.UpdateCardQueue(playerHands);
        }

        public void RemoveCardFromHand(int index)
        {
            if(index >= playerHands.Count)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                return;
            }

            playerHands.RemoveAt(index);

            battleUI.UpdateCardQueue(playerHands);
        }

        public void InitDeck()
        {
            deck = new List<int>();

            foreach(int element in Core.SaveData.Deck)
            {
                deck.Add(element);
            }

            Random random = new Random();

            for(int i = 0; i < 500; i++)
            {
                int a = random.Next(0, deck.Count);
                int b = random.Next(0, deck.Count);

                if(a == b)
                {
                    continue;
                }
                else
                {
                    int temp = deck[a];
                    deck[a] = deck[b];
                    deck[b] = temp;
                }
            }
        }

        public void UpdateEnemyATB()
        {
            enemyFilled += 1;

            if (enemyFilled >= enemy.Spd)
            {
                enemyFilled = 0;

                enemy.CastPattern(player);
                enemy.SetPattern();
            }

            battleUI.RenderEnemyATBBar(enemy.CurrentSkill.Name, enemyFilled, enemy.Spd);
        }

        public void PlayerInput()
        {
            ConsoleKeyInfo key = Core.GetKey();




            //if (key.Key == ConsoleKey.A)
            //{
            //    Test(true);
            //}
            //else if (key.Key == ConsoleKey.D)
            //{
            //    Test(false);
            //}

            Core.ReleaseKey();
        }

        public void Test(bool asd)
        {
            if (asd == true)
            {
                if (playerEmotion == 5)
                {
                    UpdateEmotion();
                    return;
                }

                playerToken++;

                if (playerToken == 5)
                {
                    playerToken = 0;
                    playerEmotion++;
                }
            }
            else
            {
                if (enemyEmotion == 5)
                {
                    UpdateEmotion();
                    return;
                }

                enemyToken++;

                if (enemyToken == 5)
                {
                    enemyToken = 0;
                    enemyEmotion++;
                }
            }

            UpdateEmotion();
        }

        public void Update()
        {
            UpdatePlayerCost();
            UpdatePlayerDraw();
            //UpdateEnemyATB();
            PlayerInput();
        }
    }
}