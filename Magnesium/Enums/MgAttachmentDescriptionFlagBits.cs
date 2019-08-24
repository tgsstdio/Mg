using System;

namespace Magnesium
{
    [Flags]
    public enum MgAttachmentDescriptionFlagBits : UInt32
    {
        /// <summary> 
        /// The attachment may alias physical memory of another attachment in the same render pass
        /// </summary> 
        MAY_ALIAS_BIT = 1 << 0,
    }
}

