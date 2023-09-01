using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System.Diagnostics;

namespace LibraryOfSparta.Classes
{
    public class Buff
    {
        public int    BuffIndex    { get; set; }
        public string Name         { get; set; }
        public PlayerBuffType Type { get; set; }
        public int Power           { get; set; }

        public Buff(int buffIndex, string name, PlayerBuffType type, int power)
        {
            BuffIndex = buffIndex;
            Name = name;
            Type = type;
            Power = power;
        }
    }

    public class Player
    {
        public int Hp    { get; set; } = 100;
        public int MaxHp { get; set; } = 100;
        public int Str   { get; set; } = 0;
        public int Def   { get; set; } = 0;
        public int Spd   { get; set; } = 10;
        public int Fcs   { get; set; } = 10;

        public int Emotion { get; set; } = 0;
        public int Token   { get; set; } = 0;

        public int PlayerCost       { get; set; }  = 3;
        public int PlayerCostFilled { get; set; }  = 0;
        public int PlayerDrawFilled { get; set; }  = 0;

        public List<int> PlayerHands = new List<int>();
        public List<int> Deck;

        public List<int> BuffList   = new List<int>();
        public List<int> DebuffList = new List<int>();

        Action<int, int>                                            hpListener       = (int i, int j) => { };
        Action<List<int>, Player>                                   drawListener     = (default);
        Action<int, int>                                            costFillListener = (int i, int j) => { };
        Action<int>                                                 drawFillListener = (int i) => { };
        Action<string, string, string, int, BattleSitulation>       dialogListener   = (default);
        Action<List<int>, List<int>, string[]>                      buffListener     = (default);
        Action<Player>                                              statusListner    = (default);
        public string[] buffData;
        bool counter = false;

        public Player(Action<int, int> hpAction, 
            Action<List<int>, Player> drawAction, 
            Action<int, int> costFillAction, 
            Action<int> drawFillAction, 
            Action<string, string, string, int, BattleSitulation> dialogAction, 
            Action<List<int>, List<int>, string[]> buffAction,
            Action<Player> statusAction)
        {
            hpListener   = hpAction;
            drawListener = drawAction;
            costFillListener = costFillAction;
            drawFillListener = drawFillAction;
            dialogListener = dialogAction;
            buffListener = buffAction;
            statusListner = statusAction;
            buffData = Core.GetData(Define.P_BUFF_DATA_PATH).Split('\n');
        }

        public int GetStrBuffValue()
        {
            int str = 0;

            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            for (int i = 0; i < BuffList.Count; i++)
            {
                int element = BuffList[i];
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.힘_버프)
                {
                    str += int.Parse(buffData[2]);

                }
            }

            for (int i = 0; i < DebuffList.Count; i++)
            {
                int element = DebuffList[i];
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.힘_디버프 || type == PlayerBuffType.해제불가_힘_디버프)
                {
                    str -= int.Parse(buffData[2]);

                }
            }

            return str;
        }

        public int GetStr(bool erase)
        {
            int str = Str + Emotion;

            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            Queue<int> queue = new Queue<int>(BuffList);
            Queue<int> Debuffqueue = new Queue<int>(DebuffList);

            for (int i = 0; i < BuffList.Count; i++)
            {
                int element = queue.Dequeue();
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if(type == PlayerBuffType.힘_버프)
                {
                    str += int.Parse(buffData[2]);

                    if (erase == false)
                    {
                        queue.Enqueue(element);
                    }
                }
                else
                {
                    queue.Enqueue(element);
                }
            }

            for (int i = 0; i < DebuffList.Count; i++)
            {
                int element = Debuffqueue.Dequeue();
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.힘_디버프 || type == PlayerBuffType.해제불가_힘_디버프)
                {
                    str -= int.Parse(buffData[2]);

                    if (erase == false || type == PlayerBuffType.해제불가_힘_디버프)
                    {
                        Debuffqueue.Enqueue(element);
                    }
                }
                else
                {
                    Debuffqueue.Enqueue(element);
                }
            }

            BuffList = new List<int>(queue);
            DebuffList = new List<int>(Debuffqueue);

            return str;
        }

        public int GetDefBuffValue()
        {
            int def = 0;

            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            for (int i = 0; i < BuffList.Count; i++)
            {
                int element = BuffList[i];
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.방어 || type == PlayerBuffType.반격)
                {
                    def += int.Parse(buffData[2]);
                }
            }

            for (int i = 0; i < DebuffList.Count; i++)
            {
                int element = DebuffList[i];
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.해제불가_방어_디버프)
                {
                    def -= int.Parse(buffData[2]);
                }
            }

            return def;
        }

        public int GetDef(bool erase)
        {
            int def = Def;

            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            Queue<int> queue = new Queue<int>(BuffList);
            Queue<int> Debuffqueue = new Queue<int>(DebuffList);

            for (int i = 0; i < BuffList.Count; i++)
            {
                int element = queue.Dequeue();
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if(type == PlayerBuffType.반격)
                {
                    counter = true;
                }

                if (type == PlayerBuffType.방어 || type == PlayerBuffType.반격)
                {
                    def += int.Parse(buffData[2]);

                    if(erase == false)
                    {
                        queue.Enqueue(element);
                    }
                }
                else
                {
                    queue.Enqueue(element);
                }
            }

            for (int i = 0; i < DebuffList.Count; i++)
            {
                int element = Debuffqueue.Dequeue();
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.해제불가_방어_디버프)
                {
                    def -= int.Parse(buffData[2]);

                    if (erase == false || type == PlayerBuffType.해제불가_방어_디버프)
                    {
                        Debuffqueue.Enqueue(element);
                    }
                }
                else
                {
                    Debuffqueue.Enqueue(element);
                }
            }

            BuffList = new List<int>(queue);
            DebuffList = new List<int>(Debuffqueue);

            return def;
        }

        public int GetDodge()
        {
            int dodgeValue = 0;

            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            Queue<int> queue = new Queue<int>(BuffList);

            for (int i = 0; i < BuffList.Count; i++)
            {
                int element = queue.Dequeue();
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.회피)
                {
                    dodgeValue += int.Parse(buffData[2]);
                }
                else
                {
                    queue.Enqueue(element);
                }
            }

            BuffList = new List<int>(queue);

            return dodgeValue;
        }

        public int GetFcs()
        {
            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            int fcsValue = 0;

            for (int i = 0; i < BuffList.Count; i++)
            {
                string[] buffData = lines[BuffList[i]].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.해제불가_집중_버프)
                {
                    fcsValue += int.Parse(buffData[2]);
                }
            }

            return fcsValue;
        }

        public bool OnHit(int damage)
        {
            int dodgeValue = GetDodge();

            if (dodgeValue != 0 && dodgeValue >= damage)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Evade.wav");
                dialogListener("", "", "", 0, BattleSitulation.EVADE);
                ((Battle)Core.CurrentScene).AddToken(true, 1);
                UpdatePlayerUI();
                return false;
            }
            else
            {
                int defValue = GetDef(true);
                int damageValue = damage;
                
                if (defValue != 0)
                {
                    if(defValue < 0)
                    {
                        counter = false;
                        Hp += defValue - damage;
                        dialogListener(((Battle)Core.CurrentScene).floorData[1], "당신", "", defValue + damage, BattleSitulation.ATK);
                        UpdatePlayerUI();
                        return true;
                    }

                    if(counter == true)
                    {
                        counter = false;
                        Core.PlaySFX(Define.SFX_PATH + "/Counter.wav");
                        ((Battle)Core.CurrentScene).Enemy.OnHit(defValue);
                        dialogListener("", ((Battle)Core.CurrentScene).floorData[1], "", damageValue, BattleSitulation.COUNTER);
                    }
                    else
                    {
                        Core.PlaySFX(Define.SFX_PATH + "/Defense.wav");
                    }

                    damageValue = (defValue + Emotion) - damage;

                    if(damageValue < 0)
                    {
                        damageValue = Math.Abs(damageValue);
                    }
                    else
                    {
                        damageValue = 0;
                        dialogListener("", "","", damageValue, BattleSitulation.DEF);
                        ((Battle)Core.CurrentScene).AddToken(true, 1);
                        UpdatePlayerUI();
                        return false;
                    }
                    Hp -= damageValue;
                    dialogListener("", "","", damageValue, BattleSitulation.DEF);
                    UpdatePlayerUI();
                    return false;
                }

                damageValue -= Emotion;

                if(damageValue <= 0)
                {
                    return false;
                }

                Hp -= damageValue;
            }
            
            UpdatePlayerUI();

            return true;
        }

        public void CastCard(int index, Enemy enemy, string[] cardData)
        {
            UpdatePlayerUI();

            if (PlayerHands.Count <= index)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                return;
            }
            else
            {
                string[] card = cardData[PlayerHands[index]].Split(',');
                int cardCost = int.Parse(card[3]);
                if (cardCost <= PlayerCost)
                {
                    Core.PlaySFX(Define.SFX_PATH + "/" + card[5] + ".wav");
                    PlayerCost -= cardCost;

                    RemoveCardFromHand(index);

                    int power = int.Parse(card[2]);
                    CardType cardType = (CardType)int.Parse(card[1]);
                    List<Buff> buffs = null;

                    if (card[4] != "-1")
                    {
                        buffs = new List<Buff>();
                        string[] buffSplited = card[4].Split("_");

                        foreach(string buffIndex in buffSplited)
                        {
                            string[] buff = buffData[int.Parse(buffIndex)].Split(',');

                            Buff newBuff = new Buff(int.Parse(buffIndex), buff[0], (PlayerBuffType)int.Parse(buff[1]), int.Parse(buff[2]));

                            buffs.Add(newBuff);
                        }
                    }

                    switch(cardType)
                    {
                        case CardType.공격 :
                            if (CheckMark(PlayerBuffType.공격_표식) == true)
                            {
                                Core.PlaySFX(Define.SFX_PATH + "/Hokma_Defeat.wav");
                                UpdatePlayerUI();
                                return;
                            }
                            ((Battle)Core.CurrentScene).AddToken(true, 1);
                            dialogListener("당신", ((Battle)Core.CurrentScene).floorData[1], card[0], power + GetStr(false), BattleSitulation.ATK);
                            enemy.OnHit(power + GetStr(true));
                            break;
                        case CardType.공격_자가버프:
                            if (CheckMark(PlayerBuffType.공격_표식) == true)
                            {
                                Core.PlaySFX(Define.SFX_PATH + "/Hokma_Defeat.wav");
                                UpdatePlayerUI();
                                return;
                            }

                            ((Battle)Core.CurrentScene).AddToken(true, 1);
                            dialogListener("당신", ((Battle)Core.CurrentScene).floorData[1], card[0], power + GetStr(false), BattleSitulation.ATK);
                            enemy.OnHit(power + GetStr(true));
                            
                            if(buffs != null)
                            {
                                string[] lines = Core.GetData(Define.P_BUFF_DATA_PATH).Split('\n');
                                List<PlayerBuffType> buffTypes = new List<PlayerBuffType>();
                                foreach (Buff buff in buffs)
                                {
                                    PlayerBuffType type = (PlayerBuffType)int.Parse(lines[buff.BuffIndex].Split(',')[1]);
                                    buffTypes.Add(type);
                                    AddBuff(buff.BuffIndex);
                                }

                                foreach (PlayerBuffType type in buffTypes)
                                {
                                    if (type == PlayerBuffType.방어)
                                    {
                                        if (CheckMark(PlayerBuffType.방어_표식) == true)
                                        {
                                            Core.PlaySFX(Define.SFX_PATH + "/Hokma_Defeat.wav");
                                            UpdatePlayerUI();
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        case CardType.버프:
                        case CardType.방어:
                            if (buffs != null)
                            {
                                string[] lines = Core.GetData(Define.P_BUFF_DATA_PATH).Split('\n');
                                List<PlayerBuffType> buffTypes = new List<PlayerBuffType>();
                                foreach (Buff buff in buffs)
                                {
                                    PlayerBuffType type = (PlayerBuffType)int.Parse(lines[buff.BuffIndex].Split(',')[1]);
                                    buffTypes.Add(type);
                                    AddBuff(buff.BuffIndex);
                                }

                                foreach(PlayerBuffType type in buffTypes)
                                {
                                    if(type == PlayerBuffType.방어)
                                    {
                                        if (CheckMark(PlayerBuffType.방어_표식) == true)
                                        {
                                            Core.PlaySFX(Define.SFX_PATH + "/Hokma_Defeat.wav");
                                            UpdatePlayerUI();
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        case CardType.회복:
                            if(CheckMark(PlayerBuffType.회복_표식) == true)
                            {
                                Core.PlaySFX(Define.SFX_PATH + "/Hokma_Defeat.wav");
                                UpdatePlayerUI();
                                return;
                            }

                            if (buffs != null)
                            {
                                foreach (Buff buff in buffs)
                                {
                                    AddBuff(buff.BuffIndex);
                                }
                            }
                            dialogListener("당신", ((Battle)Core.CurrentScene).floorData[1], card[0], power, BattleSitulation.HEAL);
                            Hp += power;
                            if(Hp > MaxHp)
                            {
                                Hp = MaxHp;
                            }
                            hpListener(Hp, MaxHp);
                            break;
                    }
                }
                else
                {
                    Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                }
                UpdatePlayerUI();
            }
        }

        public void UpdatePlayerUI()
        {
            hpListener(Hp, MaxHp);
            statusListner(this);
            buffListener(BuffList, DebuffList, buffData);
            drawListener(PlayerHands, this);
        }

        public void AddBuff(int buffIndex)
        {
            string[] buff = buffData[buffIndex].Split(',');

            PlayerBuffType type = (PlayerBuffType)int.Parse(buff[1]);

            int power = int.Parse(buff[2]);

            switch (type)
            {
                case PlayerBuffType.드로우 :
                    for(int i = 0; i < power; i++)
                    {
                        if (PlayerHands.Count >= 4)
                        {
                            RemoveCardFromHand(0);
                            AddCardToHand();
                        }
                        else
                        {
                            AddCardToHand();
                        }
                    }
                    UpdatePlayerUI();
                    break;
                case PlayerBuffType.코스트_회복:
                    for(int i = 0; i < power; i++)
                    {
                        AddCostToPlayer();
                    }
                    UpdatePlayerUI();
                    break;
                case PlayerBuffType.감정_레벨_증가:
                    for(int i = 0; i < power; i++)
                    {
                        ((Battle)Core.CurrentScene).AddToken(true, 5);
                    }
                    UpdatePlayerUI();
                    break;
                case PlayerBuffType.카드_버림:
                    for (int i = 0; i < power; i++)
                    {
                        RemoveCardFromHand(i);
                    }
                    UpdatePlayerUI();
                    break;
                case PlayerBuffType.빛_버림:
                    for (int i = 0; i < power; i++)
                    {
                        PlayerCost--;
                    }
                    UpdatePlayerUI();
                    break;
                default :
                    if (BuffList.Count >= 10)
                    {
                        return;
                    }

                    BuffList.Add(buffIndex);
                    break;
            }
        }

        public void AddDebuff(int buffIndex)
        {
            if (DebuffList.Count >= 10)
            {
                return;
            }

            DebuffList.Add(buffIndex);
        }

        public void AddCardToHand()
        {
            for (int i = 0; i < Deck.Count; i++)
            {
                if (Deck[i] != -1)
                {
                    PlayerHands.Add(Deck[i]);
                    Deck[i] = -1;
                    drawListener(PlayerHands, this);
                    return;
                }
            }

            InitDeck();

            PlayerHands.Add(Deck[0]);
            Deck[0] = -1;

            drawListener(PlayerHands, this);
        }

        public void RemoveCardFromHand(int index)
        {
            if (index >= PlayerHands.Count)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                return;
            }

            PlayerHands.RemoveAt(index);

            drawListener(PlayerHands, this);
        }

        public bool CheckMark(PlayerBuffType type)
        {
            string[] buffDataLines = Core.GetData(Define.P_BUFF_DATA_PATH).Split('\n');

            Queue<int> debuffQueue = new Queue<int>();

            int damage = 0;

            foreach(int debuffIndex in DebuffList)
            {
                string[] debuffData = buffDataLines[debuffIndex].Split(',');
                PlayerBuffType debuffType = (PlayerBuffType)int.Parse(debuffData[1]);
                int debuffDamage = int.Parse(debuffData[2]);

                if(type == debuffType)
                {
                    damage += debuffDamage;
                }
                else
                {
                    debuffQueue.Enqueue(debuffIndex);
                }
            }

            DebuffList = new List<int>(debuffQueue);

            if(damage != 0)
            {
                OnHit(damage);
                return true;
            }
            else
            {
                return false;
            }

            UpdatePlayerUI();
        }

        public void InitDeck()
        {
            Deck = new List<int>();

            foreach (int element in Core.SaveData.Deck)
            {
                Deck.Add(element);
            }
            
            foreach(int hand in PlayerHands)
            {
                for(int i = 0; i < Deck.Count; i++)
                {
                    if (Deck[i] == hand)
                    {
                        Deck[i] = -1;
                        break;
                    }
                }
            }
            Queue<int> que = new Queue<int>();

            foreach(int deckCards in Deck)
            {
                if(deckCards != -1)
                {
                    que.Enqueue(deckCards);
                }
            }
            Deck = new List<int>(que);

            Random random = new Random();

            for (int i = 0; i < 500; i++)
            {
                int a = random.Next(0, Deck.Count);
                int b = random.Next(0, Deck.Count);

                if (a == b)
                {
                    continue;
                }
                else
                {
                    int temp = Deck[a];
                    Deck[a] = Deck[b];
                    Deck[b] = temp;
                }
            }
        }

        public void AddCostToPlayer()
        {
            PlayerCostFilled = 0;
            PlayerCost++;

            if (PlayerCost >= 10)
            {
                PlayerCost = 10;
            }

            UpdatePlayerUI();
        }

        public void UpdatePlayerCost()
        {
            if (PlayerCost >= 10)
            {
                return;
            }

            PlayerCostFilled += (Spd+Emotion) / 5;

            if (PlayerCostFilled >= 31)
            {
                AddCostToPlayer();
            }

            costFillListener(PlayerCost, PlayerCostFilled);
        }

        public void UpdatePlayerDraw()
        {
            PlayerDrawFilled += (Fcs + GetFcs() + Emotion) / 10;

            if (PlayerDrawFilled >= 31)
            {
                PlayerDrawFilled = 0;

                if (PlayerHands.Count >= 4)
                {
                    RemoveCardFromHand(0);
                    AddCardToHand();
                }
                else
                {
                    AddCardToHand();
                }
            }

            drawFillListener(PlayerDrawFilled);
        }
    }
}
