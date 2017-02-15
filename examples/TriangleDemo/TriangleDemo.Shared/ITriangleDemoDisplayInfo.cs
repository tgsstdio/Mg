using Magnesium;

namespace TriangleDemo
{
	public interface ITriangleDemoDisplayInfo
	{
		MgFormat Color { get; }
		MgFormat Depth { get; }
	}
}
