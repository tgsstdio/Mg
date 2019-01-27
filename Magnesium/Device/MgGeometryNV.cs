using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgGeometryNV
	{
		public MgGeometryTypeNV geometryType;
		public MgGeometryDataNV geometry;
		public MgGeometryFlagBitsNV flags;
	}
}
