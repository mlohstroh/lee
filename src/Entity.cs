using System;

namespace LEE
{
    public class Entity
    {
        public Guid Id { get; set; }

        public Entity ()
        {
            if(Id == Guid.Empty)
                Id = Guid.NewGuid ();
        }
    }
}

