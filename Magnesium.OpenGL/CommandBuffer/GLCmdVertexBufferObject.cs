using System;

namespace Magnesium.OpenGL
{
	public class GLCmdVertexBufferObject : IDisposable
	{
		public int VBO { get; private set; }
		private int mVertexBuffer;
		private int? mIndexBuffer;
		private readonly IGLCmdVBOEntrypoint mFactory;

		public GLCmdVertexBufferObject (int vbo, int vertexBuffer, int? indexBuffer, IGLCmdVBOEntrypoint factory)
		{
			VBO = vbo;
			mVertexBuffer = vertexBuffer;
			mIndexBuffer = indexBuffer;
			mFactory = factory;
		}

		public bool Matches(int vertexBuffer)
		{
			return (mVertexBuffer == vertexBuffer);								
		}

		public bool Matches(int vertexBuffer, int indexBuffer)
		{
			return (mVertexBuffer == vertexBuffer && mIndexBuffer.HasValue && mIndexBuffer.Value == indexBuffer);	
		}

		#region IDisposable implementation

		~GLCmdVertexBufferObject()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool isDisposing)
		{
			if (mDisposed)
				return;

			if (mFactory != null && VBO != 0)
				//mFactory.DeleteVBO(VBO);

			mDisposed = true;
		}

		#endregion
	}
}

