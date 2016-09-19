using System;

namespace Magnesium.OpenGL
{
	[Flags]
	public enum GLCommandBufferFlagBits : byte
	{
		IsDisabled = 1 << 0,

		UseIndexBuffer = 1 << 1,
		UseIndirectBuffer = 1 << 2,

		// CmdDraw = ~UseIndexBuffer | ~UseIndirectBuffer,
		// CmdDrawIndexed = UseIndexBuffer | ~UseIndirectBuffer,
		CmdDrawIndexed = UseIndexBuffer,
		//CmdDrawIndirect =  ~UseIndexBuffer | UseIndirectBuffer,
		CmdDrawIndirect = UseIndirectBuffer,
		CmdDrawIndexedIndirect = UseIndexBuffer | UseIndirectBuffer,

		Index16BitMode = 1 << 3,

		AsPointsMode = 1 << 4,
		AsLinesMode = 1 << 5,
		// AsFillMode = ~AsPointsMode | ~AsLinesMode,
		Unused_0 = 1 << 6,
		Unused_1 = 1 << 7,
	};
}

