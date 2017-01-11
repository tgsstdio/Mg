
using System;

namespace Magnesium.OpenGL
{
	public class GLCmdInternalDrawIndexedIndirect
	{
        public IntPtr Indirect { get; set; }
        public MgPrimitiveTopology Topology { get; set; }
        public MgIndexType IndexType { get; set; }

		public uint Stride {
			get;
			set;
		}

		public uint DrawCount {
			get;
			set;
		}
	}

}

