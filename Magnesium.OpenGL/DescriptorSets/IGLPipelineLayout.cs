using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.Internals
{
    public interface IGLPipelineLayout : IMgPipelineLayout, IEquatable<IGLPipelineLayout>
    {
        GLUniformBinding[] Bindings { get; }

		int NoOfBindingPoints { get; }
		IDictionary<int, GLBindingPointOffsetInfo> Ranges { get; }

		uint NoOfStorageBuffers { get; }
		uint NoOfExpectedDynamicOffsets { get; }

		GLDynamicOffsetInfo[] OffsetDestinations { get; }
    }
}