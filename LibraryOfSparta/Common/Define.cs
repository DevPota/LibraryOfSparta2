namespace LibraryOfSparta.Common
{
    public static class Define
    {
        public const int SCREEN_X = 130;
        public const int SCREEN_Y = 50;

        public static readonly string LOCAL_GAME_PATH = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.Parent.FullName.ToString();
        public static readonly string DATA_PATH       = LOCAL_GAME_PATH + "/Data";
        public static readonly string IMAGE_PATH      = LOCAL_GAME_PATH + "/Images";
        public static readonly string BGM_PATH        = LOCAL_GAME_PATH + "/Sounds/BGM";
        public static readonly string SFX_PATH        = LOCAL_GAME_PATH + "/Sounds/SFX";
    }
}
