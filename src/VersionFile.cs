using System;
using System.IO;
using MsgPack.Serialization;

namespace LEE
{
    public sealed class VersionFile : Entity
    {
        [MessagePackMember(1)]
        public VersionPolicy Policy { get; set; }
        [MessagePackMember(2)]
        public long VersionID { get; set; }

        internal static VersionFile RetrieveFile(FileInfo file)
        {
            var serializer = MessagePackSerializer.Get<VersionFile>();

            using (StreamReader reader = new StreamReader(file.FullName))
            {
                return serializer.Unpack(reader.BaseStream);
            }
        }

        internal static void SaveFile(FileInfo file, VersionFile version)
        {
            var serializer = MessagePackSerializer.Get<VersionFile>();

            using (StreamWriter writer = new StreamWriter(file.FullName))
            {
                serializer.Pack(writer.BaseStream, version);
            }
        }
    }
}