using System;

namespace Magnesium
{
    [Flags] 
	public enum MgAccessFlagBits : UInt32
	{
		// Controls coherency of indirect command reads
		INDIRECT_COMMAND_READ_BIT = 1 << 0,
		// Controls coherency of index reads
		INDEX_READ_BIT = 1 << 1,
		// Controls coherency of vertex attribute reads
		VERTEX_ATTRIBUTE_READ_BIT = 1 << 2,
		// Controls coherency of uniform buffer reads
		UNIFORM_READ_BIT = 1 << 3,
		// Controls coherency of input attachment reads
		INPUT_ATTACHMENT_READ_BIT = 1 << 4,
		// Controls coherency of shader reads
		SHADER_READ_BIT = 1 << 5,
		// Controls coherency of shader writes
		SHADER_WRITE_BIT = 1 << 6,
		// Controls coherency of color attachment reads
		COLOR_ATTACHMENT_READ_BIT = 1 << 7,
		// Controls coherency of color attachment writes
		COLOR_ATTACHMENT_WRITE_BIT = 1 << 8,
		// Controls coherency of depth/stencil attachment reads
		DEPTH_STENCIL_ATTACHMENT_READ_BIT = 1 << 9,
		// Controls coherency of depth/stencil attachment writes
		DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 1 << 10,
		// Controls coherency of transfer reads
		TRANSFER_READ_BIT = 1 << 11,
		// Controls coherency of transfer writes
		TRANSFER_WRITE_BIT = 1 << 12,
		// Controls coherency of host reads
		HOST_READ_BIT = 1 << 13,
		// Controls coherency of host writes
		HOST_WRITE_BIT = 1 << 14,
		// Controls coherency of memory reads
		MEMORY_READ_BIT = 1 << 15,
		// Controls coherency of memory writes
		MEMORY_WRITE_BIT = 1 << 16,
	};

}

