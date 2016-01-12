using System;
using System.IO;
using System.Collections.Generic;
using MsgPack.Serialization;

namespace LEE
{
    public class EntityPersistence
    {
        private string _persistenceStoreDir;
        private DirectoryInfo _rootDirectoryInfo;
        private Dictionary<string, DirectoryInfo> _subDirs;

        public EntityPersistence (string storeDir)
        {
            _persistenceStoreDir = storeDir;

            EnsureStoreDir ();
        }

        private void EnsureStoreDir()
        {
            _rootDirectoryInfo = Directory.CreateDirectory (_persistenceStoreDir);

            DirectoryInfo[] dirs = _rootDirectoryInfo.GetDirectories ();

            _subDirs = new Dictionary<string, DirectoryInfo> ();
            for (int i = 0; i < dirs.Length; i++)
            {
                _subDirs [dirs [i].Name] = dirs [i];
            }
        }

        public T RetrieveEntity<T>(Guid id)
        {
            DirectoryInfo di = EnsureDirectoryForType (typeof(T));

            return InternalRead<T>(di, id);
        }

        public Guid WriteEntity<T>(T e) where T : Entity
        {
            // Ensure the GUID here, we can be sure that they will call base
            if (e.Id == Guid.Empty)
                e.Id = Guid.NewGuid ();

            DirectoryInfo di = EnsureDirectoryForType (e.GetType());
            using (StreamWriter writer = new StreamWriter (PathForDirectoryAndID (di, e.Id)))
            {
                var serializer = MessagePackSerializer.Get<T> ();
                serializer.Pack (writer.BaseStream, e);
            }

            return e.Id;
        }

        private DirectoryInfo EnsureDirectoryForType(Type t)
        {
            DirectoryInfo di;
            if (!_subDirs.TryGetValue (t.Name, out di))
            {
                di = _rootDirectoryInfo.CreateSubdirectory (t.Name);
                _subDirs [t.Name] = di;
            }

            return di;
        }

        private T InternalRead<T>(DirectoryInfo di, Guid id)
        {
            var serializer = MessagePackSerializer.Get<T> ();

            string fullPath = PathForDirectoryAndID (di, id);

            if (File.Exists (fullPath))
            {
                using (StreamReader reader = new StreamReader (fullPath))
                {
                    return serializer.Unpack (reader.BaseStream);
                }
            } 
            else
                return default(T);
        }

        private string PathForDirectoryAndID(DirectoryInfo di, Guid id)
        {
            return Path.Combine (di.FullName, id.ToString()); 
        }
    }
}

