using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBuffer : IMgBuffer
	{
		public MgBufferUsageFlagBits Usage { get; private set; }

		public MgSharingMode SharingMode { get; private set; }

		public AmtBuffer(IMTLDevice mDevice, MgBufferCreateInfo pCreateInfo)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			if (pCreateInfo.Size > nuint.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(pCreateInfo.Size) + " must be <= nuint.MaxValue");
			}

			Length = (nuint)pCreateInfo.Size;
			//var options = MTLResourceOptions.CpuCacheModeDefault;
			//VertexBuffer = mDevice.CreateBuffer(Length, options);
			Usage = pCreateInfo.Usage;
			SharingMode = pCreateInfo.SharingMode;
		}

		public nuint Length { get; private set; }

		public IMTLBuffer VertexBuffer
		{
			get;
			internal set;
		}

		public ulong BoundMemoryOffset { get; private set; }

		public Result BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			if (device == null)
			{
				throw new ArgumentNullException(nameof(device));
			}

			if (memory == null)
			{
				throw new ArgumentNullException(nameof(memory));
			}

			var bDeviceMemory = (AmtDeviceMemory)memory;
			VertexBuffer = bDeviceMemory.InternalBuffer;
			// TODO : not sure where this comes into play
			BoundMemoryOffset = memoryOffset;

			return Result.SUCCESS;
		}

		private bool mIsDisposed = false;
		public void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;
			
			VertexBuffer = null;

			mIsDisposed = true;
		}
	}
}