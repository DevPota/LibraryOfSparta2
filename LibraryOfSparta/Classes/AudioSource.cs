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

        public void StartTick()
        {
            timer.Stop();
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Tick);
            timer.Start();
        }

        public void Tick(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if(IsPaused == true)
            {
                return;
            }

            CurrentPlayed += 100;
        }

        int _deviceid = 0;

        public int CurrentPlayed = 0;

        System.Timers.Timer timer = new System.Timers.Timer();
        public bool IsPaused = false;

        private int _length = 0;
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

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

        public bool LoadMediaFile()
        {
            StopPlaying();
            string Pcommand = "open \"" + _medialocation + "\" alias " + Alias;
            int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            _isloaded = (ret == 0) ? true : false;
            if (_isloaded)
            {
                _deviceid = mciGetDeviceID(_alias);
                _length = GetLength();
            }
            return _isloaded;
        }

        public bool LoadMediaFile(string filename, string alias)
        {
            StartTick();
            StopPlaying();
            CloseMediaFile();
            _medialocation = filename;
            _alias = alias;
            string Pcommand = "open \"" + _medialocation + "\" alias " + _alias;
            int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            _isloaded = (ret == 0) ? true : false;

            if (_isloaded == true)
            {
                _deviceid = mciGetDeviceID(_alias);
                _length   = GetLength();
            }

            return _isloaded;
        }

        public void PlayFromStart()
        {
            if (_isloaded == true)
            {
                string Pcommand = "play " + Alias + " from 0";
                int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        public void PlayFromStart(IntPtr callback)
        {
            if (_isloaded == true)
            {
                string Pcommand = "play " + Alias + " from 0 notify";
                int ret = mciSendString(Pcommand, null, 0, callback);
            }
        }

        public void Pause()
        {
            IsPaused = true;
            timer.Stop();
            mciSendString("pause " + Alias, null, 0, IntPtr.Zero);
        }

        public void Resume()
        {
            IsPaused = false;
            StartTick();
            mciSendString("resume " + Alias, null, 0, IntPtr.Zero);
        }

        public void CloseMediaFile()
        {
            if(_isloaded == true)
            {
                timer.Stop();
                string Pcommand = "close " + Alias;
                int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
                IsPaused = false;
                _isloaded = false;
                Alias = null;
                _medialocation = null;
            }
        }

        public void StopPlaying()
        {
            if(_isloaded == true)
            {
                string Pcommand = "stop " + Alias;
                int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        public void ForceStopAll()
        {
            if (_isloaded == true)
            {
                string Pcommand = "stop " + Alias;
                int ret = mciSendString(Pcommand, null, 0, IntPtr.Zero);
                CloseMediaFile();
            }
        }

        public int GetLength()
        {
            if(_isloaded == true)
            {
                StringBuilder lengthBuf = new StringBuilder(256);
                string PCommand = "status " + Alias + " length";
                mciSendString(PCommand, lengthBuf, lengthBuf.Capacity, IntPtr.Zero);

                int length = 0;
                int.TryParse(lengthBuf.ToString(), out length);
                return length;
            }
            else
            {
                return 0;
            }
        }

        public int GetCurrentMilisecond()
        {
            if(_isloaded == true)
            {
                StringBuilder returnData = new StringBuilder(64);
                string Pcommand = "status " + Alias + " position";
                int error = mciSendString(Pcommand, returnData,
                                      returnData.Capacity, IntPtr.Zero);
                return int.Parse(returnData.ToString());
            }
            else
            {
                return 0;
            }
        }
    }
}