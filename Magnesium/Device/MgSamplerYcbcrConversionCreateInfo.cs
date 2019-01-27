using System;

namespace Magnesium
{
	public class MgSamplerYcbcrConversionCreateInfo
	{
		public MgFormat Format { get; set; }
		public MgSamplerYcbcrModelConversion YcbcrModel { get; set; }
		public MgSamplerYcbcrRange YcbcrRange { get; set; }
		public MgComponentMapping Components { get; set; }
		public MgChromaLocation XChromaOffset { get; set; }
		public MgChromaLocation YChromaOffset { get; set; }
		public MgFilter ChromaFilter { get; set; }
		public bool ForceExplicitReconstruction { get; set; }
	}
}
