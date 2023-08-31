using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LibraryOfSparta.Classes
{
    public class EnemySkill
    {
        public string Name       { get; set; }
        public int    Power      { get; set; }
        public PatternType Type  { get; set; }
        public int[]  Buffs      { get; set; } = null;
        public string SFXBefore  { get; set; }
        public string SFXAfter   { get; set; }

        public EnemySkill(string name, int power, PatternType type, int[] buffs, string before, string after)
        {
            Name  = name;
            Power = power;
            Type  = type;
            Buffs = buffs;
            SFXBefore = before.Trim();
            SFXAfter  = after.Trim();
        }
    }

    public class Enemy
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Spd { get; set; } = 9999;

        public int Emotion { get; set; } = 0;
        public int Token   { get; set; } = 0;
        public int enemyFilled { get; set; } = 0;

        public List<int> BuffList   = new List<int>();
        public List<int> DebuffList = new List<int>();

        Queue<int> patternQueue = new Queue<int>();

        public EnemySkill CurrentSkill = null;
        Action<int, int> hpListener = (int i, int j) => { };
        Action<string, int, int> fillListener = (string i, int j, int k) => { };
        Action<string, string, string, int, BattleSitulation> dialogListener = (default);

        public Enemy(int hp, int maxHp, string pattern, Action<int, int> hpAction, Action<string, int, int> fillAction, Action<string, string, string, int, BattleSitulation> dialogAction)
        {
            this.Hp = hp;
            this.MaxHp = maxHp;

            string[] patterns = pattern.Split('_');

            foreach(string temp in patterns)
            {
                patternQueue.Enqueue(int.Parse(temp));
            }

            hpListener   = hpAction;
            fillListener = fillAction;
            dialogListener = dialogAction;
        }
        public void OnHit(int damage)
        {
            // 버프, 디버프 계산 나중에 추가
            Hp -= damage;
            if(Hp > MaxHp)
            {
                Hp = MaxHp;
            }
            hpListener(Hp, MaxHp);
        }

        public void SetNewPattern(string pattern)
        {
            patternQueue = new Queue<int>();
            enemyFilled = 0;

            string[] patterns = pattern.Split('_');

            foreach (string temp in patterns)
            {
                patternQueue.Enqueue(int.Parse(temp));
            }

            SetPattern();
        }

        public void SetPattern()
        {
            int patternIndex = patternQueue.Dequeue();
            string skillData = Core.GetData(Define.E_SKILL_DATA_PATH);
            string[] lines = skillData.Split('\n');

            string[] skill = lines[patternIndex+1].Split(',');

            int pwr = 0;
            int.TryParse(skill[1], out pwr);

            int[] buffArr = null;

            if (skill[3].Trim() != "-1")
            {
                string[] buffStrings = skill[3].Split('_');

                buffArr = new int[buffStrings.Length];

                for (int i = 0; i < buffStrings.Length; i++)
                {
                    buffArr[i] = int.Parse(buffStrings[i]);
                }
            }

            CurrentSkill = new EnemySkill(skill[0], pwr, (PatternType)int.Parse(skill[2]), buffArr, skill[5], skill[6]);

            Spd = int.Parse(skill[4]);

            switch(CurrentSkill.Type)
            {
                case PatternType.재사용하지않고플레이어공격:
                case PatternType.재사용하지않고플레이어공격디버프:
                case PatternType.재사용하지않고플레이어버프감정토큰없음:
                case PatternType.재사용하지않고플레이어디버프감정토큰없음:
                case PatternType.재사용하지않고플레이어버프다날리고버프감정토큰없음:
                case PatternType.재사용하지않고플레이어디버프다날리고디버프감정토큰없음:
                case PatternType.오프닝:
                    break;
                default:
                    patternQueue.Enqueue(patternIndex);
                    break;
            }

            Core.PlaySFX(Define.SFX_PATH + CurrentSkill.SFXBefore);
        }

        public void CastPattern(Player player)
        {
            Core.PlaySFX(Define.SFX_PATH + CurrentSkill.SFXAfter);

            switch (CurrentSkill.Type)
            {
                case PatternType.감정토큰없는플레이어공격:
                    if (player.OnHit(CurrentSkill.Power) == true)
                    {
                        if (CurrentSkill.Buffs != null)
                        {
                            foreach (int buff in CurrentSkill.Buffs)
                            {
                                player.AddDebuff(buff);
                            }
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.플레이어공격:
                    if(player.OnHit(CurrentSkill.Power) == true)
                    {
                        if (CurrentSkill.Buffs != null)
                        {
                            foreach (int buff in CurrentSkill.Buffs)
                            {
                                player.AddDebuff(buff);
                            }
                        }
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power, BattleSitulation.ATK);
                    }
                    ((Battle)Core.CurrentScene).AddToken(false, 1);
                    player.UpdatePlayerUI();
                    break;
                case PatternType.감정토큰없는플레이어버프:
                    if (CurrentSkill.Buffs != null)
                    {
                        foreach (int buff in CurrentSkill.Buffs)
                        {
                            player.AddBuff(buff);
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.자폭:
                    player.OnHit(CurrentSkill.Power);
                    Hp = 0;
                    player.UpdatePlayerUI();
                    return;
                case PatternType.감정토큰없는디버프부여:
                    if (CurrentSkill.Buffs != null)
                    {
                        foreach (int buff in CurrentSkill.Buffs)
                        {
                            player.AddDebuff(buff);
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.재사용하지않고플레이어버프감정토큰없음:
                    if (CurrentSkill.Buffs != null)
                    {
                        foreach (int buff in CurrentSkill.Buffs)
                        {
                            player.AddBuff(buff);
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.재사용하지않고플레이어디버프감정토큰없음:
                    if (CurrentSkill.Buffs != null)
                    {
                        foreach (int buff in CurrentSkill.Buffs)
                        {
                            player.AddDebuff(buff);
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.회복:
                    OnHit(-CurrentSkill.Power);
                    dialogListener(((Battle)Core.CurrentScene).floorData[1],"", "", CurrentSkill.Power, BattleSitulation.HEAL);
                    break;
                case PatternType.감정토큰없는다이얼로그있는플레이어공격:
                    if (player.OnHit(CurrentSkill.Power) == true)
                    {
                        if (CurrentSkill.Buffs != null)
                        {
                            foreach (int buff in CurrentSkill.Buffs)
                            {
                                player.AddDebuff(buff);
                            }
                        }
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power, BattleSitulation.ATK);
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.재사용하지않고플레이어버프다날리고버프감정토큰없음:
                    if (CurrentSkill.Buffs != null)
                    {
                        player.BuffList = new List<int>();
                        foreach (int buff in CurrentSkill.Buffs)
                        {
                            player.AddBuff(buff);
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.재사용하지않고플레이어디버프다날리고디버프감정토큰없음:
                    if (CurrentSkill.Buffs != null)
                    {
                        player.DebuffList = new List<int>();
                        foreach (int buff in CurrentSkill.Buffs)
                        {
                            player.AddDebuff(buff);
                        }
                    }
                    player.UpdatePlayerUI();
                    break;
                case PatternType.회피및막지못하면손패삭제:
                    if (player.OnHit(CurrentSkill.Power) == true)
                    {
                        if (CurrentSkill.Buffs != null)
                        {
                            for (int i = 0; i < CurrentSkill.Buffs.Length; i++)
                            {
                                player.RemoveCardFromHand(0);
                            }
                        }
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power, BattleSitulation.ATK);
                    }
                    ((Battle)Core.CurrentScene).AddToken(false, 1);
                    player.UpdatePlayerUI();
                    break;
                case PatternType.회피및막지못하면코스트삭제:
                    if (player.OnHit(CurrentSkill.Power) == true)
                    {
                        if (CurrentSkill.Buffs != null)
                        {
                            for (int i = 0; i < CurrentSkill.Buffs.Length; i++)
                            {
                                player.PlayerCost--;
                            }
                        }
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power, BattleSitulation.ATK);
                    }
                    ((Battle)Core.CurrentScene).AddToken(false, 1);
                    player.UpdatePlayerUI();
                    break;
                case PatternType.플레이어모든버프삭제_해제불가_포함:
                    player.BuffList = new List<int>();
                    player.UpdatePlayerUI();
                    break;
                case PatternType.레스터라이즈:
                    int mulCount = 1;
                    string[] pBuffDataLines = Core.GetData(Define.P_BUFF_DATA_PATH).Split('\n');
                    foreach(int buffIndex in player.BuffList)
                    {
                        string[] pBuffData = pBuffDataLines[buffIndex].Split(',');

                        PlayerBuffType type =  (PlayerBuffType)int.Parse(pBuffData[1]);

                        if(type == PlayerBuffType.픽셀_셰이더_청)
                        {
                            mulCount++;   
                        }
                    }
                    player.OnHit(CurrentSkill.Power * mulCount);
                    dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power * mulCount, BattleSitulation.ATK);
                    ((Battle)Core.CurrentScene).AddToken(false, 1);
                    player.UpdatePlayerUI();
                    break;
                    case PatternType.재사용하지않고플레이어공격 :
                    if (player.OnHit(CurrentSkill.Power) == true)
                    {
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power, BattleSitulation.ATK);
                    }
                    ((Battle)Core.CurrentScene).AddToken(false, 1);
                    player.UpdatePlayerUI();
                    break;
                    case PatternType.플레이어모든디버프삭제_해제불가_포함 :
                    player.DebuffList = new List<int>();
                    break;
                    case PatternType.오프닝:
                    break;
                    case PatternType.재사용하지않고플레이어공격디버프:
                    if (player.OnHit(CurrentSkill.Power) == true)
                    {
                        if (CurrentSkill.Buffs != null)
                        {
                            foreach (int buff in CurrentSkill.Buffs)
                            {
                                player.AddDebuff(buff);
                            }
                        }
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", CurrentSkill.Name, CurrentSkill.Power, BattleSitulation.ATK);
                    }
                    ((Battle)Core.CurrentScene).AddToken(false, 1);
                    player.UpdatePlayerUI();
                    break;
            }
        }

        public void UpdateEnemyATB(Player player)
        {
            enemyFilled += 1;

            if (enemyFilled >= Spd)
            {
                enemyFilled = 0;

                CastPattern(player);
                SetPattern();
            }

            fillListener(CurrentSkill.Name, enemyFilled, Spd);
        }
    }
}
