using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium.Vulkan.Functions.Queue
{
    public class VkQueueUtility
    {
        public static IntPtr ExtractSemaphores(List<IntPtr> attachedItems, IMgSemaphore[] semaphores, out uint semaphoreCount)
        {
            var dest = IntPtr.Zero;
            uint count = 0U;

            if (semaphores != null)
            {
                semaphoreCount = (uint)semaphores.Length;
                if (semaphoreCount > 0)
                {
                    dest = VkInteropsUtility.ExtractUInt64HandleArray(
                        semaphores,
                        (arg) =>
                        {
                            var bSemaphore = (VkSemaphore)arg;
                            Debug.Assert(bSemaphore != null);
                            return bSemaphore.Handle;
                        }
                    );
                    attachedItems.Add(dest);
                }
            }
            semaphoreCount = count;
            return dest;
        }
    }
}
