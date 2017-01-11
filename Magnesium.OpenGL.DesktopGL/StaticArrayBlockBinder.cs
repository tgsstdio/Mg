using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK;

namespace BirdNest.Core
{
	public class StaticArrayBlockBinder : IUniformBlockBinder
	{
		interface IBindingPoint : IMemoryUser
		{
			string BlockKey  { get; }
			Type BlockType {get;}
			Type ReferenceType { get; }
			int BlockIndex { get; }
			void UpdateData();
		}

		public class ArrayBindingPoint<TBlock, TReference> : IBindingPoint
			where TBlock : struct
			where TReference : class
		{
			private TBlock[] mBlocks;
			private TReference[] mReferences;

			private IUniformBlockSetter<TBlock,TReference> mBlockFunc;
			private IntPtr mBlockSize;
			private int NoOfSlots;
			private int NoOfItems;

			public Type BlockType {
				get {
					return typeof(TBlock);
				}
			}

			public Type ReferenceType {
				get {
					return typeof(TReference);
				}
			}

			public string BlockKey
			{
				get {
					return typeof(TBlock).Name;
				}
			}

			public ArrayBindingPoint(int noOfSlots, IUniformBlockSetter<TBlock,TReference> blockFunc)
			{
				this.NoOfSlots = noOfSlots;
				this.mBlockFunc = blockFunc;
			}

			public void Initialise(int nextIndex)
			{
				this.mReferences = new TReference[this.NoOfSlots];
				this.mBlocks = new TBlock[this.NoOfSlots];
				this.BlockIndex = nextIndex;
				this.BufferId = GL.GenBuffer ();
				GL.BindBuffer (BufferTarget.UniformBuffer, this.BufferId);
				this.mBlockSize = (IntPtr) (System.Runtime.InteropServices.Marshal.SizeOf (typeof(TBlock)) * mBlocks.Length);

				GL.BindBufferBase (BufferRangeTarget.UniformBuffer, BlockIndex, BufferId);
				GL.BufferData (BufferTarget.UniformBuffer,
					this.mBlockSize,
					(IntPtr) null,
					BufferUsageHint.DynamicDraw);
				GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 
					BlockIndex,
					BufferId,
					(IntPtr) 0,
					this.mBlockSize);
				GL.BindBuffer (BufferTarget.UniformBuffer, 0);
			}

			public void Add(TReference item)
			{
				mReferences [this.NoOfItems] = item;
				this.NoOfItems++;
			}

			#region IBindingPoint implementation

			public void UpdateData ()
			{
				GL.BindBuffer(BufferTarget.UniformBuffer, this.BufferId);
				for (int i = 0; i < this.NoOfItems; ++i) {
					mBlockFunc.Set (ref mBlocks[i], mReferences[i]);
				}
				GL.BufferSubData(BufferTarget.UniformBuffer,
					IntPtr.Zero,
					this.mBlockSize,
					mBlocks);
				GL.BindBuffer(BufferTarget.UniformBuffer, 0);
			}

			private int BufferId { get ; set ; }
			public int BlockIndex {get; private set; }

			#endregion

			#region IMemoryUser implementation

			public void FreeMemory ()
			{
				GL.DeleteBuffer(this.BufferId);
			}

			#endregion
		}

		#region IUniformBlockBinder implementation
		public int BaseOffset {get;set;}
		public int NextIndex { get; private set; }
		private Dictionary<Type, IBindingPoint> mBindings;
		private Dictionary<string, IBindingPoint> mNameLookup;

		public StaticArrayBlockBinder (int baseBuffer)
		{
			this.BaseOffset = baseBuffer;
			this.NextIndex = this.BaseOffset;
			this.mBindings = new Dictionary<Type, IBindingPoint> ();
			this.mNameLookup = new Dictionary<string, IBindingPoint> ();
		}

		public void SetCamera(ref CameraBlock dest, CameraInformation src)
		{
			dest.eye = src.Eye;
			dest.fieldOfView = src.FieldOfView;
			dest.viewMatrix = Matrix4.CreateTranslation(-src.Eye);	
			dest.inverseViewMatrix = src.ViewMatrix.Inverted ();
			dest.x = src.X;
			dest.y = src.Y;
			dest.z = 1.0f / src.X;
			dest.w = 1.0f / src.Y;
			dest.projectionMatrix = src.ProjectionMatrix;
			dest.inverseProjectionMatrix = src.ProjectionMatrix.Inverted ();
		}

		public void Initialise()
		{
			AddBindingType<CameraBlock, CameraInformation>(10, new CameraBlockSetter());
			//AddBindingType<ModelBlock, ModelInformation> (10, null);
			AddBindingType<LightBlock, LightInformation> (10, new LightBlockSetter());
			AddBindingType<MaterialBlock, MaterialInformation> (10, new MaterialBlockSetter());
		}

		public bool AddBindingType<TBlock, TReference>(int noOfSlots, IUniformBlockSetter<TBlock,TReference> blockFunc)
			where TBlock : struct
			where TReference : class
		{
			Type referenceType = typeof(TReference);

			var binding = new ArrayBindingPoint<TBlock, TReference> (noOfSlots, blockFunc);
			binding.Initialise(this.NextIndex);

			mNameLookup.Add (binding.BlockKey, binding);
			this.NextIndex++;

			mBindings.Add (referenceType, binding);
			return true;
		}

		public bool Reserve<TBlock, TReference> (params TReference[] objs)
			where TBlock : struct
			where TReference : class
		{
			IBindingPoint found = null;
			Type referenceType = typeof(TReference);
			mBindings.TryGetValue (referenceType, out found);

			ArrayBindingPoint<TBlock, TReference> collection = found as ArrayBindingPoint<TBlock, TReference>;
			if (collection == null) {
				return false;
			}
			foreach (var item in objs) {
				collection.Add (item);
			}

			return true;	
		}

		public void Update ()
		{
			foreach (var item in mBindings.Values) {
				item.UpdateData ();
			}
		}

		public void Setup (DisplayItem item)
		{
			if (!item.AreBlocksBound) {
				int noOfUniformBlocks;
				GL.GetProgram(item.ProgramId, GetProgramParameterName.ActiveUniformBlocks, out noOfUniformBlocks);​

				int noOfUniforms;
				GL.GetProgram(item.ProgramId, GetProgramParameterName.ActiveUniforms, out noOfUniforms);​

				for(int i = 0; i < noOfUniformBlocks; ++i)
				{
					string blockName = GL.GetActiveUniformBlockName(item.ProgramId, i);
					int tokenEnd = blockName.IndexOf("[");

					string blockKey;
					if (tokenEnd > 0)
					{
						blockKey = blockName.Substring(0, tokenEnd);
					}
					else 
					{
						blockKey = blockName;
					}

					IBindingPoint found = null;
					mNameLookup.TryGetValue (blockKey, out found);
					int uniformLocation = GL.GetUniformBlockIndex (item.ProgramId, blockName);

					if (found != null)
					{
						var binding = new UniformBlock();
						binding.UniformLocation = uniformLocation;
						binding.BlockType = found.BlockType;
						binding.ReferenceType = found.ReferenceType;
						binding.BlockIndex = found.BlockIndex;

						string uniformKey = found.BlockKey.ToUpperInvariant() + "INDEX";
						bool hasIndex = false;

						for(int j = 0; j < noOfUniforms; ++j)
						{
							string uniformName = GL.GetActiveUniformName (item.ProgramId, j);
							if (uniformName.ToUpperInvariant() == uniformKey)
							{
								int indexLocation = GL.GetUniformLocation(item.ProgramId, uniformName);
								binding.IndexLocation = indexLocation;
								binding.IndexValue = 0;
								hasIndex = true;
								break;
							}
						}

						binding.HasIndex = hasIndex;

						item.BlockBindings.Add(binding);
					}
				}

				item.AreBlocksBound = true;
			}
		}

		public void Bind (DisplayItem item)
		{
			foreach(var blockBinding in item.BlockBindings)
			{
				if (blockBinding.HasIndex)
				{
					GL.ProgramUniform1(item.ProgramId, blockBinding.IndexLocation, blockBinding.IndexValue);
				}
				GL.UniformBlockBinding (item.ProgramId, blockBinding.UniformLocation, blockBinding.BlockIndex);
			}
		}

		#endregion


		#region IMemoryUser implementation
		public void FreeMemory ()
		{
			foreach (var item in mBindings.Values) {
				item.FreeMemory ();
			}
		}
		#endregion
	}
}

