using System;

namespace Magnesium
{
    public interface IMgPipeline
	{
		void DestroyPipeline(IMgDevice device, IMgAllocationCallbacks allocator);

        MgResult CompileDeferredNV(IMgDevice device, UInt32 shader);
    }
}

