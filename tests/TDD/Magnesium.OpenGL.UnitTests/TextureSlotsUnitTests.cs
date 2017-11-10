using NUnit.Framework;
using System.Collections.Generic;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
    class MgTextureSlot
    {
        public uint Binding { get; set; }
        public IGLImageView View { get; set; }
        public IGLSampler Sampler { get; set; }
    }

    class MgTextureDescriptorSet
    {
        public MgTextureSlot[] Descriptors { get; set; }
    }

    interface IMgTextureCentralEntrypoint
    {
        int GetMaximumNumberOfTextureUnits();

        void BindView(uint binding, MgImageViewType viewType, int texture);
        void UnbindView(uint binding);
        void BindSampler(uint binding, int sampler);
        void UnbindSampler(uint binding);
    }

    class MgTextureCentral
    {
        private IMgTextureCentralEntrypoint mEntrypoint;
        public MgTextureCentral(IMgTextureCentralEntrypoint entrypoint)
        {
            mEntrypoint = entrypoint;
            AvailableSlots = new MgTextureSlot[0];
        }

        public void Initialize()
        {
            var maxSlots = mEntrypoint.GetMaximumNumberOfTextureUnits();
            AvailableSlots = new MgTextureSlot[maxSlots];
            for (var i = 0U; i < maxSlots; i += 1)
            {
                AvailableSlots[i] = new MgTextureSlot
                {
                    Binding = i,
                    Sampler = null,
                    View = null,
                };
                // REMOVE ALL EXISTING BINDINGS 
                mEntrypoint.UnbindView(i);
                mEntrypoint.UnbindSampler(i);
            }
        }

        public MgTextureSlot[] AvailableSlots { get; private set; }

        public void Bind(MgTextureDescriptorSet dSet)
        {
            foreach(var srcDescriptor in dSet.Descriptors)
            {
                var destBinding = srcDescriptor.Binding;

                if (destBinding < (uint) AvailableSlots.Length)
                {
                    var dest = AvailableSlots[destBinding];

                    UpdateViewIfNeeded(srcDescriptor, destBinding, dest);

                    UpdateSamplerIfNeeded(dest, destBinding, srcDescriptor);
                }
            }
        }

        private void UpdateSamplerIfNeeded(MgTextureSlot dest, uint destBinding, MgTextureSlot srcDescriptor)
        {
            bool needsUpdate = false;
            if (dest.Sampler != null)
            {
                if (srcDescriptor.Sampler != null
                    && dest.Sampler.SamplerId != srcDescriptor.Sampler.SamplerId)
                {
                    mEntrypoint.BindSampler(destBinding, srcDescriptor.Sampler.SamplerId);
                    needsUpdate = true;
                }
                else if (srcDescriptor.Sampler == null)
                {
                    mEntrypoint.UnbindSampler(destBinding);
                    needsUpdate = true;
                }
            }
            else if (srcDescriptor.Sampler != null)
            {
                mEntrypoint.BindSampler(destBinding, srcDescriptor.Sampler.SamplerId);
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                dest.Sampler = srcDescriptor.Sampler;
            }
        }

        private void UpdateViewIfNeeded(MgTextureSlot srcDescriptor, uint destBinding, MgTextureSlot dest)
        {
            bool needsUpdate = false;
            if (dest.View != null)
            {
                if (
                    srcDescriptor.View != null
                    && (
                        dest.View.ViewTarget != srcDescriptor.View.ViewTarget
                    ||  dest.View.TextureId != srcDescriptor.View.TextureId
                    )
                )
                {
                    var view = srcDescriptor.View;
                    mEntrypoint.BindView(destBinding, view.ViewTarget, view.TextureId);
                    needsUpdate = true;
                }
                else if (srcDescriptor.View == null)
                {
                    mEntrypoint.UnbindView(destBinding);
                    needsUpdate = true;
                }
            }
            else if (srcDescriptor.View != null)
            {
                var view = srcDescriptor.View;
                mEntrypoint.BindView(destBinding, view.ViewTarget, view.TextureId);
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                dest.View = srcDescriptor.View;
            }
        }
    }

    public class MockTextureCentralEntrypoint : IMgTextureCentralEntrypoint
    {
        public class ViewInfo
        {
            public MgImageViewType? ViewType { get; set; }
            public int TextureId { get; set; }
        }

        public Dictionary<uint, ViewInfo> Views = new Dictionary<uint, ViewInfo>();
        public void BindView(uint binding, MgImageViewType viewType, int texture)
        {
            if (Views.TryGetValue(binding, out ViewInfo view))
            {
                view.TextureId = texture;
                view.ViewType = viewType;
            }
            else
            {
                Views.Add(binding, new ViewInfo { TextureId = texture, ViewType = viewType });
            }            
        }

        public void UnbindView(uint binding)
        {
            if (Views.TryGetValue(binding, out ViewInfo view))
            {
                view.TextureId = 0;
                view.ViewType = null;
            }
            else
            {
                Views.Add(binding, new ViewInfo { TextureId = 0, ViewType = null });
            }
        }

        public Dictionary<uint, int> Samplers = new Dictionary<uint, int>();
        public void BindSampler(uint binding, int sampler)
        {
            if (Samplers.ContainsKey(binding))
            {
                Samplers[binding] = sampler;
            }
            else
            {
                Samplers.Add(binding, sampler);
            }            
        }

        public int MaxSlots { get; set; }
        public int GetMaximumNumberOfTextureUnits()
        {
            return MaxSlots;
        }

        public void UnbindSampler(uint binding)
        {
            if (Samplers.ContainsKey(binding))
            {
                Samplers[binding] = 0;
            }
            else
            {
                Samplers.Add(binding, 0);
            }
        }
    }

    [TestFixture]
    public class TextureSlotsUnitTests
    {
        [TestCase]
        public void ConstructorTest()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int EXPECTED = 0;
            Assert.IsNotNull(central.AvailableSlots);
            Assert.AreEqual(EXPECTED, central.AvailableSlots.Length);
        }

        [TestCase]
        public void InitializeTest()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int EXPECTED = 13;
            entrypoint.MaxSlots = EXPECTED;

            central.Initialize();

            AssertInitializedCentral(central, EXPECTED);
            AssertEmptyEntrypoint(entrypoint, EXPECTED);
        }

        private static void AssertInitializedCentral(MgTextureCentral central, int EXPECTED)
        {
            Assert.IsNotNull(central.AvailableSlots);
            Assert.AreEqual(EXPECTED, central.AvailableSlots.Length);

            for (var i = 0U; i < central.AvailableSlots.Length; i += 1)
            {
                var item = central.AvailableSlots[i];
                Assert.AreEqual(i, item.Binding);
                Assert.IsNull(item.Sampler);
                Assert.IsNull(item.View);
            }
        }

        private static void AssertEmptyEntrypoint(MockTextureCentralEntrypoint entrypoint, int noOfSlots)
        {
            for (var i = 0U; i < noOfSlots; i += 1)
            {
                {
                    bool found = entrypoint.Views.TryGetValue(i, out MockTextureCentralEntrypoint.ViewInfo entry);
                    Assert.IsTrue(found);
                    Assert.IsNotNull(entry);
                    Assert.IsFalse(entry.ViewType.HasValue);
                    Assert.AreEqual(0, entry.TextureId);
                }

                {
                    bool found = entrypoint.Samplers.TryGetValue(i, out int sampler);
                    Assert.IsTrue(found);
                    Assert.AreEqual(0, sampler);
                }
            }
        }

        class MockGLSampler : IGLSampler
        {
            public int SamplerId { get; set; }

            public void DestroySampler(IMgDevice device, IMgAllocationCallbacks allocator)
            {
                throw new System.NotImplementedException();
            }
        }

        class MockGLImageView : IGLImageView
        {
            public int TextureId { get; set; }

            public MgImageViewType ViewTarget { get; set; }

            public bool IsNullImage => throw new System.NotImplementedException();

            public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
            {
                throw new System.NotImplementedException();
            }
        }

        [TestCase]
        public void EmptySetWithSampler()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 0U;
            const int EXPECTED_TEXTURE_ID = 23;
            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_TEXTURE_ID,
            };

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler
                    }
                }
            };

            central.Bind(delta);

            AssertBindOk(entrypoint, delta);
        }

        [TestCase]
        public void EmptySetWithTexture()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 0U;
            const int EXPECTED_TEXTURE_ID = 23;
            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = MgImageViewType.TYPE_2D,
            };

            var localSet = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        View = view
                    }
                }
            };

            central.Bind(localSet);

            AssertBindOk(entrypoint, localSet);
        }

        [TestCase]
        public void UnbindTexture()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 2U;
            const int EXPECTED_TEXTURE_ID = 23;
            const MgImageViewType EXPECTED_VIEW_TYPE = MgImageViewType.TYPE_2D;
            const int EXPECTED_SAMPLER_ID = 11;

            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = EXPECTED_VIEW_TYPE,
            };

            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_SAMPLER_ID,
            };

            var preload = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = view,
                    }
                }
            };

            central.Bind(preload);
            AssertBindOk(entrypoint, preload);

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = null,
                    }
                }
            };

            central.Bind(delta);
            AssertBindOk(entrypoint, delta);
        }

        [TestCase]
        public void UnbindSampler()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 5U;
            const int EXPECTED_TEXTURE_ID = 23;
            const MgImageViewType EXPECTED_VIEW_TYPE = MgImageViewType.TYPE_2D;
            const int EXPECTED_SAMPLER_ID = 11;

            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = EXPECTED_VIEW_TYPE,
            };

            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_SAMPLER_ID,
            };

            var preload = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = view,
                    }
                }
            };

            central.Bind(preload);
            AssertBindOk(entrypoint, preload);

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = null,
                        View = view,
                    }
                }
            };

            central.Bind(delta);
            AssertBindOk(entrypoint, delta);
        }

        [TestCase]
        public void UnbindAll()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new MgTextureCentral(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 1U;
            const int EXPECTED_TEXTURE_ID = 23;
            const MgImageViewType EXPECTED_VIEW_TYPE = MgImageViewType.TYPE_2D;
            const int EXPECTED_SAMPLER_ID = 11;

            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = EXPECTED_VIEW_TYPE,
            };

            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_SAMPLER_ID,
            };

            var preload = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = view,
                    }
                }
            };

            central.Bind(preload);
            AssertBindOk(entrypoint, preload);

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new MgTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = null,
                        View = null,
                    }
                }
            };

            central.Bind(delta);
            AssertBindOk(entrypoint, delta);
        }

        private static void AssertBindOk(MockTextureCentralEntrypoint entrypoint, MgTextureDescriptorSet source)
        {
            foreach (var descriptor in source.Descriptors)
            {
                var binding = descriptor.Binding;
                var samplerId = descriptor.Sampler != null ? descriptor.Sampler.SamplerId : 0;
                // MAKE SURE SAMPLER IS UNAFFECTED
                {
                    bool found = entrypoint.Samplers.TryGetValue(binding, out int actual);
                    Assert.IsTrue(found);
                    Assert.AreEqual(samplerId, actual);
                }

                var textureId = descriptor.View != null ? descriptor.View.TextureId : 0;
                {
                    bool found = entrypoint.Views.TryGetValue(binding, out MockTextureCentralEntrypoint.ViewInfo entry);
                    Assert.IsTrue(found);
                    Assert.IsNotNull(entry);

                    Assert.AreEqual(textureId, entry.TextureId);
                    if (descriptor.View != null)
                    {
                        Assert.IsTrue(entry.ViewType.HasValue);
                        Assert.AreEqual(descriptor.View.ViewTarget, entry.ViewType);
                    }
                    else
                    {
                        Assert.IsFalse(entry.ViewType.HasValue);
                    }
                }
            }
        }
    }
}

