namespace LibraryOfSparta.Classes
{
    class Enemy
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Atb { get; set; }
        public int MaxAtb { get; set; }
        string pattern;

        public Enemy(int hp, int maxHp, string pattern)
        {
            this.Hp = hp;
            this.MaxHp = maxHp;
            this.pattern = pattern;
        }
    }
}
