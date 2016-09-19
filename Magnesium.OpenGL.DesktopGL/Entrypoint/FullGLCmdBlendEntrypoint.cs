using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdBlendEntrypoint : IGLCmdBlendEntrypoint
	{
		#region IBlendCapabilities implementation

		public void EnableLogicOp (bool logicOpEnable)
		{
			if (logicOpEnable)
				GL.Enable (EnableCap.ColorLogicOp);
			else 
				GL.Disable (EnableCap.ColorLogicOp);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnableLogicOp : " + error);
				}
			}
		}

		private static LogicOp GetGLLogicOp(MgLogicOp logicOp)
		{
			switch (logicOp)
			{
			case MgLogicOp.CLEAR:
				return OpenTK.Graphics.OpenGL.LogicOp.Clear;
			case MgLogicOp.SET:
				return OpenTK.Graphics.OpenGL.LogicOp.Set;
			case MgLogicOp.COPY:
				return OpenTK.Graphics.OpenGL.LogicOp.Copy;	
			case MgLogicOp.COPY_INVERTED:
				return OpenTK.Graphics.OpenGL.LogicOp.CopyInverted;	
			case MgLogicOp.NO_OP:
				return OpenTK.Graphics.OpenGL.LogicOp.Noop;
			case MgLogicOp.INVERT:
				return OpenTK.Graphics.OpenGL.LogicOp.Invert;
			case MgLogicOp.AND:
				return OpenTK.Graphics.OpenGL.LogicOp.And;
			case MgLogicOp.NAND:
				return OpenTK.Graphics.OpenGL.LogicOp.Nand;
			case MgLogicOp.OR:
				return OpenTK.Graphics.OpenGL.LogicOp.Or;
			case MgLogicOp.NOR:
				return OpenTK.Graphics.OpenGL.LogicOp.Nor;
			case MgLogicOp.XOR:
				return OpenTK.Graphics.OpenGL.LogicOp.Xor;
			case MgLogicOp.EQUIVALENT:
				return OpenTK.Graphics.OpenGL.LogicOp.Equiv;
			case MgLogicOp.AND_REVERSE:
				return OpenTK.Graphics.OpenGL.LogicOp.AndReverse;
			case MgLogicOp.AND_INVERTED:
				return OpenTK.Graphics.OpenGL.LogicOp.AndInverted;
			case MgLogicOp.OR_REVERSE:
				return OpenTK.Graphics.OpenGL.LogicOp.OrReverse;
			case MgLogicOp.OR_INVERTED:
				return OpenTK.Graphics.OpenGL.LogicOp.OrInverted;
			default:
				throw new NotSupportedException ();
			}
		}
	

		public void LogicOp (MgLogicOp logicOp)
		{
			GL.LogicOp (GetGLLogicOp (logicOp));

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("LogicOp : " + error);
				}
			}
		}

		private static int GetColorAttachmentId(uint index)
		{
			All[] colorAttachmentIndices = { 
				All.ColorAttachment0, 
				All.ColorAttachment1, 
				All.ColorAttachment2, 
				All.ColorAttachment3, 
				All.ColorAttachment4,
				All.ColorAttachment5,
				All.ColorAttachment6,
				All.ColorAttachment7, 
				All.ColorAttachment8, 
				All.ColorAttachment9,
				All.ColorAttachment10,
				All.ColorAttachment11, 
				All.ColorAttachment12, 
				All.ColorAttachment13, 
				All.ColorAttachment14, 
				All.ColorAttachment15, 
			};

			if (index >= colorAttachmentIndices.Length)
			{
				throw new NotSupportedException ();
			}

			return (int)colorAttachmentIndices [index];
		}

		private static BlendingFactorDest GetBlendFactorDest (MgBlendFactor blend)
		{
			switch (blend) {
			case MgBlendFactor.DST_ALPHA:
				return BlendingFactorDest.DstAlpha;
				//			case Blend.DestinationColor:
				//				return BlendingFactorDest.DstColor;
			case MgBlendFactor.ONE_MINUS_DST_ALPHA:
				return BlendingFactorDest.OneMinusDstAlpha;
				//			case Blend.InverseDestinationColor:
				//				return BlendingFactorDest.OneMinusDstColor;
			case MgBlendFactor.ONE_MINUS_SRC_ALPHA:
				return BlendingFactorDest.OneMinusSrcAlpha;
			case MgBlendFactor.ONE_MINUS_SRC_COLOR:
				#if MONOMAC || WINDOWS
				return (BlendingFactorDest)All.OneMinusSrcColor;
				#else
				return BlendingFactorDest.OneMinusSrcColor;
				#endif
			case MgBlendFactor.ONE:
				return BlendingFactorDest.One;
			case MgBlendFactor.SRC_ALPHA:
				return BlendingFactorDest.SrcAlpha;
				//			case Blend.SourceAlphaSaturation:
				//				return BlendingFactorDest.SrcAlphaSaturate;
			case MgBlendFactor.SRC_COLOR:
				#if MONOMAC || WINDOWS
				return (BlendingFactorDest)All.SrcColor;
				#else
				return BlendingFactorDest.SrcColor;
				#endif
			case MgBlendFactor.ZERO:
				return BlendingFactorDest.Zero;
			default:
				return BlendingFactorDest.One;
			}
		}

		public bool IsEnabled (uint index)
		{
			throw new System.NotImplementedException ();
		}

		public GLGraphicsPipelineBlendColorState Initialize (uint noOfAttachments)
		{
			var initialState = new GLGraphicsPipelineBlendColorState {
				LogicOpEnable = false,
				LogicOp = MgLogicOp.COPY,
				Attachments = new GLGraphicsPipelineBlendColorAttachmentState[noOfAttachments],
			};

			for (uint i = 0; i < noOfAttachments; ++i)
			{
				var attachment = new GLGraphicsPipelineBlendColorAttachmentState {
					BlendEnable = false,
					ColorWriteMask = MgColorComponentFlagBits.R_BIT | MgColorComponentFlagBits.G_BIT | MgColorComponentFlagBits.B_BIT | MgColorComponentFlagBits.A_BIT,
					ColorBlendOp = MgBlendOp.ADD,
					AlphaBlendOp = MgBlendOp.ADD,
					SrcColorBlendFactor = MgBlendFactor.ONE,
					DstColorBlendFactor = MgBlendFactor.ZERO,
					SrcAlphaBlendFactor = MgBlendFactor.ONE,
					DstAlphaBlendFactor = MgBlendFactor.ZERO,
				};

				EnableBlending (i, attachment.BlendEnable);
				SetColorMask (i, attachment.ColorWriteMask);
				ApplyBlendSeparateFunction (i, 
					attachment.SrcColorBlendFactor,
					attachment.DstColorBlendFactor,
					attachment.SrcAlphaBlendFactor,
					attachment.DstAlphaBlendFactor);
				initialState.Attachments [i] = attachment;
			}

			return initialState;
		}

		public void EnableBlending (uint index, bool blendEnabled)
		{
			if (blendEnabled)
				GL.Enable (IndexedEnableCap.Blend, index);

			else
				GL.Disable (IndexedEnableCap.Blend, index);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnableBlending : " + error);
				}
			}
		}

		public void SetColorMask (uint index, MgColorComponentFlagBits colorMask)
		{
			GL.ColorMask (
				index,
				(colorMask & MgColorComponentFlagBits.R_BIT) ==  MgColorComponentFlagBits.R_BIT,
				(colorMask & MgColorComponentFlagBits.G_BIT) == MgColorComponentFlagBits.G_BIT,
				(colorMask & MgColorComponentFlagBits.B_BIT) == MgColorComponentFlagBits.B_BIT,
				(colorMask & MgColorComponentFlagBits.A_BIT) == MgColorComponentFlagBits.A_BIT);
		}

		private BlendingFactorSrc GetBlendFactorSrc (MgBlendFactor blend)
		{
			switch (blend) {
			case MgBlendFactor.DST_ALPHA:
				return BlendingFactorSrc.DstAlpha;
			case MgBlendFactor.DST_COLOR:
				return BlendingFactorSrc.DstColor;
			case MgBlendFactor.ONE_MINUS_DST_ALPHA:
				return BlendingFactorSrc.OneMinusDstAlpha;
			case MgBlendFactor.ONE_MINUS_DST_COLOR:
				return BlendingFactorSrc.OneMinusDstColor;
			case MgBlendFactor.ONE_MINUS_SRC_ALPHA:
				return BlendingFactorSrc.OneMinusSrcAlpha;
			case MgBlendFactor.ONE_MINUS_SRC_COLOR:
				#if MONOMAC || WINDOWS || DESKTOPGL
				return (BlendingFactorSrc)All.OneMinusSrcColor;
				#else
				return BlendingFactorSrc.OneMinusSrcColor;
				#endif
			case MgBlendFactor.ONE:
				return BlendingFactorSrc.One;
			case MgBlendFactor.SRC_ALPHA:
				return BlendingFactorSrc.SrcAlpha;
			case MgBlendFactor.SRC_ALPHA_SATURATE:
				return BlendingFactorSrc.SrcAlphaSaturate;
			case MgBlendFactor.SRC_COLOR:
				#if MONOMAC || WINDOWS || DESKTOPGL
				return (BlendingFactorSrc)All.SrcColor;
				#else
				return BlendingFactorSrc.SrcColor;
				#endif
			case MgBlendFactor.ZERO:
				return BlendingFactorSrc.Zero;
			default:
				return BlendingFactorSrc.One;
			}
		}

		public void ApplyBlendSeparateFunction (uint index, MgBlendFactor srcColor, MgBlendFactor dstColor, MgBlendFactor srcAlpha, MgBlendFactor destAlpha)
		{
			GL.BlendFuncSeparate(	
				index,
				GetBlendFactorSrc(srcColor),
				GetBlendFactorDest(dstColor), 
				GetBlendFactorSrc(srcAlpha), 
				GetBlendFactorDest(destAlpha));

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("ApplyBlendSeparateFunction : " + error);
				}
			}
		}

		#endregion
	}
}

