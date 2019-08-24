using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Toolkit
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
			return CreatePartition(0);
		}

		public IMgThreadPartition CreatePartition (MgCommandPoolCreateFlagBits flags)
		{
            IMgCommandPool commandPool;
			var cmdPoolCreateInfo = new MgCommandPoolCreateInfo {
				QueueFamilyIndex = this.QueueFamilyIndex,
				Flags = flags
			};

			var errCode = Device.CreateCommandPool (cmdPoolCreateInfo, null, out commandPool);
			Debug.Assert (errCode == MgResult.SUCCESS);

			var result = new MgThreadPartition (mParent, Device, this.Queue, commandPool);
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

