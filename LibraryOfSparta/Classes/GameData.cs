namespace LibraryOfSparta.Classes
{
    public class GameData
    {
        public int       CurrentFloor { get; set; } = 10;
        public List<int> Inventory    { get; set; } = new List<int>() { 3, 3 };
        public List<int> Deck         { get; set; } = new List<int>() { 1, 1, 1, 2, 2, 2, 4, 4, 4, 3 };
    }
}
