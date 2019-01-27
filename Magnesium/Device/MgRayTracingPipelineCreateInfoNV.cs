using System;

namespace Magnesium
{
	public class MgRayTracingPipelineCreateInfoNV
	{
        /// <summary>
        /// Pipeline creation flags
        /// </summary>
        public MgPipelineCreateFlagBits Flags { get; set; }
        /// <summary>
		/// One entry for each active shader stage
		/// </summary>
		public MgPipelineShaderStageCreateInfo[] Stages { get; set; }
		public MgRayTracingShaderGroupCreateInfoNV[] Groups { get; set; }
		public UInt32 MaxRecursionDepth { get; set; }
        /// <summary>
		/// Interface layout of the pipeline
		/// </summary>
		public IMgPipelineLayout Layout { get; set; }
        /// <summary>
        /// If VK_PIPELINE_CREATE_DERIVATIVE_BIT is set and this value is nonzero, it specifies the handle of the base pipeline this is a derivative of
        /// </summary>
        public IMgPipeline BasePipelineHandle { get; set; }
		///
		/// If VK_PIPELINE_CREATE_DERIVATIVE_BIT is set and this value is not -1, it specifies an index into pCreateInfos of the base pipeline this is a derivative of
		///
		public Int32 BasePipelineIndex { get; set; }
	}
}
