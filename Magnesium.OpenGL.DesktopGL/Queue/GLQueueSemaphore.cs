using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	/// <summary>
	/// Graphic implementation of sync object.
	/// </summary>
	public class GLQueueSemaphore : IGLSemaphore
	{
		public int Index {get;set;}
		public IntPtr ObjectPtr {get;set;}
		public float Timeout {get;set;}

		public GLQueueSemaphore ()
		{
			TotalBlockingWaits = 0;
			ObjectPtr = IntPtr.Zero;
		}

		public bool IsWaiting { get; private set; }
		public void Reset()
		{
			IsWaiting = false;
			if (ObjectPtr != IntPtr.Zero)
			{
				GL.DeleteSync (ObjectPtr);
				ObjectPtr = IntPtr.Zero;
			}
		}

		private uint mNonblockCount;
		public void BeginSync()
		{
			ObjectPtr = GL.FenceSync (SyncCondition.SyncGpuCommandsComplete, 0);
			IsWaiting = true;
			mNonblockCount = 0;
		}

		private bool NonBlockingWait ()
		{
			// only on the first time
			ClientWaitSyncFlags waitOption = ClientWaitSyncFlags.SyncFlushCommandsBit;

			WaitSyncStatus result = GL.ClientWaitSync (ObjectPtr, waitOption, 0);
			waitOption = ClientWaitSyncFlags.None;
			if (result == WaitSyncStatus.WaitFailed)
			{
				throw new InvalidOperationException ("GPU NonBlockingWait sync failed - surplus actions incomplete");
			}
			return !(result == WaitSyncStatus.ConditionSatisfied || result == WaitSyncStatus.AlreadySignaled);
		}

		private bool BlockingWait ()
		{
			int times = 0;
			++TotalBlockingWaits;
			do
			{
				WaitSyncStatus status = GL.ClientWaitSync (ObjectPtr, ClientWaitSyncFlags.None, Duration);
				// BLOCKING WAITING 
				if (status == WaitSyncStatus.WaitFailed)
				{
					throw new InvalidOperationException ("GPU BlockingWait sync failed - surplus actions completed");
				} 
				else if (status == WaitSyncStatus.ConditionSatisfied || status == WaitSyncStatus.AlreadySignaled)
				{
					return false;
				}
				++times;
			}
			while (times < BlockingRetries);

			++TotalFailures;
			// still waiting
			return true;
		}

		#region IMgSemaphore implementation

		public void DestroySemaphore (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (ObjectPtr != IntPtr.Zero)
			{
				GL.DeleteSync (ObjectPtr);
				ObjectPtr = IntPtr.Zero;
			}
		}

		#endregion

		#region ISyncObject implementation

		public uint TotalFailures {
			get;
			private set;
		}

		public uint BlockingRetries {
			get;
			set;
		}

		public bool IsReady ()
		{
			if (IsWaiting)
			{
				bool needBlocking = NonBlockingWait ();

				if (mNonblockCount >= NonBlockingRetries)
				{
					IsWaiting = needBlocking && BlockingWait ();
				}
				else
				{
					++mNonblockCount;
				}
			}

			return !(IsWaiting);
		}

		public long Duration {
			get;
			set;
		}

		public int Factor {
			get;
			set;
		}

		public uint TotalBlockingWaits {
			get;
			private set;
		}


		public uint NonBlockingRetries {
			get;
			set;
		}
		#endregion
	}
}

