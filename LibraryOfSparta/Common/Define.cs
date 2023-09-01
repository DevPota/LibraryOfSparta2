namespace LibraryOfSparta.Common
{
    public static class Define
    {
        public const int SCREEN_X = 130;
        public const int SCREEN_Y = 50;

        public static readonly string LOCAL_GAME_PATH  = ".";
        public static readonly string DATA_PATH        = LOCAL_GAME_PATH + "/Data";
        public static readonly string IMAGE_PATH       = LOCAL_GAME_PATH + "/Images";
        public static readonly string BGM_PATH         = LOCAL_GAME_PATH + "/Sounds/BGM";
        public static readonly string SFX_PATH         = LOCAL_GAME_PATH + "/Sounds/SFX";
        public static readonly string SAVE_PATH        = LOCAL_GAME_PATH + "/Data/SaveData.json";
        public static readonly string CARD_DATA_PATH   = LOCAL_GAME_PATH + "/Data/PlayerCardData.csv";
        public static readonly string E_SKILL_DATA_PATH= LOCAL_GAME_PATH + "/Data/EnemySkillData.csv";
        public static readonly string P_BUFF_DATA_PATH = LOCAL_GAME_PATH + "/Data/PlayerBuffData.csv";
        public static readonly string E_BUFF_DATA_PATH = LOCAL_GAME_PATH + "/Data/EnemyBuffData.csv";

        public static readonly string BATTLE_DATA_PATH = DATA_PATH + "/BattleData.csv";
    }
}
