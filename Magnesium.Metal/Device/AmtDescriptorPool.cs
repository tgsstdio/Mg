using System;
using System.Collections.Concurrent;

namespace Magnesium.Metal
{
	internal class AmtDescriptorPool : IMgDescriptorPool
	{
		public uint PoolSize { get; private set; }

		public AmtDescriptorPool (MgDescriptorPoolCreateInfo createInfo)
		{
			if (createInfo == null)
				throw new ArgumentNullException(nameof(createInfo));
			
			PoolSize = createInfo.MaxSets;
			FreeSets = new ConcurrentBag<AmtDescriptorSet> ();
			AttachedSets = new AmtDescriptorSet[PoolSize];

			for (var i = 0; i < PoolSize; ++i)
			{
				var descriptorSet = new AmtDescriptorSet (i);
				AttachedSets[i] = descriptorSet;
				FreeSets.Add (descriptorSet);
			}
		}

		public AmtDescriptorSet[] AttachedSets { get; private set; }

		public ConcurrentBag<AmtDescriptorSet> FreeSets { get; private set; }

		public void Add (AmtDescriptorSet localSet)
		{
			FreeSets.Add (localSet);
		}

		public bool TryTake (out AmtDescriptorSet dSet)
		{
			return FreeSets.TryTake (out dSet);
		}

		public int RemainingSets {
			get {
				return FreeSets.Count;
			}
		}

		#region IMgDescriptorPool implementation

		public Result ResetDescriptorPool (IMgDevice device, uint flags)
		{
			for (var i = 0; i < PoolSize; ++i)
			{
				AttachedSets[i].Reset();
			}
			return Result.SUCCESS;
		}

		private bool mIsDisposed = false;
		public void DestroyDescriptorPool (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		#endregion
	}
}

