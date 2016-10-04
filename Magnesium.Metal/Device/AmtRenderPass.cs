using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtRenderPass : IMgRenderPass
	{
		//public AmtRenderPassClearAttachment[] ClearAttachments { get; private set; }

		public AmtRenderPass(MgRenderPassCreateInfo createInfo)
		{
			Debug.Assert(createInfo != null, nameof(createInfo) + " is null");
			Debug.Assert(createInfo.Attachments != null, nameof(createInfo.Attachments) + "is null");
			Debug.Assert(createInfo.Subpasses != null, nameof(createInfo.Subpasses) + " is null");
			if (createInfo.Attachments.Length > 6)
			{
				throw new NotSupportedException(nameof(createInfo.Attachments.Length) + " must be <= 4");
			}

			Subpasses = new AmtRenderPassSubpassInfo[createInfo.Subpasses.Length];
			for (uint i = 0; i < Subpasses.Length; ++i)
			{
				Subpasses[i] = new AmtRenderPassSubpassInfo(createInfo, i);
			}

		}

		public AmtRenderPassSubpassInfo[] Subpasses { get; private set; }

		//public uint? DepthStencilAttachment { get; private set; }

		public void DestroyRenderPass(IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}
	}
}
