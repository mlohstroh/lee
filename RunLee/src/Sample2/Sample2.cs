using System;
using System.Collections.Generic;
using LEE;

namespace RunLee
{

    public class Weapon :Entity
    {
        public string Name { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment : Entity
    {
        public string Name { get; set; }
    }

    public class Sample2 : Sample
    {
        public override void Run ()
        {
            Weapon w = new Weapon () 
            {
                Name = "Best Weapon",
                Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        Name = "Attach1"
                    },
                    new Attachment()
                    {
                        Name = "Attach2"
                    }
                }
            };

            Console.WriteLine("Loading persistence directory...");
            EntityPersistence p = new EntityPersistence ("Database");

            Console.WriteLine("Writing entity with special subclass");
            var id = p.WriteEntity<Weapon> (w);

            Console.WriteLine("Reading entity with special subclass");
            Weapon read = p.RetrieveEntity<Weapon>(id);
        }
    }
}

