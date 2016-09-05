using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	public class MgVkAllocationCallbacks : IMgAllocationCallbacks
	{
		private enum VKIntPtrPositions : int
		{
			UserData = 0,
			PfnAllocation,
			PfnReallocation,
			PfnFree,
			PfnInternalAllocation,
			PfnInternalFree
		}

		internal IntPtr Handle { get; private set;}
		public MgVkAllocationCallbacks()
		{
			Handle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VkAllocationCallbacks)));
		}

		void SetInternalData(VKIntPtrPositions offset, IntPtr ptr)
		{
			var stride = Marshal.SizeOf(typeof(IntPtr));
			var dst = IntPtr.Add(Handle, ((int)offset) * stride);
			var src = new IntPtr[1];
			src[0] = ptr;
			Marshal.Copy(src, 0, dst, 1);
		}

		~MgVkAllocationCallbacks()
		{
			Marshal.FreeHGlobal(Handle);

			//if (mAllocationHandle.HasValue)
			//{
			//	mAllocationHandle.Value.Free();
			//}

			//if (mReallocationHandle.HasValue)
			//{
			//	mReallocationHandle.Value.Free();
			//}

			//if (mFreeHandle.HasValue)
			//{
			//	mFreeHandle.Value.Free();
			//}

			//if (mInternalAllocHandle.HasValue)
			//{
			//	mInternalAllocHandle.Value.Free();
			//}

			//if (mInternalFreeHandle.HasValue)
			//{
			//	mInternalFreeHandle.Value.Free();
			//}
		}

		#region UserData

		private IntPtr mUserData;
		public IntPtr UserData 
		{ 
			get
			{
				return mUserData;
			}
			set
			{
				mUserData = value;
				SetInternalData(
					VKIntPtrPositions.UserData,
					mUserData);
			}
		}

		#endregion UserData

		#region PfnAllocation

		//private GCHandle? mAllocationHandle;
		private PFN_vkAllocationFunction mAllocationFunction;
		public PFN_vkAllocationFunction PfnAllocation
		{
			get
			{
				return mAllocationFunction;
			}
			set
			{
				//if (mAllocationHandle.HasValue)
				//{
				//	mAllocationHandle.Value.Free();
				//}
				mAllocationFunction = value;
				//mAllocationHandle = GCHandle.Alloc(mAllocationFunction);
				SetInternalData(
					VKIntPtrPositions.PfnAllocation,
					(mAllocationFunction != null) ?Marshal.GetFunctionPointerForDelegate(mAllocationFunction) : IntPtr.Zero);
			}
		}


		#endregion

		#region PfnAllocation
		//private GCHandle? mReallocationHandle;
		private PFN_vkReallocationFunction mReallocationFunction;
		public PFN_vkReallocationFunction PfnReallocation
		{ 
			get
			{
				return mReallocationFunction;
			}
			set
			{
				//if (mReallocationHandle.HasValue)
				//{
				//	mReallocationHandle.Value.Free();
				//}
				mReallocationFunction = value;
				//mReallocationHandle = GCHandle.Alloc(mReallocationFunction, GCHandleType.Pinned);
				SetInternalData(
					VKIntPtrPositions.PfnReallocation,
					(mReallocationFunction != null) ? Marshal.GetFunctionPointerForDelegate(mReallocationFunction) : IntPtr.Zero);
			}
		}

		#endregion

		#region PfnFree
		//private GCHandle? mFreeHandle;
		private PFN_vkFreeFunction mFreeFunction;
		public PFN_vkFreeFunction PfnFree { 
			get
			{
				return mFreeFunction;
			}
			set
			{
				//if (mFreeHandle.HasValue)
				//{
				//	mFreeHandle.Value.Free();
				//}
				mFreeFunction = value;

                //mFreeHandle = GCHandle.Alloc(mFreeFunction, GCHandleType.Pinned);
                SetInternalData(
                    VKIntPtrPositions.PfnFree,
                    (mFreeFunction != null) ? Marshal.GetFunctionPointerForDelegate(mFreeFunction) : IntPtr.Zero);
                

			}
		}
		#endregion

		#region PfnInternalAllocation

		private PFN_vkInternalAllocationNotification mInternalAllocFunction;
		//private GCHandle? mInternalAllocHandle;
		public PFN_vkInternalAllocationNotification PfnInternalAllocation
		{ 
			get
			{
				return mInternalAllocFunction;
			}
			set
			{
				//if (mInternalAllocHandle.HasValue)
				//{
				//	mInternalAllocHandle.Value.Free();
				//}
				mInternalAllocFunction = value;
				//mInternalAllocHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
				SetInternalData(
					VKIntPtrPositions.PfnInternalAllocation,
                     (mInternalAllocFunction != null) ?  Marshal.GetFunctionPointerForDelegate(mInternalAllocFunction) : IntPtr.Zero);
			}
		}

		#endregion

		#region PfnInternalFree

		//private GCHandle? mInternalFreeHandle;
		private PFN_vkInternalFreeNotification mInternalFreeFunction;
		public PFN_vkInternalFreeNotification PfnInternalFree 
		{ 
			get
			{
				return mInternalFreeFunction;
			}
			set
			{
				//if (mInternalFreeHandle.HasValue)
				//{
				//	mInternalFreeHandle.Value.Free();
				//}
				mInternalFreeFunction = value;
				//mInternalFreeHandle = GCHandle.Alloc(mInternalFreeHandle, GCHandleType.Pinned);
				SetInternalData(
					VKIntPtrPositions.PfnInternalFree,
                    (mInternalFreeFunction != null) ? Marshal.GetFunctionPointerForDelegate(mInternalFreeFunction) : IntPtr.Zero);
			}
		}

		#endregion
	}
}
