using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgClearDepthStencilValue
	{
		public float Depth;
		public UInt32 Stencil;
	}
}

