using System;

namespace RunLee
{
    public abstract class Sample
    {
        public string Name { get; set; }
        public virtual void Run() { }
    }
}

