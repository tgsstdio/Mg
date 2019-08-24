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

	public delegate IntPtr PFN_vkReallocationFunction(
		IntPtr pUserData,
		IntPtr pOriginal,
		IntPtr size,
		IntPtr alignment,
        MgSystemAllocationScope allocationScope);

	public delegate IntPtr PFN_vkAllocationFunction(
		IntPtr pUserData,
		IntPtr size,
		IntPtr alignment,
        MgSystemAllocationScope allocationScope);

	public delegate void PFN_vkFreeFunction(
		IntPtr pUserData,
		IntPtr pMemory);

	public delegate UInt32 PFN_MgDebugReportCallbackEXT(
		MgDebugReportFlagBitsEXT flags,
		MgDebugReportObjectTypeEXT objectType,
		UInt64 @object,
		IntPtr location,
		Int32 messageCode,
		String pLayerPrefix,
		String pMessage,
		IntPtr pUserData);

	public interface IMgAllocationCallbacks
	{
		IntPtr UserData { get; set; }
		PFN_vkAllocationFunction PfnAllocation { get; set; }
		PFN_vkReallocationFunction PfnReallocation { get; set; }
		PFN_vkFreeFunction PfnFree { get; set; }
		PFN_vkInternalAllocationNotification PfnInternalAllocation { get; set; }
		PFN_vkInternalFreeNotification PfnInternalFree { get; set; }
	}
}

