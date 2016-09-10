using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	/// <summary>
	/// Targeted struct should be around 60 - 64 bits 
	/// for IntPtr switching between 32 and 64 bits
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct GLCmdBufferDrawItem
	{
		// 4 bytes
		public GLCommandBufferFlagBits Command { get; set; }
		public MgPrimitiveTopology Topology { get; set;}
		public byte Pipeline { get; set; }
		public byte DescriptorSet { get; set; }

		// 8 bytes
		public byte Scissor { get; set; }
		public byte Viewport { get; set; }
		public byte BlendConstants { get; set; }
		public byte LineWidth { get; set; }

		// 12 bytes
		public byte DepthBounds { get; set; }
		public byte FrontStencilCompareMask { get; set; }
		public byte FrontStencilWriteMask { get; set; }
		public byte FrontStencilReference { get; set; }

		// 16 bytes
		public byte DepthBias { get; set; }
		public byte BackStencilCompareMask { get; set; }
		public byte BackStencilWriteMask { get; set; }
		public byte BackStencilReference { get; set; }

		// 20 bytes
		public int ProgramID { get; set; }

		// 24 bytes
		public int VBO { get; set; }

		// 28 bytes

		/// <summary>
		/// Used by CmdDrawIndexed
		/// </summary>
		/// <value>The vertex offset.</value>
		public int VertexOffset { get; set; }

		// 32 bytes

		/// <summary>
		/// Passed into CmdDraw as vertexCount
		/// Passed into CmdDrawIndexed as indexCount
		/// Passed into CmdDrawIndirect and CmdDrawIndexedIndirect as drawCount 
		/// </summary>
		/// <value>The vertex count.</value>
		public uint Count { get; set; }

		// 36 bytes

		/// <summary>
		/// Used by CmdDraw, CmdDrawIndexed
		/// </summary>
		/// <value>The instance count.</value>
		public uint InstanceCount { get; set; }

		// 40 bytes

		/// <summary>
		/// Passed into CmdDrawIndexed as firstIndex
		/// Passed into CmdDraw as firstVertex
		/// </summary>
		/// <value>The first index.</value>
		public uint First { get; set; }

		// 44 bytes

		/// <summary>
		/// Used by CmdDraw, CmdDrawIndexed
		/// </summary>
		/// <value>The first instance.</value>
		public uint FirstInstance {get; set; }

		// 48 bytes

		/// <summary>
		/// Used by CmdDrawIndirect, CmdDrawIndexedIndirect
		/// </summary>
		/// <value>The stride.</value>
		public uint Stride { get; set; }

		// 56 bytes

		/// <summary>
		/// Used by CmdDrawIndirect, CmdDrawIndexedIndirect
		/// </summary>
		/// <value>The offset.</value>
		public UInt64 Offset {get;set;}

		// 60 - 64 bytes 

		/// <summary>
		/// Used by CmdDrawIndirect, CmdDrawIndexedIndirect
		/// </summary>
		/// <value>The indirect buffer.</value>
		public IntPtr Buffer { get; set; }
	}
}

