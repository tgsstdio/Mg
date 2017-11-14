
using System;

namespace Magnesium.OpenGL
{
	public class GLBufferDescriptor
	{
		public GLBufferDescriptor()
		{
			BufferId = 0;
		}

		public uint BufferId { get; set; }
		public bool IsDynamic { get; set; }
		public long Offset { get; set; }
		public int Size { get; set; }

		public void Destroy()
		{

		}

		public void Reset()
		{
			BufferId = 0;
			IsDynamic = false;
			Offset = 0L;
			Size = 0;
		}
	}
}

