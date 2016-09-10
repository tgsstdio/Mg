using System;

namespace Magnesium
{
    [Flags] 
	public enum MgAttachmentDescriptionFlagBits : byte
	{
		// The attachment may alias physical memory of another attachment in the same render pass
		ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT = 1 << 0,
	}
}

