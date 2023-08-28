using LibraryOfSparta.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraryOfSparta.Managers
{
    abstract class ModuleClass
    {
        public abstract void Init();
    }

    class SceneModule : ModuleClass
    {
        public Scene[]                    BuiltInScenes   { get; set; } = null;
        public Scene                      CurrentScene    { get; set; } = null;
        public Dictionary<string, Action> EventBase                     = null;
        public string                     DialogActorName { get; set; }
        public string                     Dialog          { get; set; }

        public override void Init()
        {
            EventBase = new Dictionary<string, Action>();
        }
    }

    class SoundModule : ModuleClass
    {
        AudioSource        BGM      = null;
        Queue<AudioSource> sfxQueue = null;

        public override void Init()
        {
            BGM      = new AudioSource();
            sfxQueue = new Queue<AudioSource>();

            for (int i = 0; i < 10; i++)
            {
                sfxQueue.Enqueue(new AudioSource());
            }
        }

        public void PlayBGM(string path)
        {
            //BGM.Play();
        }

        public void PlaySFX(string path)
        {
            //AudioSource temp = sfxQueue.Dequeue();
            //temp.SetClip(path);
            //temp.Play();
            //sfxQueue.Enqueue(temp);
        }
    }

    class InputModule : ModuleClass
    {
        public override void Init()
        {

        }
    }

    class DataModule : ModuleClass
    {
        public override void Init() { }

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

    class ResourceModule : ModuleClass
    {
        Dictionary<string, string> ResourceBase = null;

        public override void Init()
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
        static SoundModule    Sound    { get; set; } = new SoundModule();
        static InputModule    Input    { get; set; } = new InputModule();

        public static void Init()
        {
            Data.Init();
            Resource.Init();
            Scene.Init();
            Sound.Init();
            Input.Init();
        }
    }
}
