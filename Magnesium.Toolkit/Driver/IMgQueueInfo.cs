using Magnesium;

namespace Magnesium.Toolkit
{
	public interface IMgQueueInfo
	{
		uint QueueIndex { get; }
		uint QueueFamilyIndex { get; }
		IMgDevice Device { get; }
		IMgQueue Queue { get; }
		IMgThreadPartition CreatePartition (MgCommandPoolCreateFlagBits flags);
	}
}

