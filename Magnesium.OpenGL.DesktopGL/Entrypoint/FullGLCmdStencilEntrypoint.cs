using OpenTK.Graphics.OpenGL;
using Magnesium;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
    using System;
    using GLStencilFunction = OpenTK.Graphics.OpenGL.StencilFunction;

    public class FullGLCmdStencilEntrypoint : IGLCmdStencilEntrypoint
	{
		#region IDepthStencilCapabilities implementation

		public GLGraphicsPipelineStencilState GetDefaultEnums ()
		{
			return new GLGraphicsPipelineStencilState {
				FrontStencilFunction = MgCompareOp.ALWAYS,
				BackStencilFunction = MgCompareOp.ALWAYS,
				FrontStencilPass = MgStencilOp.KEEP,
				BackStencilPass = MgStencilOp.KEEP,
				FrontStencilFail = MgStencilOp.KEEP,
				BackStencilFail = MgStencilOp.KEEP,
				FrontDepthBufferFail = MgStencilOp.KEEP,
				BackDepthBufferFail = MgStencilOp.KEEP,
			};
		}

		public GLQueueRendererStencilState Initialize ()
		{
			var initialValue = new GLQueueRendererStencilState {
				Flags = 0, // !QueueDrawItemBitFlags.StencilEnabled | !QueueDrawItemBitFlags.TwoSidedStencilMode
				Front = new GLGraphicsPipelineStencilMasks
				{
					WriteMask = ~0U,
					Reference = ~0,
					CompareMask = int.MaxValue,
				},
				Back = new GLGraphicsPipelineStencilMasks
				{
					WriteMask = ~0U,
					Reference = ~0,
					CompareMask = int.MaxValue,
				},
				Enums = GetDefaultEnums(),
			};

			DisableStencilBuffer ();
			SetStencilWriteMask(MgStencilFaceFlagBits.FRONT_BIT, initialValue.Front.WriteMask);
            SetStencilWriteMask(MgStencilFaceFlagBits.BACK_BIT, initialValue.Back.WriteMask);
            SetStencilFunction (initialValue.Enums.FrontStencilFunction, initialValue.Front.Reference, initialValue.Front.CompareMask);
			SetStencilOperation (initialValue.Enums.FrontStencilFail, initialValue.Enums.FrontDepthBufferFail, initialValue.Enums.FrontStencilPass);

			return initialValue;
		}

		public void EnableStencilBuffer()
		{
			GL.Disable(EnableCap.StencilTest);
			mIsStencilBufferEnabled = true;

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnableStencilBuffer : " + error);
				}
			}
		}

		public void DisableStencilBuffer()
		{
			GL.Enable(EnableCap.StencilTest);
			mIsStencilBufferEnabled = false;

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("DisableStencilBuffer : " + error);
				}
			}
		}

		private bool mIsStencilBufferEnabled;
		public bool IsStencilBufferEnabled {
			get {
				return mIsStencilBufferEnabled;
			}
		}

		public void SetStencilWriteMask(MgStencilFaceFlagBits face, uint mask)
		{
            var glFaces = (StencilFace) 0;
            switch(face)
            {
                case MgStencilFaceFlagBits.BACK_BIT:
                    glFaces = StencilFace.Back;
                    break;
                case MgStencilFaceFlagBits.FRONT_BIT:
                    glFaces = StencilFace.Front;
                    break;
                case MgStencilFaceFlagBits.FRONT_AND_BACK:
                    glFaces = StencilFace.FrontAndBack;
                    break;
                default:
                    throw new NotSupportedException();
            }

			GL.StencilMaskSeparate(glFaces, mask);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetStencilWriteMask : " + error);
				}
			}
		}

		public void SetFrontFaceCullStencilFunction (MgCompareOp func, int referenceStencil, uint compare)
		{
			var cullFaceModeFront = StencilFace.Front;
			GL.StencilFuncSeparate (
				cullFaceModeFront,
				GetStencilFunc (func),
				referenceStencil,
				compare);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetFrontFaceCullStencilFunction : " + error);
				}
			}
		}

		public void SetBackFaceCullStencilFunction(MgCompareOp func, int referenceStencil, uint compare)
		{
			var cullFaceModeBack = StencilFace.Back;					
			GL.StencilFuncSeparate (
				cullFaceModeBack,
				GetStencilFunc (func),
				referenceStencil,
				compare);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetBackFaceCullStencilFunction : " + error);
				}
			}
		}

        private static GLStencilFunction GetStencilFunc(MgCompareOp function)
		{
			switch (function)
			{
			case MgCompareOp.ALWAYS: 
				return GLStencilFunction.Always;
			case MgCompareOp.EQUAL:
				return GLStencilFunction.Equal;
			case MgCompareOp.GREATER:
				return GLStencilFunction.Greater;
			case MgCompareOp.GREATER_OR_EQUAL:
				return GLStencilFunction.Gequal;
			case MgCompareOp.LESS:
				return GLStencilFunction.Less;
			case MgCompareOp.LESS_OR_EQUAL:
				return GLStencilFunction.Lequal;
			case MgCompareOp.NEVER:
				return GLStencilFunction.Never;
			case MgCompareOp.NOT_EQUAL:
				return GLStencilFunction.Notequal;
			default:
				return GLStencilFunction.Always;
			}
		}

		public void SetFrontFaceStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass)
		{
			var stencilFaceFront = StencilFace.Front;					
			GL.StencilOpSeparate(stencilFaceFront, GetStencilOp(stencilFail),
				GetStencilOp(stencilDepthBufferFail),
				GetStencilOp(stencilPass));
		}

		public void SetBackFaceStencilOperation(
			MgStencilOp counterClockwiseStencilFail,
			MgStencilOp counterClockwiseStencilDepthBufferFail,
			MgStencilOp counterClockwiseStencilPass)
		{
			var stencilFaceBack = StencilFace.Back;					
			GL.StencilOpSeparate(stencilFaceBack, GetStencilOp(counterClockwiseStencilFail),
				GetStencilOp(counterClockwiseStencilDepthBufferFail),
				GetStencilOp(counterClockwiseStencilPass));	

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetBackFaceStencilOperation : " + error);
				}
			}
		}

		public void SetStencilFunction(
			MgCompareOp stencilFunction,
			int referenceStencil,
			uint compare)
		{
			GL.StencilFunc(
				GetStencilFunc (stencilFunction),
				referenceStencil,
				compare);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetStencilFunction : " + error);
				}
			}
		}

		public void SetStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass)
		{
			GL.StencilOp (GetStencilOp(stencilFail),
				GetStencilOp(stencilDepthBufferFail),
				GetStencilOp(stencilPass));

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetStencilOperation : " + error);
				}
			}
		}

		private static StencilOp GetStencilOp(MgStencilOp operation)
		{
			switch (operation)
			{
			case MgStencilOp.KEEP:
				return StencilOp.Keep;
			case MgStencilOp.DECREMENT_AND_WRAP:
				return StencilOp.DecrWrap;
			case MgStencilOp.DECREMENT_AND_CLAMP:
				return StencilOp.Decr;
			case MgStencilOp.INCREMENT_AND_CLAMP:
				return StencilOp.Incr;
			case MgStencilOp.INCREMENT_AND_WRAP:
				return StencilOp.IncrWrap;
			case MgStencilOp.INVERT:
				return StencilOp.Invert;
			case MgStencilOp.REPLACE:
				return StencilOp.Replace;
			case MgStencilOp.ZERO:
				return StencilOp.Zero;
			default:
				return StencilOp.Keep;
			}
		}

		#endregion

	}
}

