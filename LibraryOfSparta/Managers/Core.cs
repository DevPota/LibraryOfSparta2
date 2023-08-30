using LibraryOfSparta.Classes;
using LibraryOfSparta.Common;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraryOfSparta.Managers
{
    class SoundModule
    {
        public string BGMAlias { get; set; }
        AudioSource        BGM      = null;

        string currentBGMPath = "";
        bool isPaused = false;

        int idx = 0;

        public void Init()
        {
            BGM      = new AudioSource();
        }

        public void PlayBGM(string path)
        {
            if(currentBGMPath == path)
            {
                return;
            }
            else
            {
                currentBGMPath = path;
                BGM.ForceStopAll();

                BGM = new AudioSource();
                BGM.LoadMediaFile(path, BGMAlias + idx++);
                BGM.PlayLoop();
            }
        }

        public void PlaySFX(string path)
        {
            AudioSource temp = new AudioSource();
            temp.LoadMediaFile(path, "SFX_" + idx++);
            temp.PlayFromStart();
        }

        public void PauseBGM()
        {
            if(isPaused == true)
            {
                return;
            }
            else
            {
                BGM.Pause();
                isPaused = true;
            }
        }

        public void ResumeBGM()
        {
            if(isPaused == true)
            {
                BGM.Resume();
                isPaused = false;
            }
            else
            {
                return;
            }
        }
    }

    class InputModule
    {
        public ConsoleKeyInfo Ipt { get; set; }

        public async void Init()
        {
            Task iptTask = Task.Run(async () =>
            {
                while (true)
                {
                    Ipt = Console.ReadKey(true);
                }
            });
        }
    }

    class DataModule
    {
        public void Init() { }

        public void SaveData<T>(string path, T jsonClass) where T : class
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(jsonClass);
            File.WriteAllText(path, Encoding.UTF8.GetString(bytes));
        }

        public string LoadData(string path)
        {
            if (File.Exists(path) == true)
            {
                return File.ReadAllText(path);
            }
            else
            {
                return null;
            }
        }

        public T LoadData<T>(string path) where T : new()
        {
            if (File.Exists(path) == true)
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
            }
            else
            {
                return new T();
            }
        }
    }

    public static class Core
    {
        public static GameData SaveData { get; private set;}  = null;

        public static int SceneIndex     { get; private set; } = 0;
        public static Scene CurrentScene { get; private set; } = null;

        static DataModule     Data      { get; set; }         = new DataModule();
        static SoundModule[]  Sound     { get; set; }         = new SoundModule[2];
        static InputModule    Input     { get; set; }         = new InputModule();

        public static void Init()
        {
            Data.Init();
            Input.Init();
            Sound[0] = new SoundModule();
            Sound[1] = new SoundModule();
            Sound[0].Init();
            Sound[1].Init();
            Sound[0].BGMAlias = "PlayerBGM";
            Sound[1].BGMAlias = "EnemyBGM";

            SaveData = Data.LoadData<GameData>(Define.SAVE_PATH);
            Save();
        }

        #region Sound
        public static void PlayPlayerBGM(string path)
        {
            Sound[0].PlayBGM(path);
        }

        public static void PlayEnemyBGM(string path)
        {
            Sound[1].PlayBGM(path);
        }

        public static void PausePlayerBGM()
        {
            Sound[0].PauseBGM();
        }

        public static void PauseEnemyBGM()
        {
            Sound[1].PauseBGM();
        }

        public static void ResumePlayerBGM()
        {
            Sound[0].ResumeBGM();
        }

        public static void ResumeEnemyBGM()
        {
            Sound[1].ResumeBGM();
        }

        public static void PlaySFX(string path)
        {
            Sound[0].PlaySFX(path);
        }
        #endregion

        public static ConsoleKeyInfo GetKey()
        {
            return Input.Ipt;
        }

        public static void ReleaseKey()
        {
            Input.Ipt = default;
        }

        public static void RenderSystemUI()
        {
            int x = 0;
            int y = 0;
            int endX = Define.SCREEN_X;
            int endY = Define.SCREEN_Y;

            char lt = '┌';
            char rt = '┐';
            char side = '│';
            char lb = '└';
            char rb = '┘';
            string line = "───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────";

            Console.SetCursorPosition(x, y);
            Console.Write(lt);
            Console.SetCursorPosition(x+1, y);
            Console.Write(line);
            Console.SetCursorPosition(x + endX - 2, y);
            Console.Write(rt);

            for (int i = 1; i < endY-1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(side);
                Console.SetCursorPosition(x + endX - 2, y + i);
                Console.Write(side);
            }

            Console.SetCursorPosition(x, y + endY-1);
            Console.Write(lb);
            Console.SetCursorPosition(x+1, y + endY-1);
            Console.Write(line);
            Console.SetCursorPosition(x+ endX -2, y + endY-1);
            Console.Write(rb);
        }

        public static string GetData(string path)
        {
            return Data.LoadData(path);
        }

        public static T GetData<T>(string path) where T : new()
        {
            return Data.LoadData<T>(path);
        }

        public static void LoadScene(int sceneIndex)
        {
            Console.Clear();

            SceneIndex = sceneIndex;

            switch (SceneIndex)
            {
                case 0:
                    CurrentScene = new TitleMenu();
                    ((TitleMenu)CurrentScene).Init();
                    break;
                case 1:
                    CurrentScene = new Entrance();
                    ((Entrance)CurrentScene).Init();
                    break;
                case 2:
                    CurrentScene = new DeckSetting();
                    ((DeckSetting)CurrentScene).Init();
                    break;
                case 3:
                    Scene temp = new Battle();
                    ((Battle)temp).Initbattle(((Entrance)CurrentScene).CursorIndex);
                    CurrentScene = temp;
                    break;
                case 4:
                    CurrentScene = new Result();
                    ((Result)CurrentScene).Init();
                    break;
                case 5:
                    break;
                case 6:
                    break;

            }
        }

        public static void Save()
        {
            Data.SaveData(Define.SAVE_PATH, SaveData);
        }

        public static void ReleaseScene()
        {
            SceneIndex = -1;
            CurrentScene = null;
        }
    }
}
