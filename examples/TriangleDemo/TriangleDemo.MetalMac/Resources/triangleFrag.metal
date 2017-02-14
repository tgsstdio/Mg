#include <metal_stdlib>
#include <simd/simd.h>

using namespace metal;

struct UBO
{
    float4x4 projectionMatrix;
    float4x4 modelMatrix;
    float4x4 viewMatrix;
};

typedef struct
{
    float3 inPos [[attribute(0)]];
    float3 inColor [[attribute(1)]];
} vertex_t;

typedef struct {
    float4 position [[position]];
    half4  color;
} ColorInOut;

// Fragment shader function
fragment half4 fragFunc(ColorInOut in [[stage_in]])
{
    return in.color;
}
