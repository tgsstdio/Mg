using System.Collections.Concurrent;

namespace Magnesium.OpenGL.DesktopGL
{
	internal class GLDescriptorPool : IGLDescriptorPool
	{
		public GLDescriptorPool (int poolSize, IGLImageDescriptorEntrypoint imgDescriptor)
		{
			Sets = new ConcurrentBag<GLDescriptorSet> ();

			for (int i = 0; i < poolSize; ++i)
			{
				var descriptorSet = new GLDescriptorSet (i, imgDescriptor);
				Sets.Add (descriptorSet);
			}
		}

		public ConcurrentBag<GLDescriptorSet> Sets { get; private set; }

		public void Add (GLDescriptorSet localSet)
		{
			Sets.Add (localSet);
		}

		public bool TryTake (out GLDescriptorSet dSet)
		{
			return Sets.TryTake (out dSet);
		}

		public int NoOfSets {
			get {
				return Sets.Count;
			}
		}

		#region IMgDescriptorPool implementation

		public Result ResetDescriptorPool (IMgDevice device, uint flags)
		{
			throw new System.NotImplementedException ();
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

