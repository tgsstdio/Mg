﻿using System.Collections.Generic;

namespace Magnesium.OpenGL.Internals
{
	public class GLCommandPool : IMgCommandPool
	{
		public MgCommandPoolCreateFlagBits Flags { get; private set; }

		public GLCommandPool (MgCommandPoolCreateFlagBits flags)
		{		
			Flags = flags;
			Buffers = new List<IGLCommandBuffer> ();
		}

		public List<IGLCommandBuffer> Buffers { get; private set; }

		#region IMgCommandPool implementation
		private bool mIsDisposed = false;
		public void DestroyCommandPool (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		public MgResult ResetCommandPool (IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			if (mIsDisposed)
				return MgResult.SUCCESS;

			foreach (var buffer in Buffers)
			{
				buffer.ResetAllData ();
			}

			return MgResult.SUCCESS;
		}

		#endregion


	}
}

