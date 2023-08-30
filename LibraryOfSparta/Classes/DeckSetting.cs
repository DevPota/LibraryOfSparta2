using System;
using System.Collections.Generic;
using LibraryOfSparta.Common;
using LibraryOfSparta.Managers;
using LibraryOfSparta.Classes;
using System.Reflection;
using LibraryOfSparta;
using System.Text;
using System.Linq;

public class DeckSetting : Scene
{
    List<Card> myCard = new List<Card>();
    List<Card> myDeck = new List<Card>();

    int page = 1;
    int cursor = 0;

    bool cursor_is_mycard = true;

    public void Init()
    {
        InitCard();
        MainPanel();
        InventoryPanel();

        DescriptionPanel();
        MyCardDraw();
        MyDeckDraw();
        CursorDraw();
    }

    public void Update()
    {
        Input();
    }

    void MainPanel()
    {

        Console.SetCursorPosition(1, 1);
        Console.ForegroundColor = ConsoleColor.Yellow;
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
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        {
            Console.SetCursorPosition(10, 3);
            Console.WriteLine(" __  __ ___ ___    _____  _____  _____  _____                __  __ ___ ___    _____  _____  _____  __ ___");
            Console.SetCursorPosition(10, 4);
            Console.WriteLine("/  \\/  \\\\  |  /   /     \\/  _  \\/  _  \\|  _  \\              /  \\/  \\\\  |  /   |  _  \\/   __\\/     \\|  |  /");
            Console.SetCursorPosition(10, 5);
            Console.WriteLine("|  \\/  | |   |    |  |--||  _  ||  _  <|  |  |              |  \\/  | |   |    |  |  ||   __||  |--||  _ < ");
            Console.SetCursorPosition(10, 6);
            Console.WriteLine("\\__/\\__/ \\___/    \\_____/\\__|__/\\__|\\_/|_____/              \\__/\\__/ \\___/    |_____/\\_____/\\_____/|__|__\\");
        }// MY CARD, MY DECK
        Console.ForegroundColor = ConsoleColor.Yellow;
        {
            Console.SetCursorPosition(10, Define.SCREEN_Y - 20);
            Console.WriteLine(@" _____  _____  _____  _____     ___  _____  _____  _____ ");
            Console.SetCursorPosition(10, Define.SCREEN_Y - 19);
            Console.WriteLine(@"/     \/  _  \/  _  \|  _  \   /   \/  _  \/   __\/  _  \");
            Console.SetCursorPosition(10, Define.SCREEN_Y - 18);
            Console.WriteLine(@"|  |--||  _  ||  _  <|  |  |   |   ||  |  ||   __||  |  |");
            Console.SetCursorPosition(10, Define.SCREEN_Y - 17);
            Console.WriteLine(@"\_____/\__|__/\__|\_/|_____/   \___/\__|__/\__/   \_____/");
        }//CARD INFO
        Console.ResetColor();
    }

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
            Console.SetCursorPosition(9, Define.SCREEN_Y - 22);
            Console.Write("[                       {0}/{1}                        ]", page, ((myCard.Count - 1) + 10) / 10);
        }//페이지 표시
    }// 메인 창 그리기

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
        try
        {
            if (cursor_is_mycard)
            {
                Console.SetCursorPosition(10, Define.SCREEN_Y - 15);
                Console.WriteLine("카드  이름 : {0}", myCard[(page - 1) * 10 + cursor].name);
                Console.SetCursorPosition(10, Define.SCREEN_Y - 13);
                Console.Write("카드  파워 : ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("({0})", myCard[(page - 1) * 10 + cursor].power);
                Console.ResetColor();

                Console.SetCursorPosition(10, Define.SCREEN_Y - 12);
                Console.WriteLine("카드  타입 : {0}", myCard[(page - 1) * 10 + cursor].type);
                Console.SetCursorPosition(10, Define.SCREEN_Y - 11);
                Console.Write("카드 코스트: "); Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("({0})", myCard[(page - 1) * 10 + cursor].cost); Console.ResetColor();

                Console.SetCursorPosition(10, Define.SCREEN_Y - 10);
                Console.WriteLine("카드  설명 : {0}", myCard[(page - 1) * 10 + cursor].info);
            }
            else
            {
                Console.SetCursorPosition(10, Define.SCREEN_Y - 15);
                Console.WriteLine("카드  이름 : {0}", myDeck[cursor].name);
                Console.SetCursorPosition(10, Define.SCREEN_Y - 13);
                Console.Write("카드  파워 : "); Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("({0})", myDeck[cursor].power); Console.ResetColor();
                Console.SetCursorPosition(10, Define.SCREEN_Y - 12);
                Console.WriteLine("카드  타입 : {0}", myDeck[cursor].type);
                Console.SetCursorPosition(10, Define.SCREEN_Y - 11);
                Console.Write("카드 코스트: "); Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("({0})", myDeck[cursor].cost); Console.ResetColor();

                Console.SetCursorPosition(10, Define.SCREEN_Y - 10);
                Console.WriteLine("카드  설명 : {0}", myDeck[cursor].info);
            }
        }
        catch
        {

        }// 카드 정보
    }// 카드 정보 창 그리기

    void MyCardDraw()
    {
        for (int i = 0; i < 10; i++)
        {
            int index = (page - 1) * 10 + i;
            if (index < myCard.Count)
            {
                Console.SetCursorPosition(13, 8 + i * 2);
                Console.Write("{0}", myCard[index].name);
            }
        }
    }// 내 카드 그리기

    void MyDeckDraw()
    {
        for (int i = 0; i<myDeck.Count; i++)
        {
            Console.SetCursorPosition(73, 8 + i* 2);
            Console.Write("{0}", myDeck[i].name);
        }
    }// 내 덱 그리기

    void CursorDraw()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (cursor_is_mycard)
        {
            Console.SetCursorPosition(10, (cursor * 2) + 8);
            Console.WriteLine("▶");
            Console.SetCursorPosition(58, (cursor * 2) + 8);
            Console.WriteLine("◀");
        }
        else
        {
            Console.SetCursorPosition(70, (cursor * 2) + 8);
            Console.WriteLine("▶");
            Console.SetCursorPosition(118, (cursor * 2) + 8);
            Console.WriteLine("◀");
        }
        Console.ResetColor();
    }// 커서 그리기

    void CardRemove()
    {
        InventoryPanel();
        try
        {
            if (cursor_is_mycard)
            {
                Card card = new Card();
                card = myCard[(page - 1) * 10 + cursor];
                
                if (myCard.Count < (page * 10))
                {
                    Console.SetCursorPosition(13, 8 + (myCard.Count % 10) * 2);
                    Console.WriteLine("                                               ");
                }
                InventoryPanel();
                myCard.Remove(card);
                Core.SaveData.Inventory.RemoveAt(card.index); // 카드 지우기 (완)
                Core.Save();
            }
            else
            {
                myCard.Add(myDeck[cursor]);
                Core.SaveData.Inventory.Add(Core.SaveData.Deck[cursor]);   // 카드 내 인벤에 추가 (완)

                myDeck.Remove(myDeck[cursor]);
                Core.SaveData.Deck.Remove(Core.SaveData.Deck[cursor]);   // 카드 내 덱이서 삭제 (완)

                Core.Save();

                Console.SetCursorPosition(73, 8 + myDeck.Count * 2);
                Console.WriteLine("                                               ");
            }
            Core.PlaySFX(Define.SFX_PATH + "/Card_Cancel.wav");
        }
        catch
        {
            Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
        }
        Console.SetCursorPosition(9, Define.SCREEN_Y - 22);
        Console.Write("[                       {0}/{1}                        ]", page, ((myCard.Count - 1) + 10) / 10);
    }// 카드 삭제

    void MyDeckAdd()
    {
        if ((page - 1) * 10 + cursor < myCard.Count)
        {
            if (myDeck.Count < 10 && cursor_is_mycard)
            {
                Core.PlaySFX(Define.SFX_PATH + "/Card_Apply.wav");

                myDeck.Add(myCard[(page - 1) * 10 + cursor]);              
                Core.SaveData.Deck.Add(Core.SaveData.Inventory[(page - 1) * 10 + cursor]);          //내 덱에 추가

                myCard.Remove(myCard[(page - 1) * 10 + cursor]);
                Core.SaveData.Inventory.Remove(Core.SaveData.Inventory[(page - 1) * 10 + cursor]); // 카드 지우기 

                Core.Save();
                
                if (myCard.Count < (page * 10))
                {
                    Console.SetCursorPosition(13, 8 + (myCard.Count % 10) * 2);
                    Console.WriteLine("                                               ");
                }
                InventoryPanel();
            }
            else
            {
                Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
            }
        }
        else
        {
            Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
        }
        Console.SetCursorPosition(9, Define.SCREEN_Y - 22);
        Console.Write("[                       {0}/{1}                        ]", page, ((myCard.Count - 1) + 10) / 10);
    }// 카드 이동
      
    void Input()
    {
        Console.SetCursorPosition(2, Define.SCREEN_Y - 4);
        {
            Console.Write("[A]◀     [W]▲     [S]▼     [D]▶      [TAB]인벤토리/덱 선택      [ENTER]선택      [BACKSPACE]카드 삭제      [ESC] 뒤로가기");
        }// 키 설명
        InputCheck();
    }// 입력 

    void InputCheck()
    {
        ConsoleKeyInfo key = Core.GetKey();
        
        switch(key.Key)
        {
            case ConsoleKey.A :
            case ConsoleKey.LeftArrow :
                Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                if (page > 1)
                {
                    page -= 1;
                    InventoryPanel();
                }
                break;
            case ConsoleKey.D :
            case ConsoleKey.RightArrow :
                Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                if ((page * 10) < myCard.Count)
                {
                    page += 1;
                    InventoryPanel();
                }
                break;
            case ConsoleKey.W :
            case ConsoleKey.UpArrow :
                Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                Console.SetCursorPosition(10, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(70, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(58, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(118, (cursor * 2) + 8);
                Console.WriteLine("  ");
                if (cursor > 0)
                {

                    cursor -= 1;
                }
                else
                {
                    cursor = 9;
                }
                break;
            case ConsoleKey.S :
            case ConsoleKey.DownArrow :
                Core.PlaySFX(Define.SFX_PATH + "/Card_Over.wav");
                Console.SetCursorPosition(10, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(70, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(58, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(118, (cursor * 2) + 8);
                Console.WriteLine("  ");
                if (cursor < 9)
                {
                    cursor += 1;
                }
                else
                {
                    cursor = 0;
                }
                break;
            case ConsoleKey.Tab :
                Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");

                if (cursor_is_mycard == true)
                {

                    cursor_is_mycard = false;
                }
                else
                {
                    cursor_is_mycard = true;
                }
                Console.SetCursorPosition(10, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(70, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(58, (cursor * 2) + 8);
                Console.WriteLine("  ");
                Console.SetCursorPosition(118, (cursor * 2) + 8);
                Console.WriteLine("  ");
                break;
            case ConsoleKey.Enter :
            case ConsoleKey.Spacebar :
                if (cursor_is_mycard)
                {
                    MyDeckAdd();
                }
                else
                {
                    CardRemove();
                }
                break;
            case ConsoleKey.Backspace :
                if(Core.SaveData.Inventory.Count + Core.SaveData.Deck.Count > 10)
                {
                    CardRemove();
                }
                else
                {
                    Core.PlaySFX(Define.SFX_PATH + "/Card_Lock.wav");
                }
                break;
            case ConsoleKey.Escape :
                Core.PlaySFX(Define.SFX_PATH + "/BookSound.wav");
                Core.LoadScene(7);
                return;
        }

        if (key == default)
        {
            return;
        }
        else
        {
            DescriptionPanel();
            MyCardDraw();
            MyDeckDraw();
            CursorDraw();
        }

    }

    void InitCard()
    {
        string   cardData = Core.GetData(Define.CARD_DATA_PATH);
        string[] lines    = cardData.Split('\n');//카드가 잘려서 들어감

        for(int i = 0; i < Core.SaveData.Inventory.Count; i++)
        {
            Card newCard = new Card(i, lines[Core.SaveData.Inventory[i]].Split(','));
            myCard.Add(newCard);
        }

        for (int i = 0; i < Core.SaveData.Deck.Count; i++)
        {
            Card newCard = new Card(i, lines[Core.SaveData.Deck[i]].Split(','));
            myDeck.Add(newCard);
        }
    }
}

struct Card
{
    public int index;
    public string name;
    public CardType type;
    public int power;
    public int cost;
    public string info;

    public Card( int Index,string[] data)
    {
        index = Index;
        name   = data[0];
        type   = (CardType)int.Parse(data[1]);
        power  = int.Parse(data[2]);
        cost   = int.Parse(data[3]);
        info = data[6];
    }
}