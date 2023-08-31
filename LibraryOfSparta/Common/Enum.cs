namespace LibraryOfSparta.Common
{
    public enum CardType
    {
        공격 = 0,
        방어 = 1,
        공격_자가버프 = 2,
        버프 = 3,
        회복 = 4
    }

    public enum PlayerBuffType
    {
        방어 = 0,
        회피 = 1,
        힘_디버프 = 2,
        힘_버프 = 3,
        드로우 = 4,
        코스트_회복 = 5,
        감정_레벨_증가 = 6
    }

    public enum PatternType
    {
        감정토큰없는플레이어공격 = 0,
        플레이어공격 = 1,
        감정토큰없는플레이어버프 = 2,
        자폭 = 3
    }
}
