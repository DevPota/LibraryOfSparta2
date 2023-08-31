using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;

namespace LibraryOfSparta.Classes
{
    public class Battle : Scene
    {
        public Player   Player { get; private set; } = null;
        public Enemy    Enemy  { get; private set; } = null;
        BattleUI battleUI { get; set; } = null;

        public string[] floorData = null;
        public string[] cardData  = null;

        bool isPlayerWinning = true;
        bool multiPhase = false;
        bool phaseChanged = false;

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

            Enemy = new Enemy
                (int.Parse(floorData[4]), 
                int.Parse(floorData[4]), 
                floorData[3], 
                battleUI.RenderEnemyHPBar, 
                battleUI.RenderEnemyATBBar, 
                battleUI.RenderBattleDialog);

            Player = new Player
                (battleUI.RenderPlayerHPBar, 
                battleUI.UpdateCardQueue, 
                battleUI.RenderPlayerCostATBBar, 
                battleUI.RenderPlayerDrawATBBar, 
                battleUI.RenderBattleDialog, 
                battleUI.RenderPlayerBuffStatus,
                battleUI.RenderPlayerStatus);

            // Draw enemy
            battleUI.RenderEnemyName(floorData[0], floorData[1]);
            battleUI.RenderEnemyHPBar(Enemy.Hp, Enemy.MaxHp);
            battleUI.RenderEnemyATBBar();
            battleUI.DrawEnemy(Define.IMAGE_PATH + "/Img_" + floorData[2] + ".txt");

            battleUI.RenderCardQueue();
            Player.InitDeck();
            Player.AddCardToHand();
            Player.AddCardToHand();
            Player.AddCardToHand();

            // battleUI status
            battleUI.RenderPlayerHPBar(Player.Hp, Player.MaxHp);
            battleUI.RenderPlayerStatus(Player);
            battleUI.RenderPlayerBuffStatus(Player.BuffList, Player.DebuffList, Player.buffData);
            UpdateEmotion();

            Enemy.SetPattern();

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

            int phase = int.Parse(floorData[7]);

            if(phase == 0)
            {
                multiPhase = false;
            }
            else
            {
                multiPhase = true;
            }
        }

        public void UpdateEmotion()
        {
            battleUI.RenderEmotionLevel(Player.Emotion, Enemy.Emotion, Player.Token, Enemy.Token);

            if(phaseChanged == true)
            {
                return;
            }

            if (Player.Emotion >= Enemy.Emotion)
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
                        Player.CastCard(0, Enemy, cardData);
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2 :
                        Player.CastCard(1, Enemy, cardData);
                        break;
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3 :
                        Player.CastCard(2, Enemy, cardData);
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4 :
                        Player.CastCard(3, Enemy, cardData);
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
                int currentPhase = rules[Player.Emotion];

                if (Player.Emotion == 5)
                {
                    return;
                }

                Player.Token += value;

                if (Player.Token >= 5)
                {
                    Player.Token = 0;
                    Player.Emotion++;
                }

                if(currentPhase != rules[Player.Emotion] && phaseChanged == false)
                {
                    isPlayerWinning = true;
                    Core.PauseEnemyBGM();
                    Core.PlayPlayerBGM(Define.BGM_PATH + "/" + floorData[2] + "_" + rules[Player.Emotion] + ".wav");
                }
            }
            else
            {
                int currentPhase = rules[Enemy.Emotion];

                if (Enemy.Emotion == 5)
                {
                    return;
                }

                Enemy.Token += value;

                if (Enemy.Token >= 5)
                {
                    Enemy.Token = 0;
                    Enemy.Emotion++;
                }

                if (currentPhase != rules[Enemy.Emotion] && phaseChanged == false)
                {
                    isPlayerWinning = false;
                    Core.PausePlayerBGM();
                    Core.PlayEnemyBGM(Define.BGM_PATH + "/" + "Enemy_" + rules[Enemy.Emotion] + ".wav");
                }
            }

            UpdateEmotion();
        }

        public void Update()
        {
            Player.UpdatePlayerCost();
            Player.UpdatePlayerDraw();
            Enemy.UpdateEnemyATB(Player);
            PlayerInput();

            if(multiPhase == true && Enemy.Hp <= (Enemy.MaxHp * 0.5) && phaseChanged == false)
            {
                phaseChanged = true;

                switch(Floor)
                {
                    case 6:
                        battleUI.DrawEnemy(Define.IMAGE_PATH + "/Img_Gebura2.txt");
                        battleUI.RenderEnemyName("컴파일러의 층 6F", "데미갓 박종민 매니저");
                        floorData[1] = "데미갓 박종민 매니저";
                        Core.PlayPlayerBGM(Define.BGM_PATH + "/Gebura_3.wav");
                        Core.PlaySFX(Define.SFX_PATH + "/Gebura2nd.wav");
                        Core.PlaySFX(Define.SFX_PATH + "/Keter_9.wav");
                        Enemy.SetNewPattern("29_30_31_32_33_34_31_34_30_34_33_32_30_34_34_32");
                        break;
                    case 10:
                        battleUI.DrawEnemy(Define.IMAGE_PATH + "/Img_Keter3.txt");
                        Core.PlayPlayerBGM(Define.BGM_PATH + "/Keter_3.wav");
                        Core.PlaySFX(Define.SFX_PATH + "/Gebura2nd.wav");
                        Core.PlaySFX(Define.SFX_PATH + "/Keter_Victory.wav");
                        Enemy.SetNewPattern("65_65_68_66_69_69_69_69_70_71_68_75_72_69_69_65_70_68_74_66_68_73_73_70_76_69_69_68_79");
                        break;
                }
            }

            if(Player.Hp <= 0)
            {
                Core.StopEnemyBGM();
                Core.PlaySFX(Define.SFX_PATH + "/" + floorData[2] + "_Defeat.wav");
                Core.PlaySFX(Define.SFX_PATH + "/Dead.wav");
                Core.LoadScene(9);
            }
            else if(Enemy.Hp <= 0)
            {
                Core.StopPlayerBGM();
                Core.StopEnemyBGM();
                Core.PlaySFX(Define.SFX_PATH + "/" + floorData[2] + "_Victory.wav");
                Core.LoadScene(6);
            }
        }

        public int GetEnemyEmotionLevel()
        {
            return Enemy.Emotion;
        }
    }
}