#version 450

#extension GL_ARB_shading_language_420pack : require

precision highp float;

layout (binding = 0) uniform sampler2D diffuseTex;

in vec2 texCoords;

out vec4 out_frag_color;

void fragFunc(void)
{
  vec4 diffuse = texture2D(diffuseTex, texCoords);
  out_frag_color = diffuse;
}