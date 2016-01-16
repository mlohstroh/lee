using System;
using System.IO;
using System.Collections.Generic;
using LEE.Logging;

namespace LEE
{
    public class EntityTable
    {
        public string Name { get { return _dir.Name; } }
        public DirectoryInfo Directory { get { return _dir; } }
        public EntityPersistence Persistence { get; set; }
        private DirectoryInfo _dir;
        private Dictionary<Guid, FileInfo> _table;
        private Logger _log;

        public EntityTable (DirectoryInfo dir)
        {
            _dir = dir;
            _log = LogManager.CreateLogger (Name);

            BuildTable ();
        }

        public long EntityCount
        {
            // because this is all in memory now
            get { return _table.Count; }
        }

        private void BuildTable()
        {
            _table = new Dictionary<Guid, FileInfo> ();

            // The file name is the actual guid
            // Contents are the serialized data

            FileInfo[] files = _dir.GetFiles ();
            for (int i = 0; i < files.Length; i++)
            {
                Guid parsed = Guid.Empty;
                if (Guid.TryParse (files [i].Name, out parsed))
                {
                    _table [parsed] = files [i];
                } 
                else
                {
                    _log.LogWarningFormat ("Unable to load file with name {0}. It is not a formatted guid", files[i].Name);   
                }
            }
        }
    }
}

