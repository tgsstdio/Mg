using System;

namespace Magnesium
{
	public class MgApplicationInfo
	{
		public String ApplicationName { get; set; }
		public UInt32 ApplicationVersion { get; set; }
		public String EngineName { get; set; }
		public UInt32 EngineVersion { get; set; }
		public UInt32 ApiVersion { get; set; }

		public static UInt32 GenerateApiVersion(UInt32 major, UInt32 minor, UInt32 patch)
		{
			return (((major) << 22) | ((minor) << 12) | (patch));
		}
		//public UInt32 Major { get; set; }
		//public UInt32 Minor { get; set; }
		//public UInt32 Patch { get; set; }
	}
}

