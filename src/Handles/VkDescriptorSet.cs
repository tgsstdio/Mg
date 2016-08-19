using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkDescriptorSet : IMgDescriptorSet
	{
		internal UInt64 Handle { get; private set;}
		internal VkDescriptorSet(UInt64 handle)
		{
			Handle = handle;
		}
	}
}
