using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    public class AmtIncrementalChunkifier : IAmtEncoderContextSorter
    {
        private AmtEncoderContext mCurrentContext;

        private readonly List<AmtEncoderContext> mContexts;
        private readonly List<AmtRecordInstruction> mInstructions;

        public AmtIncrementalChunkifier()
        {
            mContexts = new List<AmtEncoderContext>();
            mInstructions = new List<AmtRecordInstruction>();
            mCurrentContext = null;
        }

        public void Clear()
        {
            mContexts.Clear();
            mInstructions.Clear();
            mCurrentContext = null;
        }

        public void Add(AmtEncodingInstruction inst)
        {
            var currentIndex = (uint)mInstructions.Count;
            mInstructions.Add(new AmtRecordInstruction
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

        void InitializeNewContext(AmtEncodingInstruction inst, uint currentIndex)
        {
            mCurrentContext = new AmtEncoderContext
            {
                Category = inst.Category,
                First = currentIndex,
                Last = currentIndex,
            };
            mContexts.Add(mCurrentContext);
        }

        public AmtCommandBufferRecord ToReplay()
        {
            return new AmtCommandBufferRecord
            {
                Contexts = mContexts.ToArray(),
                Instructions = mInstructions.ToArray(),
            };
        }
    }
}
