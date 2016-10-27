using System;
namespace Magnesium.Metal
{
	public interface IAmtDescriptorSetBindingLocator
	{
		bool TryGetValue(uint binding, out AmtDescriptorSetUpdateKey result);
		AmtDescriptorSetUpdateKey Add(uint binding);
	}
}
