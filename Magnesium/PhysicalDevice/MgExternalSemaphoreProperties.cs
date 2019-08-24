namespace Magnesium
{
    public class MgExternalSemaphoreProperties
	{
		public MgExternalSemaphoreHandleTypeFlagBits ExportFromImportedHandleTypes { get; set; }
		public MgExternalSemaphoreHandleTypeFlagBits CompatibleHandleTypes { get; set; }
		public MgExternalSemaphoreFeatureFlagBits ExternalSemaphoreFeatures { get; set; }
	}
}
