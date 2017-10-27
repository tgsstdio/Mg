using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtRenderPassSubpassInfo
	{
		static double GetAttachmentDivisor(MgFormat format)
		{
			switch (format)
			{
				case MgFormat.D16_UNORM:
				case MgFormat.D16_UNORM_S8_UINT:
				case MgFormat.D24_UNORM_S8_UINT:
				case MgFormat.D32_SFLOAT:
				case MgFormat.D32_SFLOAT_S8_UINT:
					return 1;

				case MgFormat.R8_SINT:
				case MgFormat.R8G8_SINT:
				case MgFormat.R8G8B8_SINT:
				case MgFormat.R8G8B8A8_SINT:

				case MgFormat.R8_SNORM:
				case MgFormat.R8G8_SNORM:
				case MgFormat.R8G8B8_SNORM:
				case MgFormat.R8G8B8A8_SNORM:

				case MgFormat.B8G8R8A8_SINT:
				case MgFormat.B8G8R8_SINT:

				case MgFormat.B8G8R8A8_SNORM:
				case MgFormat.B8G8R8_SNORM:
					
					return sbyte.MaxValue;

				case MgFormat.R8_UINT:
				case MgFormat.R8G8_UINT:
				case MgFormat.R8G8B8_UINT:
				case MgFormat.R8G8B8A8_UINT:

				case MgFormat.R8_UNORM:
				case MgFormat.R8G8_UNORM:
				case MgFormat.R8G8B8_UNORM:
				case MgFormat.R8G8B8A8_UNORM:

				case MgFormat.B8G8R8A8_UINT:
				case MgFormat.B8G8R8_UINT:

				case MgFormat.B8G8R8A8_UNORM:
				case MgFormat.B8G8R8_UNORM:
					return byte.MaxValue;

				case MgFormat.R16_UINT:
				case MgFormat.R16G16_UINT:
				case MgFormat.R16G16B16_UINT:
				case MgFormat.R16G16B16A16_UINT:

				case MgFormat.R16_UNORM:
				case MgFormat.R16G16_UNORM:
				case MgFormat.R16G16B16_UNORM:
				case MgFormat.R16G16B16A16_UNORM:
					return ushort.MaxValue;

				case MgFormat.R16_SINT:
				case MgFormat.R16G16_SINT:
				case MgFormat.R16G16B16_SINT:
				case MgFormat.R16G16B16A16_SINT:

				case MgFormat.R16_SNORM:
				case MgFormat.R16G16_SNORM:
				case MgFormat.R16G16B16_SNORM:
				case MgFormat.R16G16B16A16_SNORM:
					return short.MaxValue;

				case MgFormat.R32_SINT:
				case MgFormat.R32G32_SINT:
				case MgFormat.R32G32B32_SINT:
				case MgFormat.R32G32B32A32_SINT:
					return int.MaxValue;

				// return smaller of the two
				case MgFormat.R64_SINT:
				case MgFormat.R64G64_SINT:
				case MgFormat.R64G64B64_SINT:
				case MgFormat.R64G64B64A64_SINT:
					return uint.MaxValue;

				case MgFormat.R32_UINT:
				case MgFormat.R64_UINT:
					return uint.MaxValue;

				case MgFormat.R32_SFLOAT:
				case MgFormat.R32G32_SFLOAT:
				case MgFormat.R32G32B32_SFLOAT:
				case MgFormat.R32G32B32A32_SFLOAT:
					return 1;

				default:
					throw new NotSupportedException();
			}
		}



		static AmtRenderPassClearValueType GetClearValueType(MgFormat format)
		{
			switch (format)
			{
				case MgFormat.D16_UNORM:
				case MgFormat.D16_UNORM_S8_UINT:
				case MgFormat.D24_UNORM_S8_UINT:
				case MgFormat.D32_SFLOAT:
				case MgFormat.D32_SFLOAT_S8_UINT:
					return AmtRenderPassClearValueType.DEPTH_STENCIL;

				case MgFormat.R8_SINT:
				case MgFormat.R8G8_SINT:
				case MgFormat.R8G8B8_SINT:
				case MgFormat.R8G8B8A8_SINT:
				case MgFormat.R16_SINT:
				case MgFormat.R16G16_SINT:
				case MgFormat.R16G16B16_SINT:
				case MgFormat.R16G16B16A16_SINT:
				case MgFormat.R32_SINT:
				case MgFormat.R32G32_SINT:
				case MgFormat.R32G32B32_SINT:
				case MgFormat.R32G32B32A32_SINT:
				case MgFormat.R64_SINT:
				case MgFormat.R64G64_SINT:
				case MgFormat.R64G64B64_SINT:
				case MgFormat.R64G64B64A64_SINT:
				case MgFormat.B8G8R8_SINT:
				case MgFormat.B8G8R8A8_SINT:
					return AmtRenderPassClearValueType.COLOR_INT;

				case MgFormat.R8_UINT:
				case MgFormat.R8G8_UINT:
				case MgFormat.R8G8B8_UINT:
				case MgFormat.R8G8B8A8_UINT:
				case MgFormat.R16_UINT:
				case MgFormat.R16G16_UINT:
				case MgFormat.R16G16B16_UINT:
				case MgFormat.R16G16B16A16_UINT:
				case MgFormat.R32_UINT:
				case MgFormat.R64_UINT:
				case MgFormat.B8G8R8_UINT:
				case MgFormat.B8G8R8A8_UINT:
					return AmtRenderPassClearValueType.COLOR_UINT;

				case MgFormat.R32_SFLOAT:
				case MgFormat.R32G32_SFLOAT:
				case MgFormat.R32G32B32_SFLOAT:
				case MgFormat.R32G32B32A32_SFLOAT:
				case MgFormat.R8G8B8_UNORM:
				case MgFormat.R8G8B8A8_UNORM:
				case MgFormat.R8G8B8_SNORM:
				case MgFormat.R8G8B8A8_SNORM:
					
				case MgFormat.B8G8R8_UNORM:
				case MgFormat.B8G8R8A8_UNORM:
				case MgFormat.B8G8R8_SNORM:
				case MgFormat.B8G8R8A8_SNORM:

				case MgFormat.R16G16B16_UNORM:
				case MgFormat.R16G16B16_SNORM:

				case MgFormat.R16G16B16A16_UNORM:
				case MgFormat.R16G16B16A16_SNORM:
					
					return AmtRenderPassClearValueType.COLOR_FLOAT;
				default:
					throw new NotSupportedException();
			}
		}

		public uint Subpass { get; private set; }

		public AmtRenderPassSubpassInfo(MgRenderPassCreateInfo createInfo, uint subpassIndex)
		{
			Subpass = subpassIndex;

			// check subpass description for correct values 
			var subpass = createInfo.Subpasses[subpassIndex];

            var colorAttachmentCount = subpass.ColorAttachments != null
                  ? (uint) subpass.ColorAttachments.Length
                  : 0U;
			ColorAttachments = new AmtRenderPassClearAttachment[
                colorAttachmentCount
            ];

            for (var j = 0U; j < colorAttachmentCount; ++j)
			{
				var color = subpass.ColorAttachments[j];
				var attachment = createInfo.Attachments[color.Attachment];

				ColorAttachments[j] = new AmtRenderPassClearAttachment
				{
					Index = color.Attachment,
					Format = attachment.Format,
					ClearValueType = GetClearValueType(attachment.Format),
					Divisor = GetAttachmentDivisor(attachment.Format),
					Destination = AmtRenderPassAttachmentDestination.COLOR,
					StoreAction = TranslateStoreOp(attachment.StoreOp),
					LoadAction = TranslateLoadOp(attachment.LoadOp),
					StencilLoadAction = TranslateLoadOp(attachment.StencilLoadOp),
					StencilStoreAction = TranslateStoreOp(attachment.StencilStoreOp),
				};
			}

			var depthStencil = subpass.DepthStencilAttachment;
			if (depthStencil != null)
			{
				var attachment = createInfo.Attachments[depthStencil.Attachment];
				DepthStencil = new AmtRenderPassClearAttachment
				{
					Index = depthStencil.Attachment,
					Format = attachment.Format,
					ClearValueType = GetClearValueType(attachment.Format),
					Divisor = GetAttachmentDivisor(attachment.Format),
					Destination = AmtRenderPassAttachmentDestination.DEPTH_AND_STENCIL,
					StoreAction = TranslateStoreOp(attachment.StoreOp),
					LoadAction = TranslateLoadOp(attachment.LoadOp),
					StencilLoadAction = TranslateLoadOp(attachment.StencilLoadOp),
					StencilStoreAction = TranslateStoreOp(attachment.StencilStoreOp),
				};
			}
		}

		public void InitializeFormat(MTLRenderPipelineDescriptor dest)
		{
			nint colorAttachmentIndex = 0;
			foreach (var attachment in ColorAttachments)
			{
				dest.ColorAttachments[colorAttachmentIndex].PixelFormat = AmtFormatExtensions.GetPixelFormat(attachment.Format);
				++colorAttachmentIndex;
			}

			if (DepthStencil != null)
			{
				dest.DepthAttachmentPixelFormat = AmtFormatExtensions.GetPixelFormat(DepthStencil.Format);
				dest.StencilAttachmentPixelFormat = AmtFormatExtensions.GetPixelFormat(DepthStencil.Format);
			}
		}

		MTLLoadAction TranslateLoadOp(MgAttachmentLoadOp loadOp)
		{
			switch (loadOp)
			{
				default:
					throw new NotSupportedException();
				case MgAttachmentLoadOp.CLEAR:
					return MTLLoadAction.Clear;
				case MgAttachmentLoadOp.DONT_CARE:
					return MTLLoadAction.DontCare;
				case MgAttachmentLoadOp.LOAD:
					return MTLLoadAction.Load;
			}
		}

		MTLStoreAction TranslateStoreOp(MgAttachmentStoreOp storeOp)
		{
			switch (storeOp)
			{
				case MgAttachmentStoreOp.DONT_CARE:
					return MTLStoreAction.Store;
				case MgAttachmentStoreOp.STORE:
					return MTLStoreAction.Store;
				default:
					throw new NotSupportedException();
			}
		}

		public AmtRenderPassClearAttachment[] ColorAttachments { get; set; }
		public AmtRenderPassClearAttachment DepthStencil { get; set; }
	}
}
