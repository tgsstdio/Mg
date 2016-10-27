using System;
using System.Collections.Generic;

namespace Magnesium.Metal
{
	public class AmtDescriptorSetBindingLocator : IAmtDescriptorSetBindingLocator
	{
		private IDictionary<uint, AmtDescriptorSetUpdateKey> mUpdates;
		public AmtDescriptorSetBindingLocator()
		{
			mUpdates = new Dictionary<uint, AmtDescriptorSetUpdateKey>();
		}

		public AmtDescriptorSetUpdateKey Add(uint binding)
		{
			var output = new AmtDescriptorSetUpdateKey { };
			mUpdates.Add(binding, output);
			return output;
		}

		public bool TryGetValue(uint binding, out AmtDescriptorSetUpdateKey result)
		{
			return mUpdates.TryGetValue(binding, out result);
		}

	}
}
