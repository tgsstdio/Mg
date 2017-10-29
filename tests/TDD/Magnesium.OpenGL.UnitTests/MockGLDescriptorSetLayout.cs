using System;
using Magnesium;
using Magnesium.OpenGL;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
	class MockGLDescriptorSetLayout : IGLDescriptorSetLayout
	{
		public GLUniformBinding[] Uniforms
		{
			get;
			set;
		}

		public void DestroyDescriptorSetLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}
	}
}