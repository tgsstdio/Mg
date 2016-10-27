using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtRenderPassClearAttachment
	{
		public uint Index { get; set; }
		public AmtRenderPassAttachmentDestination Destination { get; set; }
		public double Divisor { get; set; }
		public MTLStoreAction StoreAction { get; internal set; }
		public MTLLoadAction LoadAction { get; internal set; }
		public MgFormat Format { get; internal set; }
		public AmtRenderPassClearValueType ClearValueType { get; internal set; }
		public MTLLoadAction StencilLoadAction { get; internal set; }
		public MTLStoreAction StencilStoreAction { get; internal set; }
	}
}
