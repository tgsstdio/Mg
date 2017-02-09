using System;
namespace Magnesium.OpenGL
{
	public interface IGLDescriptorSet : IMgDescriptorSet, IEquatable<IGLDescriptorSet>
	{
		uint Key { get; }
		IGLNextDescriptorPool Parent { get; }
		GLDescriptorPoolResourceInfo[] Resources { get;  }

		void Initialize(GLDescriptorPoolResourceInfo[] resources);
		bool IsValidDescriptorSet { get; }
		void Invalidate();
	}
}
