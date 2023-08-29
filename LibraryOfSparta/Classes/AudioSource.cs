using System.Text;
using System.Runtime.InteropServices;
using System.Timers;

namespace LibraryOfSparta.Classes
{
    class AudioSource
    {

        [DllImport("winmm.dll")]
        private static extern int mciSendString(String strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        [DllImport("winmm.dll")]
        public static extern int mciGetErrorString(int errCode, StringBuilder errMsg, int buflen);
        [DllImport("winmm.dll")]
        public static extern int mciGetDeviceID(string lpszDevice);

        public AudioSource()
        {

        }

        public AudioSource(string filename, string alias)
        {
            _medialocation = filename;
            _alias = alias;
            LoadMediaFile(_medialocation, _alias);
        }

        int _deviceid = 0;
        bool isLooping = false;
        double currentPlayed = 0.0f;
        System.Timers.Timer timer = new System.Timers.Timer();

        public int Deviceid
        {
            get { return _deviceid; }
        }

        private bool _isloaded = false;

        public bool Isloaded
        {
            get { return _isloaded; }
            set { _isloaded = value; }
        }

        private string _medialocation = "";

        public string MediaLocation
        {
            get { return _medialocation; }
            set { _medialocation = value; }
        }
        private string _alias = "";

        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        private int _length = 0;
        public int Length
        {
            get { return _length;  }
            set { _length = value; }
        }

        public bool LoadMediaFile(string filename, string alias)
        {
            _medialocation = filename;
            _alias = alias;
            StopPlaying();
            CloseMediaFile();
            string Pcommand = "open \"" + filename + "\" alias " + alias;
            int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            _isloaded = (ret == 0) ? true : false;
            if (_isloaded)
            {
                _deviceid = mciGetDeviceID(_alias);
                _length   = GetLength();
            }
            return _isloaded;
        }

        public void PlayFromStart()
        {
            if (_isloaded)
            {
                string Pcommand = "play " + Alias + " from 0";
                int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        public void PlayFromStart(IntPtr callback)
        {
            if (_isloaded)
            {
                string Pcommand = "play " + Alias + " from 0 notify";
                int ret = mciSendString(Pcommand, null, 0, callback);
            }
        }

        public void PlayLoop()
        {
            isLooping = true;
            PlayFromStart();
            timer = new System.Timers.Timer(Length);

            timer.Elapsed += new ElapsedEventHandler(Loop);
            timer.AutoReset = true;
            timer.Start();
        }

        public void Loop(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (isLooping == false)
            {
                return;
            }
            LoadMediaFile(_medialocation, _alias);
            PlayFromStart();
            timer.Interval = Length;
        }

        public void Pause()
        {
            mciSendString("pause " + Alias, null, 0, IntPtr.Zero);
            timer.Stop();
            currentPlayed = Length - GetCurentMilisecond();
        }

        public void Resume()
        {
            mciSendString("resume " + Alias, null, 0, IntPtr.Zero);

            timer.Interval = Length - GetCurentMilisecond();
            timer.Start();
        }

        public void CloseMediaFile()
        {
            string Pcommand = "close " + Alias;
            int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            _isloaded = false;

        }

        public void StopPlaying()
        {
            string Pcommand = "stop " + Alias;
            int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
        }

        public void ForceStopAll()
        {
            string Pcommand = "stop " + Alias;
            int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            isLooping = false;
            timer.Elapsed -= Loop;
            timer.Stop();
            CloseMediaFile();
        }

        public void StopTimer()
        {
            isLooping = false;
            timer.Elapsed -= Loop;
            timer.Stop();
        }

        public int GetLength()
        {
            StringBuilder lengthBuf = new StringBuilder(256);
            string PCommand = "status " + Alias + " length";
            mciSendString(PCommand, lengthBuf, lengthBuf.Capacity, IntPtr.Zero);

            int length = 0;
            int.TryParse(lengthBuf.ToString(), out length);

            return length;
        }

        public int GetCurentMilisecond()
        {
            StringBuilder returnData = new StringBuilder(256);
            string Pcommand = "status "+ Alias + " position";
            int error = mciSendString(Pcommand, returnData,
                                  returnData.Capacity, IntPtr.Zero);
            return int.Parse(returnData.ToString());
        }
    }
}