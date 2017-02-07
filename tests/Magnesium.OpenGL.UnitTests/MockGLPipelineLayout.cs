using System;
using System.Collections.Generic;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
    class MockGLPipelineLayout : IGLPipelineLayout
	{
		public GLUniformBinding[] Bindings
		{
			get;
			set;
		}

		public GLDynamicOffsetInfo[] OffsetDestinations
		{
			get;
			set;
		}

		public uint NoOfBindingPoints
		{
			get;
			set;
		}

		public uint NoOfExpectedDynamicOffsets
		{
			get;
			set;
		}

		public uint NoOfStorageBuffers
		{
			get;
			set;
		}

		public IDictionary<uint, GLBindingPointOffsetInfo> Ranges
		{
			get;
			set;
		}

		public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}

		public void Initialise()
		{
			throw new NotImplementedException();
		}

		public bool Equals(IGLPipelineLayout other)
		{
			throw new NotImplementedException();
		}
	}
}