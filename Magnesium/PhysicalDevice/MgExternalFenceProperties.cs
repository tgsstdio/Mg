namespace Magnesium
{
    public class MgExternalFenceProperties
	{
		public MgExternalFenceHandleTypeFlagBits ExportFromImportedHandleTypes { get; set; }
		public MgExternalFenceHandleTypeFlagBits CompatibleHandleTypes { get; set; }
		public MgExternalFenceFeatureFlagBits ExternalFenceFeatures { get; set; }
	}
}
