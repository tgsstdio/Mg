using System;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	internal class AmtShaderModule : IMgShaderModule
	{
		public IMTLFunction Function { get; internal set; }
		public AmtShaderModule(MgShaderModuleCreateInfo pCreateInfo)
		{
			Info = pCreateInfo;
		}

		public MgShaderModuleCreateInfo Info { get; private set; }

		private bool mIsDisposed = false;
		public void DestroyShaderModule(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}
