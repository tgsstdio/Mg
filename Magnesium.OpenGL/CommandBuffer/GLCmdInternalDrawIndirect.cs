using System;

namespace Magnesium.OpenGL
{
	public class GLCmdInternalDrawIndirect
	{
		public uint Stride {
			get;
			set;
		}

		public uint DrawCount {
			get;
			set;
		}

        public MgPrimitiveTopology Topology { get; internal set; }
        public IntPtr Indirect { get; set; }
    }

}

