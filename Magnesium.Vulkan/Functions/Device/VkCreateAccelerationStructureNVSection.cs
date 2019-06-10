using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateAccelerationStructureNVSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateAccelerationStructureNV(IntPtr device, ref VkAccelerationStructureCreateInfoNV pCreateInfo, IntPtr pAllocator, ref UInt64 pAccelerationStructure);

        public static MgResult CreateAccelerationStructureNV(VkDeviceInfo info, MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgAccelerationStructureNV pAccelerationStructure)
        {
            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(pAllocator);

            var geometryCount = (pCreateInfo.Info.Geometries != null)
                ? (uint)pCreateInfo.Info.Geometries.Length
                : 0U;

            var pGeometries = IntPtr.Zero;

            try
            {

                pGeometries = VkInteropsUtility.AllocateHGlobalArray<MgGeometryNV, VkGeometryNV>(
                        pCreateInfo.Info.Geometries,
                        (src) =>
                        {
                            return new VkGeometryNV
                            {
                                sType = VkStructureType.StructureTypeGeometryNv,
                                pNext = IntPtr.Zero,
                                flags = src.flags,
                                geometry = new VkGeometryDataNV
                                {
                                    aabbs = ExtractAabbs(src.geometry.aabbs),
                                    triangles = ExtractTriangleData(src.geometry.triangles)
                                },
                                geometryType = src.geometryType,
                            };
                        }
                    );

                var bCreateInfo = new VkAccelerationStructureCreateInfoNV
                {
                    sType = VkStructureType.StructureTypeAccelerationStructureCreateInfoNv,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    compactedSize = pCreateInfo.CompactedSize,
                    info = new VkAccelerationStructureInfoNV
                    {
                        sType = VkStructureType.StructureTypeAccelerationStructureInfoNv,
                        // TODO: extensible
                        pNext = IntPtr.Zero,
                        type = pCreateInfo.Info.Type,
                        flags = pCreateInfo.Info.Flags,
                        instanceCount = pCreateInfo.Info.InstanceCount,
                        geometryCount = geometryCount,
                        pGeometries = pGeometries,
                    },
                };

                var pHandle = 0UL;
                var result = vkCreateAccelerationStructureNV(info.Handle, ref bCreateInfo, allocatorPtr, ref pHandle);
                pAccelerationStructure = new VkAccelerationStructureNV(pHandle);
                return result;
            }
            finally
            {
                if (pGeometries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pGeometries);
                }
            }
        }

        private static VkGeometryTrianglesNV ExtractTriangleData(MgGeometryTrianglesNV src)
        {
            var bVertexData = (VkBuffer)src.VertexData;

            var bIndexData = (VkBuffer)src.IndexData;

            var bTransformData = (VkBuffer)src.TransformData;

            return new VkGeometryTrianglesNV
            {
                sType = VkStructureType.StructureTypeGeometryTrianglesNv,
                pNext = IntPtr.Zero,
                vertexData = bVertexData.Handle,
                vertexOffset = src.VertexOffset,
                vertexCount = src.VertexCount,
                vertexStride = src.VertexStride,
                vertexFormat = src.VertexFormat,
                indexData = bIndexData.Handle,
                indexOffset = src.IndexOffset,
                indexCount = src.IndexCount,
                indexType = src.IndexType,

                transformData = bTransformData.Handle,
                transformOffset = src.TransformOffset,
            };
        }

        private static VkGeometryAABBNV ExtractAabbs(MgGeometryAABBNV aabbs)
        {
            var bAabbData = (VkBuffer)aabbs.AabbData;

            return new VkGeometryAABBNV
            {
                sType = VkStructureType.StructureTypeGeometryAabbNv,
                pNext = IntPtr.Zero,
                aabbData = bAabbData.Handle,
                numAABBs = aabbs.NumAABBs,
                offset = aabbs.Offset,
                stride = aabbs.Stride,
            };
        }
    }
}
