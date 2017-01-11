using System;

namespace Magnesium.OpenGL
{
	public class GLCmdInternalDraw
	{
        internal MgPrimitiveTopology Topology;

        public uint FirstInstance {
			get;
			set;
		}

		public uint FirstVertex {
			get;
			set;
		}

		public uint InstanceCount {
			get;
			set;
		}

		public uint VertexCount {
			get;
			set;
		}
	}

}

