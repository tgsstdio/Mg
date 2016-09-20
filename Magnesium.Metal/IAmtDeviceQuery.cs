using System;

namespace Magnesium.Metal
{
	public interface IAmtDeviceQuery
	{
		nuint NoOfCommandBufferSlots { get; set; }
	}
}