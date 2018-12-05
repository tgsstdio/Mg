using System;

namespace Magnesium
{
    [Flags] 
	public enum MgStencilFaceFlagBits : byte
	{
        /// <summary> 
        /// Front face
        /// </summary> 
        FRONT_BIT = 0x1,
        /// <summary> 
        /// Back face
        /// </summary> 
        BACK_BIT = 0x2,
        /// <summary> 
        /// Front and back faces
        /// </summary>
        FRONT_AND_BACK = 0x3,
	}
}

