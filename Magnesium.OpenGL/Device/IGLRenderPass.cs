
using Magnesium.OpenGL.Internals;
using Magnesium.Toolkit;

namespace Magnesium.OpenGL
{
	public interface IGLRenderPass : IMgRenderPass
	{
        GLRenderPassClearAttachment[] AttachmentFormats { get; }
        MgSubpassTransactionsInfo[] Subpasses { get;  }
        MgRenderPassProfile Profile { get; }
    }

}

