using LibraryOfSparta.Classes;
using System.Collections;
using System.Collections.Generic;
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
            BGM.LoadMediaFile(path, BGMAlias);
            BGM.PlayLoop();
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
            BGM.Pause();
        }

        public void ResumeBGM()
        {
            BGM.Resume();
        }
    }

    class InputModule
    {

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

    class ResourceModule
    {
        Dictionary<string, string> ResourceBase = null;

        public void Init()
        {
            ResourceBase = new Dictionary<string, string>();
        }

        public void LoadResource(string name, string element)
        {
            ResourceBase.Add(name, element);
        }

        public string GetResource(string name)
        {
            return ResourceBase.GetValueOrDefault(name);
        }
    }

    public static class Core
    {
        static DataModule     Data     { get; set; } = new DataModule();
        static ResourceModule Resource { get; set; } = new ResourceModule();
        static SceneModule    Scene    { get; set; } = new SceneModule();
        static SoundModule[]  Sound    { get; set; } = new SoundModule[2];
        static InputModule    Input    { get; set; } = new InputModule();

        public static void Init()
        {
            Data.Init();
            Resource.Init();
            Scene.Init();
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
    }
}
