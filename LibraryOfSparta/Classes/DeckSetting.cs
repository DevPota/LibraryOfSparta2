using System;
using System.Collections.Generic;
using System.Linq;
using System;
using LibraryOfSparta.Common;
using Microsoft.VisualBasic;

public class DeckSetting
{

    List<Card> myCrad = new List<Card>();
    Card[] myDeck = new Card[10];

    int page = 1;
    int cursor = 0;
    string input;

    public void Init()
    {
        TestCard();

        Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);
        MainPanel();
        InventoryPanel();
        while (true) 
        {
            DescriptionPanel();
            MyCardDrow();
            MyDeckDrow();
            CursorDrow();

            Input();
        }
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
            Console.SetCursorPosition(0, Define.SCREEN_Y - 22);
            Console.Write(" │          ◀[Q]              {0}페이지               [W]▶", page);
        }//방향표 페이지

        {
            Console.SetCursorPosition(0, Define.SCREEN_Y - 21);
            Console.WriteLine(@"
 │         _____  _____  _____  _____     ___  _____  _____  _____ 
 │        /     \/  _  \/  _  \|  _  \   /   \/  _  \/   __\/  _  \
 │        |  |--||  _  ||  _  <|  |  |   |   ||  |  ||   __||  |  |
 │        \_____/\__|__/\__|\_/|_____/   \___/\__|__/\__/   \_____/
");
        }//CARD INFO
    }// 메인 틀

    void InventoryPanel()
    {
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
            Console.Write(" │          ◀[A]              {0}페이지               [D]▶   ", page);
        }//방향표 페이지
    }

    void DescriptionPanel()
    {
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
        //이름 파워 타입 코스트 설명

        try
        {
            Console.SetCursorPosition(10, Define.SCREEN_Y - 15);
            Console.WriteLine("카드  이름 : {0}", myCrad[(page - 1) * 10 + +cursor].name);
            Console.SetCursorPosition(10, Define.SCREEN_Y - 13);
            Console.WriteLine("카드  파워 : {0}", myCrad[(page - 1) * 10 + +cursor].power);
            Console.SetCursorPosition(10, Define.SCREEN_Y - 12);
            Console.WriteLine("카드  타입 : {0}", myCrad[(page - 1) * 10 + +cursor].type);
            Console.SetCursorPosition(10, Define.SCREEN_Y - 11);
            Console.WriteLine("카드 코스트: {0}", myCrad[(page - 1) * 10 + +cursor].cost);
            Console.SetCursorPosition(10, Define.SCREEN_Y - 10);
            Console.WriteLine("카드  설명 : {0}", myCrad[(page - 1) * 10 + +cursor].dialog);
        }
        catch { }
    }
    void MyCardDrow()
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

    void MyDeckDrow()
    {
        for (int i = 0; i<myDeck.Length; i++)
        {
            Console.SetCursorPosition(73, 8 + i* 2);
            Console.Write("{0}", myDeck[i].name);
        }
    }

    void CursorDrow()
    {
        Console.SetCursorPosition(10,(cursor * 2) + 8);
        Console.WriteLine("▶");

    }

    void InputCheck()
    {
        if (input == "A")
        {
            if (page > 1)
            {
                page -= 1;
                InventoryPanel();
            }
            else
            {
                Input();
            }
        }//전 페이지
        else if (input == "D")
        {
            if ((page * 10) < myCrad.Count)
            {
                page += 1;
                InventoryPanel();
            }
            else
            {
                Input();
            }
        }
        else if (input == "W")
        {
            Console.SetCursorPosition(10, (cursor * 2) + 8);
            Console.WriteLine("  ");
            if (cursor > 0)
            {

                cursor -= 1;
            }
            else
            {
                cursor = 9;
            }
        }
        else if (input == "S")
        {
            Console.SetCursorPosition(10, (cursor * 2) + 8);
            Console.WriteLine("  ");
            if (cursor < 9)
            {
                cursor += 1;
            }
            else
            {
                cursor = 0;
            }
                }
        else if (input == " ")
        {
            MyDeckAdd();
        }
        else
        {
            Input();
        }
    }
    void MyDeckAdd()
    {
        myDeck[0] = myCrad[(page - 1) * 10 + +cursor];
        MyDeckDrow();
    }

    void Input()
    {
        Console.SetCursorPosition(2, Define.SCREEN_Y - 4);
        Console.Write("입력 : ");
        input = Console.ReadLine();

        InputCheck();
    }// 입력 

    void TestCard()
    {
        myCrad.Add(new Card("가벼운 공격", 10, (CardType)0, 1, "적에게 10 대미지를 준다"));
        myCrad.Add(new Card("회피", 8, (CardType)1, 1, "8 이하의 다음 공격은 무조건 회피한다"));
        myCrad.Add(new Card("가벼운 방어", 5, (CardType)2, 1, "다음 받는 대미지를 5 경감 시킨다"));
        myCrad.Add(new Card("공격과 수비", 12, (CardType)3, 2, "적에게 12 대미지를 주고 다음 받는 대미지를 6 경감시킨다"));
        myCrad.Add(new Card("집중 공격", 18, (CardType)0, 2, "적에게 18 대미지를 준다"));
        myCrad.Add(new Card("가벼운 공격", 10, (CardType)0, 1, "적에게 10 대미지를 준다"));
        myCrad.Add(new Card("회피", 8, (CardType)1, 1, "8 이하의 다음 공격은 무조건 회피한다"));
        myCrad.Add(new Card("가벼운 방어", 5, (CardType)2, 1, "다음 받는 대미지를 5 경감 시킨다"));
        myCrad.Add(new Card("공격과 수비", 12, (CardType)3, 2, "적에게 12 대미지를 주고 다음 받는 대미지를 6 경감시킨다"));
        myCrad.Add(new Card("집중 공격", 18, (CardType)0, 2, "적에게 18 대미지를 준다"));
        myCrad.Add(new Card("무거운 공격", 20, (CardType)0, 1, "적에게 20 대미지를 준다"));
    }
}

struct Card
{
    public string name;
    public int power;
    public CardType type;
    public int cost;
    public string dialog;

    public Card(string name_, int power_, CardType typy_, int cost_, string dialog_)
    {
        name = name_;
        power = power_;
        type = typy_;
        cost = cost_;
        dialog = dialog_;
    }
}

enum CardType
{
    공격,
    회피,
    방어,
    공수
}