using System;
using System.Collections.Generic;
using LEE;

namespace RunLee
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            List<Sample> samples = new List<Sample> ();
            samples.Add (new Sample1 ()
            {
                Name = "Basic Usage"
            });

            samples.Add (new Sample2 ()
            {
                Name = "Nested Class"
            });

            samples.Add(new Sample3()
            {
                Name = "Versioning"
            });


            for (int i = 0; i < samples.Count; i++)
            {
                Console.WriteLine ("================ Starting sample: {0} ================", samples[i].Name);
                samples [i].Run ();
                Console.WriteLine ("================ Ending sample: {0} ================", samples[i].Name);
                Console.WriteLine ();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}