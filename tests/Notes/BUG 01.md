
Sample code for issue in Mg.GL 6.0.0

~~~~ C#
IMgQueue queue = ...
IMgCommandBuffer cmdBuf = ...
IMgDevice device = ...

var beginInfo = new MgCommandBufferBeginInfo { Flags = 0, };
cmdBuf.BeginCommandBuffer (beginInfo);

var passBeginInfo = new MgRenderPassBeginInfo {
    Framebuffer = framebuffers[i],
    RenderPass = renderpass,
    RenderArea = new MgRect2D {
        Extent = new MgExtent2D {
            Width = width,
            Height = height,
        },
        Offset = new MgOffset2D {
            X = 0,
            Y = 0,
        }
    },
    ClearValues = new[] {
        MgClearValue.FromColorAndFormat(format, new MgColor4f(1f, 0, 1f, 1f)),
        new MgClearValue {
            DepthStencil = new MgClearDepthStencilValue (1f, 0),							
        }
    },
};
cmdBuf.CmdBeginRenderPass (passBeginInfo, MgSubpassContents.INLINE);

// DO NOT BIND A DESCRIPTOR SET

cmdBuf.CmdBindPipeline (MgPipelineBindPoint.GRAPHICS, batch.GraphicsPipelines [0]);

cmdBuf.CmdBindVertexBuffers (0, new []{ vertexBuffer }, new ulong[]{ 0 });
cmdBuf.CmdBindIndexBuffer (indexBuffer, 0, MgIndexType.UINT32);
cmdBuf.CmdDrawIndexed (6, 1, 0, 0, 0);

cmdBuf.CmdEndRenderPass ();

var err = cmdBuf.EndCommandBuffer ();

var submitInfo = new []
{
   new MgSubmitInfo
   {
      CommandBuffers = new []
      {
          cmdBuf,
      }
   }
};

queue.QueueSubmit(device, submitInfo, null);
queue.QueueWait(device);

~~~~~