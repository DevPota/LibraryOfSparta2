using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using System.Numerics;
using System.Xml.Linq;

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
        Action<List<int>>                                           drawListener     = (default);
        Action<int, int>                                            costFillListener = (int i, int j) => { };
        Action<int>                                                 drawFillListener = (int i) => { };
        Action<string, string, string, int, BattleSitulation>       dialogListener   = (default);
        Action<List<int>, List<int>, string[]>                      buffListener     = (default);
        Action<Player>                                              statusListner    = (default);
        public string[] buffData;


        public Player(Action<int, int> hpAction, 
            Action<List<int>> drawAction, 
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

        public int GetStr(bool erase)
        {
            int str = Str;

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

                if (type == PlayerBuffType.힘_디버프)
                {
                    str -= int.Parse(buffData[2]);

                    if (erase == false)
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

        public int GetDef(bool erase)
        {
            int def = Def;

            string pBuffData = Core.GetData(Define.P_BUFF_DATA_PATH);
            string[] lines = pBuffData.Split('\n');

            Queue<int> queue = new Queue<int>(BuffList);

            for(int i = 0; i < BuffList.Count; i++)
            {
                int element = queue.Dequeue();
                string[] buffData = lines[element].Split(',');
                PlayerBuffType type = (PlayerBuffType)int.Parse(buffData[1]);

                if (type == PlayerBuffType.방어)
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

            BuffList = new List<int>(queue);

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

        public bool OnHit(int damage)
        {
            int dodgeValue = GetDodge();

            if (dodgeValue != 0 && dodgeValue >= damage)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Evade.wav");
                dialogListener("", "", "", 0, BattleSitulation.EVADE);
                UpdatePlayerUI();
                return false;
            }
            else
            {
                int defValue = GetDef(true);
                int damageValue = damage;

                if (defValue != 0)
                {
                    Core.PlaySFX(Define.SFX_PATH + "/Defense.wav");
                    damageValue = defValue - damage;

                    if(damageValue < 0)
                    {
                        damageValue = Math.Abs(damageValue);
                    }
                    else
                    {
                        damageValue = 0;
                    }
                    dialogListener("", "","", damageValue, BattleSitulation.DEF);
                }

                Hp -= damageValue;
            }

            UpdatePlayerUI();

            return true;
        }

        public void CastCard(int index, Enemy enemy, string[] cardData)
        {
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
                            ((Battle)Core.CurrentScene).AddToken(true);
                            dialogListener("당신", ((Battle)Core.CurrentScene).floorData[1], card[0], power + GetStr(true), BattleSitulation.ATK);
                            enemy.OnHit(power + GetStr(true));
                            break;
                        case CardType.공격_자가버프:
                            ((Battle)Core.CurrentScene).AddToken(true);
                            dialogListener("당신", ((Battle)Core.CurrentScene).floorData[1], card[0], power + GetStr(true), BattleSitulation.ATK);
                            enemy.OnHit(power + GetStr(true));
                            
                            if(buffs != null)
                            {
                                foreach(Buff buff in buffs)
                                {
                                    AddBuff(buff.BuffIndex);
                                }
                            }
                            break;
                        case CardType.버프:
                        case CardType.방어:
                            if (buffs != null)
                            {
                                foreach (Buff buff in buffs)
                                {
                                    AddBuff(buff.BuffIndex);
                                }
                            }
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
        }

        public void AddBuff(int buffIndex)
        {
            if(BuffList.Count >= 10)
            {
                return;
            }

            BuffList.Add(buffIndex);
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
                    drawListener(PlayerHands);
                    return;
                }
            }

            InitDeck();

            PlayerHands.Add(Deck[0]);
            Deck[0] = -1;

            drawListener(PlayerHands);
        }

        public void RemoveCardFromHand(int index)
        {
            if (index >= PlayerHands.Count)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                return;
            }

            PlayerHands.RemoveAt(index);

            drawListener(PlayerHands);
        }

        public void InitDeck()
        {
            Deck = new List<int>();

            foreach (int element in Core.SaveData.Deck)
            {
                Deck.Add(element);
            }

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

        public void UpdatePlayerCost()
        {
            if (PlayerCost >= 10)
            {
                return;
            }

            PlayerCostFilled += Spd / 5;

            if (PlayerCostFilled >= 31)
            {
                PlayerCostFilled = 0;
                PlayerCost++;

                if (PlayerCost >= 10)
                {
                    PlayerCost = 10;
                }
            }

            costFillListener(PlayerCost, PlayerCostFilled);
        }

        public void UpdatePlayerDraw()
        {
            PlayerDrawFilled += Fcs / 10;

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
