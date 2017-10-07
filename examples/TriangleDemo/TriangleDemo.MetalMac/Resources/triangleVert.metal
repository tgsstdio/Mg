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
    half4 color;
} ColorInOut;

vertex ColorInOut vertFunc(vertex_t vao [[stage_in]],
                           constant UBO& ubo [[buffer(1)]])
{
    ColorInOut out;
    half4 vertexColor = half4(vao.inColor.r, vao.inColor.g, vao.inColor.b, 1.0);
    out.color = vertexColor;
    out.position = ubo.projectionMatrix * ubo.viewMatrix * ubo.modelMatrix * float4(vao.inPos.xyz, 1.0);
    out.position.y *= -1.0;
    
    return out;
}
