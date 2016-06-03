using Magnesium;
using System.Diagnostics;

namespace Magnesium
{
	public class MgQueueInfo : IMgQueueInfo
	{
		public uint QueueFamilyIndex {
			get;
			private set;
		}

		public uint QueueIndex {
			get;
			private set;
		}

		private readonly IMgPhysicalDevice mParent;
		public MgQueueInfo (uint queueIndex, IMgPhysicalDevice gpu, IMgDevice device, uint queueFamilyIndex, IMgQueue queue)
		{
			QueueIndex = queueIndex;
			QueueFamilyIndex = queueFamilyIndex;
			mParent = gpu;
			Device = device;
			Queue = queue;
		}

		#region IMgQueuePartition implementation

		public IMgThreadPartition CreatePartition()
		{
			var descPoolCreateInfo = new MgDescriptorPoolCreateInfo {
				MaxSets = 0,
				};

			return CreatePartition(0, descPoolCreateInfo);
		}

		public IMgThreadPartition CreatePartition (MgCommandPoolCreateFlagBits flags, MgDescriptorPoolCreateInfo descPoolCreateInfo)
		{
			IMgCommandPool commandPool;
			var cmdPoolCreateInfo = new MgCommandPoolCreateInfo {
				QueueFamilyIndex = this.QueueFamilyIndex,
				Flags = flags
			};

			var errCode = Device.CreateCommandPool (cmdPoolCreateInfo, null, out commandPool);
			Debug.Assert (errCode == Result.SUCCESS);

			IMgDescriptorPool descPool;
			errCode = Device.CreateDescriptorPool (descPoolCreateInfo, null, out descPool);
			Debug.Assert (errCode == Result.SUCCESS);

			var result = new MgThreadPartition (mParent, Device, this.Queue, commandPool, descPool);
			return result;
		}

		public IMgDevice Device {
			get;
			private set;
		}

		public IMgQueue Queue {
			get;
			private set;
		}
		#endregion
	}

}

