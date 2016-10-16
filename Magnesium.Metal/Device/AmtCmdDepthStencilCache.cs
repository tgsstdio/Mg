using System.Collections.Generic;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCmdDepthStencilCache : IAmtCmdDepthStencilCache
	{
		private readonly Dictionary<AmtDepthStencilStateKey, IMTLDepthStencilState> mCache;
		public AmtCmdDepthStencilCache()
		{
			mCache = new Dictionary<AmtDepthStencilStateKey, IMTLDepthStencilState>();
		}

		public void Add(AmtDepthStencilStateKey key, IMTLDepthStencilState record)
		{
			mCache.Add(key, record);
		}

		public bool TryGetValue(AmtDepthStencilStateKey key, out IMTLDepthStencilState record)
		{
			return mCache.TryGetValue(key, out record);
		}
	}
}