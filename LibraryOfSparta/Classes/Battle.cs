using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;

namespace LibraryOfSparta.Classes
{
    public class Battle : Scene
    {
        Player   player { get; set; } = null;
        Enemy    enemy { get; set; } = null;
        BattleUI battleUI { get; set; } = null;

        public string[] floorData = null;
        public string[] cardData  = null;

        bool isPlayerWinning = true;

        public int Floor { get; private set; }

        public void Initbattle(int battleIndex)
        {
            Floor = battleIndex+1;
            string battleUIData = Core.GetData(Define.BATTLE_DATA_PATH);
            string[] lines      = battleUIData.Split('\n');

            cardData   = Core.GetData(Define.CARD_DATA_PATH).Split('\n');

            floorData = lines[battleIndex+1].Split(',');

            battleUI = new BattleUI();
            battleUI.Init();
            Core.PlayPlayerBGM(Define.BGM_PATH + "/" + floorData[2] + "_0.wav");
            Core.InitEnemySound();
            Core.PlayEnemyBGM(Define.BGM_PATH + "/" + "Enemy_0.wav");
            Core.PauseEnemyBGM();

            enemy  = new Enemy
                (int.Parse(floorData[4]), 
                int.Parse(floorData[4]), 
                floorData[3], 
                battleUI.RenderEnemyHPBar, 
                battleUI.RenderEnemyATBBar, 
                battleUI.RenderBattleDialog);

            player = new Player
                (battleUI.RenderPlayerHPBar, 
                battleUI.UpdateCardQueue, 
                battleUI.RenderPlayerCostATBBar, 
                battleUI.RenderPlayerDrawATBBar, 
                battleUI.RenderBattleDialog, 
                battleUI.RenderPlayerBuffStatus,
                battleUI.RenderPlayerStatus);

            // Draw enemy
            battleUI.RenderEnemyName(floorData[0], floorData[1]);
            battleUI.RenderEnemyHPBar(enemy.Hp, enemy.MaxHp);
            battleUI.RenderEnemyATBBar();
            battleUI.DrawEnemy(Define.IMAGE_PATH + "/Img_" + floorData[2] + ".txt");

            battleUI.RenderCardQueue();
            player.InitDeck();
            player.AddCardToHand();
            player.AddCardToHand();
            player.AddCardToHand();

            // battleUI status
            battleUI.RenderPlayerHPBar(player.Hp, player.MaxHp);
            battleUI.RenderPlayerStatus(player);
            battleUI.RenderPlayerBuffStatus(player.BuffList, player.DebuffList, player.buffData);
            UpdateEmotion();

            enemy.SetPattern();

            switch(battleIndex)
            {
                case 0:
                battleUI.RenderBattleDialog("장윤서 매니저", "안녕하세요");
                    break;
                case 1:
                battleUI.RenderBattleDialog("강인 튜터", "...");
                    break;
            }

            Core.PlaySFX(Define.SFX_PATH + "/" + floorData[2] + "_Enter.wav");
        }

        public void UpdateEmotion()
        {
            battleUI.RenderEmotionLevel(player.Emotion, enemy.Emotion, player.Token, enemy.Token);

            if (player.Emotion >= enemy.Emotion)
            {
                if (isPlayerWinning == false)
                {
                    isPlayerWinning = true;
                    Core.PlaySFX(Define.SFX_PATH + "/FingerTip.wav");
                    Core.PauseEnemyBGM();
                    Core.ResumePlayerBGM();
                }
            }
            else
            {
                if (isPlayerWinning == true)
                {
                    isPlayerWinning = false;
                    Core.PlaySFX(Define.SFX_PATH + "/FingerTip.wav");
                    Core.PausePlayerBGM();
                    Core.ResumeEnemyBGM();
                }
            }
        }

        public void PlayerInput()
        {
            ConsoleKeyInfo key = Core.GetKey();

            if(key == default)
            {
                return;
            }
            else
            {
                switch(key.Key)
                {
                    case ConsoleKey.NumPad1 :
                    case ConsoleKey.D1 :
                        player.CastCard(0, enemy, cardData);
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2 :
                        player.CastCard(1, enemy, cardData);
                        break;
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3 :
                        player.CastCard(2, enemy, cardData);
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4 :
                        player.CastCard(3, enemy, cardData);
                        break;
                    case ConsoleKey.Escape:
                        Core.LoadScene(8);
                        return;
                }
            }

            Core.ReleaseKey();
        }

        public void AddToken(bool isPlayer, int value)
        {
            int[] rules = { 0, 0, 1, 1, 1, 2 };

            if (isPlayer == true)
            {
                int currentPhase = rules[player.Emotion];

                if (player.Emotion == 5)
                {
                    return;
                }

                player.Token += value;

                if (player.Token == 5)
                {
                    player.Token = 0;
                    player.Emotion++;
                }

                if(currentPhase != rules[player.Emotion])
                {
                    isPlayerWinning = true;
                    Core.PauseEnemyBGM();
                    Core.PlayPlayerBGM(Define.BGM_PATH + "/" + floorData[2] + "_" + rules[player.Emotion] + ".wav");
                }
            }
            else
            {
                int currentPhase = rules[enemy.Emotion];

                if (enemy.Emotion == 5)
                {
                    return;
                }

                enemy.Token += value;

                if (enemy.Token == 5)
                {
                    enemy.Token = 0;
                    enemy.Emotion++;
                }

                if (currentPhase != rules[enemy.Emotion])
                {
                    isPlayerWinning = false;
                    Core.PausePlayerBGM();
                    Core.PlayEnemyBGM(Define.BGM_PATH + "/" + "Enemy_" + rules[enemy.Emotion] + ".wav");
                }
            }

            UpdateEmotion();
        }

        public void Update()
        {
            player.UpdatePlayerCost();
            player.UpdatePlayerDraw();
            enemy.UpdateEnemyATB(player);
            PlayerInput();

            if(player.Hp <= 0)
            {
                Core.StopEnemyBGM();
                Core.PlaySFX(Define.SFX_PATH + "/" + floorData[2] + "_Defeat.wav");
                Core.PlaySFX(Define.SFX_PATH + "/Dead.wav");
                Core.LoadScene(9);
            }
            else if(enemy.Hp <= 0)
            {
                Core.StopPlayerBGM();
                Core.StopEnemyBGM();
                Core.PlaySFX(Define.SFX_PATH + "/" + floorData[2] + "_Victory.wav");
                Core.LoadScene(6);
            }
        }

        public int GetEnemyEmotionLevel()
        {
            return enemy.Emotion;
        }
    }
}