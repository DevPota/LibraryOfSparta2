﻿using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using LibraryOfSparta;
using LibraryOfSparta.Classes;
using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;

class Program
{
    static bool applicationQuit = false;

    static void Main()
    {
        Awake();
        Start();
    }

    static void Awake()
    {
        Console.InputEncoding  = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);

        Core.Init();
        Core.RenderSystemUI();
    }

    static void Start()
    {
        Core.LoadScene(11);

        while (applicationQuit == false)
        {
            Update();

            Thread.Sleep(100);
        }
    }

    static void Update()
    {
        Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);

        Core.BGMUpdate();

        switch (Core.SceneIndex)
        {
            case 0:
                ((TitleMenu)Core.CurrentScene).Update();
                break;
            case 1:
                ((Entrance)Core.CurrentScene).Update();
                break;
            case 2:
                ((DeckSetting)Core.CurrentScene).Update();
                break;
            case 3:
                ((Battle)Core.CurrentScene).Update();
                break;
            case 4:
                ((Result)Core.CurrentScene).Update();
                break;
            case 5:
                ((Credit)Core.CurrentScene).Update();
                break;
            case 6:
                ((VictoryCutScene)Core.CurrentScene).Update();
                break;
            case 11:
                ((Intro)Core.CurrentScene).Update();
                break;
            case 12:
                ((BattleIntro)Core.CurrentScene).Update();
                break;
        }

        Core.ReleaseKey();
    }

    public static void Exit()
    {
        applicationQuit = true;
    }
}