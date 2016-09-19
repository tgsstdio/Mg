using System;

namespace Magnesium.OpenGL
{
	public class GLDescriptorSet : IMgDescriptorSet, IEquatable<GLDescriptorSet>
	{
		public int Key { get; private set; }

		IGLImageDescriptorEntrypoint mImageEntrypoint;

		public GLDescriptorSet (int key, IGLImageDescriptorEntrypoint imageEntrypoint)
		{
			Key = key;
			mImageEntrypoint = imageEntrypoint;
		}

		public GLDescriptorBinding[] Bindings { get; private set; }

		public void Populate(GLDescriptorSetLayout layout)
		{	
			// LET'S USE ARRAY INDEXING
			Bindings = new GLDescriptorBinding[layout.Uniforms.Count];
			int index = 0;
			foreach (var bind in layout.Uniforms)
			{
				if (bind.DescriptorType == MgDescriptorType.SAMPLER || bind.DescriptorType == MgDescriptorType.COMBINED_IMAGE_SAMPLER)
				{
					Bindings [index] = new GLDescriptorBinding (bind.Location,
						new GLImageDescriptor (mImageEntrypoint));
				}
				else if (bind.DescriptorType == MgDescriptorType.STORAGE_BUFFER || bind.DescriptorType == MgDescriptorType.STORAGE_BUFFER_DYNAMIC)
				{
					Bindings [index] = new GLDescriptorBinding (bind.Location,
						new GLBufferDescriptor ());
				}
				++index;
			}
		}

		public void Destroy ()
		{
			foreach (var image in Bindings)
			{
				image.Destroy ();
			}
			Bindings = null;
		}

		#region IEquatable implementation

		public bool Equals (GLDescriptorSet other)
		{
			return Key == other.Key;
		}

		#endregion

		public override int GetHashCode()
		{ 
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				//hash = hash * 23 + Pool.GetHashCode();
				hash = hash * 23 + Key.GetHashCode();
				return hash;
			}
		} 

	}
}

