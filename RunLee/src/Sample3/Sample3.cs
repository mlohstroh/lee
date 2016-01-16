using System;
using LEE;

namespace RunLee
{
    public class ChangedClass : LEE.Entity
    {
        /*
            To run this, please run once and then switch or add properties to this class 
            and then add them to the run method below in the sample. You will see that 
            LEE will not throw exceptions and some properties will never get filled
        */
        public string Name1 { get; set; }
        // public string Name2 { get; set; }
    }


    public class Sample3 : Sample
    {
        private const string _fixedGuid = "e97b445f-b280-4bb7-a1dc-894533a74c35";

        public override void Run()
        {
            base.Run();

            Console.WriteLine("Loading persistence directory...");
            EntityPersistence p = new EntityPersistence("Database");

            Console.WriteLine("Writing versioned class...");
            var x = p.WriteEntity<ChangedClass>(new ChangedClass()
            {
                Id = new Guid(_fixedGuid),
                Name1 = "other version!"
            });

            Console.WriteLine("Reading versioned class...");
            var ent = p.RetrieveEntity<ChangedClass>(new Guid(_fixedGuid));
        }
    }
}
