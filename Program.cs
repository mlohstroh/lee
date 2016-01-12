using System;
using MsgPack.Serialization;
using System.IO;

namespace LEE
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");

            Player p = new Player () {
                Name = "Baller"
            };

            EntityPersistence m = new EntityPersistence ("Database");
            Guid id = m.WriteEntity (p);

            Player readFromDisk = m.ReadEntity<Player> (id);

            Player noneExistent = m.ReadEntity<Player>(Guid.NewGuid());

            Console.Read();
        }
    }
}
