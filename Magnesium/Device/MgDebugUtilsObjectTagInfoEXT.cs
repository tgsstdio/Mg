using System;

namespace Magnesium
{
	public class MgDebugUtilsObjectTagInfoEXT
	{
		public MgObjectType ObjectType { get; set; }
		public UInt64 ObjectHandle { get; set; }
		public UInt64 TagName { get; set; }
		public UIntPtr TagSize { get; set; }
		public IntPtr PTag { get; set; }
	}
}
