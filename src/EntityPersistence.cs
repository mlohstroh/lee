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
        private Dictionary<string, EntityTable> _subDirs;

        public EntityPersistence (string storeDir)
        {
            _persistenceStoreDir = storeDir;

            EnsureStoreDir ();

            // Probably an overall deficit to performance, but one that will really help with version tolaerance
            SerializationContext.Default.SerializationMethod = SerializationMethod.Map;
        }

        private void EnsureStoreDir()
        {
            _rootDirectoryInfo = Directory.CreateDirectory (_persistenceStoreDir);

            DirectoryInfo[] dirs = _rootDirectoryInfo.GetDirectories ();

            _subDirs = new Dictionary<string, EntityTable> ();
            for (int i = 0; i < dirs.Length; i++)
            {
                _subDirs [dirs [i].Name] = new EntityTable (dirs [i]);
            }
        }

        public T RetrieveEntity<T>(Guid id) where T : Entity
        {
            EntityTable table = EnsureDirectoryForType (typeof(T));

            return InternalRead<T>(table.Directory, id);
        }

        public Guid WriteEntity<T>(T e) where T : Entity
        {
            // Ensure the GUID here, we can be sure that they will call base
            if (e.Id == Guid.Empty)
                e.Id = Guid.NewGuid ();

            EntityTable table = EnsureDirectoryForType (e.GetType());
            using (StreamWriter writer = new StreamWriter (PathForDirectoryAndID (table.Directory, e.Id)))
            {
                var serializer = MessagePackSerializer.Get<T> ();
                serializer.Pack (writer.BaseStream, e);
            }

            return e.Id;
        }

        private EntityTable EnsureDirectoryForType(Type t)
        {
            EntityTable table;
            if (!_subDirs.TryGetValue (t.Name, out table))
            {
                var di = _rootDirectoryInfo.CreateSubdirectory (t.Name);
                table = new EntityTable (di);
                _subDirs [t.Name] = table;
            }

            return table;
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

