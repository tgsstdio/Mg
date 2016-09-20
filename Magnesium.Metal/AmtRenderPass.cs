using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtRenderPass : IMgRenderPass
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
					return sbyte.MaxValue;

				case MgFormat.R8_UINT:
				case MgFormat.R8G8_UINT:
				case MgFormat.R8G8B8_UINT:
				case MgFormat.R8G8B8A8_UINT:
					return byte.MaxValue;

				case MgFormat.R16_UINT:
				case MgFormat.R16G16_UINT:
				case MgFormat.R16G16B16_UINT:
				case MgFormat.R16G16B16A16_UINT:
					return ushort.MaxValue;

				case MgFormat.R16_SINT:
				case MgFormat.R16G16_SINT:
				case MgFormat.R16G16B16_SINT:
				case MgFormat.R16G16B16A16_SINT:
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

		public AmtRenderPassClearAttachment[] ClearAttachments { get; private set; }

		public AmtRenderPass(MgRenderPassCreateInfo createInfo)
		{
			Debug.Assert(createInfo != null, nameof(createInfo) + " is null");
			Debug.Assert(createInfo.Attachments != null, nameof(createInfo.Attachments) + "is null");
			Debug.Assert(createInfo.Subpasses != null, nameof(createInfo.Subpasses) + " is null");
			if (createInfo.Attachments.Length > 6)
			{
				throw new NotSupportedException(nameof(createInfo.Attachments.Length) + " must be <= 4");
			}

			var noOfAttachments = createInfo.Attachments.Length;
			ClearAttachments = new AmtRenderPassClearAttachment[createInfo.Attachments.Length];
			for (var i = 0; i < noOfAttachments; ++i)
			{
				var attachment = createInfo.Attachments[i];

				ClearAttachments[i] = new AmtRenderPassClearAttachment
				{
					Index = (uint)i,
					Format = attachment.Format,

					Divisor = GetAttachmentDivisor(attachment.Format),
					Destination = AmtRenderPassAttachmentDestination.IGNORE,
					StoreAction = TranslateStoreOp(attachment.StoreOp),
					LoadAction = TranslateLoadOp(attachment.LoadOp),
				};

			}

			// TODO : multiple subpass i.e. Multiple RenderPassAttachmentDescriptor
			if (createInfo.Subpasses.Length != 1)
			{
				throw new ArgumentOutOfRangeException(nameof(createInfo.Subpasses.Length) + " != 1");
			}

			// check subpass description for correct values 
			var subpass = createInfo.Subpasses[0];
			Debug.Assert(subpass.ColorAttachmentCount <= 4);

			foreach (var color in subpass.ColorAttachments)
			{
				ClearAttachments[color.Attachment].Destination = AmtRenderPassAttachmentDestination.COLOR;
			}

			Debug.Assert(subpass.DepthStencilAttachment != null);

			var depthStencil = subpass.DepthStencilAttachment;
			  
			ClearAttachments[depthStencil.Attachment].Destination = AmtRenderPassAttachmentDestination.DEPTH_AND_STENCIL;
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

		public void DestroyRenderPass(IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}
	}
}
