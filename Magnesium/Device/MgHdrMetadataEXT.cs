using System;

namespace Magnesium
{
	public class MgHdrMetadataEXT
	{
		///
		/// Display primary's Red
		///
		public MgXYColorEXT DisplayPrimaryRed { get; set; }
		///
		/// Display primary's Green
		///
		public MgXYColorEXT DisplayPrimaryGreen { get; set; }
		///
		/// Display primary's Blue
		///
		public MgXYColorEXT DisplayPrimaryBlue { get; set; }
		///
		/// Display primary's Blue
		///
		public MgXYColorEXT WhitePoint { get; set; }
		///
		/// Display maximum luminance
		///
		public float MaxLuminance { get; set; }
		///
		/// Display minimum luminance
		///
		public float MinLuminance { get; set; }
		///
		/// Content maximum luminance
		///
		public float MaxContentLightLevel { get; set; }
		public float MaxFrameAverageLightLevel { get; set; }
	}
}
