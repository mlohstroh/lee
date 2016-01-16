using System;
using System.IO;
using System.Collections.Generic;
using MsgPack.Serialization;
using LEE.Logging;

namespace LEE
{
    public class EntityPersistence
    {
        // Please never change this
        internal const string _versionFileName = "_version";

        private string _persistenceStoreDir;
        private DirectoryInfo _rootDirectoryInfo;
        private Dictionary<string, EntityTable> _subDirs;
        internal VersionPolicy _policy;
        private VersionFile _version;
        private Logger _log = LogManager.CreateLogger("Lee::EntityPersistence");

        public EntityPersistence(string storeDir, VersionPolicy policy = VersionPolicy.Tolerant)
        {
            _persistenceStoreDir = storeDir;
            _policy = policy;

            // enable profiling by default
            Profiling.Profiler.Default.Log.Level = LogLevel.Info;

            using (Profiling.Profiler.Default.ProfileBlock("EntityPersistence::Load"))
            {
                EnsureStoreDir();
                SetSyncContextForVersionPolicy();
                SaveVersionFile();
            }
        }

        private void EnsureStoreDir()
        {
            _log.LogDebug("Ensuring database directory...");
            _rootDirectoryInfo = Directory.CreateDirectory(_persistenceStoreDir);

            DirectoryInfo[] dirs = _rootDirectoryInfo.GetDirectories();
            _subDirs = new Dictionary<string, EntityTable>();

            // We need to nuke the directory
            if (DidVersionsChange())
            {
                // maybe we should do a move here instead of a delete?
                _log.LogWarningFormat("VersionPolicy was changed from {0} to {1}. Wiping directory due to unreadable data.", _version.Policy, _policy);
                // set the policy
                _version.Policy = _policy;

                for (int i = 0; i < dirs.Length; i++)
                {
                    dirs[i].Delete(true);
                }
            }
            else
            {
                _log.LogDebug("Loading entity tables...");
                for (int i = 0; i < dirs.Length; i++)
                {
                    _subDirs[dirs[i].Name] = new EntityTable(dirs[i])
                    {
                        Persistence = this
                    };
                }
            }
        }

        private bool DidVersionsChange()
        {
            string versionPath = Path.Combine(_rootDirectoryInfo.Name, _versionFileName);
            // see if any changes have been made
            if (File.Exists(versionPath))
            {
                FileInfo fileHandle = new FileInfo(versionPath);
                VersionFile version = VersionFile.RetrieveFile(fileHandle);

                _version = version;

                return version.Policy != _policy;
            }
            // else, nothing changed, just create a new file
            _version = new VersionFile()
            {
                Policy = _policy,
                // global versions will probably never change
                VersionID = 1
            };

            return false;
        }

        private void SetSyncContextForVersionPolicy()
        {
            switch(_version.Policy)
            {
                case VersionPolicy.Tolerant:
                    SerializationContext.Default.SerializationMethod = SerializationMethod.Map;
                    break;
                case VersionPolicy.InTolerant:
                    SerializationContext.Default.SerializationMethod = SerializationMethod.Array;
                    break;
            }
        }

        private void SaveVersionFile()
        {
            string versionPath = Path.Combine(_rootDirectoryInfo.Name, _versionFileName);
            VersionFile.SaveFile(new FileInfo(versionPath), _version);
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

