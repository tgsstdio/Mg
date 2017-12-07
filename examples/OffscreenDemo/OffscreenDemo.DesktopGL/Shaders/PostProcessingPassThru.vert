#version 450

precision highp float;

layout (std140, binding = 0) uniform UBO 
{
	mat4 projection_matrix;
	mat4 modelview_matrix;
	vec4 offset;
} ubo;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_uv;

out vec2 texCoords;

void vertFunc(void)
{
  texCoords = in_uv;
  
  vec4 adjusted = vec4(in_position, 0, 1) + vec4(ubo.offset);

  gl_Position = ubo.projection_matrix * ubo.modelview_matrix * adjusted;
  gl_Position.y *= -1.0;
}