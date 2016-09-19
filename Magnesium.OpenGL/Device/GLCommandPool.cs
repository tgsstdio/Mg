using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	// RENDERER HERE
	public class GLCommandPool : IMgCommandPool
	{
		public MgCommandPoolCreateFlagBits Flags { get; private set; }

		public GLCommandPool (MgCommandPoolCreateFlagBits flags)
		{		
			Flags = flags;
			Buffers = new List<GLCommandBuffer> ();
		}

		public List<GLCommandBuffer> Buffers { get; private set; }

		#region IMgCommandPool implementation
		private bool mIsDisposed = false;
		public void DestroyCommandPool (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		public Result ResetCommandPool (IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			if (mIsDisposed)
				return Result.SUCCESS;

			foreach (var buffer in Buffers)
			{
				buffer.ResetAllData ();
			}

			return Result.SUCCESS;
		}

		#endregion


	}
}

