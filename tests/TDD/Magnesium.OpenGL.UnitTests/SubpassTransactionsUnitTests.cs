using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
    public class SubpassTransactionsUnitTests
    {
        [TestCase]
        public void Subpass_0()
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

            var actual = MgSubpassTransactionExtractor.Extract(createInfo);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            var first = actual[0];
            Assert.IsNotNull(first);
            Assert.AreEqual(0, first.Subpass);
            Assert.IsNotNull(first.Loads);
            Assert.AreEqual(1, first.Loads.Length);
            Assert.IsNotNull(first.Stores);
            Assert.AreEqual(1, first.Stores.Length);
            Assert.IsNotNull(first.ColorAttachments);
            Assert.AreEqual(1, first.ColorAttachments.Length);
            Assert.AreEqual(0, first.ColorAttachments[0]);
            Assert.IsFalse(first.DepthAttachment.HasValue);
        }

        [TestCase]
        public void Subpass_1()
        {
            // calculate the first subpass for each attachment and work out when to apply it

            var createInfo = new Magnesium.MgRenderPassCreateInfo
            {
                Attachments = new MgAttachmentDescription[]
                {
                    new MgAttachmentDescription
                    {
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                    },
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
                    },
                    new MgSubpassDescription
                    {
                        ColorAttachments = new []
                        {
                            new MgAttachmentReference
                            {
                                Attachment = 0,
                                Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            },
                            new MgAttachmentReference
                            {
                                Attachment = 1,
                                Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            }
                        },
                    }
                }
            };

            var actual = MgSubpassTransactionExtractor.Extract(createInfo);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);

            {
                var first = actual[0];
                Assert.IsNotNull(first);
                Assert.AreEqual(0, first.Subpass);
                Assert.IsNotNull(first.Loads);
                Assert.AreEqual(1, first.Loads.Length);
                Assert.AreEqual(0, first.Loads[0]);
                Assert.IsNotNull(first.Stores);
                Assert.AreEqual(0, first.Stores.Length);

                Assert.IsNotNull(first.ColorAttachments);
                Assert.AreEqual(1, first.ColorAttachments.Length);
                Assert.AreEqual(0, first.ColorAttachments[0]);
                Assert.IsFalse(first.DepthAttachment.HasValue);
            }

            {
                var second = actual[1];
                Assert.IsNotNull(second);
                Assert.AreEqual(1, second.Subpass);
                Assert.IsNotNull(second.Loads);
                Assert.AreEqual(1, second.Loads.Length);
                Assert.AreEqual(1, second.Loads[0]);
                Assert.IsNotNull(second.Stores);
                Assert.AreEqual(2, second.Stores.Length);
                Assert.AreEqual(0, second.Stores[0]);
                Assert.AreEqual(1, second.Stores[1]);

                Assert.IsNotNull(second.ColorAttachments);
                Assert.AreEqual(2, second.ColorAttachments.Length);
                Assert.AreEqual(0, second.ColorAttachments[0]);
                Assert.AreEqual(1, second.ColorAttachments[1]);
                Assert.IsFalse(second.DepthAttachment.HasValue);
            }
        }

        [TestCase]
        public void Subpass_2()
        {
            // calculate the first subpass for each attachment and work out when to apply it

            var createInfo = new Magnesium.MgRenderPassCreateInfo
            {
                Attachments = new MgAttachmentDescription[]
                {
                    new MgAttachmentDescription
                    {
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                    },
                    new MgAttachmentDescription
                    {
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                    },
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
                        DepthStencilAttachment = new MgAttachmentReference
                        {
                            Attachment = 2,
                            Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                        }
                    },
                    new MgSubpassDescription
                    {
                        ColorAttachments = new []
                        {
                            new MgAttachmentReference
                            {
                                Attachment = 0,
                                Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            },
                            new MgAttachmentReference
                            {
                                Attachment = 1,
                                Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            }
                        },
                        DepthStencilAttachment = new MgAttachmentReference
                        {
                            Attachment = 2,
                            Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                        }
                    }
                }
            };

            var actual = MgSubpassTransactionExtractor.Extract(createInfo);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);

            {
                var first = actual[0];
                Assert.IsNotNull(first);
                Assert.AreEqual(0, first.Subpass);
                Assert.IsNotNull(first.Loads);
                Assert.AreEqual(2, first.Loads.Length);
                Assert.AreEqual(0, first.Loads[0]);
                Assert.AreEqual(2, first.Loads[1]);
                Assert.IsNotNull(first.Stores);
                Assert.AreEqual(0, first.Stores.Length);

                Assert.IsNotNull(first.ColorAttachments);
                Assert.AreEqual(1, first.ColorAttachments.Length);
                Assert.AreEqual(0, first.ColorAttachments[0]);
                Assert.IsTrue(first.DepthAttachment.HasValue);
                Assert.AreEqual(2, first.DepthAttachment.Value);
            }

            {
                var second = actual[1];
                Assert.IsNotNull(second);
                Assert.AreEqual(1, second.Subpass);
                Assert.IsNotNull(second.Loads);
                Assert.AreEqual(1, second.Loads.Length);
                Assert.AreEqual(1, second.Loads[0]);
                Assert.IsNotNull(second.Stores);
                Assert.AreEqual(3, second.Stores.Length);
                Assert.AreEqual(0, second.Stores[0]);
                Assert.AreEqual(1, second.Stores[1]);
                Assert.AreEqual(2, second.Stores[2]);

                Assert.IsNotNull(second.ColorAttachments);
                Assert.AreEqual(2, second.ColorAttachments.Length);
                Assert.AreEqual(0, second.ColorAttachments[0]);
                Assert.AreEqual(1, second.ColorAttachments[1]);
                Assert.IsTrue(second.DepthAttachment.HasValue);
                Assert.AreEqual(2, second.DepthAttachment.Value);
            }
        }
    }
}
