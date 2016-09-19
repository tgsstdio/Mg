using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgClearDepthStencilValue
	{
		public MgClearDepthStencilValue (float depth, uint stencil)
		{
			Depth = depth;
			Stencil = stencil;
		}

		public float Depth;
		public UInt32 Stencil;
	}
}

