using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLNextPipelineLayout : IGLPipelineLayout
	{
		public GLUniformBinding[] Bindings { get; private set; }

		public uint NoOfBindingPoints { get; private set; }
		public IDictionary<uint, GLBindingPointOffsetInfo> Ranges { get; private set; }

		public uint NoOfStorageBuffers { get; private set; }
		public uint NoOfExpectedDynamicOffsets { get; private set; }

		public GLDynamicOffsetInfo[] OffsetDestinations { get; private set; }

		public GLNextPipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo)
		{
			if (pCreateInfo.SetLayouts.Length == 1)
			{
				var layout = (IGLDescriptorSetLayout)pCreateInfo.SetLayouts[0];
				Bindings = layout.Uniforms;
			}
			else
			{
				Bindings = new GLUniformBinding[0];
			}

			NoOfBindingPoints = 0U;
			NoOfStorageBuffers = 0U;
			NoOfExpectedDynamicOffsets = 0U;

			Ranges = new SortedDictionary<uint, GLBindingPointOffsetInfo>();
			OffsetDestinations = ExtractBindingsInformation();
			SetupOffsetRangesByBindings();
		}

		private GLDynamicOffsetInfo[] ExtractBindingsInformation()
		{
			var signPosts = new List<GLDynamicOffsetInfo>();
			// build flat slots array for uniforms 
			foreach (var desc in Bindings)
			{
				if (desc.DescriptorType == MgDescriptorType.UNIFORM_BUFFER
					|| desc.DescriptorType == MgDescriptorType.UNIFORM_BUFFER_DYNAMIC)
				{
					NoOfBindingPoints += desc.DescriptorCount;
					Ranges.Add(desc.Binding,
						   new GLBindingPointOffsetInfo
						   {
							   Binding = desc.Binding,
							   First = 0U,
							   Last = desc.DescriptorCount - 1
						   });

					if (desc.DescriptorType == MgDescriptorType.UNIFORM_BUFFER_DYNAMIC)
					{
						signPosts.Add(new GLDynamicOffsetInfo
						{
							Target = GLBufferRangeTarget.UNIFORM_BUFFER,
							DstIndex = NoOfExpectedDynamicOffsets,
						});
						NoOfExpectedDynamicOffsets += desc.DescriptorCount;
					}
				}
				else if (desc.DescriptorType == MgDescriptorType.STORAGE_BUFFER
						 || desc.DescriptorType == MgDescriptorType.STORAGE_BUFFER_DYNAMIC)
				{
					NoOfStorageBuffers += desc.DescriptorCount;

					if (desc.DescriptorType == MgDescriptorType.STORAGE_BUFFER_DYNAMIC)
					{
						signPosts.Add(new GLDynamicOffsetInfo
						{
							Target = GLBufferRangeTarget.STORAGE_BUFFER,
							DstIndex = desc.Binding,
						});
						NoOfExpectedDynamicOffsets += desc.DescriptorCount;
					}
				}

			}
			return signPosts.ToArray();
		}

		void SetupOffsetRangesByBindings()
		{
			var startingOffset = 0U;
			foreach (var g in Ranges.Values)
			{
				g.First += startingOffset;
				g.Last += startingOffset;
				startingOffset = g.Last + 1;
			}
		}

		public bool Equals(IGLPipelineLayout other)
		{
			if (other == null)
				return false;

			if (ReferenceEquals(this, other))
				return true;

			if (NoOfBindingPoints != other.NoOfBindingPoints)
				return false;

			if (NoOfExpectedDynamicOffsets != other.NoOfExpectedDynamicOffsets)
				return false;

			if (NoOfStorageBuffers != other.NoOfStorageBuffers)
				return false;

			if (Bindings != null && other.Bindings == null)
				return false;

			if (Bindings == null && other.Bindings != null)
				return false;

			if (OffsetDestinations != null && other.OffsetDestinations == null)
				return false;

			if (OffsetDestinations == null && other.OffsetDestinations != null)
				return false;

			if (Ranges != null && other.Ranges == null)
				return false;

			if (Ranges == null && other.Ranges != null)
				return false;

			if (Bindings.Length != other.Bindings.Length)
				return false;

			{
				var count = Bindings.Length;
				for (var i = 0; i < count; i += 1)
				{
					var left = Bindings[i];
					var right = other.Bindings[i];

					if (!left.Equals(right))
						return false;
				}
			}

			if (OffsetDestinations.Length != other.OffsetDestinations.Length)
				return false;

			{
				var count = OffsetDestinations.Length;
				for (var i = 0; i < count; i += 1)
				{
					var left = OffsetDestinations[i];
					var right = other.OffsetDestinations[i];

					if (!left.Equals(right))
						return false;
				}
			}

			if (Ranges.Count != other.Ranges.Count)
				return false;

			return Ranges.Equals(other.Ranges);
		}

		#region IMgPipelineLayout implementation
		private bool mIsDisposed = false;
		public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		#endregion
	}
}

