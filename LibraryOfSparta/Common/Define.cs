﻿namespace LibraryOfSparta.Common
{
    public static class Define
    {
        public const int SCREEN_X = 130;
        public const int SCREEN_Y = 50;

        public static readonly string LOCAL_GAME_PATH = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.Parent.FullName.ToString();
    }
}
