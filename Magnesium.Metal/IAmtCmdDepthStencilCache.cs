using Metal;

namespace Magnesium.Metal
{
	public interface IAmtCmdDepthStencilCache
	{
		bool TryGetValue(AmtDepthStencilStateKey key, out IMTLDepthStencilState record);
		void Add(AmtDepthStencilStateKey key, IMTLDepthStencilState record);
	}
}
