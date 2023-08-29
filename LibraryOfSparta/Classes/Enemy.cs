using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System.Collections;
using System.Collections.Generic;

namespace LibraryOfSparta.Classes
{
    class EnemySkill
    {
        public string Name  { get; set; }
        public int    Power { get; set; }
        public PatternType Type  { get; set; }
        public int[]  Buffs { get; set; }
        public string SFX   { get; set; }

        public EnemySkill(string name, int power, PatternType type, int[] buffs, string sFX)
        {
            Name  = name;
            Power = power;
            Type  = type;
            Buffs = buffs;
            SFX   = sFX;
        }
    }

    class Enemy
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Spd { get; set; } = 9999;

        public List<int> BuffList = new List<int>();
        public List<int> DebuffList = new List<int>();

        Queue<int> patternQueue = new Queue<int>();

        public EnemySkill CurrentSkill = null;

        public Enemy(int hp, int maxHp, string pattern)
        {
            this.Hp = hp;
            this.MaxHp = maxHp;

            string[] patterns = pattern.Split('_');

            foreach(string temp in patterns)
            {
                patternQueue.Enqueue(int.Parse(temp));
            }
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

            if (skill[3] != "")
            {
                string[] buffStrings = skill[3].Split('_');

                buffArr = new int[buffStrings.Length];

                for (int i = 0; i < buffArr.Length; i++)
                {
                    buffArr[i] = int.Parse(buffStrings[i]);
                }
            }

            CurrentSkill = new EnemySkill(skill[0], pwr, (PatternType)int.Parse(skill[2]), buffArr, skill[5]);

            Spd = int.Parse(skill[4]);

            patternQueue.Enqueue(patternIndex);
        }

        public void CastPattern(Player player)
        {
            switch(CurrentSkill.Type)
            {
                case PatternType.PLAYER_TARGET:
                    player.OnHit(CurrentSkill.Power);
                    break;
            }
        }
    }
}
