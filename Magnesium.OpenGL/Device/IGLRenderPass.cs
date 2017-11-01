
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL
{
	public interface IGLRenderPass : IMgRenderPass
	{
        GLRenderPassClearAttachment[] AttachmentFormats { get; }
        MgSubpassTransactionsInfo[] Subpasses { get;  }
        MgRenderPassProfile Profile { get; }
    }

}

