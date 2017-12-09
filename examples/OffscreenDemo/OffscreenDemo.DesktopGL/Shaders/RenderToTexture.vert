#version 450

precision highp float;

layout (std140, binding = 0) uniform UBO 
{
	mat4 projection_matrix;
	mat4 modelview_matrix;
} ubo;

layout(location = 0) in vec3 in_position;
layout(location = 1) in vec3 in_normal;
layout(location = 2) in vec2 in_uv;

out vec3 normal;
out vec2 texCoords;

void vertFunc(void)
{
    //works only for orthogonal modelview
    texCoords = in_uv;
    normal = (ubo.modelview_matrix * vec4(in_normal, 0)).xyz;
  
    gl_Position = ubo.projection_matrix * ubo.modelview_matrix * vec4(in_position, 1);
	gl_Position.y *= -1.0;
}