using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
    public class Class1
    {
        class Extraction
        {
            public uint[] Loads { get; set; }
            public uint[] Stores { get; set; }
        }

        /**
         * https://www.khronos.org/registry/vulkan/specs/1.0/html/vkspec.html#renderpass-creation
         * 
         * If the attachment uses a color format, then loadOp and storeOp are used, and stencilLoadOp and stencilStoreOp are ignored. 
         * If the format has depth and/or stencil components, loadOp and storeOp apply only to the depth data, while stencilLoadOp 
         * and stencilStoreOp define how the stencil data is handled. loadOp and stencilLoadOp define the load operations that 
         * execute as part of the first subpass that uses the attachment. storeOp and stencilStoreOp define the store 
         * operations that execute as part of the last subpass that uses the attachment.
         * 
         * The load operation for each sample in an attachment happens-before any recorded command which accesses the sample
         * in the first subpass where the attachment is used. Load operations for attachments with a depth/stencil format execute
         * in the VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT pipeline stage. Load operations for attachments with a color 
         * format execute in the VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT pipeline stage. 
         * 
         * The store operation for each sample in an attachment happens-after any recorded command which accesses the sample in the
         * last subpass where the attachment is used. Store operations for attachments with a depth/stencil format execute in the
         * VK_PIPELINE_STAGE_LATE_FRAGMENT_TESTS_BIT pipeline stage. Store operations for attachments with a color format 
         * execute in the VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT pipeline stage. 
         * 
         * If an attachment is not used by any subpass, then loadOp, storeOp, stencilStoreOp, and stencilLoadOp are ignored,
         * and the attachment’s memory contents will not be modified by execution of a render pass instance.
         **/
        class Extractor
        {
            struct MapAttachment
            {
                public uint First { get; set; }
                public uint Last { get; set; }
            }

            private static void Poll(MapAttachment[] map, uint subpass, uint attachment)
            {
                map[attachment].First = Math.Min(subpass, map[attachment].First);
                map[attachment].Last = subpass;
            }

            public Extraction[] Extract(Magnesium.MgRenderPassCreateInfo createInfo)
            {
                var noOfAttachments = createInfo.Attachments != null ? createInfo.Attachments.Length : 0;
                var noOfSubpasses = createInfo.Subpasses != null ? createInfo.Subpasses.Length : 0;

                var map = new MapAttachment[noOfAttachments];
                var output = new Extraction[noOfSubpasses];

                for (var i = 0; i < noOfAttachments; i += 1)
                {
                    map[i].First = uint.MaxValue;
                    map[i].Last = uint.MaxValue;
                }

                for (var i = 0U; i < noOfSubpasses; i += 1)
                {
                    var currentSubpass = createInfo.Subpasses[i];
                    output[i] = new Extraction { };

                    foreach(var reference in currentSubpass.ColorAttachments)
                    {
                        Poll(map, i, reference.Attachment);
                    }

                    if (currentSubpass.DepthStencilAttachment != null)
                        Poll(map, i ,currentSubpass.DepthStencilAttachment.Attachment);

                    var loads = new List<uint>();
                    for (var j = 0; j < noOfAttachments; j += 1)
                    {
                        var first = map[j].First;
                        if (first != uint.MaxValue && first == map[j].Last)
                        {
                            loads.Add(first);
                        }
                    }
                    output[i].Loads = loads.ToArray();
                }

                for (var i = 0U; i < noOfSubpasses; i += 1)
                {
                    var stores = new List<uint>();
                    for (var j = 0; j < noOfAttachments; j += 1)
                    {
                        stores.Clear();
                        var last = map[j].Last;
                        if (last != uint.MaxValue)
                        {
                            stores.Add(last);
                        }
                    }
                    output[i].Stores = stores.ToArray();
                }

                return output;
            }
        }

        [TestCase]
        public void TestMethod()
        {
            // calculate the first subpass for each attachment and work out when to apply it

            var createInfo = new Magnesium.MgRenderPassCreateInfo
            {
                Attachments = new MgAttachmentDescription[]
                {
                    new MgAttachmentDescription
                    {
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                    }
                },

                Subpasses = new MgSubpassDescription[]
                {
                    new MgSubpassDescription
                    {
                        ColorAttachments = new []
                        {
                            new MgAttachmentReference
                            {
                                Attachment = 0,
                                Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            }
                        },
                    }
                }
            };

            var extractor = new Extractor();
            var actual = extractor.Extract(createInfo);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            var first = actual[0];
            Assert.IsNotNull(first);
            Assert.IsNotNull(first.Loads);
            Assert.IsNotNull(first.Stores);
        }
    }
}
