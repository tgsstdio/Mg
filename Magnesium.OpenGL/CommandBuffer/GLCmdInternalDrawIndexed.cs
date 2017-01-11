
namespace Magnesium.OpenGL
{
	public class GLCmdInternalDrawIndexed
	{
		public uint FirstInstance {
			get;
			set;
		}

		public int VertexOffset {
			get;
			set;
		}

		public uint FirstIndex {
			get;
			set;
		}

		public uint InstanceCount {
			get;
			set;
		}

		public uint IndexCount {
			get;
			set;
		}
        public MgPrimitiveTopology Topology { get; internal set; }
        public MgIndexType IndexType { get; internal set; }
    }

}

