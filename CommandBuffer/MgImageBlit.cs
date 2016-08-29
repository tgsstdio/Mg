using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgImageBlit
	{
		public MgImageSubresourceLayers SrcSubresource;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 2)]
		public MgOffset3D[] SrcOffsets; // 2

		public MgImageSubresourceLayers DstSubresource;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 2)]
		public MgOffset3D[] DstOffsets; // 2
	}
}

