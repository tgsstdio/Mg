using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBuffer : IMgBuffer
	{
		public IMTLBuffer Buffer
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public Result BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			throw new NotImplementedException();
		}

		public void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}
	}
}