using Magnesium;

namespace TriangleDemo.MetalMac
{
	class MacTriangleDemoDisplayInfo : ITriangleDemoDisplayInfo
	{
		public MgFormat Color { get; internal set; }
		public MgFormat Depth { get; internal set; }
	}
}