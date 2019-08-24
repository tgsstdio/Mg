using System;

namespace Magnesium
{
	public class MgSurfaceCapabilities2EXT
	{
		///
		/// Supported minimum number of images for the surface
		///
		public UInt32 MinImageCount { get; set; }
		///
		/// Supported maximum number of images for the surface, 0 for unlimited
		///
		public UInt32 MaxImageCount { get; set; }
		///
		/// Current image width and height for the surface, (0, 0) if undefined
		///
		public MgExtent2D CurrentExtent { get; set; }
		///
		/// Supported minimum image width and height for the surface
		///
		public MgExtent2D MinImageExtent { get; set; }
		///
		/// Supported maximum image width and height for the surface
		///
		public MgExtent2D MaxImageExtent { get; set; }
		///
		/// Supported maximum number of image layers for the surface
		///
		public UInt32 MaxImageArrayLayers { get; set; }
		///
		/// 1 or more bits representing the transforms supported
		///
		public MgSurfaceTransformFlagBitsKHR SupportedTransforms { get; set; }
		///
		/// The surface's current transform relative to the device's natural orientation
		///
		public MgSurfaceTransformFlagBitsKHR CurrentTransform { get; set; }
		///
		/// 1 or more bits representing the alpha compositing modes supported
		///
		public MgCompositeAlphaFlagBitsKHR SupportedCompositeAlpha { get; set; }
		///
		/// Supported image usage flags for the surface
		///
		public MgImageUsageFlagBits SupportedUsageFlags { get; set; }
		public MgSurfaceCounterFlagBitsEXT SupportedSurfaceCounters { get; set; }
	}
}
