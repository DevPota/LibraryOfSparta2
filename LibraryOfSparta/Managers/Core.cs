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
    class SceneModule
    {
        public Scene[]                    BuiltInScenes   { get; set; } = null;
        public Scene                      CurrentScene    { get; set; } = null;
        public Dictionary<string, Action> EventBase                     = null;
        public string                     DialogActorName { get; set; }
        public string                     Dialog          { get; set; }

        public void Init()
        {
            EventBase = new Dictionary<string, Action>();
        }
    }

    class SoundModule
    {
        public string BGMAlias { get; set; }
        AudioSource        BGM      = null;
        Queue<AudioSource> sfxQueue = null;

        string currentBGMPath = "";
        bool isPaused = false;

        public void Init(bool sfxInit = true)
        {
            BGM      = new AudioSource();

            if(sfxInit == true)
            {
                sfxQueue = new Queue<AudioSource>();

                for (int i = 0; i < 10; i++)
                {
                    sfxQueue.Enqueue(new AudioSource());
                }
            }
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
                BGM.StopTimer();
                BGM.LoadMediaFile(currentBGMPath, BGMAlias);
                BGM.PlayLoop();

                if (isPaused == true)
                {
                    BGM.Pause();
                }
            }
        }

        public void PlaySFX(string path)
        {
            AudioSource temp = sfxQueue.Dequeue();
            temp.LoadMediaFile(path, "sfx_" + sfxQueue.Count);
            temp.PlayFromStart();
            sfxQueue.Enqueue(temp);
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
        static DataModule     Data     { get; set; } = new DataModule();
        static SceneModule    Scene    { get; set; } = new SceneModule();
        static SoundModule[]  Sound    { get; set; } = new SoundModule[2];
        static InputModule    Input    { get; set; } = new InputModule();

        public static void Init()
        {
            Data.Init();
            Scene.Init();
            Input.Init();
            Sound[0] = new SoundModule();
            Sound[1] = new SoundModule();
            Sound[0].Init();
            Sound[1].Init(false);
            Sound[0].BGMAlias = "PlayerBGM";
            Sound[1].BGMAlias = "EnemyBGM";
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
    }
}
