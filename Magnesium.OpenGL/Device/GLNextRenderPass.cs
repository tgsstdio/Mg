using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLNextRenderPass : IGLRenderPass
    {
        public MgRenderPassProfile Profile { get; private set; }

        public GLNextRenderPass(MgRenderPassCreateInfo createInfo)
        {
            if(createInfo == null)
                throw new ArgumentNullException("createInfo");

            if (createInfo.Attachments == null)
                throw new ArgumentNullException("createInfo.Attachments");

            if (createInfo.Subpasses == null)
                throw new ArgumentNullException("createInfo.Subpasses");

            Subpasses = new GLNextRenderPassSubpassInfo[createInfo.Subpasses.Length];
            for (uint i = 0; i < Subpasses.Length; ++i)
            {
                Subpasses[i] = new GLNextRenderPassSubpassInfo(createInfo, i);
            }

            Profile = new MgRenderPassProfile(createInfo);

        }

        public GLNextRenderPassSubpassInfo[] Subpasses { get; private set; }

        public GLClearAttachmentInfo[] AttachmentFormats => throw new NotImplementedException();

        public void DestroyRenderPass(IMgDevice device, IMgAllocationCallbacks allocator)
        {

        }
    }
}
