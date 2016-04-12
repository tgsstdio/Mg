using System;

namespace Magnesium
{
    // DELEGATES
    public delegate void PFN_vkInternalAllocationNotification(
		IntPtr pUserData,
		IntPtr size,
		MgInternalAllocationType allocationType,
		MgSystemAllocationScope allocationScope);

	public delegate void PFN_vkInternalFreeNotification(
		IntPtr pUserData,
		IntPtr size,
		MgInternalAllocationType allocationType,
		MgSystemAllocationScope allocationScope);

	public delegate void PFN_vkReallocationFunction(
		IntPtr pUserData,
		IntPtr pOriginal,
		IntPtr size,
		IntPtr alignment,
		MgSystemAllocationScope allocationScope);

	public delegate void PFN_vkAllocationFunction(
		IntPtr pUserData,
		IntPtr size,
		IntPtr alignment,
		MgSystemAllocationScope allocationScope);

	public delegate void PFN_vkFreeFunction(
		IntPtr pUserData,
		IntPtr pMemory);

	public delegate void PFN_vkVoidFunction();

	public delegate void PFN_vkDebugReportCallbackEXT(
		MgDebugReportFlagBitsEXT flags,
		MgDebugReportObjectTypeEXT objectType,
		UInt64 @object,
		IntPtr location,
		Int32 messageCode,
		String pLayerPrefix,
		String pMessage,
		IntPtr pUserData);

	public class MgAllocationCallbacks
	{
		public IntPtr UserData { get; set; }
		public PFN_vkAllocationFunction PfnAllocation { get; set; }
		public PFN_vkReallocationFunction PfnReallocation { get; set; }
		public PFN_vkFreeFunction PfnFree { get; set; }
		public PFN_vkInternalAllocationNotification PfnInternalAllocation { get; set; }
		public PFN_vkInternalFreeNotification PfnInternalFree { get; set; }
	}
}

