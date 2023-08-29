namespace LibraryOfSparta.Classes
{
    public class Player
    {
        public int Hp    { get; set; } = 100;
        public int MaxHp { get; set; } = 100;
        public int Str   { get; set; } = 10;
        public int Def   { get; set; } = 0;
        public int Spd   { get; set; } = 10;
        public int Fcs   { get; set; } = 10;

        public List<int> BuffList   = new List<int>();
        public List<int> DebuffList = new List<int>();

        Action<int, int> UIListener = (int i, int j) => { };

        public Player(Action<int, int> uIAction)
        {
            UIListener = uIAction;
        }

        public void OnHit(int damage)
        {
            // 버프, 디버프 계산 나중에 추가
            Hp -= damage;
            UIListener(Hp, MaxHp);
        }
    }
}
