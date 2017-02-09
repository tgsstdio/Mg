using Magnesium.OpenGL.Internals;
using System.Collections.Generic;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdIncrementalContextSorter : IGLCmdEncoderContextSorter
    {
        private GLCmdEncoderContext mCurrentContext;

        private readonly List<GLCmdEncoderContext> mContexts;
        private readonly List<GLCmdRecordInstruction> mInstructions;

        public GLCmdIncrementalContextSorter()
        {
            mContexts = new List<GLCmdEncoderContext>();
            mInstructions = new List<GLCmdRecordInstruction>();
            mCurrentContext = null;
        }

        public void Clear()
        {
            mContexts.Clear();
            mInstructions.Clear();
            mCurrentContext = null;
        }

        public void Add(GLCmdEncodingInstruction inst)
        {
            var currentIndex = (uint)mInstructions.Count;
            mInstructions.Add(new GLCmdRecordInstruction
            {
                Index = inst.Index,
                Operation = inst.Operation,
            });

            if (mCurrentContext == null)
            {
                InitializeNewContext(inst, currentIndex);
            }
            else
            {
                if (mCurrentContext.Category == inst.Category)
                {
                    mCurrentContext.Last = currentIndex;
                }
                else
                {
                    InitializeNewContext(inst, currentIndex);
                }
            }
        }

        void InitializeNewContext(GLCmdEncodingInstruction inst, uint currentIndex)
        {
            mCurrentContext = new GLCmdEncoderContext
            {
                Category = inst.Category,
                First = currentIndex,
                Last = currentIndex,
            };
            mContexts.Add(mCurrentContext);
        }

        public GLCmdCommandBufferRecord ToReplay()
        {
            return new GLCmdCommandBufferRecord
            {
                Contexts = mContexts.ToArray(),
                Instructions = mInstructions.ToArray(),
            };
        }
    }
}
