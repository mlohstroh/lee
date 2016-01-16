using System;
using System.Collections.Generic;
using System.Diagnostics;
using LEE.Logging;
 
namespace LEE.Profiling
{
    public class Profiler
    {
        #region Singleton

        private static Profiler instance;

        /// <summary>
        /// Singleton for a global instance of profiler
        /// </summary>
        public static Profiler Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new Profiler();
                }
                return instance;
            }
        }

        #endregion

        public Logger Log = LogManager.CreateLogger("LEE::Profiler");

        public ProfileTag ProfileBlock(string blockName)
        {
            return new ProfileTag(blockName, this);
        }
    }

    public class ProfileTag : IDisposable
    {
        private Stopwatch _watch;
        public string Tag { get; private set; }
        public Profiler Profiler { get; private set; }

        public ProfileTag(string tag, Profiler profiler)
        {
            _watch = new Stopwatch();
            _watch.Start();
            Tag = tag;
            Profiler = profiler;
        }

        public void Dispose()
        {
            if (_watch != null)
            {
                _watch.Stop();

                if (Profiler.Log.IsInfoEnabled)
                    Profiler.Log.LogInfoFormat("{0}: {1}", Tag, _watch.Elapsed.ToString("G"));
            }
        }
    }
}
