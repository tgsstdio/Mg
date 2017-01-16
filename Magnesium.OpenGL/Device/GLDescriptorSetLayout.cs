using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLDescriptorSetLayout : IMgDescriptorSetLayout
	{
		public GLDescriptorSetLayout (MgDescriptorSetLayoutCreateInfo pCreateInfo)
		{
			Uniforms = new List<GLUniformBinding>();

			if (pCreateInfo.Bindings != null)
			{
				foreach (var binding in pCreateInfo.Bindings)
				{
					var uniform = new GLUniformBinding{                        
                        Location = (int) binding.Binding,
                        DescriptorType = binding.DescriptorType,
                        DescriptorCount = binding.DescriptorCount,
                        StageFlags = binding.StageFlags,
                    };
					Uniforms.Add (uniform);
				}
			}
		}

		public List<GLUniformBinding> Uniforms { get; private set; }

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

