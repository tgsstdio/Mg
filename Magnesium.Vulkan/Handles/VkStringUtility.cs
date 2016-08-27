using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	internal static class VkStringUtility
	{
		// Using Hans Passant's answer
		// http://stackoverflow.com/questions/10773440/conversion-in-net-native-utf-8-managed-string
		internal static IntPtr NativeUtf8FromString(string managedString)
		{
			int len = System.Text.Encoding.UTF8.GetByteCount(managedString);
			byte[] buffer = new byte[len + 1];
			System.Text.Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
			IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
			return nativeUtf8;
		}
	}
}

