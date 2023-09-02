﻿namespace LibraryOfSparta.Common
{
    public enum CardType
    {
        공격 = 0,
        방어 = 1,
        공격_자가버프 = 2,
        버프 = 3,
        회복 = 4,
        대검 = 5,
        버프_디버프 = 6
    }

    public enum PlayerBuffType
    {
        방어 = 0,
        회피 = 1,
        힘_디버프 = 2,
        힘_버프 = 3,
        드로우 = 4,
        코스트_회복 = 5,
        감정_레벨_증가 = 6,
        카드_버림 = 7,
        빛_버림 = 8,
        해제불가_더미 = 9,
        해제불가_속도_버프 = 10,
        해제불가_집중_버프 = 11,
        반격 = 12,
        해제불가_힘_디버프 = 13,
        해제불가_방어_디버프 = 14,
        픽셀_셰이더_청 = 15,
        공격_표식 = 16,
        회복_표식 = 17,
        방어_표식 = 18,
        버린만큼_다시_뽑기 = 19
    }

    public enum PatternType
    {
        감정토큰없는플레이어공격 = 0,
        플레이어공격 = 1,
        감정토큰없는플레이어버프 = 2,
        자폭 = 3,
        감정토큰없는디버프부여 = 4,
        플레이어공격및플레이어버프 = 5,
        재사용하지않고플레이어버프감정토큰없음 = 6,
        재사용하지않고플레이어디버프감정토큰없음 = 7,
        회복 = 8,
        감정토큰없는다이얼로그있는플레이어공격 = 9,
        재사용하지않고플레이어버프다날리고버프감정토큰없음 = 10,
        재사용하지않고플레이어디버프다날리고디버프감정토큰없음 = 11,
        플레이어모든버프삭제_해제불가_포함 = 12,
        회피및막지못하면손패삭제 = 13,
        회피및막지못하면코스트삭제 = 14,
        레스터라이즈 = 15,
        재사용하지않고플레이어공격 = 16,
        플레이어모든디버프삭제_해제불가_포함 = 17,
        오프닝 = 18,
        재사용하지않고플레이어공격디버프 = 19,
    }
}
