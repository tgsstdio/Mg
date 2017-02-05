using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLDescriptorSetLayout : IGLDescriptorSetLayout
    {
		public GLDescriptorSetLayout (MgDescriptorSetLayoutCreateInfo pCreateInfo)
		{
			if (pCreateInfo.Bindings != null)
			{
                var highestBinding = 0U;
                var uniforms = new List<GLUniformBinding>();

                foreach (var binding in pCreateInfo.Bindings)
				{
                    highestBinding = Math.Max(binding.Binding, highestBinding);

                    var uniform = new GLUniformBinding{                        
                        Binding = binding.Binding,
                        DescriptorType = binding.DescriptorType,
                        DescriptorCount = binding.DescriptorCount,
                        StageFlags = binding.StageFlags,
                    };
					uniforms.Add (uniform);
				}

                var count = highestBinding + 1;

                Uniforms = new GLUniformBinding[count];
                foreach(var uni in uniforms)
                {
                    Uniforms[uni.Binding] = uni;
                }
            }
            else
            {
                Uniforms = new GLUniformBinding[0];
            }

		}

		public GLUniformBinding[] Uniforms { get; private set; }

		#region IMgDescriptorSetLayout implementation
		private bool mIsDisposed = false;
		public void DestroyDescriptorSetLayout (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		#endregion
	}
}

