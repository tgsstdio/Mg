using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace BirdNest.Core
{
	public class StaticBindingPointBlockBinder : IUniformBlockBinder, IMemoryUser
	{
		interface IBindingPoint : IMemoryUser
		{
			int Index { get; }
			void UpdateData();
		}

		public delegate void SetBlock<TData, TReference>(ref TData dest, TReference src) 
			where TData : struct
			where TReference : class;

		public class StructBindingPoint<TData, TReference> : IBindingPoint
									where TData : struct
									where TReference : class
		{
			private TData mBlock;
			private TReference mReference;
			private SetBlock<TData, TReference> mBlockFunc;
			private IntPtr mBlockSize;

			public StructBindingPoint(TReference reference, SetBlock<TData, TReference> blockFunc)
			{
				this.mReference = reference;
				this.mBlockFunc = blockFunc;

			}

			public void Initialise(int nextIndex)
			{
				this.Index = nextIndex;
				this.BufferId = GL.GenBuffer ();
				GL.BindBuffer (BufferTarget.UniformBuffer, this.BufferId);
				this.mBlockSize = (IntPtr) System.Runtime.InteropServices.Marshal.SizeOf (typeof(TData));

				GL.BindBufferBase (BufferRangeTarget.UniformBuffer, Index, BufferId);
				GL.BufferData (BufferTarget.UniformBuffer,
					this.mBlockSize,
					(IntPtr) null,
					BufferUsageHint.DynamicDraw);
				GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 
					Index,
					BufferId,
					(IntPtr) 0,
					this.mBlockSize);
				GL.BindBuffer (BufferTarget.UniformBuffer, 0);
			}

			#region IBindingPoint implementation

			public void UpdateData ()
			{
				GL.BindBuffer(BufferTarget.UniformBuffer, this.BufferId);
				mBlockFunc (ref mBlock, mReference);
				GL.BufferSubData(BufferTarget.UniformBuffer,
					IntPtr.Zero,
					this.mBlockSize,
					ref mBlock);
				GL.BindBuffer(BufferTarget.UniformBuffer, 0);
			}

			private int BufferId { get ; set ; }
			public int Index {get; private set; }

			#endregion

			#region IMemoryUser implementation

			public void FreeMemory ()
			{
				GL.DeleteBuffer(this.BufferId);
			}

			#endregion
		}

		public interface IBindingPointSetter
		{
			Type BlockType {get;}
			Type ReferenceType { get; }
		}

		public class BlockSetter<TData, TReference> : IBindingPointSetter
			where TData : struct
			where TReference : class
		{
			#region IBindingPointSetter implementation

			public Type BlockType {
				get {
					return typeof(TData);
				}
			}

			public Type ReferenceType {
				get {
					return typeof(TReference);
				}
			}

			#endregion

			public SetBlock<TData,TReference>  Setter { get; private set; }
			public BlockSetter(SetBlock<TData,TReference>  blockFunc)
			{
				Setter = blockFunc;
			}
		}

		#region IUniformBlockBinder implementation

		public void Initialise ()
		{

		}

		public int BaseOffset {get;set;}
		public int NextIndex { get; private set; }
		private Dictionary<object, IBindingPoint> mBindings;
		private Dictionary<Type, IBindingPointSetter> mSetters;
		public StaticBindingPointBlockBinder (int baseBuffer)
		{
			this.BaseOffset = baseBuffer;
			this.NextIndex = this.BaseOffset;
			this.mBindings = new Dictionary<object, IBindingPoint> ();
			this.mSetters = new Dictionary<Type, IBindingPointSetter> ();
		}

		public bool AddSetter<TData, TReference>(SetBlock<TData,TReference> blockFunc)
			where TData : struct
			where TReference : class
		{
			Type referenceType = typeof(TReference);

			if (mSetters.ContainsKey (referenceType)) {
				return false;
			}

			mSetters.Add (referenceType, new BlockSetter<TData, TReference> (blockFunc));
			return true;
		}

		public bool Reserve<TData, TReference> (params TReference[] objs)
			where TData : struct
			where TReference : class
		{
			IBindingPointSetter found = null;
			Type referenceType = typeof(TReference);
			mSetters.TryGetValue (referenceType, out found);

			BlockSetter<TData, TReference> setter = found as BlockSetter<TData, TReference>;
			if (setter == null) {
				return false;
			}
			SetBlock<TData, TReference> del = setter.Setter;
			foreach (var item in objs) {
				var binding = new StructBindingPoint<TData, TReference> (item, del);
				binding.Initialise(this.NextIndex);
				mBindings.Add (referenceType, binding);
				this.NextIndex++;
			}
	
			return true;	
		}

		public void Update ()
		{
			foreach (var item in mBindings.Values) {
				item.UpdateData ();
			}
		}

		public void Bind (DisplayItem item)
		{
		
		}

		public void Setup (DisplayItem item)
		{
			//throw new NotImplementedException ();
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

