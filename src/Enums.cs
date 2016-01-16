using System;
using System.Collections.Generic;

namespace LEE
{
    /// <summary>
    /// This dictates how MsgPack serializes to the disk. This should really only be changed
    /// for performance reasons.
    /// </summary>
    public enum VersionPolicy
    {
        /// <summary>
        /// Slowest but safest method. This allows classes to have properties added and removed
        /// without any data being completely lost. Great for development that is ongoing.
        /// </summary>
        Tolerant = 0,
        /// <summary>
        /// Faster method of loading data. You can never remove properties from classes now.
        /// This is great if you have a set data structure that you know will not mutate,
        /// or if you set the ordering correctly on all the attributes.
        /// </summary>
        InTolerant,
    }
}
