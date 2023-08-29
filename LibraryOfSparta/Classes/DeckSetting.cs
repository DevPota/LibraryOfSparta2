using System;
using System.Collections.Generic;
using System.Linq;
using System;
using LibraryOfSparta.Common;
using Microsoft.VisualBasic;

public class DeckSetting
{

    List<Card> myCrad = new List<Card>();
    Card?[] myDeck = new Card?[10];
    int page => 2;

    public void Init()
    {
        TestCard();

        Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);
        MainPanel();
        CardDrow();

        Input();
    }

    public void Update()
    {


    }

    void MainPanel()
    {

        Console.SetCursorPosition(1, 1);
        {
            Console.Write("┌");
            for (int i = 0; i < Define.SCREEN_X - 5; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┐");
        }// 가장 위
        {
            for (int i = 0; i < Define.SCREEN_Y - 8; i++)
            {
                Console.SetCursorPosition(1, i + 2);
                Console.Write("│");
                for (int j = 0; j < Define.SCREEN_X - 5; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("│");
            }
        }// 가운데
        {
            Console.SetCursorPosition(1, Define.SCREEN_Y - 6);
            Console.Write("└");
            for (int i = 0; i < Define.SCREEN_X - 5; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┘");
        }// 가장 아래
        {
            Console.SetCursorPosition(2, Define.SCREEN_Y - 2);
            for (int i = 0; i < Define.SCREEN_X - 5; i++)
            {
                Console.Write("─");
            }
        }// 입력 아래

        {
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(@"
 │         __  __ ___ ___    _____  _____  _____  _____                __  __ ___ ___    _____  _____  _____  __ ___
 │        /  \/  \\  |  /   /     \/  _  \/  _  \|  _  \              /  \/  \\  |  /   |  _  \/   __\/     \|  |  /
 │        |  \/  | |   |    |  |--||  _  ||  _  <|  |  |              |  \/  | |   |    |  |  ||   __||  |--||  _ < 
 │        \__/\__/ \___/    \_____/\__|__/\__|\_/|_____/              \__/\__/ \___/    |_____/\_____/\_____/|__|__\
");
        }// MY CARD, MY DECK

        {
            Console.SetCursorPosition(8, 7);
            Console.Write("┌");
            for (int i = 0; i < (Define.SCREEN_X / 2) - 14; i++)
            {
                Console.Write("─");
            }
            Console.Write("┐");
            Console.Write("       ");
            Console.Write("┌");
            for (int i = 0; i < (Define.SCREEN_X / 2) - 14; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┐");
        }// 인벤토리 가장 위
        {
            for (int j = 0; j < Define.SCREEN_Y - 31; j++)
            {
                Console.SetCursorPosition(8, j + 8);
                Console.Write("│ ");
                for (int i = 0; i < (Define.SCREEN_X / 2) - 16; i++)
                {
                    if (j % 2 == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("─");
                    }
                }
                Console.Write(" │");
                Console.Write("       ");
                Console.Write("│ ");
                for (int i = 0; i < (Define.SCREEN_X / 2) - 16; i++)
                {
                    if (j % 2 == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("─");
                    }
                }
                Console.WriteLine(" │");
            }
        }// 인벤토리 가운데
        {
            Console.SetCursorPosition(8, Define.SCREEN_Y - 23);
            Console.Write("└");
            for (int i = 0; i < (Define.SCREEN_X / 2) - 14; i++)
            {
                Console.Write("─");
            }
            Console.Write("┘");
            Console.Write("       ");
            Console.Write("└");
            for (int i = 0; i < (Define.SCREEN_X / 2) - 14; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┘");
        }// 인벤토리 가장 아래

        {
            Console.SetCursorPosition(0, Define.SCREEN_Y - 22);
            Console.Write(" │          ◀[Q]              {0}페이지               [W]▶", page);
        }

        {
            Console.SetCursorPosition(0, Define.SCREEN_Y - 21);
            Console.WriteLine(@"
 │         _____  _____  _____  _____     ___  _____  _____  _____ 
 │        /     \/  _  \/  _  \|  _  \   /   \/  _  \/   __\/  _  \
 │        |  |--||  _  ||  _  <|  |  |   |   ||  |  ||   __||  |  |
 │        \_____/\__|__/\__|\_/|_____/   \___/\__|__/\__/   \_____/
");
        }//CARD INFO


        {
            Console.SetCursorPosition(8, Define.SCREEN_Y - 16);
            Console.Write("┌");
            for (int i = 0; i < Define.SCREEN_X - 19; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┐");
        }// 설명 창 가장 위
        {
            for (int i = 0; i < 6; i++)
            {
                Console.SetCursorPosition(8, Define.SCREEN_Y - 15 + i);
                Console.Write("│");
                for (int j = 0; j < Define.SCREEN_X - 19; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("│");
            }
        }// 설명 창 가운데
        {
            Console.SetCursorPosition(8, Define.SCREEN_Y - 9);
            Console.Write("└");
            for (int i = 0; i < Define.SCREEN_X - 19; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┘");
        }// 설명 창 가장 아래

    }// 메인 틀

    void CardDrow()
    {
        for (int i = 0; i < 10; i++)
        {
            int index = (page - 1) * 10 + i;
            if (index < myCrad.Count)
            {
                Console.SetCursorPosition(13, 8 + i * 2);
                Console.Write("{0}", myCrad[index].name);
            }
        }
    }

    void Input()
    {
        Console.SetCursorPosition(2, Define.SCREEN_Y - 4);
        Console.Write("입력 : ");
        string Input = Console.ReadLine();
    }// 입력 

    void TestCard()
    {
        myCrad.Add(new Card("가벼운 공격", 10, 0, 1, "적에게 10 대미지를 준다"));
        myCrad.Add(new Card("회피", 8, 1, 1, "8 이하의 다음 공격은 무조건 회피한다"));
        myCrad.Add(new Card("가벼운 방어", 5, 2, 1, "다음 받는 대미지를 5 경감 시킨다"));
        myCrad.Add(new Card("공격과 수비", 12, 3, 2, "적에게 12 대미지를 주고 다음 받는 대미지를 6 경감시킨다"));
        myCrad.Add(new Card("집중 공격", 18, 0, 2, "적에게 18 대미지를 준다"));
        myCrad.Add(new Card("가벼운 공격", 10, 0, 1, "적에게 10 대미지를 준다"));
        myCrad.Add(new Card("회피", 8, 1, 1, "8 이하의 다음 공격은 무조건 회피한다"));
        myCrad.Add(new Card("가벼운 방어", 5, 2, 1, "다음 받는 대미지를 5 경감 시킨다"));
        myCrad.Add(new Card("공격과 수비", 12, 3, 2, "적에게 12 대미지를 주고 다음 받는 대미지를 6 경감시킨다"));
        myCrad.Add(new Card("집중 공격", 18, 0, 2, "적에게 18 대미지를 준다"));
        myCrad.Add(new Card("가벼운 공격", 10, 0, 1, "적에게 10 대미지를 준다"));
    }
}

struct Card
{
    public string name;
    public int power;
    public int type;
    public int cost;
    public string dialog;

    public Card(string name_, int power_, int typy_, int cost_, string dialog_)
    {
        name = name_;
        power = power_;
        type = typy_;
        cost = cost_;
        dialog = dialog_;
    }
}
