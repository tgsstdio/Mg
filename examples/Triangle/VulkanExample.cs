/*
* Translation into C# and Magnesium interface 2016
* Vulkan Example - Basic indexed triangle rendering by 2016 by Copyright (C) Sascha Willems - www.saschawillems.de
*
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

using OpenTK;
using Magnesium;

namespace TriangleDemo
{
    public class VulkanExample
    {
        // Vertex buffer and attributes
        struct VertexBufferInfo
        {
            public IMgDeviceMemory memory;	// Handle to the device memory for this buffer
            public IMgBuffer buffer; // Handle to the Vulkan buffer object that the memory is bound to
            public MgPipelineVertexInputStateCreateInfo inputState;
            public MgVertexInputBindingDescription inputBinding;
            public List<MgVertexInputAttributeDescription> inputAttributes;
        }        

        VertexBufferInfo vertices;

        struct IndicesInfo
        {
            IMgDeviceMemory memory;		
            IMgBuffer buffer;			
            UInt32 count;
        } 

        IndicesInfo indices;

        // Uniform block object
        struct uniformDataVS
        {
            VkDeviceMemory memory;		
            VkBuffer buffer;			
            VkDescriptorBufferInfo descriptor;
        }      

        public VulkanExample (IMgGraphicsConfiguration configuration)
        {
            mConfiguration = configuration;

            var width = 1280;
            var height = 720;

            mConfiguration.Initialize(1280, 720);

            zoom = -2.5f;
            title = "Vulkan Example - Basic indexed triangle";            
        }

        ~VulkanExample()
        {
            // Clean up used Vulkan resources 
            // Note: Inherited destructor cleans up resources stored in base class
            if (pipeline != null)
               pipeline.DestroyPipeline(device, null);

            if (pipelineLayout != null)
                pipelineLayout.DestroyPipelineLayout(device, null);

            if (descriptorSetLayout != null)    
                descriptorSetLayout.DestroyDescriptorSetLayout(device, null);

            if (vertices.buffer != null)
                vertices.buffer.DestroyBuffer(device, null);
            
            if (vertices.memory != null)
                vertices.memory.FreeMemory(device, null);

            if (indices.buffer != null)
                indices.buffer.DestroyBuffer(device, null);

            if (indices.memory != null)
                indices.memory.FreeMemory(device, null);

            if (uniformDataVS.buffer != null)
                uniformDataVS.buffer.DestroyBuffer(device, null);

            if (uniformDataVS.memory != null) 
                uniformDataVS.memory.FreeMemory(device, null);

            vkDestroySemaphore(device, presentCompleteSemaphore, null);
            vkDestroySemaphore(device, renderCompleteSemaphore, null);

            for (auto& fence : waitFences)
            {
                vkDestroyFence(device, fence, null);
            }
        }        
    }
}