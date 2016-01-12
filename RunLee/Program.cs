using System;
using LEE;

namespace RunLee
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Player p = new Player () {
                Name = "Baller"
            };

            EntityPersistence m = new EntityPersistence ("Database");
            Guid id = m.WriteEntity (p);

            Player readFromDisk = m.RetrieveEntity<Player> (id);

            Player noneExistent = m.RetrieveEntity<Player>(Guid.NewGuid());

            Console.Read();   
        }
    }
}