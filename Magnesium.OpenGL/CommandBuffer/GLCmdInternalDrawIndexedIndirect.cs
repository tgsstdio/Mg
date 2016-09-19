
namespace Magnesium.OpenGL
{
	public class GLCmdInternalDrawIndexedIndirect
	{
		public uint stride {
			get;
			set;
		}

		public uint drawCount {
			get;
			set;
		}

		public ulong offset {
			get;
			set;
		}

		public IMgBuffer buffer {
			get;
			set;
		}
	}

}

