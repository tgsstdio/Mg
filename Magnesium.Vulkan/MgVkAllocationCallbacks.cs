using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	public class MgVkAllocationCallbacks : IMgAllocationCallbacks
	{
		internal IntPtr Handle { get; private set;}
		public MgVkAllocationCallbacks(IntPtr handle)
		{
			Handle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Magnesium.Vulkan.VkAllocationCallbacks)));
		}

		~MgVkAllocationCallbacks()
		{
			Marshal.FreeHGlobal(Handle);
		}

		public IntPtr UserData { get; set; }
		public PFN_vkAllocationFunction PfnAllocation { get; set; }
		public PFN_vkReallocationFunction PfnReallocation { get; set; }
		public PFN_vkFreeFunction PfnFree { get; set; }
		public PFN_vkInternalAllocationNotification PfnInternalAllocation { get; set; }
		public PFN_vkInternalFreeNotification PfnInternalFree { get; set; }

	}
}
