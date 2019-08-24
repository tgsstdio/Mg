namespace Magnesium
{
    public class MgExternalMemoryProperties
	{
		public MgExternalMemoryFeatureFlagBits ExternalMemoryFeatures { get; set; }
		public MgExternalMemoryHandleTypeFlagBits ExportFromImportedHandleTypes { get; set; }
		public MgExternalMemoryHandleTypeFlagBits CompatibleHandleTypes { get; set; }
	}
}
