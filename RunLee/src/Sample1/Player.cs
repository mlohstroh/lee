using System;
using LEE;

namespace RunLee
{
    public class Player : Entity
    {
        public string Name { get; set; }

        public Player ()
            : base()
        {
            
        }
    }

    public class Sample1 : Sample
    {
        public override void Run ()
        {
            Player p = new Player () {
                Name = "MsgPackPlayer"
            };
                    
            Console.WriteLine ("Creating persistence model");
            EntityPersistence m = new EntityPersistence ("Database");
            Console.WriteLine ("Writing model");
            Guid id = m.WriteEntity (p);

            Console.WriteLine ("Reading model");
            m.RetrieveEntity<Player> (id);

            Console.WriteLine ("Reading nonexistent smodel");
            m.RetrieveEntity<Player>(Guid.NewGuid());
        }
    }
}

