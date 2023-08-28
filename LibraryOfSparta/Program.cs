using System.Text;
using System.Threading.Tasks;
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

        return;
    }

    static void Awake()
    {
        Console.InputEncoding  = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.SetWindowSize(Define.SCREEN_X, Define.SCREEN_Y);

        Core.Init();
    }

    static async void Start()
    {
        while (applicationQuit == false)
        {
            Update();
            
            Thread.Sleep(100);
        }
    }

    static void Update()
    {

    }
}