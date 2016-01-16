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

        public ProfileTag ProfileBlock(string blockName)
        {
            return new ProfileTag(blockName);
        }
    }

    public class ProfileTag : IDisposable
    {
        private Logger _log = LogManager.CreateLogger("LEE::Profiler");
        private Stopwatch _watch;
        public string Tag { get; private set; }

        public ProfileTag(string tag)
        {
            _watch = new Stopwatch();
            _watch.Start();
            Tag = tag;
        }

        public void Dispose()
        {
            if (_watch != null)
            {
                _watch.Stop();

                if (_log.IsInfoEnabled)
                    _log.LogInfoFormat("{0}: {1}", Tag, _watch.Elapsed.ToString("G"));
            }
        }
    }
}
