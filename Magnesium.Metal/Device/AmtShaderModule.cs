using System;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	internal class AmtShaderModule : IMgShaderModule
	{
		public IMTLLibrary Library { get; internal set; }
		public AmtShaderModule(MgShaderModuleCreateInfo pCreateInfo)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Info = pCreateInfo;
		}

		public MgShaderModuleCreateInfo Info { get; private set; }

		public void DestroyShaderModule(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}
