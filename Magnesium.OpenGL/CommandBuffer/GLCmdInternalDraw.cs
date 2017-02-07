namespace Magnesium.OpenGL.Internals
{
    public class GLCmdInternalDraw
	{
        public MgPrimitiveTopology Topology { get; set; }

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

