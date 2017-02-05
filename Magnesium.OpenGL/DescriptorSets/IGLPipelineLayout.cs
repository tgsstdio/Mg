using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    public interface IGLPipelineLayout : IMgPipelineLayout, IEquatable<IGLPipelineLayout>
    {
        GLUniformBinding[] Bindings { get; }

		uint NoOfBindingPoints { get; }
		IDictionary<uint, GLBindingPointOffsetInfo> Ranges { get; }

		uint NoOfStorageBuffers { get; }
		uint NoOfExpectedDynamicOffsets { get; }

		GLDynamicOffsetInfo[] OffsetDestinations { get; }
    }
}