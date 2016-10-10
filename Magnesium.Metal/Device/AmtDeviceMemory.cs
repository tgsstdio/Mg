using System;
using System.Diagnostics;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	internal class AmtDeviceMemory : IMgDeviceMemory
	{
		public nuint AllocationSize { get; private set; }
		public AmtDeviceMemory(IMTLDevice device, MgMemoryAllocateInfo pAllocateInfo)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pAllocateInfo.AllocationSize > nuint.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.AllocationSize)
				                                      + " must be <= nuint.MaxValue");
			}

			AllocationSize = (nuint)pAllocateInfo.AllocationSize;

			var options = MTLResourceOptions.CpuCacheModeDefault;
			InternalBuffer = device.CreateBuffer(AllocationSize, options);
		}

		internal IMTLBuffer InternalBuffer { get; private set; }

		private bool mIsDisposed = false;
		public void FreeMemory(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			InternalBuffer = null;

			mIsDisposed = true;
		}

		private NSRange? mRange;
		public Result MapMemory(IMgDevice device, ulong offset, ulong size, uint flags, out IntPtr ppData)
		{
			if (offset > (ulong) int.MaxValue)
			{
				// for generating new intptr
				throw new ArgumentOutOfRangeException(nameof(offset) + "must be <= " + int.MaxValue);
			}

			if (size > (ulong)nint.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(size) + "must be <= " + nint.MaxValue);
			}

			Debug.Assert(!mIsDisposed);

			mRange = new NSRange((nint) offset, (nint) size);

			IntPtr dest = IntPtr.Add(InternalBuffer.Contents, (int)offset);
			ppData =  dest;

			return Result.SUCCESS;
		}

		public void UnmapMemory(IMgDevice device)
		{
			Debug.Assert(!mIsDisposed);

			if (mRange.HasValue)
			{
				InternalBuffer.DidModify(mRange.Value);
				mRange = null;
			}
		}
	}
}