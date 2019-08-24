using System;

namespace Magnesium
{
	public class MgExternalImageFormatPropertiesNV
	{
		public MgImageFormatProperties ImageFormatProperties { get; set; }
		public UInt32 ExternalMemoryFeatures { get; set; }
		public UInt32 ExportFromImportedHandleTypes { get; set; }
		public UInt32 CompatibleHandleTypes { get; set; }
	}
}
