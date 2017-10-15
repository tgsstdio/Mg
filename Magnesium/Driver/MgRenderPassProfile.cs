using System;
using System.Collections.Generic;

namespace Magnesium
{

/**
 * https://vulkan.lunarg.com/doc/view/1.0.26.0/windows/vkspec.chunked/ch07s02.html
 * 7.2. Render Pass Compatibility
 *  Framebuffers and graphics pipelines are created based on a specific render pass object. They must only be used with
 * that render pass object, or one compatible with it.
 * 
 * 	Two attachment references are compatible if they have matching format and sample count, or are both 
 * VK_ATTACHMENT_UNUSED or the pointer that would contain the reference is NULL.
 * 
 * 	Two arrays of attachment references are compatible if all corresponding pairs of attachments are compatible. 
 * 	If the arrays are of different lengths, attachment references not present in the smaller array are treated as
 * VK_ATTACHMENT_UNUSED.
 * 
 * 	Two render passes that contain only a single subpass are compatible if their corresponding color, input, resolve,
 * and depth/stencil attachment references are compatible.
 * 
 * 	If two render passes contain more than one subpass, they are compatible if they are identical except for:
 * 	- Initial and final image layout in attachment descriptions
 * 	- Load and store operations in attachment descriptions
 * 	- Image layout in attachment references
 * 
 * 	A framebuffer is compatible with a render pass if it was created using the same render pass or a compatible render
 * pass.
**/

	public class MgRenderPassProfile
	{
		private struct MgRenderPassProfileAttachmentInfo
		{
			public MgFormat Format { get; set;}
			public MgSampleCountFlagBits Samples { get; set;}
		}

		private class MgRenderPassProfileSubpassInfo
		{
			public uint[] Color { get; set; }
			public uint[] Input { get; set; }
			public uint[] Resolve { get; set; }
			public uint DepthStencil { get; set; } 
		}

		const uint VK_ATTACHMENT_UNUSED = ~0U;

		private readonly MgRenderPassProfileAttachmentInfo[] mAttachments;
		private readonly MgRenderPassProfileSubpassInfo[] mSubpasses;
		public MgRenderPassProfile(MgRenderPassCreateInfo createInfo)
		{
			mAttachments = InitializeAttachments(createInfo);
			mSubpasses = InitializeSubpasses(createInfo);
		}

		static MgRenderPassProfileSubpassInfo[] InitializeSubpasses(MgRenderPassCreateInfo createInfo)
		{
			if (createInfo == null || createInfo.Subpasses == null)
				return new MgRenderPassProfileSubpassInfo[] { };

			var subpasses = new List<MgRenderPassProfileSubpassInfo>();

			foreach (var sp in createInfo.Subpasses)
			{
				var item = new MgRenderPassProfileSubpassInfo 
				{ 
					Color = ExtractAttachmentIndices(sp.ColorAttachments),
					Input = ExtractAttachmentIndices(sp.InputAttachments),
					Resolve = ExtractAttachmentIndices(sp.ResolveAttachments),
					DepthStencil = sp.DepthStencilAttachment != null 
					                 ? sp.DepthStencilAttachment.Attachment : VK_ATTACHMENT_UNUSED, 
				};
				subpasses.Add(item);
			}

			return subpasses.ToArray();
		}

		static UInt32[] ExtractAttachmentIndices(MgAttachmentReference[] attachmentReferences)
		{
			var colors = new List<uint>();
			if (attachmentReferences != null)
			{
				foreach (var c in attachmentReferences)
				{
					colors.Add(c.Attachment);
				}
			}
			return colors.ToArray();
		}

		public bool IsCompatible(MgRenderPassProfile other)
		{
			if (other == null)
				return false;

			// Two attachment reference are compatible if they have matching format and sample count
			if (this.mSubpasses.Length != other.mSubpasses.Length)
			{
				return false;
			}

			for (var i = 0; i < this.mSubpasses.Length; ++i)
			{
				var leftSubpass = this.mSubpasses[i];
				var rightSubpass = other.mSubpasses[i];

				if (!CompareDepthStencil(
					leftSubpass.DepthStencil,
					rightSubpass.DepthStencil,
					this.mAttachments,
					other.mAttachments))
				{
					return false;
				}

				if (!CompareSubpassReference(
						leftSubpass.Color,
						rightSubpass.Color,
						this.mAttachments,
						other.mAttachments))
				{
					return false;
				}

				if (!CompareSubpassReference(
						leftSubpass.Input,
						rightSubpass.Input,
						this.mAttachments,
						other.mAttachments))
				{
					return false;
				}

				if (!CompareSubpassReference(
						leftSubpass.Resolve,
						rightSubpass.Resolve,
						this.mAttachments,
						other.mAttachments))
				{
					return false;
				}
			}

			return true;
		}

		bool CompareDepthStencil(
			uint left,
			uint right,
			MgRenderPassProfileAttachmentInfo[] leftAttachments,
			MgRenderPassProfileAttachmentInfo[] rightAttachments)
		{
			if (left == ~0U && right == VK_ATTACHMENT_UNUSED)
				return true;

			if (left != VK_ATTACHMENT_UNUSED && right == VK_ATTACHMENT_UNUSED)
				return false;

			if (left == VK_ATTACHMENT_UNUSED && right != VK_ATTACHMENT_UNUSED)
				return false;

			var a = leftAttachments[left];
			var b = rightAttachments[right];

			if (a.Format != b.Format)
			{
				return false;
			}

			if (a.Samples != b.Samples)
			{
				return false;
			}

			return true;
		}

		bool CompareSubpassReference(
			uint[] left,
			uint[] right, 
			MgRenderPassProfileAttachmentInfo[] leftAttachments,
			MgRenderPassProfileAttachmentInfo[] rightAttachments
		)
		{
			var min = Math.Min(left.Length, right.Length);
			var max = Math.Max(left.Length, right.Length);
			var isLeftLonger = left.Length > right.Length;

			for (var i = 0; i < min; ++i)
			{
				var a = left[i];
				var b = right[i];

				if (a != VK_ATTACHMENT_UNUSED && b == VK_ATTACHMENT_UNUSED)
					return false;

				if (a == VK_ATTACHMENT_UNUSED && b != VK_ATTACHMENT_UNUSED)
					return false;

				if (leftAttachments[a].Format != rightAttachments[b].Format)
				{
					return false;
				}

				if (leftAttachments[a].Samples != rightAttachments[b].Samples)
				{
					return false;
				}
			}

			if (isLeftLonger)
			{
				for (var i = min + 1; i < max; ++i)
				{
					if (left[i] != VK_ATTACHMENT_UNUSED)
					{
						return false;
					}
				}
			}
			else
			{
				for (var i = min + 1; i < max; ++i)
				{
					if (right[i] != VK_ATTACHMENT_UNUSED)
					{
						return false;
					}
				}
			}

			return true;
		}


		static MgRenderPassProfileAttachmentInfo[] InitializeAttachments(MgRenderPassCreateInfo createInfo)
		{
			if (createInfo == null || createInfo.Attachments == null)
				return new MgRenderPassProfileAttachmentInfo[] { };

			var attachments = new List<MgRenderPassProfileAttachmentInfo>();

			foreach (var attach in createInfo.Attachments)
			{
				attachments.Add(new MgRenderPassProfileAttachmentInfo
				{
					Format = attach.Format,
					Samples = attach.Samples,
				});
			}
			return attachments.ToArray();
		}

	}
}
